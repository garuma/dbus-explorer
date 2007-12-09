// Main.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information. 
using System;
using System.Diagnostics;

using Gtk;

using NDesk.DBus;

namespace DBusExplorer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			BusG.Init();
			Mono.Unix.Catalog.Init("dbus-explorer", "./");
			
			DBusExplorator explorator = new DBusExplorator();
			MainWindow win = new MainWindow (explorator);
			
			win.Show ();
			Application.Run ();
		}
	}
}