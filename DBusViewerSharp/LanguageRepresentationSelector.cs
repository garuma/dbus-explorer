// LanguageRepresentationSelector.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using Gtk;

namespace DBusExplorer
{
	
	
	public partial class LanguageRepresentationSelector : Gtk.Dialog
	{
		ListStore model = new ListStore(typeof(string), typeof(RadioButton));
		
		public LanguageRepresentationSelector()
		{
			this.Build();
		}
	}
}
