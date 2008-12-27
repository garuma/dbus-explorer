// LangageRepresentationViewer.cs
// Copyright (c) 2008 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using Gtk;

namespace DBusExplorer
{
	
	
	public partial class GenerationDialog : Gtk.Dialog
	{	
		public GenerationDialog()
		{
			this.Build();
		}
		
		public string PathToSave {
			get {
				return filechooserwidget1.Filename;
			}
		}
	}
}
