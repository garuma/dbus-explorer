// BusContentView.cs
// Copyright (c) 2008-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using Gtk;
using Gdk;

namespace DBusExplorer
{
	public class ElementUpdatedEventArgs: EventArgs
	{
		IElement element;
		
		public IElement Element {
			get {
				return element;
			}
		}
		
		public ElementUpdatedEventArgs(IElement element)
		{
			this.element = element;	
		}
	}
	
	public class BusContentView: TreeView
	{
		TreeStore model;
		static Gdk.Pixbuf empty = Gdk.Pixbuf.LoadFromResource("empty.png");
		public event EventHandler<ElementUpdatedEventArgs> ElementUpdated;
		
		public BusContentView()
		{
			this.Model = this.model = new TreeStore(typeof(string), typeof(Gdk.Pixbuf), typeof(object));
			CellRendererText textCell = new CellRendererText();
			TreeViewColumn col = this.AppendColumn("Name", textCell, "text", 0);
			this.AppendColumn(" ", new CellRendererPixbuf(), "pixbuf", 1);
		
			col.Clickable = true;
			col.Clicked += OnColumnLblClicked;
			
			this.Selection.Mode = SelectionMode.Browse;
		
			this.SearchColumn = 0;
			this.SearchEqualFunc = delegate (TreeModel mdl, int column, string key, TreeIter iter) {
				string row = mdl.GetValue(iter, 0) as string;
				
				return (row == null) ? false : !(row.StartsWith(key, StringComparison.OrdinalIgnoreCase));
			};
		
			this.model.SetSortFunc(0, delegate (TreeModel mdl, TreeIter tia, TreeIter tib) {
				IElement a = mdl.GetValue(tia, 2) as IElement;
				IElement b = mdl.GetValue(tib, 2) as IElement;
				
				if (b == null || a == null)
					return 0;
				else
					return a.CompareTo(b);
			});
			
			this.CursorChanged += OnRowSelected;
		}
		
		public void Reinitialize()
		{
			this.model.Clear();
		}
		
		public void AddPath(PathContainer path)
		{
			if (path == null)
				return;
				
			TreeIter parent = model.AppendValues(path.Path, empty, path);
			foreach (Interface @interface in path.Interfaces) {
				AddInterface (parent, @interface);
			}
		}
	
		void AddInterface (TreeIter parent, Interface element)
		{
			if (element == null)
				return;
		
			TreeIter child = model.AppendValues(parent, element.Name, empty, element);
		
			AddChildSymbols(child, element);
		}
	
		void AddChildSymbols (TreeIter parent, Interface element)
		{
			foreach (IElement entry in element.Symbols) {
				if (entry == null)
					continue;
				if (string.IsNullOrEmpty(element.Name) || entry.Image == null)
					continue;
			
				model.AppendValues(parent, entry.Name, entry.Image, entry);
			}
		}
		
		protected virtual void OnColumnLblClicked (object sender, EventArgs e)
		{
			TreeViewColumn col = sender as TreeViewColumn;
			if (col == null) 
				return;
			col.SortOrder = (col.SortIndicator && col.SortOrder == SortType.Ascending) ? SortType.Descending : SortType.Ascending;
			col.SortIndicator = true;
			this.model.SetSortColumnId(0, col.SortOrder);
		}
		
		protected virtual void OnRowSelected (object o, EventArgs args)
		{
			if (ElementUpdated == null)
				return;
			
			TreeIter tIter;
			TreeSelection selection = this.Selection;
			if (selection == null || (selection != null && selection.CountSelectedRows() == 0))
				return;
			selection.GetSelected(out tIter);
		
			IElement element = model.GetValue(tIter, 2) as IElement;

			if (element != null)
				ElementUpdated(this, new ElementUpdatedEventArgs(element));
		}
		
		protected override bool OnButtonReleaseEvent (EventButton evnt)
		{
			//right click
			if (evnt.Button == 3) {
				TreePath path;
				TreeIter iter;
		        if(!GetPathAtPos((int)evnt.X, (int)evnt.Y, out path))
		            return true;
				if (!model.GetIter(out iter, path))
					return true;
				object target = model.GetValue(iter, 2);
				GenerationMenuWidget menu = null;
				
				switch (path.Depth) {
				case 1:
					menu = new GenerationMenuWidget((PathContainer)target);
					break;
				case 2:
					menu = new GenerationMenuWidget((Interface)target);
					break;
				/*case 3:
					menu = new GenerationMenuWidget((IElement)target);
					break;*/
				}
				if (menu == null)
					return base.OnButtonReleaseEvent (evnt);
				
				menu.ShowAll();
				menu.Popup (null, null, null, 3, Gtk.Global.CurrentEventTime);
			}
			
			return base.OnButtonReleaseEvent (evnt);
		}

	}
}
