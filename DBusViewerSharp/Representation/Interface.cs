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
		PathContainer parent;
		
		public string Name {
			get {
				return name;
			}
		}
		
		public PathContainer Parent {
			get {
				return parent;
			}
			set {
				parent = value;
			}
		}

		public IEnumerable<IElement> Symbols {
			get {
				return symbols;
			}
		}
		
		public Interface(string name, IEnumerable<IElement> symbols)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (symbols == null)
				throw new ArgumentNullException("symbols");
			
			this.name = name;
			this.symbols = symbols;
			foreach (IElement element in symbols)
				element.Parent = this;
		}
	}
}
