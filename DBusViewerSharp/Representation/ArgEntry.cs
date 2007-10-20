// ArgEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusViewerSharp
{
	public struct ArgEntry
	{
		string name;
		string type;
		
		public string Type {
			get {
				return type;
			}
		}

		public string Name {
			get {
				return name;
			}
		}
		
		public ArgEntry(string type, string name)
		{
			this.name = name;
			this.type = type;
		}
	}
}
