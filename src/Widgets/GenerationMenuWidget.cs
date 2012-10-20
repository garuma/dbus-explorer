// LangageRepresentationViewer.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.Collections.Generic;

using Gtk;

using DBus;

namespace DBusExplorer
{
	public class GenerationMenuWidget: Menu
	{
		
		/*public GenerationMenuWidget(PathContainer referer)
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
		}*/
		
		public GenerationMenuWidget (Window parent, Interface referer)
		{
			MenuItem path = new MenuItem("Generate " + referer.Name + "...");
			
			IGenerator generator = new CSharpCodeDomGenerator();
			Func<IEnumerable<IElement>, string> renderer = (elements) => {
				try {
					return generator.Generate(elements);
				} catch (Exception e) {
					Logging.Error ("Error during generation", e, parent);
					return string.Empty;
				}
			};
			
			path.Activated += delegate {
				GenerationDialog d = new GenerationDialog (parent, referer, renderer);
				d.Modal = true;
				d.Run ();
				d.Destroy();
			};
			
			this.Append(path);
			path.ShowAll();
		}
		
		public GenerationMenuWidget (Window parent, IElement referer, Bus bus, string busName)
		{
			if (referer.Data != null) {
				MenuItem path = new MenuItem("Call " + referer.Name + "...");
				ObjectPath p = new ObjectPath(referer.Parent.Parent.Path);
				
				if (!referer.Data.IsProperty) {
					path.Activated += delegate {
						MethodInvokeDialog diag = new MethodInvokeDialog (parent, bus, busName, p, referer);
						
						while (diag.Run () == (int)ResponseType.None);
						diag.Destroy();
					};
				} else {
					path.Activated += delegate {
						PropertyInvokeDialog diag = new PropertyInvokeDialog (parent, bus, busName, p, referer);
						
						while (diag.Run () == (int)ResponseType.None);
						diag.Destroy();
					};
				}
				
				this.Append(path);
				path.ShowAll();
			}
		}
	}
}
