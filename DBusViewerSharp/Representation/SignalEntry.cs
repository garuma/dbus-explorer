// SignalEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusViewerSharp
{
	
	
	public class SignalEntry: IEntry
	{
		string name;
		ArgEntry arg;
		
		ElementRepresentation representation;
		
		static Gdk.Pixbuf pixbuf = Gdk.Pixbuf.LoadFromResource("event.png");
		
		public SignalEntry(string name, ArgEntry arg)
		{
			this.name = name;
			this.arg = arg;
			
			this.representation = new ElementRepresentation(pixbuf, string.Format("signal {0} : {1}", name, arg.Type), 
			                                                string.Format("event EventHandler<{0}> {1}", arg.Type, name));
		}

		public ElementRepresentation Visual {
			get {
				return representation;
			}
		}
		
		public string Name {
			get {
				return name;
			}
		}
	}
}
