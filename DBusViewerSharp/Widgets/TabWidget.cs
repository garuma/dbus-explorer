/* TabWidget.cs
 * Copyright (C) 2007  LAVAL Jérémie
 *
 * This file is licensed under the terms of the LGPL.
 * 
 * Code adapted from Stetic which is Copyright (c) 2004 Novell, Inc
 * 
 */

using Gtk;
using System;

namespace DBusExplorer
{
	public class TabWidget: HBox
	{
		Label tabLabel;
		
		public event EventHandler CloseRequested;
		
		public TabWidget(string tabName, Notebook nb, int index)
		{
			tabLabel = new Label(tabName);
			this.PackStart (tabLabel, true, true, 3);
			Button b = new Button (new Gtk.Image ("gtk-close", IconSize.Menu));
			b.Relief = Gtk.ReliefStyle.None;
			b.WidthRequest = b.HeightRequest = 24;
			
			b.Clicked += delegate (object s, EventArgs a) {
				if (CloseRequested != null)
					CloseRequested(this, EventArgs.Empty);
				
				nb.RemovePage(index);
			};
				
			this.PackStart (b, false, false, 0);
			this.ShowAll();		
		}
		
		public string TabName {
			get {
				return tabLabel.Text;
			}
			set {
				if (!string.IsNullOrEmpty(value))
					tabLabel.Text = value;
			}
		}
	}
}
