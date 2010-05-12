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
		Func<IEnumerable<IElement>, string> renderer;
		List<IElement> elements;
		ListStore model;
		int selectedCount;
		
		public GenerationDialog(Window parent, Interface referer, Func<IEnumerable<IElement>, string> renderer)
			: base (referer.Name, parent, DialogFlags.Modal | DialogFlags.DestroyWithParent)
		{
			this.TransientFor = parent;
			this.Build();
			
			this.renderer = renderer;
			elements = new List<IElement> (referer.Symbols);
			model = new ListStore (typeof (bool), typeof (string), typeof (IElement));
			selectionTv.Model = model;
			
			SetupModel (referer);
			SetupTreeview ();
		}
		
		void SetupModel (Interface referer)
		{
			foreach (IElement elem in referer.Symbols) {
				model.AppendValues (true, elem.Name, elem);
			}
			
			selectedCount = model.IterNChildren ();
			countLabel.Text = selectedCount.ToString ();
			codeTextView.Buffer.Text = renderer (elements);
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
					countLabel.Text = (--selectedCount).ToString ();
					elements.Remove (e);
				} else {
					countLabel.Text = (++selectedCount).ToString ();
					elements.Add (e);
				}
				
				model.SetValue (iter, 0, !oldValue);
				codeTextView.Buffer.Text = renderer (elements);
			};
			selectionTv.AppendColumn (string.Empty, crToggle, "active", 0);
			selectionTv.AppendColumn ("Name", new CellRendererText (), "text", 1);
		}
		
		protected virtual void OnSelectAllButtonClicked (object sender, System.EventArgs e)
		{
			SelectChange (true);
		}
		
		protected virtual void OnUnselectAllButtonClicked (object sender, System.EventArgs e)
		{
			SelectChange (false);
		}
		
		void SelectChange (bool setSelect)
		{
			model.Foreach ((m, p, i) => {
				bool oldValue = (bool)model.GetValue (i, 0);
				
				if (oldValue && !setSelect) {
					model.SetValue (i, 0, false);
					--selectedCount;
				} else if (!oldValue && setSelect) {
					model.SetValue (i, 0, true);
					++selectedCount;
				}
			
				return false;
			});
			
			countLabel.Text = selectedCount.ToString ();
		}	
		
		protected virtual void OnClipboardCopyClicked (object sender, System.EventArgs e)
		{
			Clipboard clipboard = Clipboard.Get (Gdk.Atom.Intern ("CLIPBOARD", true));
			if (clipboard == null)
				return;
			
			clipboard.Text = codeTextView.Buffer.Text;
		}
	}
}
