// MainWindow.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Collections.Generic;
using System.Threading;

using Gtk;

using DBusViewerSharp;

public partial class MainWindow: Gtk.Window
{	
	DBusViewerSharp.DBusExplorator explorator;
	
	TreeStore model = new TreeStore(typeof(string), typeof(Gdk.Pixbuf));
	
	Dictionary<string, IElement> currentData = new Dictionary<string,IElement>(); 
	
	static Gdk.Pixbuf empty = Gdk.Pixbuf.LoadFromResource("empty.png");
	ImageAnimation spinner;
	
	public MainWindow (DBusViewerSharp.DBusExplorator explorator): base (Gtk.WindowType.Toplevel)
	{
		this.explorator = explorator;
		
		// Graphical setup
		Build ();
		this.DeleteEvent += OnDeleteEvent;
		this.tv.CursorChanged += OnRowSelected;
		
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
		ComboBox cb = ComboBox.NewText();
		
		foreach (string s in buses) {
			if (string.IsNullOrEmpty(s))
				continue;
			cb.AppendText(s);
		}
		busCbCont.Remove(busCb);
		busCb = cb;
		busCbCont.Add(busCb);
		busCb.Changed += OnBusComboChanged;
		busCbCont.ShowAll();
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
			IEnumerable<PathContainer> elements = explorator.EndGetElementsFromBus(result);
			Application.Invoke(delegate {
				foreach (PathContainer path in elements) {
					//Console.WriteLine("Adding ElementsEntry");
					AddPath(path);
				}
				
				spinnerBox.HideAll();
				spinner.Active = false;
			});
		});
		/*foreach (PathContainer path in explorator.GetElementsFromBus(busName)) {
			//Console.WriteLine("Adding ElementsEntry");
			AddPath(path);
		}*/
	}
	
	void AddPath(PathContainer path)
	{
		if (path == null)
			return;
		/*if (path.Interfaces.Length == 0)
			return;*/
		    
		TreeIter parent = model.AppendValues(path.Path, empty);
		foreach (Interface @interface in path.Interfaces) {
			AddInterface (parent, @interface);
		}
	}
	
	void AddInterface (TreeIter parent, Interface element)
	{
		if (element == null)
			return;
		
		TreeIter child = model.AppendValues(parent, element.Name, empty);
		
		AddChildSymbols(child, element);
	}
	
	void AddChildSymbols (TreeIter parent, Interface element)
	{
		foreach (IElement entry in element.Symbols) {
			if (string.IsNullOrEmpty(element.Name) || entry.Image == null)
				continue;
			
			model.AppendValues(parent, entry.Name, entry.Image);
			try {
				currentData.Add(entry.Name, entry);
			} catch {}	
		}
	}
	
	void FillBottom (string key)
	{
		IElement element;
		if (!currentData.TryGetValue(key, out element))
			return;
		
		ElementRepresentation representation = element.Visual;
		informationLabel.Text = key;
		symbolImage.Pixbuf = element.Image;
		specstyleDecl.Markup = "<b><tt>" + representation.SpecDesc + "</tt></b>";
		cstyleDecl.Markup = "<tt>" + representation.CStyle.Replace("<", "&lt;") + "</tt>";
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
		model.Clear();
		this.explorator = new DBusExplorator();
		FeedBusComboBox(this.explorator.AvalaibleBusNames);
	}

	protected virtual void OnSystemBusActivated (object sender, System.EventArgs e)
	{
		model.Clear();
		this.explorator = new DBusExplorator(NDesk.DBus.Bus.System);
		FeedBusComboBox(this.explorator.AvalaibleBusNames);
	}
	
	protected virtual void OnAboutActivated (object sender, System.EventArgs e)
	{
		AboutDialog ad = new AboutDialog();
		ad.Authors = new string[] { "Jérémie \"Garuma\" Laval" };
		ad.Copyright = "Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>";
		ad.License = "See the COPYING file";
		ad.Version = "0.2";
		
		ad.Run();
		ad.Destroy();
	}
}