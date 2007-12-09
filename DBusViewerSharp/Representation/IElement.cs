// IElement.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Reflection;

namespace DBusExplorer
{
	public interface IElement: IComparable<IElement>
	{
		ElementRepresentation Visual { get; }
		Gdk.Pixbuf Image { get; }
		// The symbol name
		string Name { get; }
		//InvocationData Data { get; }
	}
}
