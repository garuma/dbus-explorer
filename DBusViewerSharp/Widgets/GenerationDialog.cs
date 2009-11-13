// LangageRepresentationViewer.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;
using Gtk;

namespace DBusExplorer
{
	
	
	public partial class GenerationDialog : Gtk.Dialog
	{	
		List<IElement> elements;
		ListStore model;
		
		public GenerationDialog(Window parent, Interface referer)
			: base (referer.Name, parent, DialogFlags.Modal | DialogFlags.DestroyWithParent)
		{
			this.TransientFor = parent;
			this.Build();
			
			elements = new List<IElement> (referer.Symbols);
			model = new ListStore (typeof (bool), typeof (string), typeof (IElement));
			selectionTv.Model = model;
			
			SetupFileWidget ();
			SetupModel (referer);
			SetupTreeview ();			
		}
		
		void SetupFileWidget ()
		{
			FileFilter filter = new FileFilter();
			filter.AddPattern ("*.cs");
			filter.Name = "C# source file";
			filechooserwidget1.AddFilter (filter);
			filechooserwidget1.SetCurrentFolder (Environment.GetFolderPath (Environment.SpecialFolder.Personal));			
		}
		
		void SetupModel (Interface referer)
		{
			foreach (IElement elem in referer.Symbols) {
				model.AppendValues (true, elem.Name, elem);
			}
		}
		
		void SetupTreeview ()
		{
			CellRendererToggle crToggle = new CellRendererToggle ();
			crToggle.Activatable = true;
			crToggle.Toggled += delegate(object o, ToggledArgs args) {
				TreeIter iter;
				if (!model.GetIterFromString (out iter, args.Path)) {
					return;
				}
				IElement e = (IElement)model.GetValue (iter, 2);
				bool oldValue = (bool)model.GetValue (iter, 0);
				
				if (oldValue) {
					elements.Remove (e);
				} else {
					elements.Add (e);
				}
				
				model.SetValue (iter, 0, !oldValue);
			};
			selectionTv.AppendColumn (string.Empty, crToggle, "active", 0);
			selectionTv.AppendColumn ("Name", new CellRendererText (), "text", 1);
		}
		
		public IEnumerable<IElement> Selected {
			get {
				return elements.AsReadOnly ();
			}
		}
		
		public string PathToSave {
			get {
				return filechooserwidget1.Filename;
			}
		}
	}
}
