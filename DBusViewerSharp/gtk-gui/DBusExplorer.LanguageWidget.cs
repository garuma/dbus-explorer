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
    
    
    public partial class LanguageWidget {
        
        private Gtk.EventBox evts;
        
        private Gtk.VBox langsVbox;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget DBusExplorer.LanguageWidget
            Stetic.BinContainer.Attach(this);
            this.Name = "DBusExplorer.LanguageWidget";
            // Container child DBusExplorer.LanguageWidget.Gtk.Container+ContainerChild
            this.evts = new Gtk.EventBox();
            this.evts.Name = "evts";
            // Container child evts.Gtk.Container+ContainerChild
            this.langsVbox = new Gtk.VBox();
            this.langsVbox.Name = "langsVbox";
            this.langsVbox.Homogeneous = true;
            this.langsVbox.Spacing = 6;
            this.evts.Add(this.langsVbox);
            this.Add(this.evts);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
        }
    }
}
