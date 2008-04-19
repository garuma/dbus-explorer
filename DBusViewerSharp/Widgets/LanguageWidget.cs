// LangageRepresentationViewer.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;

using Gtk;
using Gdk;

namespace DBusExplorer
{
	// TODO: How hard would it be to use GtkSourceView2 theme for beautyfying the prototypes ?
	public partial class LanguageWidget : Gtk.Bin
	{
		Dictionary<string, Label> langs = new Dictionary<string,Label>();
		Menu rightMenu;
		
		public LanguageWidget()
		{
			this.Build();
			foreach (KeyValuePair<string, ILangDefinition> tuple in LangDefinitionService.DefaultPool.Languages) {
				try {
					 AddLangage(tuple.Key, string.Empty);
				} catch {}
			}
			rightMenu = MakeMenu(LangDefinitionService.DefaultPool.Languages.Keys);
		}
		
		Menu MakeMenu(IEnumerable<string> defs)
		{
			Menu tmp = new Menu();
			foreach (string langDef in defs) {
				CheckMenuItem cmi = new CheckMenuItem(langDef);
				cmi.Activated += delegate { ToggleLangage(langDef); };
				tmp.Append(cmi);
			}
			return tmp;
		}
		
		public void AddLangage(string langKey, string prototype)
		{
			Label temp = new Label();
			temp.UseMarkup = true;
			temp.Markup = FormatPrototype(prototype);
			
			try {
				langs.Add(langKey, temp);
				langsVbox.Add(temp);
			} catch {}
		}
		
		public void UpdateLangage(string langKey, string newPrototype)
		{
			Label temp;
			if (!langs.TryGetValue(langKey, out temp))
				return;
			temp.Markup = FormatPrototype(newPrototype);
		}
		
		public void ToggleLangage(string langKey)
		{
			Label temp;
			if (!langs.TryGetValue(langKey, out temp))
				return;
			
			if (temp.Parent != null) langsVbox.Remove(temp); else { langsVbox.PackEnd(temp); temp.ShowAll(); };
		}
		
		string FormatPrototype(string proto)
		{
			if (string.IsNullOrEmpty(proto))
				return string.Empty;
			return "<tt>" + proto.Replace("<", "&lt;") + "</tt>";
		}
		
		protected override bool OnButtonReleaseEvent (EventButton evnt)
		{
			//right click
			if (evnt.Button == 3) {
				
				Console.WriteLine("Rightclicked");
				// TODO: use Menu subclass which adapt to model.GetValue(iter) type (path, interface...)
				// and make the generation dialog
			}
			
			return base.OnButtonReleaseEvent (evnt);
		}
	}
}
