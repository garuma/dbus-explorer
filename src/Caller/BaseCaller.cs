// 
// BaseCaller.cs
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

using DBus;

namespace DBusExplorer
{
	
	
	public abstract class BaseCaller
	{
		static readonly ReflectionVisitor visitor = new ReflectionVisitor ();
		static readonly ParserNg<Type> parser = new ParserNg<Type> ();
		static int id = int.MinValue;
		
		protected TypeBuilder builder;
		readonly string iname;
		bool memberCreated;
		
		static ModuleBuilder mb;
		static System.Reflection.ConstructorInfo ci
			= typeof(InterfaceAttribute).GetConstructor (new Type[] { typeof(string) });
		
		protected BaseCaller (string iname)
		{
			this.iname = iname;
			SetupBuilder ();
		}
		
		static BaseCaller ()
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
		
		public object Invoke (object[] ps)
		{
			if (!memberCreated) {
				CreateMember ();
				memberCreated = true;
			}
			return InvokeInternal (ps);
		}
		
		protected Type Parse (string type)
		{
			Type t = parser.ParseDBusTypeExpression (type, visitor);
			return t;
		}
		
		protected abstract void CreateMember ();
		protected abstract object InvokeInternal (object[] ps);
	}
}
