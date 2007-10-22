// ElementRepresentation.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

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
		
		public ElementRepresentation(Gdk.Pixbuf image, string specDesc, string cStyle)
		{
			this.image = image;
			this.specDesc = specDesc;
			this.cStyle = cStyle;
		}
	}
}
