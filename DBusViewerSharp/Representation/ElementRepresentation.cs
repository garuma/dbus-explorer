// ElementRepresentation.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

namespace DBusExplorer
{
	public class ElementRepresentation
	{
		string specDesc;
		string cStyle;
		

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
		
		
		
		public ElementRepresentation(string specDesc, string cStyle)
		{
			this.specDesc = specDesc;
			this.cStyle = cStyle;
		}
	}
}
