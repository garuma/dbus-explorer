// IElement.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Reflection;

namespace DBusViewerSharp
{
	public interface IElement
	{
		ElementRepresentation Visual { get; }
		// The symbol name
		string Name { get; }
	}
}
