// InformationView.cs
// Copyright (c) 2008 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusExplorer
{
	
	public partial class InformationView : Gtk.Bin
	{
		LanguageWidget languageRepresentation;
		
		public InformationView()
		{
			this.Build();
			this.langsPh.Add(languageRepresentation);
		}
		
		public void FillBottom (IElement element)
		{
			ElementRepresentation representation = element.Visual;
			informationLabel.Text = element.Name;
			symbolImage.Pixbuf = element.Image;
			specstyleDecl.Markup = "<b><tt>" + representation.SpecDesc + "</tt></b>";
			foreach (string lang in representation.AllLanguage) {
				languageRepresentation.UpdateLangage(lang, representation[lang]);
			}
		}
	}
}
