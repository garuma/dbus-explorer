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
	public class MethodCaller : BaseCaller
	{
		Bus bus;
		string name;
		string busName;
		ObjectPath path;
		InvocationData data;
		
		Func<object[], object> callFunc;
		
		public MethodCaller(Bus bus, string busName, ObjectPath path,
		                    string iname, string name, InvocationData data)
			: base (iname)
		{
			this.bus = bus;
			this.busName = busName;
			this.path = path;
			this.name = name;
			this.data = data;
		}
		
		protected override void CreateMember ()
		{	
			MethodAttributes attrs = MethodAttributes.Abstract | MethodAttributes.Public | MethodAttributes.Virtual;
			builder.DefineMethod (name, attrs,
			                      GetReturnType (data.ReturnType),
			                      GetArgumentList (data.Args));
			
			Type proxyType = builder.CreateType ();
			object obj = bus.GetObject (proxyType, busName, path);
			MethodInfo mi = proxyType.GetMethod (name);
			callFunc = (os) => mi.Invoke (obj, os);
		}
		
		Type GetReturnType (string returnType)
		{
			if (string.IsNullOrEmpty(returnType) || returnType == "e")
				return typeof(void);
			
			return Parse (returnType);
		}
		
		Type[] GetArgumentList (IEnumerable<Argument> argsType)
		{
			return argsType == null ?
				Type.EmptyTypes : argsType.Select (a => Parse(a.Type)).ToArray();
		}
		
		protected override object InvokeInternal (object[] ps)
		{			
			return callFunc(ps);
		}
		
	}
}
