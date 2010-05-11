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
		
		public string Generate (Interface @interface)
		{
			CodeTypeDeclaration type = GenerateCodeDom(@interface);
			PopulateWithElements (@interface.Symbols, type);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine();
			provider.GenerateCodeFromType(type, new StringWriter(sb), opt);
			sb.AppendLine();
			
			return sb.ToString ();
		}

		public string Generate (PathContainer path)
		{
			StringBuilder sb = new StringBuilder ();
			
			foreach (Interface inter in path.Interfaces) {
				sb.AppendLine (Generate (inter));
			}
			
			return sb.ToString ();
		}

		public string Generate (IEnumerable<IElement> elements)
		{
			IElement elem = elements.FirstOrDefault ();
			if (elem == null)
				return string.Empty;
			
			CodeTypeDeclaration type = GenerateCodeDom(elem.Parent);
			PopulateWithElements (elements, type);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine();
			provider.GenerateCodeFromType(type, new StringWriter(sb), opt);
			sb.AppendLine();
			
			return sb.ToString ();
		}
	}
}
