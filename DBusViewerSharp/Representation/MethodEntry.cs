// MethodEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Collections.Generic;

namespace DBusViewerSharp
{
	public class MethodEntry: IEntry
	{
		ArgEntry[] argParams;
		string   methodName;
		string   returnType;
		
		ElementRepresentation representation;
		
		static Gdk.Pixbuf pixbuf = Gdk.Pixbuf.LoadFromResource("method.png");
		
		public ArgEntry[] Parameters {
			get {
				return argParams;
			}
		}

		public string MethodName {
			get {
				return methodName;
			}
		}

		public string ReturnType {
			get {
				return returnType;
			}
		}
		
		public ElementRepresentation Visual {
			get {
				return representation;
			}
		}
		
		public string Name {
			get {
				return methodName;
			}
		}
		
		public MethodEntry(string methodName, string returnType, ArgEntry[] argParams)
		{
			this.methodName = methodName;
			this.returnType = returnType;
			this.argParams = argParams;
			
			this.representation = InitRepresentation();
		}

		public ElementRepresentation InitRepresentation()
		{			
			string parameters = string.Empty;
			
			if (argParams != null) {
				foreach (ArgEntry parameter in argParams) {
					parameters += (parameters.Length == 0 ? string.Empty : ", ") + Parser.ParseDBusTypeExpression(parameter.Type) + " " + parameter.Name;
				}
			}
				
			string cStyle = Parser.ParseDBusTypeExpression(returnType) + " " + methodName + " (" + parameters + ")";
			
			parameters = string.Empty;
			
			if (argParams != null) {
				foreach (ArgEntry parameter in argParams) {
					parameters += (parameters.Length == 0 ? string.Empty : ", ") + parameter.Name + ": " + parameter.Type;
				}
			}
			string specDesc = methodName + " (" + parameters + ") : " + returnType;
					
			return new ElementRepresentation(pixbuf, specDesc, cStyle);
		}
		
		
	}
}
