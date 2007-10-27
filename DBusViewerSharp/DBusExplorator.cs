// DBusExplorator.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.IO;
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
		static readonly string rootPath = "/";
		
		IBus ibus;
		Bus bus;
		
		public DBusExplorator(): this(Bus.Session)
		{
		}
		
		public DBusExplorator(Bus bus)
		{
			this.bus = bus;
			ibus = this.bus.GetObject<IBus>(DBusName, DBusPath);
			updater =  GetElementsFromBus;
		}
		
		public string[] AvalaibleBusNames {
			get {
				return ibus.ListNames();
			}
		}
		
		public Bus BusUsed {
			get {
				return bus;
			}
		}
		
		delegate PathContainer[] UpdateDelegate(string busName);
		UpdateDelegate updater;
		
		public IAsyncResult BeginGetElementsFromBus(string busName, AsyncCallback callback)
		{
			return updater.BeginInvoke(busName, callback, null);
		}
		
		public PathContainer[] EndGetElementsFromBus(IAsyncResult result)
		{
			return updater.EndInvoke(result);
		}
		
		delegate Introspectable IntrospectableGetter(string path);
		
		public PathContainer[] GetElementsFromBus(string busName)
		{
			IntrospectableGetter getter = delegate (string path) {
				return bus.GetObject<Introspectable>(busName, new ObjectPath(path));
			};
			
			List<PathContainer> paths = new List<PathContainer>();
			
			ParseIntrospectable(rootPath, getter, paths);
			
			return paths.ToArray();
		}
		
		void ParseIntrospectable(string currentPath, IntrospectableGetter getter,
		                                    List<PathContainer> paths)
		{
			Introspectable intr = getter(currentPath);
			
			List<Interface> interfaces = new List<Interface>();
			
			string intrData = intr.Introspect();
			XPathDocument doc = new XPathDocument(new System.IO.StringReader(intrData));
			XPathNavigator navigator = doc.CreateNavigator();
			
			XPathNodeIterator interfaceList = navigator.Select("node/interface");
			foreach (XPathNavigator node in interfaceList) {
				interfaces.Add(MakeInterfaceFromNode(node, node.GetAttribute("name", string.Empty)));
			}
			
			paths.Add(new PathContainer(currentPath, interfaces.ToArray()));
			
			XPathNodeIterator nodeList = navigator.Select("node/node");
			foreach (XPathNavigator node in nodeList) {
				string newPath = JoinPath(currentPath, node.GetAttribute("name", string.Empty));
				ParseIntrospectable(newPath, getter, paths);
			}
		}
		
		Interface MakeInterfaceFromNode(XPathNavigator node, string name)
		{	
			List<IElement> entries = new List<IElement>();
			
			foreach (XPathNavigator method in node.Select("method")) {
				entries.Add(ParseMethod(method));
			}
			foreach (XPathNavigator method in node.Select("signal")) {
				entries.Add(ParseSignal(method));
			}
			foreach (XPathNavigator method in node.Select("property")) {
				entries.Add(ParseProperty(method));
			}
			
			return new Interface(name, entries.ToArray());
		}
				                            
	    IElement ParseMethod(XPathNavigator method)
		{
			if (method == null)
				return null;
			
			string name = method.GetAttribute("name", string.Empty);
			
			XPathNavigator returnNode = method.SelectSingleNode("arg[@direction='out']");
			string returnArg = returnNode == null ? string.Empty : returnNode.GetAttribute("type", string.Empty);
			
			XPathNodeIterator argNodeList = method.Select("arg[@direction='in']");
			Argument[] args = null;
			
			if (argNodeList.Count > 0) {
				args = new Argument[argNodeList.Count];
				int i = 0;
				foreach (XPathNavigator arg in argNodeList) {
					string paramName = arg.GetAttribute("name", string.Empty);
					args[i++] = new Argument(arg.GetAttribute("type", string.Empty), paramName);
				}
			}
				                       
		    return ElementFactory.FromMethodDefinition(returnArg, name, args);
		}
		
		IElement ParseSignal(XPathNavigator signal)
		{
			string name = signal.GetAttribute("name", string.Empty);
			
			XPathNavigator arg = signal.SelectSingleNode("arg");
			string argType = arg.GetAttribute("type", string.Empty);
			//string argName = arg.GetAttribute("name", string.Empty);
			
			return ElementFactory.FromSignalDefinition(name, new Argument(argType, null));
		}
		
		IElement ParseProperty(XPathNavigator property)
		{
			string name = property.GetAttribute("name", string.Empty);
			Argument type = new Argument(property.GetAttribute("type", string.Empty), null);
			PropertyAccess access = PropertyAccess.Read;
			switch (property.GetAttribute("access", string.Empty)) {
				case "readwrite":
					access = PropertyAccess.ReadWrite;
					break;
				case "read":
					access = PropertyAccess.Read;
					break;
				case "write":
					access = PropertyAccess.Write;
					break;
			}
			
			return ElementFactory.FromPropertyDefinition(name, type, access);
		}
		
		string JoinPath(string path1, string path2)
		{
			string path = path1.EndsWith("/") ? path1 : path1 + "/";
			path += path2;
			return path;
		}
	}
}
