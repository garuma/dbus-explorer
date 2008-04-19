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
			Mono.Unix.Catalog.Init("dbus-explorer", string.Empty);
			
			MainWindow win = new MainWindow ();
			
			win.Show ();
			Application.Run ();
		}
	}
}