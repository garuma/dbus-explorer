// InformationView.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;

using Gtk;

namespace DBusExplorer
{
	
	public partial class InformationView : Gtk.Bin
	{
		LanguageWidget languageRepresentation = new LanguageWidget();
		EventBox evt = new EventBox();
		Menu menu;
		
		public InformationView()
		{
			this.Build();
			this.langsPh.Add(evt);
			
			evt.Add(languageRepresentation);
			evt.ShowAll();
			
			languageRepresentation.ShowAll();
			menu = MakeMenu(LangDefinitionService.DefaultPool.Languages.Keys);
			evt.ButtonReleaseEvent += OnBtnReleaseEvent;
			
		}
		
		Menu MakeMenu(IEnumerable<string> defs)
		{
			Menu tmp = new Menu();
			tmp.Append(new MenuItem("Toggle language"));
			tmp.Append(new SeparatorMenuItem());
			foreach (string langDef in defs) {
				CheckMenuItem cmi = new CheckMenuItem(langDef);
				cmi.Activated += delegate { languageRepresentation.ToggleLangage(langDef); };
				tmp.Append(cmi);
			}
			
			return tmp;
		}
		
		public void FillBottom (IElement element)
		{
			//Console.WriteLine("FillBottom called with " + element.Name);
			ElementRepresentation representation = element.Visual;
			informationLabel.Text = element.Name;
			symbolImage.Pixbuf = element.Image;
			specstyleDecl.Markup = "<b><tt>" + representation.SpecDesc + "</tt></b>";
			foreach (string lang in representation.AllLanguage) {
				languageRepresentation.UpdateLangage(lang, representation[lang]);
			}
		}
		
		protected void OnBtnReleaseEvent (object sender, ButtonReleaseEventArgs e)
		{
			Gdk.EventButton evnt = e.Event;
			//right click
			if (evnt.Button == 3) {
				menu.ShowAll();
				menu.Popup (null, null, null, 3, Gtk.Global.CurrentEventTime);
				// TODO: use Menu subclass which adapt to model.GetValue(iter) type (path, interface...)
				// and make the generation dialog
			}
		}
	}
}
