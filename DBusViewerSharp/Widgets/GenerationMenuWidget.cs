// LangageRepresentationViewer.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using Gtk;

using NDesk.DBus;

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
		
		public GenerationMenuWidget(Interface referer)
		{
			MenuItem path = new MenuItem("Generate " + referer.Name + "...");
			path.Activated += delegate {
				GenerationDialog d = new GenerationDialog(referer);
				d.Modal = true;
				if (d.Run() == (int)ResponseType.Ok) {
					IGenerator generator = new CSharpCodeDomGenerator();
					string p = d.PathToSave;
					if (!p.EndsWith (".cs"))
						p += ".cs";
					
					generator.Generate(d.Selected, p);
				}
				d.Destroy();
			};
			this.Append(path);
			path.ShowAll();
		}
		
		public GenerationMenuWidget(IElement referer, Bus bus, string busName)
		{
			if (referer.Data != null) {
				MenuItem path = new MenuItem("Call " + referer.Name + "...");
				
				if (!referer.Data.IsProperty) {
					path.Activated += delegate {
						var p = new ObjectPath(referer.Parent.Parent.Path);
						InvokeDialog diag = new InvokeDialog (bus, busName,
						                                      p, referer);
						
						while (diag.Run () == (int)ResponseType.None);
						diag.Destroy();
					};
				} else {
					path.Activated += delegate {
						var p = new ObjectPath(referer.Parent.Parent.Path);
						PropertyInvokeDialog diag = new PropertyInvokeDialog (bus, busName,
						                                              p, referer);
						
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
