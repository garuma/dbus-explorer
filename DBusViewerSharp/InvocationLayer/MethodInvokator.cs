// MethodInvokator.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DBusViewerSharp
{
	public class MethodInvokator
	{
		static AssemblyBuilder asmBuilder;
		static ModuleBuilder modBuilder;
		
		static MethodInvokator()
		{
			asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("DBusViewerSharpInvokator"),
			                                                           AssemblyBuilderAccess.Run);
			modBuilder = asmBuilder.DefineDynamicModule("DBusViewerSharpInvoke");
		}
		
		public MethodInvokator()
		{
		}
	}
}
