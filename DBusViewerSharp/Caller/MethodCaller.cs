// 
// MethodCaller.cs
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
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;

using NDesk.DBus;

namespace DBusExplorer
{
	public class MethodCaller
	{
		static ReflectionVisitor visitor = new ReflectionVisitor ();
		static ParserNg<Type> parser = new ParserNg<Type> ();
		static int id = int.MinValue;
		
		TypeBuilder builder;
		Bus bus;
		string iname;
		string name;
		string busName;
		ObjectPath path;
		Func<object[], object> callFunc;
		
		// Some caching
		
		public MethodCaller(Bus bus, string busName, ObjectPath path,
		                    string iname, string name, InvocationData data)
		{
			this.bus = bus;
			this.busName = busName;
			this.path = path;
			this.name = name;
			this.iname = iname;
			
			SetupBuilder ();
			CreateMethod(name, data.ReturnType, data.Args);
		}
		
		static ModuleBuilder mb;
		static System.Reflection.ConstructorInfo ci
			= typeof(InterfaceAttribute).GetConstructor (new Type[] { typeof(string) });
		
		static MethodCaller ()
		{
			AssemblyName aName = new AssemblyName("DBusExplorerProxies");
			AssemblyBuilder ab = 
				AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
			
			mb = ab.DefineDynamicModule(aName.Name);
		}
		
		void SetupBuilder ()
		{
			TypeAttributes attrs = TypeAttributes.Interface | TypeAttributes.Public | TypeAttributes.Abstract;
			builder = mb.DefineType ("IDBusExplorerProxy" + id++, attrs);
			builder.SetCustomAttribute (new CustomAttributeBuilder (ci, new object [] { iname }));
		}
		
		void CreateMethod (string name, string returnType, IEnumerable<Argument> argsType)
		{
			MethodAttributes attrs = MethodAttributes.Abstract | MethodAttributes.Public | MethodAttributes.Virtual;
			builder.DefineMethod (name, attrs,
			                      GetReturnType (returnType),
			                      GetArgumentList (argsType));
									                      
		}
		
		Type GetReturnType (string returnType)
		{
			if (string.IsNullOrEmpty(returnType) || returnType == "e")
				return typeof(void);
			
			return Parse(returnType);
		}
		
		Type[] GetArgumentList (IEnumerable<Argument> argsType)
		{
			return argsType == null ?
				Type.EmptyTypes : argsType.Select (a => Parse(a.Type)).ToArray();
		}
		
		Type Parse (string type)
		{
			Type t = parser.ParseDBusTypeExpression (type, visitor);
			return t;
		}
		
		public object Invoke (object[] ps)
		{
			if (callFunc == null) {
				Type proxyType = builder.CreateType ();
				object obj = bus.GetObject (proxyType, busName, path);
				MethodInfo mi = proxyType.GetMethod (name);
				callFunc = (os) => mi.Invoke (obj, os);
			}
			
			return callFunc(ps);
		}
	}
}
