// PathContainer.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	public class PathContainer
	{
		IEnumerable<Interface> interfaces;
		string path;
		
		public IEnumerable<Interface> Interfaces {
			get {
				return interfaces;
			}
		}

		public string Path {
			get {
				return path;
			}
		}
		
		public PathContainer(string path, IEnumerable<Interface> interfaces)
		{
			this.path = path;
			this.interfaces = interfaces;
		}
	}
}
