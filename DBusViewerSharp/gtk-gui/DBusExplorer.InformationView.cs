// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace DBusExplorer {
    
    
    public partial class InformationView {
        
        private Gtk.Expander expander2;
        
        private Gtk.VBox vbox3;
        
        private Gtk.Alignment alignment5;
        
        private Gtk.HBox hbox5;
        
        private Gtk.Image symbolImage;
        
        private Gtk.Alignment alignment7;
        
        private Gtk.Label specstyleDecl;
        
        private Gtk.Alignment langsPh;
        
        private Gtk.HBox hbox7;
        
        private Gtk.Label label6;
        
        private Gtk.Label informationLabel;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget DBusExplorer.InformationView
            Stetic.BinContainer.Attach(this);
            this.Name = "DBusExplorer.InformationView";
            // Container child DBusExplorer.InformationView.Gtk.Container+ContainerChild
            this.expander2 = new Gtk.Expander(null);
            this.expander2.CanFocus = true;
            this.expander2.Name = "expander2";
            this.expander2.Expanded = true;
            this.expander2.Spacing = 2;
            this.expander2.BorderWidth = ((uint)(5));
            // Container child expander2.Gtk.Container+ContainerChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            this.vbox3.BorderWidth = ((uint)(6));
            // Container child vbox3.Gtk.Box+BoxChild
            this.alignment5 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment5.Name = "alignment5";
            this.alignment5.LeftPadding = ((uint)(44));
            this.alignment5.TopPadding = ((uint)(7));
            this.alignment5.BottomPadding = ((uint)(8));
            // Container child alignment5.Gtk.Container+ContainerChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Spacing = 6;
            // Container child hbox5.Gtk.Box+BoxChild
            this.symbolImage = new Gtk.Image();
            this.symbolImage.Name = "symbolImage";
            this.hbox5.Add(this.symbolImage);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox5[this.symbolImage]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child hbox5.Gtk.Box+BoxChild
            this.alignment7 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment7.Name = "alignment7";
            this.alignment7.LeftPadding = ((uint)(10));
            // Container child alignment7.Gtk.Container+ContainerChild
            this.specstyleDecl = new Gtk.Label();
            this.specstyleDecl.Name = "specstyleDecl";
            this.specstyleDecl.LabelProp = "";
            this.specstyleDecl.UseMarkup = true;
            this.specstyleDecl.Selectable = true;
            this.alignment7.Add(this.specstyleDecl);
            this.hbox5.Add(this.alignment7);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox5[this.alignment7]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
            this.alignment5.Add(this.hbox5);
            this.vbox3.Add(this.alignment5);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox3[this.alignment5]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.langsPh = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.langsPh.Name = "langsPh";
            this.langsPh.LeftPadding = ((uint)(7));
            this.langsPh.TopPadding = ((uint)(4));
            this.langsPh.BottomPadding = ((uint)(7));
            this.vbox3.Add(this.langsPh);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox3[this.langsPh]));
            w6.Position = 1;
            w6.Expand = false;
            this.expander2.Add(this.vbox3);
            this.hbox7 = new Gtk.HBox();
            this.hbox7.Name = "hbox7";
            this.hbox7.Spacing = 1;
            // Container child hbox7.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("<b>Informations on : </b>");
            this.label6.UseMarkup = true;
            this.hbox7.Add(this.label6);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox7[this.label6]));
            w8.Position = 0;
            w8.Expand = false;
            w8.Fill = false;
            // Container child hbox7.Gtk.Box+BoxChild
            this.informationLabel = new Gtk.Label();
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.LabelProp = "";
            this.informationLabel.Selectable = true;
            this.hbox7.Add(this.informationLabel);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox7[this.informationLabel]));
            w9.Position = 1;
            w9.Expand = false;
            w9.Fill = false;
            this.expander2.LabelWidget = this.hbox7;
            this.Add(this.expander2);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
        }
    }
}
