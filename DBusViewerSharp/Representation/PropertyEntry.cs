// PropertyEntry.cs created with MonoDevelop
// User: jeremie at 20:46 06/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

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
