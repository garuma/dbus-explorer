// IGenerator.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.CodeDom.Compiler;

namespace DBusExplorer
{
	public interface IGenerator
	{
		void Generate(Interface @interface, TextWriter tw);
		void Generate(PathContainer path, TextWriter tw);
		void Generate(IElement element, TextWriter tw);
	}
}
