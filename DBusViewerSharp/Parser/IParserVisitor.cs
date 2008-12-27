// IParserVisitor.cs
// Copyright (c) 2008-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.Collections.Generic;

namespace DBusExplorer
{
	public interface IParserVisitor<TReturn>
	{
		TReturn ParseStructDefinition(IEnumerator<DType> tokens);
		
		TReturn ParseArrayDefinition(IEnumerator<DType> tokens);
		
		TReturn ParseDictDefinition(IEnumerator<DType> tokens);
		
		TReturn ParseBaseTypeDefinition(DType type);
		
		TReturn Default { get; }
		TReturn Error { get; }
	}
}
