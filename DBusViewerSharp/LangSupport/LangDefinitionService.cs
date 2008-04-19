// LangDefinitionService.cs created with MonoDevelop
// User: jeremie at 19:24 18/04/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DBusExplorer
{
	public static class LangDefinitionService
	{
		static LangDefinitionPool defaultPool = new LangDefinitionPool(
		    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".dbus-explorer"));
		
		public static LangDefinitionPool DefaultPool {
			get {
				return defaultPool;
			}
		}
		
	}
}
