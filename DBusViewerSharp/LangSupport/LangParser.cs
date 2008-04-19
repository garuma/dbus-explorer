// Parser.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Generic;

namespace DBusExplorer
{
	public static class LangParser
	{
		static XPathDocument doc;
		static XPathNavigator nav;
		
		public static ILangDefinition ParseFromFile(string path)
		{
			if (!File.Exists(path))
				Logging.Warning("Lang definition file is missing at : " + path);
			
			doc = new XPathDocument(new StreamReader(path));
			nav = doc.CreateNavigator();
			if (nav == null)
				throw new ApplicationException("Error the XPathNavigator for the document ("+path+") is null");
			
			return new LangDefinition(GetTypes(), GetName(), GetMethodFormating(), null, null, null, null);
		}
		
		static Dictionary<DType, string> GetTypes()
		{
			Dictionary<DType, string> types = new Dictionary<DType,string>();
			foreach (XPathNavigator t in nav.Select("map")) {
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
		
		static MethodFormatDelegate GetMethodFormating()
		{
			XPathNavigator methodNode = nav.SelectSingleNode("//method");
			if (methodNode == null)
				throw new ApplicationException("Parsing error : there is no method node in the file");
			string general = methodNode.GetAttribute("general", string.Empty);
			ArgsFormatingDelegate argsFormat = GetArgsFormating(methodNode.SelectSingleNode("arguments"));
			
			return delegate (string name, string returnType, IEnumerable<Argument> args) {
				return general.Replace("%{return}", returnType).Replace("%{name}", name).Replace("%{args}", argsFormat(args));
			};
		}
		
		static EventFormatDelegate GetEventFormating()
		{
			XPathNavigator eventNode = nav.SelectSingleNode("//event");
			string general = eventNode.GetAttribute("general", string.Empty);
			ArgsFormatingDelegate argsFormat = GetArgsFormating(eventNode.SelectSingleNode("arguments"));
			
			return null;
		}
		
		static ArgsFormatingDelegate GetArgsFormating(XPathNavigator argsNode)
		{
			string accumulator = argsNode.GetAttribute("accumulator", string.Empty);
			string start = argsNode.GetAttribute("start", string.Empty);
			string end = argsNode.GetAttribute("end", string.Empty);
			string general = argsNode.GetAttribute("general", string.Empty);
			
			return delegate (IEnumerable<Argument> args) {
				// Should use Linq when Mono gets older
				string temp = start;
				foreach (Argument tuple in args) {
					temp += general.Replace("%{arg-type}", tuple.Type).Replace("%{arg-name}", tuple.Name);
					temp += accumulator;
				}
				temp += end;
				
				return temp;
			};
		}
		
		delegate string MethodFormatDelegate(string name, string returnType, IEnumerable<Argument> args);
		delegate string EventFormatDelegate(string name, IEnumerable<Argument> args);
		delegate string DictionaryFormatDelegate(string type1, string type2);
		delegate string StructFormatDelegate(IEnumerable<string> types);
		delegate string ArrayFormatDelegate(string type);
		
		delegate string ArgsFormatingDelegate(IEnumerable<Argument> args);
		
		class LangDefinition: ILangDefinition
		{
			Dictionary<DType, string> types;
			string                    name;
			MethodFormatDelegate      methDeleg;
			EventFormatDelegate       evtDeleg;
			DictionaryFormatDelegate  dictDeleg;
			StructFormatDelegate      structDeleg;
			ArrayFormatDelegate       arrayDeleg;
			
			
			public LangDefinition(Dictionary<DType, string>  types,
									string                   name,
									MethodFormatDelegate     methDeleg,
			                        EventFormatDelegate      evtDeleg,
			                        DictionaryFormatDelegate dictDeleg,
			                        StructFormatDelegate     structDeleg,
			                        ArrayFormatDelegate      arrayDeleg)
			{
				this.types       = types;
				this.name        = name;
				this.methDeleg   = methDeleg;
				this.evtDeleg    = evtDeleg;
				this.dictDeleg   = dictDeleg;
				this.structDeleg = structDeleg;
				this.arrayDeleg  = arrayDeleg;
			}
			
			public string Name {
				get {
					return name;
				}
			}

			public IDictionary<DType, string> Types {
				get {
					return types;
				}
			}

			public string MethodFormat (string name, string returnType, IEnumerable<Argument> args)
			{
				return methDeleg(name, returnType, args);
			}

			public string EventFormat (string name, IEnumerable<KeyValuePair<string, string>> args)
			{
				return string.Empty;
				//return evtDeleg(name, args);
			}
			
			public string DictionaryFormat(string type1, string type2)
			{
				return dictDeleg(type1, type2);
			}
			
			public string StructFormat(IEnumerable<string> types)
			{
				return structDeleg(types);
			}
			
			public string ArrayFormat(string type)
			{
				return arrayDeleg(type);
			}

			public string PropertyFormat ()
			{
				throw new NotImplementedException();
			}
			
			public override int GetHashCode()
			{
				return name.GetHashCode();
			}
		}
	}
}
