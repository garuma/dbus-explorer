// LangDefVisitor.cs
// Copyright (c) 2008-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Linq;
using System.Collections.Generic;

namespace DBusExplorer
{
	public class LangDefVisitor: IParserVisitor<string>
	{
		ILangDefinition lang;
		IDictionary<DType, string> types;
		
		public LangDefVisitor(ILangDefinition lang)
		{
			this.lang = lang;
			this.types = lang.Types;
		}
		
		public string ParseStructDefinition(IEnumerable<string> exprs)
		{
			return exprs.Any () ? lang.StructFormat(exprs) : Error;
		}
		
		public string ParseArrayDefinition(string type)
		{
			return lang.ArrayFormat(string.IsNullOrEmpty(type) ? Error : type);
		}
		
		public string ParseDictDefinition(string type1, string type2)
		{
			return lang.DictionaryFormat(string.IsNullOrEmpty(type1) ? Error : type1,
			                             string.IsNullOrEmpty(type2) ? Error : type2);
		}
		
		public string ParseBaseTypeDefinition(DType type)
		{
			string value;
			if (!types.TryGetValue(type, out value))
				value = Error;
			return value;
		}
		
		public string Default {
			get {
				return string.Empty;
			}
		}
		
		public string Error {
			get {
				return "(Err)";
			}
		}
	}
}
