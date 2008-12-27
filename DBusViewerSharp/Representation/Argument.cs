// Argument.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusExplorer
{
	public struct Argument
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
		
		public Argument(string type, string name)
		{
			this.name = name;
			this.type = type;
		}
	}
}
