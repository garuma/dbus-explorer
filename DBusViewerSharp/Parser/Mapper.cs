// Mapper.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Collections.Generic;

namespace DBusViewerSharp
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
		}
		
		public static Type DTypeToType(DType type)
		{
			Type temp = null;
			if (!fundamentalTypes.TryGetValue(type, out temp))
				temp = null;
			
			return temp;
		}
	}
}
