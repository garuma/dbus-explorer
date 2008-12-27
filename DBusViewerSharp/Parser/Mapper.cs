// Mapper.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	public static class Mapper
	{
		static Dictionary<DType,Type> fundamentalTypes = new Dictionary<DType,Type>();
		
		static Mapper()
		{
			fundamentalTypes.Add(DType.Boolean, typeof(bool));
			fundamentalTypes.Add(DType.Byte, typeof(byte));
			fundamentalTypes.Add(DType.Double, typeof(double));
			fundamentalTypes.Add(DType.Int16, typeof(short));
			fundamentalTypes.Add(DType.Int32, typeof(int));
			fundamentalTypes.Add(DType.Int64, typeof(long));
			fundamentalTypes.Add(DType.Single, typeof(float));
			fundamentalTypes.Add(DType.String, typeof(string));
			fundamentalTypes.Add(DType.UInt16, typeof(ushort));
			fundamentalTypes.Add(DType.UInt32, typeof(uint));
			fundamentalTypes.Add(DType.UInt64, typeof(ulong));
			fundamentalTypes.Add(DType.Variant, typeof(object));
		}
		
		public static Type DTypeToType(DType type)
		{
			Type temp = null;
			if (!fundamentalTypes.TryGetValue(type, out temp))
				temp = null;
			
			return temp;
		}
		
		public static string DTypeToStr(DType type)
		{
			return type.ToString();
		}
	}
}
