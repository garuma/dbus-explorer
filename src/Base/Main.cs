// Main.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.

using System;

using System.IO;
using System.Diagnostics;

using Gtk;

using DBus;

namespace DBusExplorer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			BusG.Init();
			Mono.Unix.Catalog.Init("dbus-explorer", "/usr/share/local");
			
			MainWindow win = new MainWindow ();
			
			win.Show ();
			Application.Run ();
		}
	}
}