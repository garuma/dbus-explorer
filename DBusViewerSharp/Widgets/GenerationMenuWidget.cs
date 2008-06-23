// GenerationMenuWidget.cs created with MonoDevelop
// User: jeremie at 20:19 17/04/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using Gtk;

namespace DBusExplorer
{
	public class GenerationMenuWidget: Menu
	{
		
		public GenerationMenuWidget(PathContainer referer)
		{
			MenuItem path = new MenuItem("Generate path " + referer.Path + " ...");
			path.Activated += delegate {
				GenerationDialog d = new GenerationDialog();
				d.Modal = true;
				if (d.Run() == (int)ResponseType.Ok) {
					IGenerator generator = new CSharpCodeDomGenerator();
					generator.Generate(referer, d.PathToSave);
				}
				d.Destroy();
			};
			this.Append(path);
			path.ShowAll();
		}
		
		public GenerationMenuWidget(Interface referer)
		{
			MenuItem path = new MenuItem("Generate interface " + referer.Name + " ...");
			path.Activated += delegate {
				GenerationDialog d = new GenerationDialog();
				d.Modal = true;
				if (d.Run() == (int)ResponseType.Ok) {
					IGenerator generator = new CSharpCodeDomGenerator();
					generator.Generate(referer, d.PathToSave);
				}
				d.Destroy();
			};
			this.Append(path);
			path.ShowAll();
		}
		
		public GenerationMenuWidget(IElement referer)
		{
			MenuItem path = new MenuItem("Generate element " + referer.Name + " ...");
			
			this.Append(path);
			path.ShowAll();
		}
		
		void OnPathGenerate(object sender, EventArgs e)
		{
				
		}
			
		void OnInterfaceGenerate(object sender, EventArgs e)
		{
				
		}
		
		void OnElementGenerate(object sender, EventArgs e)
		{
				
		}
	}
}
