using System;
using System.Collections.Generic;
using System.Threading;


using Gtk;

using DBusViewerSharp;

public partial class MainWindow: Gtk.Window
{	
	DBusViewerSharp.DBusExplorator explorator;
	TreeStore model = new TreeStore(typeof(string), typeof(Gdk.Pixbuf));
	
	Dictionary<string, ElementRepresentation> currentData = new Dictionary<string,ElementRepresentation>(); 
	
	static Gdk.Pixbuf empty = Gdk.Pixbuf.LoadFromResource("empty.png");
	ImageAnimation spinner;
	
	public MainWindow (DBusViewerSharp.DBusExplorator explorator): base (Gtk.WindowType.Toplevel)
	{
		this.explorator = explorator;
		
		// Graphical setup
		Build ();
		this.DeleteEvent += OnDeleteEvent;
		this.tv.CursorChanged += OnRowSelected;
		this.busCb.Changed += OnBusComboChanged;
		
		// Spinner
		spinner = new ImageAnimation(
			Gdk.Pixbuf.LoadFromResource("process-working.png"),
			40, 32, 32, 32);
		spinner.Active = false;
		spinnerAlign.Add(spinner);
		spinnerBox.HideAll();
		
		// Graphical elements
		FeedBusComboBox(explorator.AvalaibleBusNames);
		InitTreeView();
	}
	
	void FeedBusComboBox(string[] buses)
	{
		foreach (string s in buses) {
			if (string.IsNullOrEmpty(s))
				continue;
			busCb.AppendText(s);
		}
	}
	
	void InitTreeView()
	{
		tv.Model = model;
		tv.AppendColumn("Name", new CellRendererText(), "text", 0);
		tv.AppendColumn("Logo", new CellRendererPixbuf(), "pixbuf", 1);
	}

	
	void UpdateTreeView (string busName)
	{
		//Console.WriteLine("Update the treeview");
		model.Clear();
		currentData.Clear();
		
		spinnerBox.ShowAll();
		spinner.Active = true;
		
		explorator.BeginGetElementsFromBus(busName, delegate (IAsyncResult result) {
			ElementsEntry[] elements = explorator.EndGetElementsFromBus(result);
			Application.Invoke(delegate {
				foreach (ElementsEntry element in elements) {
					//Console.WriteLine("Adding ElementsEntry");
					AddElement(element);
				}
				
				spinnerBox.HideAll();
				spinner.Active = false;
			});
		});
	}
	
	void AddElement (ElementsEntry element)
	{
		if (element == null)
			return;
		
		TreeIter parent = model.AppendValues(element.Path, empty);
		//Console.WriteLine("Added ElementsEntry with path : " + element.Path);
		AddChildSymbols(parent, element);
	}
	
	void AddChildSymbols (TreeIter parent, ElementsEntry element)
	{
		foreach (IEntry entry in element.Symbols) {
			ElementRepresentation representation = entry.Visual;
			
			model.AppendValues(parent, entry.Name, representation.Image);
			try {
				currentData.Add(entry.Name, representation);
			} catch {}	
		}
	}
	
	void FillBottom (string key)
	{
		ElementRepresentation representation;
		if (!currentData.TryGetValue(key, out representation))
			return;
		
		informationLabel.Text = key;
		symbolImage.Pixbuf = representation.Image;
		specstyleDecl.Markup = "<b><tt>" + representation.SpecDesc + "</tt></b>";
		cstyleDecl.Markup = "<tt>" + representation.CStyle + "</tt>";
	}
	
	protected void OnDeleteEvent (object sender, EventArgs a)
	{
		Application.Quit ();
	}

	protected virtual void OnBusComboChanged (object sender, System.EventArgs e)
	{
		UpdateTreeView(busCb.ActiveText);
	}

	protected virtual void OnRowSelected (object o, EventArgs args)
	{
		TreeIter tIter;
		tv.Selection.GetSelected(out tIter);
		
		string valSelected = (string)model.GetValue(tIter, 0);
		//Console.WriteLine("Running FillBottom with : " + valSelected);
		FillBottom(valSelected);
	}

	protected virtual void OnSessionBusActivated (object sender, System.EventArgs e)
	{
	}

	protected virtual void OnSystemBusActivated (object sender, System.EventArgs e)
	{
	}
}