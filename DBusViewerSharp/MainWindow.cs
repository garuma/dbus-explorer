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
		//DBusExplorator explorator;
	
		//TreeStore model = new TreeStore(typeof(string), typeof(Gdk.Pixbuf), typeof(object)); 
		//BusContentView tv;
	
		ImageAnimation spinner;
		Entry sentry = new Entry();
		BusPageWidget currentPageWidget = null;
	
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			Logging.AddWatcher(LoggingEventHandler);
			this.DeleteEvent += OnDeleteEvent;
			DBusExplorator.SessionExplorator.DBusError += OnDBusError;
			DBusExplorator.SystemExplorator.DBusError += OnDBusError;
		
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
	
		void FeedBusComboBox(string[] buses)
		{
			FeedBusComboBox(buses, currentPageWidget);
		}
		
		void FeedBusComboBox(string[] buses, BusPageWidget page)
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
			buses_Nb.SetTabLabelText(currentPageWidget, busName);
			view.Reinitialize();
			
			spinnerBox.ShowAll();
			spinner.Active = true;
		
			foreach (PathContainer path in explorator.GetElementsFromBus(busName)) {
				view.AddPath(path);
				//Console.WriteLine(path.Path);
			}
				
			spinnerBox.HideAll();
			spinner.Active = false;
			/*explorator.BeginGetElementsFromBus(busName, delegate (IAsyncResult result) {
				IEnumerable<PathContainer> elements = explorator.EndGetElementsFromBus(result);
				Application.Invoke(delegate {
					foreach (PathContainer path in elements) {
						view.AddPath(path);
					}
				
					spinnerBox.HideAll();
					spinner.Active = false;
				});
			});*/
		}	
		
		void ReinitBus (DBusExplorator exp)
		{
			this.currentPageWidget.Explorator = exp;
			// If it's a custom bus
			if (exp != DBusExplorator.SessionExplorator && exp != DBusExplorator.SystemExplorator)
				exp.DBusError += OnDBusError;
			FeedBusComboBox(exp.AvalaibleBusNames);
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
		
		void LoggingEventHandler(LogType type, string message, Exception ex) {
			Application.Invoke( delegate {
				spinnerBox.HideAll();
				spinner.Active = false;
				string mess = ex == null ? message : message + Environment.NewLine + "Exception : " + ex.Message; 
				MessageDialog diag = new MessageDialog(this, DialogFlags.DestroyWithParent,
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
			ad.Copyright = "Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>";
			ad.License = "See the COPYING file";
			ad.Version = "0.4";
		
			ad.Run();
			ad.Destroy();
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
				new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Sorry, this feature is currently not available.");
			d.Run();
			d.Destroy();
			/*
			DBusMonitor d = new DBusMonitor(explorator.BusUsed, NDesk.DBus.ObjectPath.Root);
			d.ShowAll();
			d.Run();
			d.Destroy();*/
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
			BusPageWidget page = new BusPageWidget();
			page.ShowAll();
			page.Explorator = DBusExplorator.SessionExplorator;
			FeedBusComboBox(page.Explorator.AvalaibleBusNames, page);
			buses_Nb.AppendPage(page, new Label("(No Title)"));
			// Switch to the newly append page which trigger the normal events
			buses_Nb.CurrentPage = buses_Nb.NPages - 1;
		}
#endregion
	}
}