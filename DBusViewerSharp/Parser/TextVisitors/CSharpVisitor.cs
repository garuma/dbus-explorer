// CSharpVisitor.cs
// Copyright (c) 2008 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	public class CSharpVisitor: IParserVisitor<string>
	{
		ParserNg<string> parent;
		
		public CSharpVisitor(ParserNg<string> parent)
		{
			this.parent = parent;
		}
		
		public string ParseStructDefinition(IEnumerator<DType> exprs)
		{
			string temp = "struct { ";
			foreach (string s in parent.ConvertFromExprList(exprs, this)) {
				temp += s + "; ";
			}
			temp += "}";
			
			return temp;
		}
		
		public string ParseArrayDefinition(IEnumerator<DType> exprs)
		{
			string tmp = parent.ConvertFromExprList(exprs, this, 1)[0];
			tmp += "[]";
			
			return tmp;
		}
		
		public string ParseDictDefinition(IEnumerator<DType> exprList)
		{
			IList<string> list = parent.ConvertFromExprList(exprList, this, 2);
			return "Dictionary<" + list[0] + ", " + list[1] + ">";
		}
		
		public string ParseBaseTypeDefinition(DType type)
		{
			return Mapper.DTypeToStr(type);
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
