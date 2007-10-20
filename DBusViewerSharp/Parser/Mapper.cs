// Mapper.cs created with MonoDevelop
// User: jeremie at 13:42 10/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

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
