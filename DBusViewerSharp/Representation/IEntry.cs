// IEntry.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;

using Gtk;

namespace DBusViewerSharp
{
	public interface IEntry
	{
		ElementRepresentation Visual { get; }
		string Name { get; }
	}
}
