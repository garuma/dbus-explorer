// 
// LangDefinition.cs
//  
// Author:
//       Jérémie "Garuma" Laval <jeremie.laval@gmail.com>
// 
// Copyright (c) 2009 Jérémie "Garuma" Laval
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	
	internal class LangDefinition: ILangDefinition
	{
		Dictionary<DType, string> types;
		string                    name;
		Func<string, string, IEnumerable<Argument>, string>      methDeleg;
		Func<string, IEnumerable<Argument>, string>       evtDeleg;
		Func<string, string, PropertyAccess, string>    propDelegate;
		Func<string, string, string>  dictDeleg;
		Func<IEnumerable<string>, string>      structDeleg;
		Func<string, string>       arrayDeleg;
		
		
		public LangDefinition(Dictionary<DType, string>  types,
		                      string name,
		                      Func<string, string, IEnumerable<Argument>, string> methDeleg,
		                      Func<string, IEnumerable<Argument>, string> evtDeleg,
		                      Func<string, string, PropertyAccess, string> propDelegate,
		                      Func<string, string, string> dictDeleg,
		                      Func<IEnumerable<string>, string> structDeleg,
		                      Func<string, string> arrayDeleg)
		{
			this.types       = types;
			this.name        = name;
			this.methDeleg   = methDeleg;
			this.evtDeleg    = evtDeleg;
			this.propDelegate = propDelegate;
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

		public string EventFormat (string name, IEnumerable<Argument> args)
		{
			return evtDeleg(name, args);
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

		public string PropertyFormat (string name, string type, PropertyAccess access)
		{
			return propDelegate(name, type, access);
		}
		
		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
		
		public override string ToString()
		{
			string s = "LangDefinition of " + name;
			s += Environment.NewLine;
			s += "Types:";
			s += Environment.NewLine;
			foreach (var type in types) {
				s += string.Format("\t{0} = > {1}", type.Key, type.Value);
				s += Environment.NewLine;
			}
			return s;
		}
	}
}
