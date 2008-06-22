// LangDefinitionService.cs created with MonoDevelop
// User: jeremie at 19:24 18/04/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;

namespace DBusExplorer
{
	public static class LangDefinitionService
	{
		static LangDefinitionPool defaultPool;
		
		static LangDefinitionService()
		{
			char s = Path.DirectorySeparatorChar;
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + s +
				".dbus-explorer";
			string path2 = path + s + "langs" + s + "csharp.lang.xml";
			FileInfo fileInfo = new FileInfo(path2);
			if (!fileInfo.Exists) {
				if (!fileInfo.Directory.Exists)
					fileInfo.Directory.Create();
				StreamWriter sw = new StreamWriter(fileInfo.OpenWrite());
				Type t = typeof(LangDefinitionService);
				StreamReader sr = new StreamReader(t.Assembly.GetManifestResourceStream("csharp.lang.xml"));
				sw.Write(sr.ReadToEnd());
				sw.Dispose();
				sr.Dispose();
			}
			
			defaultPool = new LangDefinitionPool(path);
		}
		
		public static LangDefinitionPool DefaultPool {
			get {
				return defaultPool;
			}
		}
		
	}
}
