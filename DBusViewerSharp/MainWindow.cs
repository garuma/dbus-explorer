// MainWindow.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Collections.Generic;
using System.Threading;

using Gtk;

namespace DBusExplorer
{
	public partial class MainWindow: Gtk.Window
	{	
		DBusExplorator explorator;
	
		TreeStore model = new TreeStore(typeof(string), typeof(Gdk.Pixbuf));
	
		Dictionary<string, IElement> currentData = new Dictionary<string,IElement>(); 
	
		static Gdk.Pixbuf empty = Gdk.Pixbuf.LoadFromResource("empty.png");
		ImageAnimation spinner;
		Entry sentry = new Entry();
	
		public MainWindow (DBusExplorator explorator): base (Gtk.WindowType.Toplevel)
		{
			this.explorator = explorator;
			this.explorator.DBusError += OnDBusError;
		
			// Graphical setup
			Build ();
			this.DeleteEvent += OnDeleteEvent;
			this.tv.CursorChanged += OnRowSelected;
			tv.Selection.Mode = SelectionMode.Browse;
		
			this.tv.SearchColumn = 0;
			this.tv.SearchEqualFunc = delegate (TreeModel model, int column, string key, TreeIter iter) {
				string row;
				try {
					row = (string)model.GetValue(iter, 0);
				} catch { return false; }
				return !(row.Equals(key, StringComparison.OrdinalIgnoreCase));
			};
		
			this.model.SetSortFunc(0, delegate (TreeModel model, TreeIter tia, TreeIter tib) {
			
				string key1 = (string)model.GetValue(tia, 0);
				string key2 = (string)model.GetValue(tib, 0);
				IElement elem1;
				IElement elem2;
			
				if (!currentData.TryGetValue(key1, out elem1))
					return string.CompareOrdinal(key1, key2);
				if (!currentData.TryGetValue(key2, out elem2))
					return string.CompareOrdinal(key1, key2);
			
				return elem1.CompareTo(elem2);			
			});
		
		
			// Spinner
			spinner = new ImageAnimation(
				Gdk.Pixbuf.LoadFromResource("process-working.png"),
				40, 32, 32, 32);
			spinner.Active = false;
			spinnerAlign.Add(spinner);
			spinnerBox.HideAll();
		
			sentry.Activated += delegate {
			
			};
		
			// Graphical elements
			FeedBusComboBox(explorator.AvalaibleBusNames);
			InitTreeView();
		}
	
		void FeedBusComboBox(string[] buses)
		{
			ComboBox cb = ComboBox.NewText();
		
			foreach (string s in buses) {
				if (string.IsNullOrEmpty(s))
					continue;
				cb.AppendText(s);
			}
			busCbCont.Remove(busCb);
			busCb = cb;
			busCbCont.Add(busCb);
			busCb.Changed += OnBusComboChanged;
			busCbCont.ShowAll();
		}
	
		void InitTreeView()
		{
			tv.Model = model;
			TreeViewColumn col = tv.AppendColumn("Name", new CellRendererText(), "text", 0);
			tv.AppendColumn(" ", new CellRendererPixbuf(), "pixbuf", 1);
		
			col.Clickable = true;
			col.Clicked += OnColumnLblClicked;
		}

	
		void UpdateTreeView (string busName)
		{
			//Console.WriteLine("Update the treeview");
			model.Clear();
			currentData.Clear();
		
			spinnerBox.ShowAll();
			spinner.Active = true;
		
			explorator.BeginGetElementsFromBus(busName, delegate (IAsyncResult result) {
				IEnumerable<PathContainer> elements = explorator.EndGetElementsFromBus(result);
				Application.Invoke(delegate {
					foreach (PathContainer path in elements) {
						//Console.WriteLine("Adding ElementsEntry");
						AddPath(path);
					}
				
					spinnerBox.HideAll();
					spinner.Active = false;
				});
			});
			/*foreach (PathContainer path in explorator.GetElementsFromBus(busName)) {
				//Console.WriteLine("Adding ElementsEntry");
				AddPath(path);
			}*/
		}
	
		void AddPath(PathContainer path)
		{
			if (path == null)
				return;
			/*if (path.Interfaces.Length == 0)
				return;*/
				
			TreeIter parent = model.AppendValues(path.Path, empty);
			foreach (Interface @interface in path.Interfaces) {
				AddInterface (parent, @interface);
			}
		}
	
		void AddInterface (TreeIter parent, Interface element)
		{
			if (element == null)
				return;
		
			TreeIter child = model.AppendValues(parent, element.Name, empty);
		
			AddChildSymbols(child, element);
		}
	
		void AddChildSymbols (TreeIter parent, Interface element)
		{
			foreach (IElement entry in element.Symbols) {
				if (string.IsNullOrEmpty(element.Name) || entry.Image == null)
					continue;
			
				model.AppendValues(parent, entry.Name, entry.Image);
				try {
					currentData.Add(entry.Name, entry);
				} catch {}	
			}
		}
	
		void FillBottom (string key)
		{
			IElement element;
			if (!currentData.TryGetValue(key, out element))
				return;
		
			ElementRepresentation representation = element.Visual;
			informationLabel.Text = key;
			symbolImage.Pixbuf = element.Image;
			specstyleDecl.Markup = "<b><tt>" + representation.SpecDesc + "</tt></b>";
			cstyleDecl.Markup = "<tt>" + representation.CStyle.Replace("<", "&lt;") + "</tt>";
		}
		
		void ReinitBus (DBusExplorator exp)
		{
			model.Clear();
			this.explorator = exp;
			this.explorator.DBusError += OnDBusError;
			FeedBusComboBox(this.explorator.AvalaibleBusNames);
		}
	
		protected void OnDeleteEvent (object sender, EventArgs a)
		{
			Application.Quit ();
		}

		protected virtual void OnBusComboChanged (object sender, System.EventArgs e)
		{
			UpdateTreeView(busCb.ActiveText);
		}

		protected virtual void OnRowSelected (object o, EventArgs args)
		{
			TreeIter tIter;
			TreeSelection selection = tv.Selection;
			if (selection.CountSelectedRows() == 0)
				return;
			selection.GetSelected(out tIter);
		
			string valSelected = null;
			try {
				valSelected = (string)model.GetValue(tIter, 0);
			} catch { return; }
			//Console.WriteLine("Running FillBottom with : " + valSelected);
			FillBottom(valSelected);
		}

		protected virtual void OnSessionBusActivated (object sender, System.EventArgs e)
		{
			ReinitBus(new DBusExplorator());
		}

		protected virtual void OnSystemBusActivated (object sender, System.EventArgs e)
		{
			ReinitBus(new DBusExplorator(NDesk.DBus.Bus.System));			
		}
	
		protected virtual void OnAboutActivated (object sender, System.EventArgs e)
		{
			AboutDialog ad = new AboutDialog();
			ad.Authors = new string[] { "Jérémie \"Garuma\" Laval" };
			ad.Copyright = "Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>";
			ad.License = "See the COPYING file";
			ad.Version = "0.3";
		
			ad.Run();
			ad.Destroy();
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
	
		void OnDBusError(object sender, DBusErrorEventArgs e)
		{
			Application.Invoke( delegate {
				spinnerBox.HideAll();
				spinner.Active = false;
				MessageDialog diag = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, e.ErrorMessage);
				diag.Run();
				diag.Destroy();
			});
		}

		protected virtual void OnCustomBusActivated (object sender, System.EventArgs e)
		{
			CustomBusDialog d = new CustomBusDialog();
			d.ShowAll();
			if (d.Run() == (int)ResponseType.Ok) {
				if (!string.IsNullOrEmpty(d.RequestedBus))
					ReinitBus(new DBusExplorator(NDesk.DBus.Bus.Open(d.RequestedBus)));
			}
			d.Destroy();
		}

		protected virtual void OnMonitorActivated (object sender, System.EventArgs e)
		{
			MessageDialog d = 
				new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Sorry, this feature is currently not finalized.");
			d.Run();
			d.Destroy();
			/*
			DBusMonitor d = new DBusMonitor(explorator.BusUsed, NDesk.DBus.ObjectPath.Root);
			d.ShowAll();
			d.Run();
			d.Destroy();*/
		}
	}
}