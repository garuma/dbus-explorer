// InvocationData.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusViewerSharp
{
	public class InvocationData
	{
		Type returnValue;
		Type[] args;
		
		public Type ReturnValue {
			get {
				return returnValue;
			}
		}

		public Type[] Args {
			get {
				return args;
			}
		}
		
		public InvocationData(Type returnValue, Type[] args)
		{
			this.returnValue = returnValue;
			this.args = args;
		}
	}
}
