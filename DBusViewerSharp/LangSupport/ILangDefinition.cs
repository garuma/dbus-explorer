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
		IDictionary<DType, string> Types { get; }
		
		// Careful, the string that represent type here have already been parsed somewhere before entering here.
		string MethodFormat(string name, string returnType, IEnumerable<Argument> args);
		string EventFormat(string name, IEnumerable<KeyValuePair<string, string>> args);
		string PropertyFormat();
		
		string DictionaryFormat(string type1, string type2);
		string StructFormat(IEnumerable<string> types);
		string ArrayFormat(string type);

	}
}
