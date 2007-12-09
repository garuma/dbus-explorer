// CustomBusDialog.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusExplorer
{
	public partial class CustomBusDialog : Gtk.Dialog
	{
		
		public CustomBusDialog()
		{
			this.Build();
		}
		
		public string RequestedBus {
			get {
				return this.busName.Text;
			}
		}
	}
}
