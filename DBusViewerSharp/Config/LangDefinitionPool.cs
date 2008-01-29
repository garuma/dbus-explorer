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
		
		public LangDefinitionPool(string basePath)
		{
			foreach (string file in Directory.GetFiles(basePath)) {
				ILangDefinition def = LangParser.ParseFromFile(file);
				langs.Add(def.Name, def);
			}
		}
		
		
	}
}
