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
		Menu menu;
		
		public LanguageWidget()
		{
			this.Build();
			foreach (KeyValuePair<string, ILangDefinition> tuple in LangDefinitionService.DefaultPool.Languages) {
				try {
					 AddLangage(tuple.Key, string.Empty);
				} catch {}
			}
		}
		
		public void AddLangage(string langKey, string prototype)
		{
			Label temp = new Label();
			temp.UseMarkup = true;
			temp.Markup = FormatPrototype(langKey, prototype);
			temp.Selectable = true;
			temp.Layout.Alignment = Pango.Alignment.Left;
			
			try {
				langs.Add(langKey, temp);
				langsVbox.PackEnd(temp, false, false, 0);
			} catch {}
		}
		
		public void UpdateLangage(string langKey, string newPrototype)
		{
			Label temp;
			if (!langs.TryGetValue(langKey, out temp))
				return;
			temp.Markup = FormatPrototype(langKey, newPrototype);
		}
		
		public void ToggleLangage(string langKey)
		{
			Label temp;
			if (!langs.TryGetValue(langKey, out temp))
				return;
			
			if (temp.Parent != null) langsVbox.Remove(temp); else { langsVbox.PackEnd(temp, false, false, 0); temp.ShowAll(); };
		}
		
		string FormatPrototype(string lang, string proto)
		{
			if (string.IsNullOrEmpty(proto))
				return string.Empty;
			return "<b>" + lang + " : </b><tt>" + proto.Replace("<", "&lt;") + "</tt>";
		}
	}
}
