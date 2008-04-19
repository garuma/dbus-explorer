// GenerationMenuWidget.cs created with MonoDevelop
// User: jeremie at 20:19 17/04/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace DBusExplorer
{
	public class GenerationMenuWidget: Menu
	{
		
		public GenerationMenuWidget(PathContainer referer)
		{
			MenuItem path = new MenuItem("Generate wrapper code for " + referer.Path);
			this.Append(path);
			path.ShowAll();
		}
		
		public GenerationMenuWidget(Interface referer)
		{
			
		}
		
		public GenerationMenuWidget(IElement referer)
		{
			
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
