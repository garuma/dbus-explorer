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
		string LangageName { get; }
		IDictionary<string, string> LangageTypes { get; }
		
		string MethodFormating { get; }
		string EventFormating { get; }
		
	}
}
