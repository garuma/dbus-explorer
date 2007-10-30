// MethodInvokator.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Reflection;
using System.Reflection.Emit;

using NDesk.DBus;

namespace DBusViewerSharp
{
	public class MethodInvokator
	{
		static AssemblyBuilder asmBuilder;
		static ModuleBuilder modBuilder;
		static uint count = 0;
		
		static MethodInvokator()
		{
			asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("DBusViewerSharpInvokator"),
			                                                           AssemblyBuilderAccess.Run);
			modBuilder = asmBuilder.DefineDynamicModule("DBusViewerSharpInvoke");
		}
		
		Bus bus;
		
		public MethodInvokator(Bus bus)
		{
			this.bus = bus;
		}
		
		public TReturn Invoke<TReturn>(IElement element, string bus_name, string objectPath)
		{
			/*InvocationData data = element.Data;
			
			TypeBuilder tb = modBuilder.DefineType("Interface" + count++.ToString(), TypeAttributes.Interface);
			MethodBuilder mb = tb.DefineMethod(element.Name, MethodAttributes.Public);
			mb.SetParameters(data.Args);
			mb.SetReturnType(data.ReturnValue);
			Type myInterface = tb.CreateType();
			
			MethodInfo mi = typeof(bus).GetMethod("GetObject");
			mi.Invoke(bus, myInterface, bus_name, new ObjectPath(objectPath));*/
			
			
		}
	}
}
