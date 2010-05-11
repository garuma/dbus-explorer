// CustomBusDialog.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using Gtk;

namespace DBusExplorer
{
	public partial class CustomBusDialog : Gtk.Dialog
	{
		
		public CustomBusDialog (Window parent)
			: base ("Custom bus", parent, DialogFlags.DestroyWithParent | DialogFlags.Modal)
		{
			this.TransientFor = parent;
			this.Build();
		}
		
		public string RequestedBus {
			get {
				return this.busName.Text;
			}
		}
	}
}
