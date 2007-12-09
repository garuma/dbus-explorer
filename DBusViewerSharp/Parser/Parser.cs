// Parser.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Text;
using System.Collections.Generic;

namespace DBusExplorer
{
	public static class Parser
	{
		public static string ParseDBusTypeExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
				return expression;
			// Assume it's a base type and thus directly return the corresponding type
			if (expression.Length == 1)
				return Mapper.DTypeToStr((DType)(byte)expression[0]);
			
			List<DType> expressionList = new List<DType>(expression.Length);
			// Fill expressionList
			foreach (char c in expression) {
				expressionList.Add((DType)(byte)c);
			}
			
			string temp;
			try {			
				temp = StrFromExprList(expressionList)[0];
			} catch {
				temp = "(Error while parsing)";
			}
			return temp;
		}
		
		static IList<string> StrFromExprList(List<DType> expressionList)
		{
			List<string> list = new List<string>();
			
			for (int index = 0; index < expressionList.Count; index++) {
				DType currentToken = expressionList[index];
				if (currentToken == DType.DictEntryEnd || currentToken == DType.StructEnd)
					continue;
				
				/*Console.WriteLine("Starting a new StrFromExprList");
				Console.WriteLine("Current Token : " + currentToken.ToString());*/
				
				switch (currentToken) {
					case DType.StructBegin:
						int count = GetStructLimit(expressionList, ++index);
						/*Console.WriteLine("Struct count : " + count.ToString());
						Console.Write("{ ");
						foreach (DType t in expressionList) Console.Write(t.ToString());
						Console.WriteLine(" }");*/
						
						list.Add(ParseStructDefinition(expressionList.GetRange(index, count)));
						index += count;
						break;
					// In fact this case is both for array and dictionary
					case DType.Array:
						count = GetArrayLimit(expressionList, ++index);
						//Console.WriteLine("Array count : " + count.ToString());
						list.Add(ParseArrayDefinition(expressionList.GetRange(index, count)));
						index += count;
						break;
					default:
						list.Add(Mapper.DTypeToStr(currentToken));
						break;
				}
			}
			
			return list.AsReadOnly();
		}
		
		static int GetStructLimit(List<DType> list, int start)
		{
			int count = -1;
			for (count = list.Count - 1; count > start; count--) {
				if (list[count] == DType.StructEnd)
					break;
			}
			return count - 1;
		}
		
		static int GetArrayLimit(List<DType> list, int start)
		{
			if (list[start] == DType.StructBegin)
				return GetStructLimit(list, start + 1) + 1;
			if (list[start] == DType.DictEntryBegin)
				return GetDictionaryLimit(list, start + 1);
			else
				// Basic type
				return 1;
		}
		
		static int GetDictionaryLimit(List<DType> list, int start)
		{
			int count = -1;
			for (count = list.Count - 1; count > start; count--) {
				if (list[count] == DType.DictEntryEnd)
					break;
			}
			return count;
		}
		
		static string ParseStructDefinition(List<DType> exprs)
		{
			string temp = "struct { ";
			foreach (string s in StrFromExprList(exprs)) {
				temp += s + "; ";
			}
			temp += "}";
			
			return temp;
		}
		
		static string ParseArrayDefinition(List<DType> exprs)
		{
			// In the case it's a dictionary
			if (exprs[0] == DType.DictEntryBegin)
				return ParseDictDefinition(exprs.GetRange(1, exprs.Count - 1));
			
			string tmp = StrFromExprList(exprs)[0];
			tmp += "[]";
			
			return tmp;
		}
		
		static string ParseDictDefinition(List<DType> exprList)
		{
			IList<string> list = StrFromExprList(exprList);
			return "Dictionary<" + list[0] + ", " + list[1] + ">";
		}
	}
	
	public enum DType : byte
	{
		Invalid = (byte)'\0',

		Byte = (byte)'y',
		Boolean = (byte)'b',
		Int16 = (byte)'n',
		UInt16 = (byte)'q',
		Int32 = (byte)'i',
		UInt32 = (byte)'u',
		Int64 = (byte)'x',
		UInt64 = (byte)'t',
		Single = (byte)'f', //This is not yet supported!
		Double = (byte)'d',
		String = (byte)'s',
		ObjectPath = (byte)'o',
		Signature = (byte)'g',

		Array = (byte)'a',
		Variant = (byte)'v',

		StructBegin = (byte)'(',
		StructEnd = (byte)')',
		DictEntryBegin = (byte)'{',
		DictEntryEnd = (byte)'}',
	}
}
