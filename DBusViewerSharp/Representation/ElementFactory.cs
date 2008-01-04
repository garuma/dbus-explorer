// ElementFactory.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Text;
using System.Collections.Generic;

namespace DBusExplorer
{
	public static class ElementFactory
	{
		private class Element: IElement, IComparable<IElement>
		{
			ElementRepresentation representation = null;
			string name;
			Gdk.Pixbuf image;
			// For comparison
			int type;
			Interface parent;
			
			public ElementRepresentation Visual {
				get {
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
			
			public Interface Parent { 
				get {
					return parent;
				}
				set {
					parent = value;
				}
			}
			
			public int CompareTo(IElement other)
			{
				// First comparison
				int comparison = String.Compare(this.name, other.Name, StringComparison.Ordinal);
				// Then with the types
				Element elem = (Element)other;
				if (elem.type == this.type)
					return comparison;
				else
					return this.type.CompareTo(elem.type);
			}

			public Element(string name, ElementRepresentation representation, Gdk.Pixbuf image, int type)
			{
				this.name = name;
				this.representation = representation;
				this.image = image;
				this.type = type;
			}
		}
		
		readonly static Gdk.Pixbuf methodPb   = Gdk.Pixbuf.LoadFromResource("method.png");
		readonly static Gdk.Pixbuf signalPb   = Gdk.Pixbuf.LoadFromResource("event.png");
		readonly static Gdk.Pixbuf propertyPb = Gdk.Pixbuf.LoadFromResource("property.png");
		
		public static IElement FromMethodDefinition(string returnType, string name, IEnumerable<Argument> args)
		{
			string cStyle = Parser.ParseDBusTypeExpression(returnType) + " " + name + " (" + MakeArgumentList(args, ", ", "{T} {N}", true) + ")";
			string specDesc = name + " (" + MakeArgumentList(args, ", ", "{N} : {T}", false) + ") : " + returnType;
			
			return new Element(name, new ElementRepresentation(specDesc, cStyle), methodPb, 0);
		}
		
		public static IElement FromSignalDefinition(string name, IEnumerable<Argument> args)
		{
			string spec = "signal " + name + " : " + MakeArgumentList(args, ", ", "{T}", false);
			string cdecl = "event EventHandler<" + MakeArgumentList(args, ", ", "{T}", true) + "> " + name;
			
			return new Element(name, new ElementRepresentation(spec, cdecl), signalPb, 2);
		}
		
		public static IElement FromPropertyDefinition(string name, Argument type, PropertyAccess access)
		{
			string spec = access.ToString().ToLowerInvariant() + "property " + name + " : " + type.Type;
			string cdecl = Parser.ParseDBusTypeExpression(type.Type) + " " + name + " { ";
			if (access == PropertyAccess.Read  || access == PropertyAccess.ReadWrite) cdecl += "get; ";
			if (access == PropertyAccess.Write || access == PropertyAccess.ReadWrite) cdecl += "set; ";
			cdecl += "}";

			return new Element(name, new ElementRepresentation(spec, cdecl), propertyPb, 1);
		}
		
		static StringBuilder sb = new StringBuilder(20);
		
		static string MakeArgumentList(IEnumerable<Argument> args, string separator, string format, bool parse)
		{
			if (args == null)
				return string.Empty;
			
			sb.Remove(0, sb.Length);
			foreach (Argument arg in args) {
				sb.Append(sb.Length == 0 ? string.Empty : separator);
				sb.Append(format.Replace("{T}", parse ? Parser.ParseDBusTypeExpression(arg.Type) : arg.Type)
				          .Replace("{N}", arg.Name));				
			}
			return sb.ToString();
		}
	}
}
