// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace DBusExplorer {
    
    
    public partial class MethodInvokeDialog {
        
        private Gtk.Alignment alignment2;
        
        private Gtk.VBox containerVbox;
        
        private Gtk.Alignment alignment4;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label1;
        
        private Gtk.Alignment alignment5;
        
        private Gtk.Label methodName;
        
        private Gtk.Alignment argAlign;
        
        private Gtk.Frame argFrame;
        
        private Gtk.Alignment GtkAlignment2;
        
        private Gtk.Alignment alignment6;
        
        private Gtk.Table argumentTable;
        
        private Gtk.Label GtkLabel2;
        
        private Gtk.Alignment alignment7;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Label label3;
        
        private Gtk.Alignment alignment8;
        
        private Gtk.Label resultLabel;
        
        private Gtk.Button buttonExecute;
        
        private Gtk.Button buttonClose;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget DBusExplorer.MethodInvokeDialog
            this.Name = "DBusExplorer.MethodInvokeDialog";
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Resizable = false;
            this.AllowGrow = false;
            // Internal child DBusExplorer.MethodInvokeDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.alignment2 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment2.Name = "alignment2";
            this.alignment2.BorderWidth = ((uint)(10));
            // Container child alignment2.Gtk.Container+ContainerChild
            this.containerVbox = new Gtk.VBox();
            this.containerVbox.Name = "containerVbox";
            this.containerVbox.Spacing = 6;
            // Container child containerVbox.Gtk.Box+BoxChild
            this.alignment4 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment4.Name = "alignment4";
            this.alignment4.LeftPadding = ((uint)(5));
            this.alignment4.BottomPadding = ((uint)(12));
            // Container child alignment4.Gtk.Container+ContainerChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("<b>Method name : </b>");
            this.label1.UseMarkup = true;
            this.hbox2.Add(this.label1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox2[this.label1]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.alignment5 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment5.Name = "alignment5";
            // Container child alignment5.Gtk.Container+ContainerChild
            this.methodName = new Gtk.Label();
            this.methodName.Name = "methodName";
            this.methodName.LabelProp = Mono.Unix.Catalog.GetString("label2");
            this.alignment5.Add(this.methodName);
            this.hbox2.Add(this.alignment5);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox2[this.alignment5]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            this.alignment4.Add(this.hbox2);
            this.containerVbox.Add(this.alignment4);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.containerVbox[this.alignment4]));
            w6.Position = 0;
            w6.Expand = false;
            w6.Fill = false;
            // Container child containerVbox.Gtk.Box+BoxChild
            this.argAlign = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.argAlign.Name = "argAlign";
            // Container child argAlign.Gtk.Container+ContainerChild
            this.argFrame = new Gtk.Frame();
            this.argFrame.Name = "argFrame";
            this.argFrame.ShadowType = ((Gtk.ShadowType)(0));
            // Container child argFrame.Gtk.Container+ContainerChild
            this.GtkAlignment2 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment2.Name = "GtkAlignment2";
            this.GtkAlignment2.LeftPadding = ((uint)(12));
            // Container child GtkAlignment2.Gtk.Container+ContainerChild
            this.alignment6 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment6.Name = "alignment6";
            this.alignment6.TopPadding = ((uint)(8));
            // Container child alignment6.Gtk.Container+ContainerChild
            this.argumentTable = new Gtk.Table(((uint)(1)), ((uint)(2)), false);
            this.argumentTable.Name = "argumentTable";
            this.argumentTable.RowSpacing = ((uint)(6));
            this.argumentTable.ColumnSpacing = ((uint)(6));
            this.alignment6.Add(this.argumentTable);
            this.GtkAlignment2.Add(this.alignment6);
            this.argFrame.Add(this.GtkAlignment2);
            this.GtkLabel2 = new Gtk.Label();
            this.GtkLabel2.Name = "GtkLabel2";
            this.GtkLabel2.LabelProp = Mono.Unix.Catalog.GetString("<b>Arguments</b>");
            this.GtkLabel2.UseMarkup = true;
            this.argFrame.LabelWidget = this.GtkLabel2;
            this.argAlign.Add(this.argFrame);
            this.containerVbox.Add(this.argAlign);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.containerVbox[this.argAlign]));
            w11.Position = 1;
            // Container child containerVbox.Gtk.Box+BoxChild
            this.alignment7 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment7.Name = "alignment7";
            this.alignment7.LeftPadding = ((uint)(5));
            this.alignment7.TopPadding = ((uint)(5));
            this.alignment7.BottomPadding = ((uint)(4));
            // Container child alignment7.Gtk.Container+ContainerChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("<b>Return value :</b>");
            this.label3.UseMarkup = true;
            this.hbox3.Add(this.label3);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox3[this.label3]));
            w12.Position = 0;
            w12.Expand = false;
            w12.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.alignment8 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment8.Name = "alignment8";
            // Container child alignment8.Gtk.Container+ContainerChild
            this.resultLabel = new Gtk.Label();
            this.resultLabel.Name = "resultLabel";
            this.alignment8.Add(this.resultLabel);
            this.hbox3.Add(this.alignment8);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox3[this.alignment8]));
            w14.Position = 1;
            w14.Expand = false;
            w14.Fill = false;
            this.alignment7.Add(this.hbox3);
            this.containerVbox.Add(this.alignment7);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.containerVbox[this.alignment7]));
            w16.Position = 2;
            w16.Expand = false;
            w16.Fill = false;
            this.alignment2.Add(this.containerVbox);
            w1.Add(this.alignment2);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(w1[this.alignment2]));
            w18.Position = 0;
            // Internal child DBusExplorer.MethodInvokeDialog.ActionArea
            Gtk.HButtonBox w19 = this.ActionArea;
            w19.Name = "dialog1_ActionArea";
            w19.Spacing = 10;
            w19.BorderWidth = ((uint)(5));
            w19.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonExecute = new Gtk.Button();
            this.buttonExecute.CanDefault = true;
            this.buttonExecute.CanFocus = true;
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.UseStock = true;
            this.buttonExecute.UseUnderline = true;
            this.buttonExecute.Label = "gtk-execute";
            w19.Add(this.buttonExecute);
            Gtk.ButtonBox.ButtonBoxChild w20 = ((Gtk.ButtonBox.ButtonBoxChild)(w19[this.buttonExecute]));
            w20.Expand = false;
            w20.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonClose = new Gtk.Button();
            this.buttonClose.CanDefault = true;
            this.buttonClose.CanFocus = true;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.UseStock = true;
            this.buttonClose.UseUnderline = true;
            this.buttonClose.Label = "gtk-close";
            this.AddActionWidget(this.buttonClose, -7);
            Gtk.ButtonBox.ButtonBoxChild w21 = ((Gtk.ButtonBox.ButtonBoxChild)(w19[this.buttonClose]));
            w21.Position = 1;
            w21.Expand = false;
            w21.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 189;
            this.argFrame.Hide();
            this.Show();
            this.buttonExecute.Clicked += new System.EventHandler(this.OnButtonExecuteClicked);
            this.buttonClose.Clicked += new System.EventHandler(this.OnButtonCloseClicked);
        }
    }
}
