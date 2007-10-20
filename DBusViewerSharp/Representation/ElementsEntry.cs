// ElementsEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusViewerSharp
{
	// Represent the interface node
	public class ElementsEntry
	{
		string path;
		IEntry[] symbols;
		
		public string Path {
			get {
				return path;
			}
		}

		public IEntry[] Symbols {
			get {
				return symbols;
			}
		}
		
		public ElementsEntry(string path, IEntry[] symbols)
		{
			this.path = path;
			this.symbols = symbols;
		}
	}
}
