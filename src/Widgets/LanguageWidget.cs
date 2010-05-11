// LangageRepresentationViewer.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
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
		class LangTableChild
		{
			Label name;
			Label prototype;
			
			public LangTableChild(Label name, Label prototype)
			{
				this.name = name;
				this.prototype = prototype;
			}
			
			public void Show()
			{
				name.Show();
				prototype.Show();
			}
			
			public void Hide()
			{
				name.Hide();
				prototype.Hide();
			}
			
			public Label Name {
				get {
					return name;	
				}
			}
			
			public Label Prototype {
				get {
					return prototype;	
				}
			}
			
			public bool HasParent {
				get {
					return name.Parent != null && prototype.Parent != null;	
				}
			}
		}
		
		Dictionary<string, LangTableChild> langs = new Dictionary<string,LangTableChild>();
		
		public LanguageWidget()
		{
			this.Build();
			foreach (KeyValuePair<string, ILangDefinition> tuple in LangDefinitionService.DefaultPool.Languages) {
				try {
					 AddLangage(tuple.Key, string.Empty);
				} catch {}
			}
		}
		
		public void AddLangage(string langKey, string proto)
		{
			Label prototype = new Label();
			prototype.UseMarkup = true;
			prototype.Markup = FormatPrototype(proto);
			prototype.Selectable = true;
			prototype.Layout.Alignment = Pango.Alignment.Left;
			
			Label lang = new Label();
			lang.UseMarkup = true;
			lang.Markup = FormatName(langKey);
			lang.Selectable = false;
			lang.Layout.Alignment = Pango.Alignment.Left;
			
			LangTableChild child = new LangTableChild(lang, prototype);
			
			try {
				langs.Add(langKey, child);
				PackEnd(child);
			} catch {}
		}
		
		public void UpdateLangage(string langKey, string newPrototype)
		{
			LangTableChild temp;
			if (!langs.TryGetValue(langKey, out temp))
				return;
			temp.Prototype.Markup = FormatPrototype(newPrototype);
		}
		
		public void ToggleLangage(string langKey)
		{
			LangTableChild temp;
			if (!langs.TryGetValue(langKey, out temp))
				return;
			
			if (!temp.HasParent)
				temp.Hide();
			else
				temp.Show();
		}
		
		uint rowTrack = 0;
		
		void PackEnd(LangTableChild child)
		{
			uint row = rowTrack++;
			
			table.Attach(child.Name, (uint)0, (uint)1, row, row + 1, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);
			table.Attach(child.Prototype, (uint)1, (uint)2, row, row + 1, AttachOptions.Shrink, AttachOptions.Shrink, 0, 0);
		}
		
		string FormatPrototype(string proto)
		{
			if (string.IsNullOrEmpty(proto))
				return string.Empty;
			return "<tt>" + proto.Replace("<", "&lt;") + "</tt>";
		}
		
		string FormatName (string lang)
		{
			return "<b>" + lang + " : </b>";
		}
	}
}
