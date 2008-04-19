// ElementRepresentation.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	public delegate string LangProcesser(); 
	
	public class ElementRepresentation
	{
		string specDesc;
		IDictionary<string, LangProcesser> langs;

		public string SpecDesc {
			get {
				return specDesc;
			}
		}
		
		public string this[string language] {
			get {
				LangProcesser temp;
				if (!langs.TryGetValue(language, out temp))
					return "N/A";
				
				return temp();
			}
		}
		
		public IEnumerable<string> AllLanguage {
			get {
				return langs.Keys;
			}
		}
		
		public ElementRepresentation(string specDesc, IDictionary<string, LangProcesser> langs)
		{
			this.specDesc = specDesc;
			this.langs = langs;
		}
	}
}
