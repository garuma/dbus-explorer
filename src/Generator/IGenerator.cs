// IGenerator.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace DBusExplorer
{
	public interface IGenerator
	{
		string Generate(Interface @interface);
		string Generate(PathContainer path);
		string Generate(IEnumerable<IElement> elements);
	}
}
