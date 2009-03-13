// 
// ReflectionVisitor.cs
//  
// Author:
//       Jérémie "Garuma" Laval <jeremie.laval@gmail.com>
// 
// Copyright (c) 2009 Jérémie "Garuma" Laval
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Reflection;
using System.Reflection.Emit;

using System.Collections.Generic;

namespace DBusExplorer
{
	public class ReflectionVisitor : IParserVisitor<Type>
	{
		ParserNg<Type> parent;
		
		public ReflectionVisitor (ParserNg<Type> parent)
		{
			this.parent = parent;
		}

		#region IParserVisitor<Type> implementation
		public Type ParseStructDefinition (IEnumerator<DType> tokens)
		{
			return null;
		}
		
		public Type ParseArrayDefinition (IEnumerator<DType> tokens)
		{
			IList<Type> tmp = parent.ConvertFromExprList(tokens, this, 1);
			
			return tmp.Count > 0 ? tmp[0].MakeArrayType() : Error;
		}
		
		public Type ParseDictDefinition (IEnumerator<DType> tokens)
		{
			
			return null;
		}
		
		public Type ParseBaseTypeDefinition (DType type)
		{
			return Mapper.DTypeToType(type);
		}
		
		public Type Default {
			get {
				return typeof(Object);
			}
		}
		
		public Type Error {
			get {
				return typeof(Object);
			}
		}
		#endregion
		public ReflectionVisitor()
		{
		}
	}
}
