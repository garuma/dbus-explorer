// Parser.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using System.Xml.XPath;
using System.Collections.Generic;

namespace DBusExplorer
{
	public static class LangParser
	{
		static XPathDocument doc;
		static XPathNavigator nav;
		static StringBuilder sb = new StringBuilder(20);
		
		public static ILangDefinition ParseFromFile(string path)
		{
			if (!File.Exists(path))
				Logging.Warning("Lang definition file is missing at : " + path);
			
			doc = new XPathDocument(new StreamReader(path));
			nav = doc.CreateNavigator();
			if (nav == null)
				throw new ApplicationException("Error the XPathNavigator for the document ("+path+") is null");
			
			LangDefinition def = new LangDefinition(GetTypes(), GetName(), GetMethodFormating(), GetEventFormating(),
			                                        GetPropertyFormating(), GetDictionaryFormating(), GetStructFormating(), GetArrayFormating());
			return def;
		}
		
		static Dictionary<DType, string> GetTypes()
		{
			Dictionary<DType, string> types = new Dictionary<DType,string>();
			foreach (XPathNavigator t in nav.Select("//map")) {
				DType temp;
				try {
					temp = (DType)Enum.Parse(typeof(DType), t.GetAttribute("src", string.Empty));
				} catch { continue; }

				types.Add(temp, t.GetAttribute("to", string.Empty));
			}
			
			return types;
		}
		
		static string GetName()
		{
			return nav.SelectSingleNode("language").GetAttribute("name", string.Empty);
		}
		
		static Func<string, string, IEnumerable<Argument>, string> GetMethodFormating()
		{
			XPathNavigator methodNode = nav.SelectSingleNode("//method");
			if (methodNode == null)
				throw new ApplicationException("Parsing error : there is no method node in the file");
			
			string general = methodNode.GetAttribute("general", string.Empty);
			Func<IEnumerable<Argument>, string> argsFormat = GetArgsFormating(methodNode.SelectSingleNode("arguments"));
			
			return delegate (string name, string returnType, IEnumerable<Argument> args) {
				return Replace(general, "%{return}", returnType,
				               "%{name}", name, "%{args}", argsFormat(args));
			};
		}
		
		static Func<string, IEnumerable<Argument>, string> GetEventFormating()
		{
			XPathNavigator eventNode = nav.SelectSingleNode("//event");
			if (eventNode == null)
			  throw new ApplicationException("Parsing error : there is no event node in the file");
			
			string general = eventNode.GetAttribute("general", string.Empty);
			Func<IEnumerable<Argument>, string> argsFormat = GetArgsFormating(eventNode.SelectSingleNode("arguments"));
			
			return delegate (string name, IEnumerable<Argument> args) {
				return Replace (general, "%{name}", name, "%{types}", argsFormat(args));
			};
		}

		static Func<string, string, PropertyAccess, string> GetPropertyFormating()
		{
			XPathNavigator propNode = nav.SelectSingleNode("//property");
			if (propNode == null)
				throw new ApplicationException("Parsing error : there is no property node in the file");
			
			string read = propNode.GetAttribute("read", string.Empty);
			string write = propNode.GetAttribute("write", string.Empty);
			string readwrite = propNode.GetAttribute("readwrite", string.Empty);
			
			return delegate (string name, string type, PropertyAccess access) {
				switch (access) {
				case PropertyAccess.Read:
					return InternalPropertyFormat (name, type, read);
					
				case PropertyAccess.Write:
					return InternalPropertyFormat (name, type, write);
					
				case PropertyAccess.ReadWrite:
					return InternalPropertyFormat (name, type, readwrite);
					
				default:
					return null;
				}
				
			};
		}
		
		static string InternalPropertyFormat (string name, string type, string format)
		{
			return Replace (format, "%{type}", type, "%{name}", name);
		}
		
		static Func<string, string, string> GetDictionaryFormating()
		{
			XPathNavigator dictNode = nav.SelectSingleNode("//dictionary");
			string general = dictNode.GetAttribute("general", string.Empty);
			
			return delegate (string type1, string type2) {
				return Replace (general, "%{type1}", type1, "%{type2}", type2);
			};
		}
		
		static Func<IEnumerable<string>,string> GetStructFormating()
		{
			XPathNavigator structNode = nav.SelectSingleNode("//struct");
			string prefix = structNode.GetAttribute("prefix", string.Empty);
			string suffix = structNode.GetAttribute("suffix", string.Empty);
			string general = structNode.GetAttribute("general", string.Empty);
			string accumulator = structNode.GetAttribute("accumulator", string.Empty);
			StringBuilder temp = new StringBuilder(20);
			
			return delegate (IEnumerable<string> types) {
				temp.Remove(0, temp.Length);
				temp.Append(prefix);
				foreach (var t in types) {
					temp.Append(general.Replace("%{type}", t));
					temp.Append(accumulator);
				}
				temp.Append(suffix);
				return temp.ToString();
			};
		}
		
		static Func<string, string> GetArrayFormating()
		{
			XPathNavigator arrayNode = nav.SelectSingleNode("//array");
			string general = arrayNode.GetAttribute("general", string.Empty);
			
			return delegate (string type) {
				return Replace (general, "%{type}", type);
			};
		}
		
		static Func<IEnumerable<Argument>, string> GetArgsFormating(XPathNavigator argsNode)
		{
			string accumulator = argsNode.GetAttribute("accumulator", string.Empty);
			string start = argsNode.GetAttribute("start", string.Empty);
			string end = argsNode.GetAttribute("end", string.Empty);
			string general = argsNode.GetAttribute("general", string.Empty);
			
			return GetArgsFormating(accumulator, start, end, general);
		}
		
		static Func<IEnumerable<Argument>, string> GetArgsFormating(string accumulator, string start,
		                                                            string end, string general)
		{
			StringBuilder temp = new StringBuilder(20);
			return delegate (IEnumerable<Argument> args) {
				if (!args.Any ())
					return string.Empty;
				
				temp.Remove (0, temp.Length);
				temp.Append(start);
				args.Aggregate (temp, (acc, t) => { 
					acc.Append (Replace(general, "%{type}", t.Type, "%{name}", t.Name));
					acc.Append (accumulator);
					return acc;
				});
				
				temp.Remove (temp.Length - accumulator.Length, accumulator.Length);
					
				temp.Append (end);
				
				return temp.ToString();
			};
		}
		
		static void CleanSb ()
		{
			sb.Remove (0, sb.Length);
		}
		
		static string Replace (params string[] replacement)
		{
			if (replacement.Length % 2 != 1)
				return string.Empty;
			
			CleanSb();
			sb.Append(replacement[0]);
			
			for (int i = 1; i < replacement.Length - 1; i += 2)
				sb.Replace (replacement[i], replacement[i + 1]);
			
			return sb.ToString ();
		}
	}
}
