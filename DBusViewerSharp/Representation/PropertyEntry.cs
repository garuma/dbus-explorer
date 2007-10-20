// PropertyEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusViewerSharp
{
	public class PropertyEntry: IEntry
	{
		string name;
		ArgEntry arg;
		ReadWriteFlag flag;
		
		ElementRepresentation representation;
		
		static Gdk.Pixbuf pixbuf = Gdk.Pixbuf.LoadFromResource("property.png");
		
		public PropertyEntry(string name, ArgEntry arg, ReadWriteFlag flag)
		{
			this.name = name;
			this.arg = arg;
			this.flag = flag;
			
			this.representation = 
				new ElementRepresentation(pixbuf, string.Empty,
				                          string.Format("public {0} {1} { {2} }", arg.Type, name,
				                                        (flag == ReadWriteFlag.Read) ? "get;" : (flag == ReadWriteFlag.Write) ? "set;" : "get; set;"));
		}

		public ElementRepresentation Visual {
			get {
				return this.representation;
			}
		}
		
		public string Name {
			get {
				return name;
			}
		}
	}
	
	public enum ReadWriteFlag
	{
		Read,
		Write,
		ReadWrite
	}
}
