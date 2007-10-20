// project created on 04/10/2007 at 20:12
using System;
using System.Diagnostics;

using Gtk;

using NDesk.DBus;

namespace DBusViewerSharp
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			BusG.Init();
			
			DBusExplorator explorator = new DBusExplorator();
			MainWindow win = new MainWindow (explorator);
			
			win.Show ();
			Application.Run ();
		}
	}
}