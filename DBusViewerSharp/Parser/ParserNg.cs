// Parser.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Text;
using System.Collections.Generic;

namespace DBusExplorer
{
	public class ParserNg<TReturn>
	{
		
		public TReturn ParseDBusTypeExpression(string expression, IParserVisitor<TReturn> visitor)
		{
			if (string.IsNullOrEmpty(expression))
				return visitor.Default;
			// Assume it's a base type and thus directly return the corresponding type
			if (expression.Length == 1)
				return visitor.ParseBaseTypeDefinition((DType)(byte)expression[0]);
			
			List<DType> expressionList = new List<DType>(expression.Length);
			// Fill expressionList
			foreach (char c in expression) {
				expressionList.Add((DType)(byte)c);
			}
			
			TReturn temp;
			IEnumerator<DType> enumerator = expressionList.GetEnumerator();
			enumerator.MoveNext();
			try {
				temp = ConvertFromExprList(enumerator, visitor)[0];
			} catch {
				temp = visitor.Error;
			}
			return temp;
		}
		
		public IList<TReturn> ConvertFromExprList(IEnumerator<DType> tokens, IParserVisitor<TReturn> visitor)
		{
			return ConvertFromExprList(tokens, visitor, -1);
		}
		
		public IList<TReturn> ConvertFromExprList(IEnumerator<DType> tokens, IParserVisitor<TReturn> visitor,
		                                          int countdown)
		{
			List<TReturn> list = new List<TReturn>();
			
			bool arrayState = false;
			
			// This whole loop is hacky about countdown and tokens.MoveNext()
			// TODO: need refractoring
			do {
				DType currentToken;
				// Ugly but it's in the case the DBus type end with an array where tokens.MoveNext() returning false
				// can't be catched;
				try {
					currentToken = tokens.Current;
				} catch { break; }
				//if (!arrayState) Console.Write("\n" + currentToken.ToString());
				
				if (currentToken == DType.StructEnd) {
					break;
				}
				
				if (currentToken == DType.DictEntryEnd) {
					if (!tokens.MoveNext()) break;
					countdown++;
					continue;
				}
				
				// If precedent character was an array character determine if it's
				// a real array or a dictionary
				if (arrayState) {
					if (currentToken == DType.DictEntryBegin) {
						tokens.MoveNext();
						list.Add(visitor.ParseDictDefinition(tokens));
					} else {
						list.Add(visitor.ParseArrayDefinition(tokens));
					}
					
					arrayState = false;
					
					continue;
				}
				
				if (currentToken == DType.DictEntryBegin) {
					if (!tokens.MoveNext()) break;
					continue;
				}
				
				switch (currentToken) {
					case DType.StructBegin:
						tokens.MoveNext();
						list.Add(visitor.ParseStructDefinition(tokens));
						break;
					// In fact this case is both for array and dictionary
					case DType.Array:
						arrayState = true;
						countdown++;
						if (!tokens.MoveNext()) break;
						continue;
					default:
						list.Add(visitor.ParseBaseTypeDefinition(currentToken));
						break;
				}
				
				if (!tokens.MoveNext()) break;
			} while (--countdown != 0);
			
			return list.AsReadOnly();
		}
	}
	
	public enum DType : byte
	{
		Invalid = (byte)'\0',
		// Addition for use in DBus-Explorer
		// e like empty
		Void = (byte)'e',

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
