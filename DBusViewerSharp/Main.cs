// Main.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information. 
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