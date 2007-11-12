// Types.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;

namespace DBusExplorer
{
	public interface IValue
	{
	}
	
	public interface IAtom
	{
	}
	
	public class SString: IAtom
	{
		public readonly string Value;
		
		public SString(string value)
		{
			Value = value;
		}
	}
	
	public class SSymbol: IAtom
	{
		public readonly string Name;
		
		public SSymbol(string name)
		{
			this.Name = name;
		}
	}
}
