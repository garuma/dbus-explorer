
// This file has been generated by the GUI designer. Do not modify.
namespace DBusExplorer
{
	public partial class PropertyInvokeDialog
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.Alignment alignment4;

		private global::Gtk.HBox hbox5;

		private global::Gtk.Label label1;

		private global::Gtk.Alignment alignment5;

		private global::Gtk.Label propertyName;

		private global::Gtk.Alignment alignment1;

		private global::Gtk.HBox hbox2;

		private global::Gtk.RadioButton getBtn;

		private global::Gtk.RadioButton setBtn;

		private global::Gtk.Alignment actionAlign;

		private global::Gtk.Alignment setAlign;

		private global::Gtk.HBox hbox3;

		private global::Gtk.Label label2;

		private global::Gtk.Alignment alignment6;

		private global::Gtk.Entry setEntry;

		private global::Gtk.Alignment getAlign;

		private global::Gtk.HBox hbox4;

		private global::Gtk.Label label3;

		private global::Gtk.Alignment alignment7;

		private global::Gtk.Label getLbl;

		private global::Gtk.Button buttonExecute;

		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget DBusExplorer.PropertyInvokeDialog
			this.Name = "DBusExplorer.PropertyInvokeDialog";
			this.Title = global::Mono.Unix.Catalog.GetString ("Invoke property");
			this.Icon = global::Gdk.Pixbuf.LoadFromResource ("DBusExplorer.data.property.png");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Resizable = false;
			this.DefaultWidth = 400;
			this.DefaultHeight = 156;
			// Internal child DBusExplorer.PropertyInvokeDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.vbox2.BorderWidth = ((uint)(3));
			// Container child vbox2.Gtk.Box+BoxChild
			this.alignment4 = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.alignment4.Name = "alignment4";
			this.alignment4.LeftPadding = ((uint)(5));
			this.alignment4.TopPadding = ((uint)(6));
			this.alignment4.BottomPadding = ((uint)(2));
			// Container child alignment4.Gtk.Container+ContainerChild
			this.hbox5 = new global::Gtk.HBox ();
			this.hbox5.Name = "hbox5";
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Property name: </b>");
			this.label1.UseMarkup = true;
			this.hbox5.Add (this.label1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox5[this.label1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox5.Gtk.Box+BoxChild
			this.alignment5 = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.alignment5.Name = "alignment5";
			// Container child alignment5.Gtk.Container+ContainerChild
			this.propertyName = new global::Gtk.Label ();
			this.propertyName.Name = "propertyName";
			this.propertyName.LabelProp = global::Mono.Unix.Catalog.GetString ("  ");
			this.alignment5.Add (this.propertyName);
			this.hbox5.Add (this.alignment5);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox5[this.alignment5]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			this.alignment4.Add (this.hbox5);
			this.vbox2.Add (this.alignment4);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.alignment4]));
			w6.Position = 0;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.alignment1 = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.alignment1.BorderWidth = ((uint)(3));
			// Container child alignment1.Gtk.Container+ContainerChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Homogeneous = true;
			// Container child hbox2.Gtk.Box+BoxChild
			this.getBtn = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("get"));
			this.getBtn.CanFocus = true;
			this.getBtn.Name = "getBtn";
			this.getBtn.DrawIndicator = true;
			this.getBtn.UseUnderline = true;
			this.getBtn.Group = new global::GLib.SList (global::System.IntPtr.Zero);
			this.hbox2.Add (this.getBtn);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.getBtn]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.setBtn = new global::Gtk.RadioButton (global::Mono.Unix.Catalog.GetString ("set"));
			this.setBtn.CanFocus = true;
			this.setBtn.Name = "setBtn";
			this.setBtn.DrawIndicator = true;
			this.setBtn.UseUnderline = true;
			this.setBtn.Group = this.getBtn.Group;
			this.hbox2.Add (this.setBtn);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.setBtn]));
			w8.Position = 1;
			this.alignment1.Add (this.hbox2);
			this.vbox2.Add (this.alignment1);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.alignment1]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.actionAlign = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.actionAlign.Name = "actionAlign";
			// Container child actionAlign.Gtk.Container+ContainerChild
			this.setAlign = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.setAlign.Name = "setAlign";
			// Container child setAlign.Gtk.Container+ContainerChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 0f;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("New value :");
			this.hbox3.Add (this.label2);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.label2]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.alignment6 = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.alignment6.Name = "alignment6";
			// Container child alignment6.Gtk.Container+ContainerChild
			this.setEntry = new global::Gtk.Entry ();
			this.setEntry.CanFocus = true;
			this.setEntry.Name = "setEntry";
			this.setEntry.IsEditable = true;
			this.setEntry.InvisibleChar = '•';
			this.alignment6.Add (this.setEntry);
			this.hbox3.Add (this.alignment6);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.alignment6]));
			w13.Position = 1;
			this.setAlign.Add (this.hbox3);
			this.actionAlign.Add (this.setAlign);
			this.vbox2.Add (this.actionAlign);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.actionAlign]));
			w16.Position = 2;
			w16.Expand = false;
			w16.Fill = false;
			w16.Padding = ((uint)(4));
			// Container child vbox2.Gtk.Box+BoxChild
			this.getAlign = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.getAlign.Name = "getAlign";
			this.getAlign.BottomPadding = ((uint)(8));
			// Container child getAlign.Gtk.Container+ContainerChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 0f;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Current value : </b>");
			this.label3.UseMarkup = true;
			this.hbox4.Add (this.label3);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.label3]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.alignment7 = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.alignment7.Name = "alignment7";
			// Container child alignment7.Gtk.Container+ContainerChild
			this.getLbl = new global::Gtk.Label ();
			this.getLbl.Name = "getLbl";
			this.getLbl.LabelProp = global::Mono.Unix.Catalog.GetString (" ");
			this.alignment7.Add (this.getLbl);
			this.hbox4.Add (this.alignment7);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.alignment7]));
			w19.Position = 1;
			w19.Expand = false;
			w19.Fill = false;
			this.getAlign.Add (this.hbox4);
			this.vbox2.Add (this.getAlign);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.getAlign]));
			w21.Position = 3;
			w21.Expand = false;
			w21.Fill = false;
			w1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(w1[this.vbox2]));
			w22.Position = 0;
			w22.Expand = false;
			w22.Fill = false;
			// Internal child DBusExplorer.PropertyInvokeDialog.ActionArea
			global::Gtk.HButtonBox w23 = this.ActionArea;
			w23.Name = "dialog1_ActionArea";
			w23.Spacing = 6;
			w23.BorderWidth = ((uint)(5));
			w23.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonExecute = new global::Gtk.Button ();
			this.buttonExecute.CanDefault = true;
			this.buttonExecute.CanFocus = true;
			this.buttonExecute.Name = "buttonExecute";
			this.buttonExecute.UseStock = true;
			this.buttonExecute.UseUnderline = true;
			this.buttonExecute.Label = "gtk-execute";
			w23.Add (this.buttonExecute);
			global::Gtk.ButtonBox.ButtonBoxChild w24 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w23[this.buttonExecute]));
			w24.Expand = false;
			w24.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-close";
			this.AddActionWidget (this.buttonOk, -7);
			global::Gtk.ButtonBox.ButtonBoxChild w25 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w23[this.buttonOk]));
			w25.Position = 1;
			w25.Expand = false;
			w25.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
			this.getBtn.Toggled += new global::System.EventHandler (this.OnGetBtnToggled);
			this.setBtn.Toggled += new global::System.EventHandler (this.OnSetBtnToggled);
			this.buttonExecute.Clicked += new global::System.EventHandler (this.OnButtonExecuteClicked);
		}
	}
}
