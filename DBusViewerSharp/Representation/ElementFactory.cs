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
	public class ElementFactory
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
		
		IDictionary<ILangDefinition, IParserVisitor<string>> visitors = new Dictionary<ILangDefinition, IParserVisitor<string>>();
		
		public ElementFactory(IEnumerable<ILangDefinition> langs)
		{
			if (langs == null)
				throw new ArgumentNullException("langs");
			
			foreach (ILangDefinition def in langs) {
				visitors.Add(def, new LangDefVisitor(def, Parser.RealParser));
			}
		}
		
		readonly static Gdk.Pixbuf methodPb   = Gdk.Pixbuf.LoadFromResource("method.png");
		readonly static Gdk.Pixbuf signalPb   = Gdk.Pixbuf.LoadFromResource("event.png");
		readonly static Gdk.Pixbuf propertyPb = Gdk.Pixbuf.LoadFromResource("property.png");
		
		public IElement FromMethodDefinition(string returnType, string name, IEnumerable<Argument> args)
		{
			string specDesc = name + " (" + MakeArgumentList(args, ", ", "{N} : {T}") + ") : " + returnType;
			Dictionary<string, LangProcesser> temp = new Dictionary<string,LangProcesser>();
			
			foreach (KeyValuePair<ILangDefinition, IParserVisitor<string>> visitor in visitors) {
				temp.Add(visitor.Key.Name, delegate {
					string retRealType = Parser.ParseDBusTypeExpression(returnType, visitor.Value);
					List<Argument> argsReal = new List<Argument>();
					if (args != null) {
						foreach (Argument arg in args)
							argsReal.Add(new Argument(Parser.ParseDBusTypeExpression(arg.Type, visitor.Value), arg.Name));
					}
					return visitor.Key.MethodFormat(name, retRealType, argsReal); 
				});
			}
			
			return new Element(name, new ElementRepresentation(specDesc, temp), methodPb, 0);
		}
		
		public IElement FromSignalDefinition(string name, IEnumerable<Argument> args)
		{
			string spec = "signal " + name + " : " + MakeArgumentList(args, ", ", "{T}");
			Dictionary<string, LangProcesser> temp = new Dictionary<string,LangProcesser>();
			
			foreach (KeyValuePair<ILangDefinition, IParserVisitor<string>> visitor in visitors) {
				temp.Add(visitor.Key.Name, delegate {
					List<Argument> argsReal = new List<Argument>();
					if (args != null) {
						foreach (Argument arg in args)
							argsReal.Add(new Argument(Parser.ParseDBusTypeExpression(arg.Type, visitor.Value), arg.Name));
					}
					return visitor.Key.EventFormat(name, argsReal); 
				});
			}
			
			return new Element(name, new ElementRepresentation(spec, temp), signalPb, 2);
		}
		
		public IElement FromPropertyDefinition(string name, Argument type, PropertyAccess access)
		{
			/*string spec = access.ToString().ToLowerInvariant() + "property " + name + " : " + type.Type;
			//string cdecl = Parser.ParseDBusTypeExpression(type.Type) + " " + name + " { ";
			if (access == PropertyAccess.Read  || access == PropertyAccess.ReadWrite) cdecl += "get; ";
			if (access == PropertyAccess.Write || access == PropertyAccess.ReadWrite) cdecl += "set; ";
			cdecl += "}";

			return new Element(name, new ElementRepresentation(spec, cdecl), propertyPb, 1);*/
			return null;
		}
		
		StringBuilder sb = new StringBuilder(20);
		
		string MakeArgumentList(IEnumerable<Argument> args, string separator, string format)
		{
			if (args == null)
				return string.Empty;
			
			sb.Remove(0, sb.Length);
			foreach (Argument arg in args) {
				sb.Append(sb.Length == 0 ? string.Empty : separator);
				sb.Append(format.Replace("{T}", arg.Type)
				          .Replace("{N}", arg.Name));				
			}
			return sb.ToString();
		}
	}
}
