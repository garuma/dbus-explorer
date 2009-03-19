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
		ParserNg<string> parent;
		ILangDefinition lang;
		IDictionary<DType, string> types;
		
		public LangDefVisitor(ILangDefinition lang, ParserNg<string> parent)
		{
			this.lang = lang;
			this.parent = parent;
			this.types = lang.Types;
		}
		
		public string ParseStructDefinition(IEnumerator<DType> exprs)
		{
			IList<string> tmp = parent.ConvertFromExprList(exprs, this);

			return tmp.Count > 0 ? lang.StructFormat(tmp) : Error;
		}
		
		public string ParseStructDefinition(IEnumerable<string> exprs)
		{
			return exprs.Any () ? lang.StructFormat(exprs) : Error;
		}
		
		public string ParseArrayDefinition(IEnumerator<DType> exprs)
		{
			IList<string> tmp = parent.ConvertFromExprList(exprs, this, 1);
			
			return lang.ArrayFormat(tmp.Count > 0 ? tmp[0] : Error);
		}
		
		public string ParseArrayDefinition(string type)
		{
			return lang.ArrayFormat(string.IsNullOrEmpty(type) ? Error : type);
		}
		
		public string ParseDictDefinition(IEnumerator<DType> exprList)
		{
			IList<string> list = parent.ConvertFromExprList(exprList, this, 2);
			
			return lang.DictionaryFormat((list.Count > 0) ? list[0] : Error, (list.Count > 1) ? list[1] : Error);
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
