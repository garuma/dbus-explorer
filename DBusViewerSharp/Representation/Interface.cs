// ElementsEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusViewerSharp
{
	// Represent the interface node
	public class Interface
	{
		string name;
		IElement[] symbols;
		
		public string Name {
			get {
				return name;
			}
		}

		public IElement[] Symbols {
			get {
				return symbols;
			}
		}
		
		public Interface(string name, IElement[] symbols)
		{
			this.name = name;
			this.symbols = symbols;
		}
	}
}
