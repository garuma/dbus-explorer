
// This file has been generated by the GUI designer. Do not modify.
namespace DBusExplorer
{
	public partial class BusPageWidget
	{
		private global::Gtk.VBox vbox3;

		private global::Gtk.ScrolledWindow tvWindow;

		private global::Gtk.Alignment tvPlaceholder;

		private global::Gtk.VBox vbox4;

		private global::Gtk.Alignment informationViewPlaceholder;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget DBusExplorer.BusPageWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "DBusExplorer.BusPageWidget";
			// Container child DBusExplorer.BusPageWidget.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.tvWindow = new global::Gtk.ScrolledWindow ();
			this.tvWindow.CanFocus = true;
			this.tvWindow.Name = "tvWindow";
			this.tvWindow.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			this.tvWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			this.tvWindow.BorderWidth = ((uint)(3));
			// Container child tvWindow.Gtk.Container+ContainerChild
			global::Gtk.Viewport w1 = new global::Gtk.Viewport ();
			w1.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.tvPlaceholder = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.tvPlaceholder.Name = "tvPlaceholder";
			w1.Add (this.tvPlaceholder);
			this.tvWindow.Add (w1);
			this.vbox3.Add (this.tvWindow);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.tvWindow]));
			w4.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.informationViewPlaceholder = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.informationViewPlaceholder.Name = "informationViewPlaceholder";
			this.vbox4.Add (this.informationViewPlaceholder);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.informationViewPlaceholder]));
			w5.Position = 0;
			w5.Expand = false;
			this.vbox3.Add (this.vbox4);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.vbox4]));
			w6.Position = 1;
			w6.Expand = false;
			this.Add (this.vbox3);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
		}
	}
}