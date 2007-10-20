// MethodInvokator.cs created with MonoDevelop
// User: jeremie at 14:09 10/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

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
