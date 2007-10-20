// IEntry.cs created with MonoDevelop
// User: jeremie at 21:25 06/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

using Gtk;

namespace DBusViewerSharp
{
	public interface IEntry
	{
		ElementRepresentation Visual { get; }
		string Name { get; }
	}
}
