// Parser.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Text;
using System.Linq;
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
			
			TReturn temp;
			IEnumerable<DType> expressionList = expression.Select((c) => (DType)(byte)c);
			IEnumerator<DType> enumerator = expressionList.GetEnumerator();
			enumerator.MoveNext();
			try {
				temp = Parse(enumerator, visitor).First ();
			} catch {
				temp = visitor.Error;
			}
			return temp;
		}
		
		public IEnumerable<TReturn> Parse (IEnumerator<DType> tokens, IParserVisitor<TReturn> visitor)
		{
			DType current = tokens.Current;
			
			if (current == DType.DictEntryEnd || current == DType.StructEnd) {
				tokens.MoveNext ();
				return null;
			}
			
			// May be an array or a dict
			if (current == DType.Array) {
				if (!tokens.MoveNext ())
					throw new ParseException ("An array type is malformed");
				
				if (tokens.Current == DType.DictEntryBegin)
					return Wrap(ParseDict(tokens, visitor));
				else
					return Wrap(visitor.ParseArrayDefinition(Parse(tokens, visitor).First()));
			}
			
			if (current == DType.StructBegin) {
				if (!tokens.MoveNext ())
					throw new ParseException ("A structure is malformed");
				
				return Wrap(ParseStruct (tokens, visitor));
			}
			
			IEnumerable<TReturn> result = Wrap(visitor.ParseBaseTypeDefinition (current));
			tokens.MoveNext ();
			
			return result;
		}
		
		TReturn ParseDict (IEnumerator<DType> tokens, IParserVisitor<TReturn> visitor)
		{
			if (!tokens.MoveNext())
				throw new ParseException("A dictionary type is malformed");
			
			IEnumerable<TReturn> type1 = Parse(tokens, visitor);
			IEnumerable<TReturn> type2 = Parse(tokens, visitor);
			tokens.MoveNext ();
			
			return visitor.ParseDictDefinition (type1.First(), type2.First());
		}
		
		TReturn ParseStruct (IEnumerator<DType> tokens, IParserVisitor<TReturn> visitor)
		{
			IEnumerable<TReturn> result = Parse (tokens, visitor);
			IEnumerable<TReturn> temp = null;
			
			while ((temp = Parse (tokens, visitor)) != null) {
				result = Enumerable.Concat (result, temp);
			}
			
			return visitor.ParseStructDefinition (result);
		}
		
		IEnumerable<TReturn> Wrap (TReturn foo)
		{
			return Enumerable.Repeat(foo, 1);
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
	
	class ParseException: Exception
	{	
		public ParseException(string message): base(message)
		{
		}
	}
}
