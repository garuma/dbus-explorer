// ParameterEntry.cs created with MonoDevelop
// User: jeremie at 20:45 06/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

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
