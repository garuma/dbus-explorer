// ElementRepresentation.cs created with MonoDevelop
// User: jeremie at 13:43 09/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DBusViewerSharp
{
	
	
	public class ElementRepresentation
	{
		Gdk.Pixbuf image;
		string specDesc;
		string cStyle;
		//string additionalFlags;
		
		public Gdk.Pixbuf Image {
			get {
				return image;
			}
		}

		public string SpecDesc {
			get {
				return specDesc;
			}
		}

		public string CStyle {
			get {
				return cStyle;
			}
		}

		/*public string AdditionalFlags {
			get {
				return additionalFlags;
			}
		}*/
		
		public ElementRepresentation(Gdk.Pixbuf image, string specDesc, string cStyle)
		{
			this.image = image;
			this.specDesc = specDesc;
			this.cStyle = cStyle;
			//this.additionalFlags = additionalFlags;
		}
	}
}
