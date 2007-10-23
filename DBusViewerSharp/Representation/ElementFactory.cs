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
		private class Element: IElement
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
		
		static Gdk.Pixbuf methodPb = Gdk.Pixbuf.LoadFromResource("method.png");
		
		
		public static IElement FromMethodDefinition(string returnType, string name, Argument[] args)
		{
			string parameters = string.Empty;
			
			if (args != null) {
				foreach (Argument parameter in args) {
					Console.WriteLine("Beginning parsing of : " + parameter.Type);
					parameters += (parameters.Length == 0 ? string.Empty : ", ") + Parser.ParseDBusTypeExpression(parameter.Type) + " " + parameter.Name;
					Console.WriteLine("Finished parsing of : " + parameter.Type);
				}
			}
			
			Console.WriteLine("Beginning parsing in cStyle of : " + returnType);
			string cStyle = Parser.ParseDBusTypeExpression(returnType) + " " + name + " (" + parameters + ")";
			Console.WriteLine("Finished parsing in cStyle of : " + returnType);
			
			parameters = string.Empty;
			
			if (args != null) {
				foreach (Argument parameter in args) {
					parameters += (parameters.Length == 0 ? string.Empty : ", ") + parameter.Name + ": " + parameter.Type;
				}
			}
			
			string specDesc = name + " (" + parameters + ") : " + returnType;
					
			return new Element(name, new ElementRepresentation(methodPb, specDesc, cStyle));
		}
	}
}
