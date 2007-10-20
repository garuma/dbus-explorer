// ElementsEntry.cs created with MonoDevelop
// User: jeremie at 13:38 06/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

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
