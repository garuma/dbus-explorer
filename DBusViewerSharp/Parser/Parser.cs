// Parser.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusViewerSharp
{
	// Yeah I know. Very stupid.
	public static class Parser
	{
		public static string ParseDBusTypeExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
				return expression;
			
			DType backType = DType.Invalid;
			DType frontType = DType.Invalid;
			
			switch (expression[0]) {
			case (char)DType.Array:
				frontType = DType.Array;
				break;
			default:
				frontType = DType.Invalid;
				break;
			}
			
			backType = (frontType == DType.Invalid) ? (DType)(byte)expression[0] : (DType)(byte)expression[1];
			Type temp = Mapper.DTypeToType(backType);
			string back = (temp == null) ? (frontType != DType.Invalid ? expression : expression.Substring(1)) : temp.Name;
			string result = (frontType == DType.Array) ? back + "[]" : back;
			
			return result;
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
