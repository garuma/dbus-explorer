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
		static ParserNg<string> realParser = new ParserNg<string>();
		static IParserVisitor<string> visitor = new CSharpVisitor(realParser);
		
		
		
		public static string ParseDBusTypeExpression(string expression)
		{
			return realParser.ParseDBusTypeExpression(expression, visitor);
		}
	}
	
}
