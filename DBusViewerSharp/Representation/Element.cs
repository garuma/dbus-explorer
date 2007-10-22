// Element.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusViewerSharp
{
	
	
	public class Element: IElement
	{
		ElementRepresentation representation;
		string name;
		
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

		public Element(string name, ElementRepresentation representation)
		{
			this.name = name;
			this.representation = representation;
		}
	}
}
