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
		void Generate(Interface @interface, string path);
		void Generate(PathContainer path, string file_path);
		void Generate(IElement element, string path);
	}
}
