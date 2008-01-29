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
	
		TreeStore model = new TreeStore(typeof(string), typeof(Gdk.Pixbuf), typeof(object)); 
		BusContentView tv;
	
		ImageAnimation spinner;
		Entry sentry = new Entry();
	
		public MainWindow (DBusExplorator explorator): base (Gtk.WindowType.Toplevel)
		{
			this.explorator = explorator;
			this.explorator.DBusError += OnDBusError;
			this.tv = new BusContentView(model);
		
			// Graphical setup
			Build ();
			this.tvWindow.Add(tv);
			this.tvWindow.ShowAll();
			
			this.DeleteEvent += OnDeleteEvent;
			this.tv.CursorChanged += OnRowSelected;
		
			// Spinner
			spinner = new ImageAnimation(
				Gdk.Pixbuf.LoadFromResource("process-working.png"),
				40, 32, 32, 32);
			spinner.Active = false;
			spinnerAlign.Add(spinner);
			spinnerBox.HideAll();
			
		
			// Graphical elements
			FeedBusComboBox(explorator.AvalaibleBusNames);
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

		void UpdateView (string busName)
		{
			model.Clear();
		
			spinnerBox.ShowAll();
			spinner.Active = true;
		
			explorator.BeginGetElementsFromBus(busName, delegate (IAsyncResult result) {
				IEnumerable<PathContainer> elements = explorator.EndGetElementsFromBus(result);
				Application.Invoke(delegate {
					foreach (PathContainer path in elements) {
						tv.AddPath(path);
					}
				
					spinnerBox.HideAll();
					spinner.Active = false;
				});
			});
		}
	
		void FillBottom (IElement element)
		{
			ElementRepresentation representation = element.Visual;
			informationLabel.Text = element.Name;
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
			UpdateView(busCb.ActiveText);
		}

		protected virtual void OnRowSelected (object o, EventArgs args)
		{
			TreeIter tIter;
			TreeSelection selection = tv.Selection;
			if (selection == null || (selection != null && selection.CountSelectedRows() == 0))
				return;
			selection.GetSelected(out tIter);
		
			IElement element = model.GetValue(tIter, 2) as IElement;

			if (element != null)
				FillBottom(element);
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