// Parser.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Text;
using System.Collections.Generic;

namespace DBusExplorer
{
	public static class Parser
	{
		static ParserNg<string> realParser = new ParserNg<string>();
		/*static IParserVisitor<string> visitor = new CSharpVisitor(realParser);*/
		
		/*public static string ParseDBusTypeExpression(string expression)
		{
			return realParser.ParseDBusTypeExpression(expression, visitor);
		}*/
		
		public static string ParseDBusTypeExpression(string expression, IParserVisitor<string> visitor)
		{
			return realParser.ParseDBusTypeExpression(expression, visitor);
		}
		
		public static ParserNg<string> RealParser {
			get {
				return realParser;
			}
		}
	}
}
