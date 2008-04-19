// EventFormat.cs
// Copyright (c) 2008 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	public struct EventFormat
	{
		string formating;
		
		public EventFormat(string formating)
		{
			this.formating = formating;
		}
		
		public string GetFormated(IEnumerable<string> types, string name)
		{
			return null;
		}
	}
}
