// ILangDefinition.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	public interface ILangDefinition
	{
		string Name { get; }
		IDictionary<string, string> Types { get; }
		
		string MethodFormat(string name, string returnType, IEnumerable<KeyValuePair<string, string>> args);
		string EventFormat(string name, IEnumerable<KeyValuePair<string, string>> args);
		string PropertyFormat();
	}
}
