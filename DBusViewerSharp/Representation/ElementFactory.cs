// ElementFactory.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusViewerSharp
{
	public static class ElementFactory
	{
		delegate ElementRepresentation ElemGetter();
		
		private class Element: IElement
		{
			ElementRepresentation representation = null;
			string name;
			ElemGetter getter;
			Gdk.Pixbuf image;
			
			public ElementRepresentation Visual {
				get {
					if (representation == null) {
						representation = getter();
					}
					return representation;
				}
			}
			
			public Gdk.Pixbuf Image {
				get {
					return image;
				}
			}

			public string Name {
				get {
					return name;
				}
			}

			public Element(string name, ElemGetter getter, Gdk.Pixbuf image)
			{
				this.name = name;
				this.getter = getter;
				this.image = image;
			}
		}
		
		readonly static Gdk.Pixbuf methodPb   = Gdk.Pixbuf.LoadFromResource("method.png");
		readonly static Gdk.Pixbuf signalPb   = Gdk.Pixbuf.LoadFromResource("event.png");
		readonly static Gdk.Pixbuf propertyPb = Gdk.Pixbuf.LoadFromResource("property.png");
		
		public static IElement FromMethodDefinition(string returnType, string name, Argument[] args)
		{
			return new Element(name, delegate {
				string parameters = string.Empty;
				if (args != null) {
					foreach (Argument parameter in args) {
						parameters += (parameters.Length == 0 ? string.Empty : ", ") + Parser.ParseDBusTypeExpression(parameter.Type) + " " + parameter.Name;
					}
				}
				string cStyle = Parser.ParseDBusTypeExpression(returnType) + " " + name + " (" + parameters + ")";
				
				parameters = string.Empty;
				if (args != null) {
					foreach (Argument parameter in args) {
						parameters += (parameters.Length == 0 ? string.Empty : ", ") + parameter.Name + ": " + parameter.Type;
					}
				}
				string specDesc = name + " (" + parameters + ") : " + returnType;
				
				return new ElementRepresentation(specDesc, cStyle);
			}, methodPb);
		}
		
		public static IElement FromSignalDefinition(string name, Argument parameter)
		{
			return new Element(name, delegate {
				string spec = "signal " + name + " : " + parameter.Type;
				string cdecl = "event EventHandler<" + Parser.ParseDBusTypeExpression(parameter.Type) + "> " + name;
				
				return new ElementRepresentation(spec, cdecl);
			}, signalPb);
		}
		
		public static IElement FromPropertyDefinition(string name, Argument type, PropertyAccess access)
		{
			return new Element(name, delegate {
				string spec = access.ToString().ToLowerInvariant() + "property " + name + " : " + type.Type;
			
				string cdecl = Parser.ParseDBusTypeExpression(type.Type) + " " + name + " { ";
				if (access == PropertyAccess.Read  || access == PropertyAccess.ReadWrite) cdecl += "get; ";
				if (access == PropertyAccess.Write || access == PropertyAccess.ReadWrite) cdecl += "set; ";
				cdecl += "}";
				
				return new ElementRepresentation(spec, cdecl);
			}, propertyPb);
		}
	}
}
