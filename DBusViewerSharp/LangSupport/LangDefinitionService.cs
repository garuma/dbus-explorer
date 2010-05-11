// LangDefinitionService.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
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
		static string defaultPath = GetDefaultPath ();
		
		static LangDefinitionService ()
		{
			InitLangDirectory ();
			
			defaultPool = new LangDefinitionPool (defaultPath);
		}
		
		public static LangDefinitionPool DefaultPool {
			get {
				return defaultPool;
			}
		}
		
		static string GetDefaultPath ()
		{
			return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
			                     ".dbus-explorer");
		}
		
		static void InitLangDirectory ()
		{
			char s = Path.DirectorySeparatorChar;
			
			string filePath = defaultPath + s + "langs" + s + "csharp.lang.xml";
			
			FileInfo fileInfo = new FileInfo (filePath);
			
			if (fileInfo.Exists)
				return;
			
			if (!fileInfo.Directory.Exists)
				fileInfo.Directory.Create ();
			
			StreamWriter sw = new StreamWriter (fileInfo.OpenWrite());
			Type t = typeof (LangDefinitionService);
			StreamReader sr = new StreamReader (t.Assembly.GetManifestResourceStream ("csharp.lang.xml"));
			sw.Write (sr.ReadToEnd ());
			
			sw.Dispose ();
			sr.Dispose ();
		}
	}
}
