// Parser.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DBusExplorer
{
	public class Parser
	{
		SExp entryPoint;
		
		public Parser(string path)
		{
			Parse(File.ReadAllText(path));
		}
		
		void Parse(string content)
		{
			
		}
		
		public SExp EntryPoint {
			get {
				return entryPoint;
			}
		}
	}
}
