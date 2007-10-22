// PathContainer.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusViewerSharp
{
	public class PathContainer
	{
		Interface[] interfaces;
		string path;
		
		public Interface[] Interfaces {
			get {
				return interfaces;
			}
		}

		public string Path {
			get {
				return path;
			}
		}
		
		public PathContainer(string path, Interface[] interfaces)
		{
			this.path = path;
			this.interfaces = interfaces;
		}
	}
}
