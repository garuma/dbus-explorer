// MainWindow.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.

using System;
using System.Collections.Generic;
using System.Threading;

using Gtk;

namespace DBusExplorer
{
	public partial class MainWindow: Gtk.Window
	{	
		//DBusExplorator explorator;
	
		//TreeStore model = new TreeStore(typeof(string), typeof(Gdk.Pixbuf), typeof(object)); 
		//BusContentView tv;
	
		ImageAnimation spinner;
		BusPageWidget currentPageWidget = null;
	
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			Logging.AddWatcher(LoggingEventHandler);
			this.DeleteEvent += OnDeleteEvent;
		
			// Graphical setup
			Build ();
			// Remove the page inital page imposed by the RAD
			buses_Nb.RemovePage(0);
			// Fake the action to create a nicely formated new page
			OnNewTabActionActivated(this, EventArgs.Empty);
		
			// Spinner
			spinner = new ImageAnimation(
				Gdk.Pixbuf.LoadFromResource("process-working.png"),
				40, 32, 32, 32);
			spinner.Active = false;
			spinnerAlign.Add(spinner);
			spinnerBox.HideAll();
		}
	
		void FeedBusComboBox(IEnumerable<string> buses)
		{
			FeedBusComboBox(buses, currentPageWidget);
		}
		
		void FeedBusComboBox(IEnumerable<string> buses, BusPageWidget page)
		{
			ComboBox cb = ComboBox.NewText();
		
			foreach (string s in buses) {
				if (string.IsNullOrEmpty(s))
					continue;
				cb.AppendText(s);
			}
			
			UpdateComboBox(cb);
			page.CurrentComboBox = cb;
		}
		
		void UpdateComboBox(ComboBox cb)
		{
			if (cb == null)
				return;
			if (System.Object.ReferenceEquals(cb, busCb))
				return;
			
			busCbCont.Remove(busCb);
			busCb = cb;
			busCbCont.Add(busCb);
			busCb.Changed += OnBusComboChanged;
			busCbCont.ShowAll();
		}

		void UpdateView (string busName)
		{
			BusContentView view = this.currentPageWidget.BusContent;
			DBusExplorator explorator = currentPageWidget.Explorator;
			TabWidget tab = this.currentPageWidget.Tab;
			tab.TabName = busName;
			view.Reinitialize();
			
			try {
				IEnumerable<PathContainer> elements = explorator.GetElementsFromBus (busName);
				foreach (PathContainer path in elements) {
					view.AddPath(path);
				}
			} catch (Exception e) {
				LoggingEventHandler (LogType.Error, "Error while retrieving bus elements", e, null);
			}
		}	
		
		void ReinitBus (DBusExplorator exp)
		{
			this.currentPageWidget.Explorator = exp;
			
			exp.DBusError += OnDBusError;
			exp.AvailableNamesUpdated += OnAvailableNamesUpdated;
			
			currentPageWidget.BusContent.Reinitialize ();
			FeedBusComboBox(exp.AvailableBusNames);
		}
		
		void OnDBusError(object sender, DBusErrorEventArgs e)
		{
			LoggingEventHandler (LogType.Error, e.ErrorMessage, null, null);
		}
		
		void OnAvailableNamesUpdated(object sender, EventArgs e)
		{
			DBusExplorator exp = sender as DBusExplorator;
			if (exp == null)
				return;
			
			for (int i = 0; i < buses_Nb.NPages; i++) {
				BusPageWidget page = buses_Nb.GetNthPage (i) as BusPageWidget;
				if (page == null)
					continue;
				if (page.Explorator != exp)
					continue;
				
				FeedBusComboBox (exp.AvailableBusNames, page);
			}
		}
		
		void LoggingEventHandler(LogType type, string message, Exception ex, Window parent) {
			Application.Invoke( delegate {
				spinnerBox.HideAll();
				spinner.Active = false;
				string mess = ex == null ? message : 
					message + Environment.NewLine + Environment.NewLine + "<b>Exception : </b>" + ex.Message; 
				
				MessageDialog diag = new MessageDialog (parent == null ? this : parent,
				                                        DialogFlags.DestroyWithParent | DialogFlags.Modal,
				                                        type == LogType.Error ? MessageType.Error : MessageType.Warning,
				                                        ButtonsType.Ok, mess);
				diag.Run();
				diag.Destroy();
			});
		}
		
#region UI Event methods
	
		protected void OnDeleteEvent (object sender, EventArgs a)
		{
			Application.Quit ();
		}

		protected virtual void OnBusComboChanged (object sender, System.EventArgs e)
		{
			UpdateView(busCb.ActiveText);
		}

		protected virtual void OnSessionBusActivated (object sender, System.EventArgs e)
		{
			ReinitBus(DBusExplorator.SessionExplorator);
		}

		protected virtual void OnSystemBusActivated (object sender, System.EventArgs e)
		{
			ReinitBus(DBusExplorator.SystemExplorator);			
		}
	
		protected virtual void OnAboutActivated (object sender, System.EventArgs e)
		{
			AboutDialog ad = new AboutDialog();
			ad.Authors = new string[] { "Jérémie \"Garuma\" Laval" };
			ad.Copyright = "Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>";
			ad.License = "See the COPYING file";
			ad.Version = "0.6";
		
			ad.Run();
			ad.Destroy();
		}

		protected virtual void OnCustomBusActivated (object sender, System.EventArgs e)
		{
			CustomBusDialog d = new CustomBusDialog(this);
			d.ShowAll();
			if (d.Run() == (int)ResponseType.Ok) {
				if (!string.IsNullOrEmpty(d.RequestedBus))
					ReinitBus(new DBusExplorator(NDesk.DBus.Bus.Open(d.RequestedBus)));
			}
			d.Destroy();
		}
		
		protected virtual void OnBusesNbSwitchPage (object o, Gtk.SwitchPageArgs args)
		{
			currentPageWidget = buses_Nb.CurrentPageWidget as BusPageWidget;
			if (currentPageWidget == null) {
				buses_Nb.CurrentPage = 0;
				currentPageWidget = buses_Nb.CurrentPageWidget as BusPageWidget;
			}
			UpdateComboBox(currentPageWidget.CurrentComboBox);
		}

		protected virtual void OnNewTabActionActivated (object sender, System.EventArgs e)
		{
			TabWidget tab = new TabWidget(Mono.Unix.Catalog.GetString("(No Title)"), buses_Nb, buses_Nb.NPages);
			BusPageWidget page = new BusPageWidget(tab);
			page.ShowAll();
			page.Explorator = DBusExplorator.SessionExplorator;
			FeedBusComboBox(page.Explorator.AvailableBusNames, page);
			buses_Nb.AppendPage(page, tab);
			// Switch to the newly append page which trigger the normal events
			buses_Nb.CurrentPage = buses_Nb.NPages - 1;
		}

#endregion
	}
}