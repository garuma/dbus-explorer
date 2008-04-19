// BusPageWidget.cs
// Copyright (c) 2008 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using Gtk;

namespace DBusExplorer
{
	
	
	public partial class BusPageWidget : Gtk.Bin
	{
		BusContentView busContent = new BusContentView();
		InformationView infoView = new InformationView();
		DBusExplorator explorator;
		string currentSelectedBus;
		ComboBox currentComboBox;
		
		public BusPageWidget()
		{
			this.Build();
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
				return currentSelectedBus;
			}
			set {
				currentSelectedBus = value;
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
	
	}
}
