// DBusExplorator.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Generic;

using NDesk.DBus;
using org.freedesktop.DBus;

namespace DBusViewerSharp
{
	public class DBusExplorator
	{
		static readonly string DBusName = "org.freedesktop.DBus";
		static readonly ObjectPath DBusPath = new ObjectPath ("/org/freedesktop/DBus");
		
		IBus ibus;
		Bus bus;
		
		public DBusExplorator()
		{
			ibus = Bus.Session.GetObject<IBus>(DBusName, DBusPath);
			bus = Bus.Session;
			updater =  GetElementsFromBus;
		}
		
		public string[] AvalaibleBusNames {
			get {
				return ibus.ListNames();
			}
		}
		
		delegate ElementsEntry[] UpdateDelegate(string busName);
		UpdateDelegate updater;
		
		public IAsyncResult BeginGetElementsFromBus(string busName, AsyncCallback callback)
		{
			return updater.BeginInvoke(busName, callback, null);
		}
		
		public ElementsEntry[] EndGetElementsFromBus(IAsyncResult result)
		{
			return updater.EndInvoke(result);
		}
		
		public ElementsEntry[] GetElementsFromBus(string busName)
		{
			Introspectable intr = 
				bus.GetObject<Introspectable>(busName, ObjectPath.Root);
			
			return ParseIntrospectable(intr, busName);
		}
		
		ElementsEntry[] ParseIntrospectable(Introspectable intr, string busName)
		{
			return ParseIntrospectable(intr, busName, "/");	
		}
		
		ElementsEntry[] ParseIntrospectable(Introspectable intr, string busName, string currentPath)
		{
			List<ElementsEntry> elements = new List<ElementsEntry>();
			XPathDocument doc = new XPathDocument(new System.IO.StringReader(intr.Introspect()));
			XPathNavigator navigator = doc.CreateNavigator();
			
			XPathNodeIterator interfaceList = navigator.Select("node/interface");
			foreach (XPathNavigator node in interfaceList) {
				elements.Add(MakeElementsEntryFromNode(node, currentPath));
			}
			
			XPathNodeIterator nodeList = navigator.Select("node/node");
			foreach (XPathNavigator node in nodeList) {
				string newPath = JoinPath(currentPath, node.GetAttribute("name", string.Empty));
				ObjectPath objPath = new ObjectPath(newPath);
				Introspectable intro = bus.GetObject<Introspectable>(busName, objPath);
				elements.AddRange(ParseIntrospectable(intro, busName, newPath));
			}
			return elements.ToArray();
		}
		
		string JoinPath(string path1, string path2)
		{
			string path = path1.EndsWith("/") ? path1 : path1 + "/";
			path += path2;
			return path;
		}
		
		ElementsEntry MakeElementsEntryFromNode(XPathNavigator node, string path)
		{	
			List<IEntry> entries = new List<IEntry>();
			
			foreach (XPathNavigator method in node.Select("method")) {
				entries.Add(ParseMethods(method));
			}
			
			path = string.IsNullOrEmpty(path) ? "/" : path;
			
			return new ElementsEntry(path, entries.ToArray());
		}
				                            
	    IEntry ParseMethods(XPathNavigator method)
		{
			if (method == null)
				return null;
			
			string name = method.GetAttribute("name", string.Empty);
			
			XPathNavigator returnNode = method.SelectSingleNode("arg[@direction='out']");
			string returnArg = returnNode == null ? string.Empty : returnNode.GetAttribute("type", string.Empty);
			
			XPathNodeIterator argNodeList = method.Select("arg[@direction='in']");
			ArgEntry[] args = null;
			
			if (argNodeList.Count > 0) {
				args = new ArgEntry[argNodeList.Count];
				int i = 0;
				foreach (XPathNavigator arg in argNodeList) {
					string paramName = arg.GetAttribute("name", string.Empty);
					args[i++] = new ArgEntry(arg.GetAttribute("type", string.Empty), paramName);
				}
			}
				                       
		    return new MethodEntry(name, returnArg, args);
		}
	}
}
