// CodeDomGenerator.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace DBusExplorer
{
	public class CodeDomGenerator: IGenerator
	{
		ICodeGenerator generator;
		CodeGeneratorOptions opt;
		
		public CodeDomGenerator(ICodeGenerator generator)
		{
			this.generator = generator;
			opt = new CodeGeneratorOptions();
			opt.BlankLinesBetweenMembers = true;
		}
		
		CodeTypeDeclaration GenerateCodeDom(Interface @interface)
		{
			int index = @interface.Name.LastIndexOf(".");
			string name = index != - 1 ? @interface.Name.Substring(index + 1) : @interface.Name;
			CodeTypeDeclaration type = new CodeTypeDeclaration(name);
			type.Attributes = MemberAttributes.Public;
			type.IsInterface = true;
			
			// Add "Interface" attribute
			CodeAttributeDeclarationCollection attrs = new CodeAttributeDeclarationCollection();
			attrs.Add(new CodeAttributeDeclaration("Interface",
			      new CodeAttributeArgument(new CodePrimitiveExpression(@interface.Name))));
			type.CustomAttributes = attrs;
			
			return type;
		}
		
		CodeMemberMethod GenerateMemberCodeDom(IElement element)
		{
			throw new NotImplementedException();
		}
		
		public void Generate (Interface @interface, TextWriter tw)
		{
			CodeTypeDeclaration type = GenerateCodeDom(@interface);
			
			generator.GenerateCodeFromType(type, tw, opt);
		}

		public void Generate (PathContainer path, TextWriter tw)
		{
			foreach (Interface inter in path.Interfaces) {
				Generate(inter, tw);
			}
		}

		public void Generate (IElement element, TextWriter tw)
		{
			throw new NotImplementedException();
		}
	}
}
