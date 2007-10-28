// DBusExplorator.cs
// Copyright (c) 2007 Jérémie Laval <jeremie.laval@gmail.com>
// See COPYING file for license information.

using System;
using System.IO;
using System.Text;
using System.Xml;
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
		
		static readonly XmlReaderSettings settings = new XmlReaderSettings();
		
		IBus ibus;
		Bus bus;

		static DBusExplorator()
		{
			settings.IgnoreComments = true;
			settings.IgnoreProcessingInstructions = true;
			settings.ValidationType = ValidationType.None;
			settings.ProhibitDtd = false;
			settings.IgnoreWhitespace = true;
			settings.CheckCharacters = false;
		}
		
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
			
			List<PathContainer> paths = new List<PathContainer>(7);
			
			ParseIntrospectable(rootPath, getter, paths);
			
			return paths.ToArray();
		}
		
		void ParseIntrospectable(string currentPath, IntrospectableGetter getter,
		                                    List<PathContainer> paths)
		{
			Introspectable intr = getter(currentPath);
			
			List<Interface> interfaces = null;
			
			string intrData = intr.Introspect();
			
			using (XmlReader reader = XmlReader.Create(new StringReader(intrData), settings)) {
				while (reader.ReadToFollowing("interface")) {
					if (interfaces == null)
						interfaces = new List<Interface>(5);
					interfaces.Add(MakeInterfaceFromNode(reader.ReadSubtree(), reader["name"]));
				}
			}
			
			if (interfaces != null)
				paths.Add(new PathContainer(currentPath, interfaces.ToArray()));
			
			using (XmlReader reader = XmlReader.Create(new StringReader(intrData), settings)) {
				while (reader.ReadToFollowing("node")) {
					if (reader["name"] != null) {
						string newPath = JoinPath(currentPath, reader["name"]);
						ParseIntrospectable(newPath, getter, paths);
					}
				}
			}
		}
		
		Interface MakeInterfaceFromNode(XmlReader reader, string name)
		{	
			List<IElement> entries = new List<IElement>(10);
			
			while (reader.Read()) {
				if (reader.NodeType == XmlNodeType.EndElement)
					continue;
				switch (reader.Name) {
					case "method":
						entries.Add(ParseMethod(reader.ReadSubtree()));
						break;
					case "signal":
						entries.Add(ParseSignal(reader.ReadSubtree()));
						break;
					case "property":
						entries.Add(ParseProperty(reader.ReadSubtree()));
						break;
				}
			}
			
			reader.Close();
			
			return new Interface(name, entries.ToArray());
		}
				                            
	    IElement ParseMethod(XmlReader method)
		{
			if (method == null)
				return null;
			
			method.Read();
			string name = method["name"];
			
			string returnArg = string.Empty;
			List<Argument> args = null;
			
			while (method.ReadToFollowing("arg")) {
				if (method["direction"] == "out") {
					returnArg = method["type"];
				} else {
					if (args == null)
						args = new List<Argument>(5);
					args.Add(new Argument(method["type"], method["name"]));
				}
			}
			
			method.Close();
			
			return ElementFactory.FromMethodDefinition(returnArg, name, args != null ? args.ToArray() : null);
		}
		
		IElement ParseSignal(XmlReader signal)
		{
			if (signal == null)
				return null;
			
			signal.Read();
			string name = signal["name"];
			
			signal.ReadToFollowing("arg");	
			string argType = signal["type"];
			
			signal.Close();
			
			return ElementFactory.FromSignalDefinition(name, new Argument(argType, null));
		}
		
		IElement ParseProperty(XmlReader property)
		{
			if (property == null)
				return null;
			
			property.Read();
			string name = property["name"];
			Argument type = new Argument(property["type"], null);
			PropertyAccess access = PropertyAccess.Read;
			switch (property["access"]) {
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
			
			property.Close();
			
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
