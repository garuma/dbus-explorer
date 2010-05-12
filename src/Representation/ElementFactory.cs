// ElementFactory.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
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
		IDictionary<ILangDefinition, IParserVisitor<string>> visitors
			= new Dictionary<ILangDefinition, IParserVisitor<string>>();
		StringBuilder sb = new StringBuilder(20);
		
		public ElementFactory(IEnumerable<ILangDefinition> langs)
		{
			if (langs == null)
				throw new ArgumentNullException("langs");
			
			foreach (ILangDefinition def in langs) {
				visitors.Add(def, new LangDefVisitor(def));
			}
		}
		
		readonly static Gdk.Pixbuf methodPb   = Gdk.Pixbuf.LoadFromResource("DBusExplorer.data.method.png");
		readonly static Gdk.Pixbuf signalPb   = Gdk.Pixbuf.LoadFromResource("DBusExplorer.data.event.png");
		readonly static Gdk.Pixbuf propertyPb = Gdk.Pixbuf.LoadFromResource("DBusExplorer.data.property.png");
		
		public IElement FromMethodDefinition(string returnType, string name, IEnumerable<Argument> args)
		{
			string specDesc = Concat (name, " (", MakeArgumentList(args, ", ", "{N} : {T}"),
			                          ") : ", returnType);
			Dictionary<string, LangProcesser> temp = new Dictionary<string,LangProcesser>();
			
			foreach (KeyValuePair<ILangDefinition, IParserVisitor<string>> visitor in visitors) {
				temp.Add(visitor.Key.Name, delegate {
					string retRealType = Parser.ParseDBusTypeExpression(returnType, visitor.Value);
					List<Argument> argsReal = new List<Argument>();
					if (args != null) {
					  foreach (Argument arg in args)
						argsReal.Add(new Argument(Parser.ParseDBusTypeExpression(arg.Type, visitor.Value),
							                          arg.Name));
					}
					return visitor.Key.MethodFormat(name, retRealType, argsReal);
				});
			}
			
			Element elem = new Element(name, new ElementRepresentation(specDesc, temp), methodPb, 0);
			elem.Data = new InvocationData (returnType, args);
			
			return elem;
		}
		
		public IElement FromSignalDefinition(string name, IEnumerable<Argument> args)
		{
			string spec = Concat ("signal ", name, " : ", MakeArgumentList(args, ", ", "{T}"));
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
		
		public IElement FromPropertyDefinition(string name, string type, PropertyAccess access)
		{
		  string spec = Concat (access.ToString().ToLowerInvariant(),
			                      " property ", name, " : ", type);
		  Dictionary<string, LangProcesser> temp = new Dictionary<string,LangProcesser>();
		  
		  foreach (KeyValuePair<ILangDefinition, IParserVisitor<string>> visitor in visitors) {
			temp.Add(visitor.Key.Name, delegate {
				string realType = Parser.ParseDBusTypeExpression(type, visitor.Value);
				return visitor.Key.PropertyFormat(name, realType, access); 
			  });
			}
		  
		  return new Element(name, new ElementRepresentation(spec, temp), propertyPb, 3);
		}
		
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
		
		string Concat(params string[] strings)
		{
			sb.Remove(0, sb.Length);
			foreach (var s in strings)
				sb.Append (s);
			
			return sb.ToString ();
		}
	}
}
