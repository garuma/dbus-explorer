// LangDefinitionPool.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.Collections.Generic;

namespace DBusExplorer
{
	public class LangDefinitionPool
	{
		Dictionary<string, ILangDefinition> langs = new Dictionary<string,ILangDefinition>();
		
		public Dictionary<string, ILangDefinition> Languages {
			get {
				return langs;
			}
		}
		
		public LangDefinitionPool(params string[] basePaths)
		{
			foreach (string bPath in basePaths) {
				string path = System.IO.Path.Combine(bPath, "langs");
				foreach (string file in Directory.GetFiles(path, "*.lang.xml")) {
					ILangDefinition def = LangParser.ParseFromFile(file);
					langs.Add(def.Name, def);
				}
			}
		}
		
		public void Add(ILangDefinition def)
		{
			if (def == null)
				throw new ArgumentNullException("def");
			try {
				langs.Add(def.Name, def);	
			} catch {}
		}
	}
}
