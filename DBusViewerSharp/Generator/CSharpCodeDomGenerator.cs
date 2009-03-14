// CodeDomGenerator.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using System.IO;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace DBusExplorer
{
	public class CSharpCodeDomGenerator: IGenerator
	{
		CodeDomProvider provider;
		CodeGeneratorOptions opt;
		
		public CSharpCodeDomGenerator()
		{
			this.provider = new Microsoft.CSharp.CSharpCodeProvider();
			opt = new CodeGeneratorOptions();
			opt.BlankLinesBetweenMembers = true;
		}
		
		CodeTypeDeclaration GenerateCodeDom(Interface i)
		{
			int index = i.Name.LastIndexOf(".");
			string name = index != - 1 ? i.Name.Substring(index + 1) : i.Name;
			CodeTypeDeclaration type = new CodeTypeDeclaration(name);
			type.Attributes = MemberAttributes.Public;
			type.IsInterface = true;
			
			// Add "Interface" attribute
			CodeAttributeDeclarationCollection attrs = new CodeAttributeDeclarationCollection();
			attrs.Add(new CodeAttributeDeclaration("Interface",
			      new CodeAttributeArgument(new CodePrimitiveExpression(i.Name))));
			type.CustomAttributes = attrs;
			
			return type;
		}
		
		void PopulateWithElements (IEnumerable<IElement> elements, CodeTypeDeclaration type)
		{
			foreach (IElement e in elements) {
				type.Members.Add(new CodeSnippetTypeMember(e.Visual["C#"]));
			}
		}
		
		public void Generate (Interface @interface, string path)
		{
			CodeTypeDeclaration type = GenerateCodeDom(@interface);
			PopulateWithElements (@interface.Symbols, type);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine();
			provider.GenerateCodeFromType(type, new StringWriter(sb), opt);
			sb.AppendLine();
			File.AppendAllText(path, sb.ToString());
		}

		public void Generate (PathContainer path, string file_path)
		{
			foreach (Interface inter in path.Interfaces) {
				Generate(inter, file_path);
			}
		}

		public void Generate (IEnumerable<IElement> elements, string path)
		{
			IElement elem = elements.FirstOrDefault ();
			if (elem == null)
				return;
			
			CodeTypeDeclaration type = GenerateCodeDom(elem.Parent);
			PopulateWithElements (elements, type);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine();
			provider.GenerateCodeFromType(type, new StringWriter(sb), opt);
			sb.AppendLine();
			File.AppendAllText(path, sb.ToString());
		}
	}
}
