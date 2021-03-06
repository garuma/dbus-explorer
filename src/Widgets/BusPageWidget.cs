// BusPageWidget.cs
// Copyright (c) 2008-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using Gtk;

namespace DBusExplorer
{
	
	public partial class BusPageWidget : Gtk.Bin
	{
		BusContentView busContent;
		InformationView infoView = new InformationView();
		TabWidget tab;
		DBusExplorator explorator;
		ComboBox currentComboBox;
		
		public BusPageWidget(TabWidget tab)
		{
			this.Build();
			this.busContent = new BusContentView(this);
			this.tab = tab;
			this.informationViewPlaceholder.Add(infoView);
			this.tvPlaceholder.Add(busContent);
			
			busContent.ElementUpdated += delegate (object sender, ElementUpdatedEventArgs e) {
				infoView.FillBottom(e.Element);
			};
		}
		
		public BusContentView BusContent {
			get {
				return busContent;
			}
		}

		public DBusExplorator Explorator {
			get {
				return explorator;
			}
			set {
				explorator = value;
			}
		}

		public string CurrentSelectedBus {
			get {
				return currentComboBox.ActiveText;
			}
		}

		public ComboBox CurrentComboBox {
			get {
				return currentComboBox;
			}
			set {
				currentComboBox = value;
			}
		}

		public InformationView InfoView {
			get {
				return infoView;
			}
		}

		public TabWidget Tab {
			get {
				return tab;
			}
		}
	
	}
}
