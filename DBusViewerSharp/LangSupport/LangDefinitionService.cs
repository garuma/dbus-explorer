// LangDefinitionService.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
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
