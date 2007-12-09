// ElementsEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	// Represent the interface node
	public class Interface
	{
		string name;
		IEnumerable<IElement> symbols;
		
		public string Name {
			get {
				return name;
			}
		}

		public IEnumerable<IElement> Symbols {
			get {
				return symbols;
			}
		}
		
		public Interface(string name, IEnumerable<IElement> symbols)
		{
			this.name = name;
			this.symbols = symbols;
		}
	}
}
