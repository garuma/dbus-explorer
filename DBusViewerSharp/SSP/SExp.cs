// SExp.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections;
using System.Collections.Generic;

namespace DBusExplorer
{
	public class SExp: IValue, IEnumerable<IValue>
	{
		public readonly string Name;
		IEnumerable<IValue> content;
		
		public SExp(IEnumerable<IValue> content, string name)
		{
			Name = name;
			this.content = content;
		}
		
		IEnumerator<IValue> IEnumerable<IValue>.GetEnumerator()
		{
			return content.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)content.GetEnumerator();
		}
	}
}
