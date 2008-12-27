// Common.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusExplorer
{
	public class DBusErrorEventArgs : EventArgs
	{
		string errorMessage;
		
		public string ErrorMessage {
			get {
				return errorMessage;
			}
		}
		
		public DBusErrorEventArgs(string errorMessage)
		{
			this.errorMessage = errorMessage;
		}
	}
}
