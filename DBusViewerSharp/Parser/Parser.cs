// Parser.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Text;
using System.Collections.Generic;

namespace DBusViewerSharp
{
	public static class Parser
	{
		public static string ParseDBusTypeExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
				return expression;
			// Assume it's a base type and thus directly return the corresponding type
			if (expression.Length == 1)
				return Mapper.DTypeToType((DType)(byte)expression[0]);
			
			List<DType> expressionList = new List<DType>();
			
			foreach (char c in expression) {
				expressionList.Add((DType)(byte)c);
			}
			
			return StrFromExprList(expressionList)[0];
		}
		
		static string[] StrFromExprList(List<DType> expressionList)
		{
			List<string> list = new List<string>();
			int index = 0;
			
			foreach (DType currentToken in expressionList) {
				if (currentToken == DType.Variant || currentToken == DType.DictEntryEnd || currentToken == DType.StructEnd
				    || currentToken == DType.DictEntry || currentToken == DType.Invalid || currentToken == DType.ObjectPath
				    || currentToken == DType.Signature)
					continue;
				
				switch (currentToken) {
					case DType.StructBegin:
						list.Add(ParseStructDefinition(expressionList.GetRange(index, expressionList.Count - index)));
						break;
					case DType.DictEntryBegin:
						list.Add(ParseDictDefinition(expressionList.GetRange(index, expressionList.Count - index)));
						break;
					case DType.Array:
						list.Add(ParseArrayDefinition(expressionList.GetRange(index, expressionList.Count - index)));
						break;
					default:
						list.Add(Mapper.DTypeToType(currentToken).Name);
						break;
				}
				index++;
			}
			
			return list.ToArray();
		}
		
		static string ParseStructDefinition(List<DType> exprs)
		{
			Console.WriteLine("Parsing struct");
			
			string temp = "struct { ";
			int count;
			int start = exprs[0] == DType.StructBegin ? 1 : 0;
			
			for (count = exprs.Count - 1; count > 0; count--) {
				if (exprs[count] == DType.StructEnd)
					break;
			}
			
			foreach (string s in StrFromExprList(exprs.GetRange(start, count + 1))) {
				temp += s + "; ";
			}
			temp += "}";
			
			return temp;
		}
		
		static string ParseArrayDefinition(List<DType> exprs)
		{
			string tmp = StrFromExprList(exprs.GetRange(1, exprs.Count - 1))[0];
			tmp += "[]";
			
			return tmp;
		}
		
		static string ParseDictDefinition(List<DType> exprList)
		{
			return string.Empty;
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
		//TODO: remove Struct and DictEntry -- they are not relevant to wire protocol
		Struct = (byte)'r',
		DictEntry = (byte)'e',
		Variant = (byte)'v',

		StructBegin = (byte)'(',
		StructEnd = (byte)')',
		DictEntryBegin = (byte)'{',
		DictEntryEnd = (byte)'}',
	}
}
