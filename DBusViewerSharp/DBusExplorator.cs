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

namespace DBusExplorer
{
	public class DBusExplorator
	{
		static readonly string DBusName = "org.freedesktop.DBus";
		static readonly ObjectPath DBusPath = new ObjectPath ("/org/freedesktop/DBus");
		static readonly string rootPath = "/";
		public static readonly DBusExplorator sessionExplorator = new DBusExplorator();
		public static readonly DBusExplorator systemExplorator = new DBusExplorator(Bus.System);
		static ElementFactory elementFactory = new ElementFactory(LangDefinitionService.DefaultPool.Languages.Values);
		
		public event EventHandler<DBusErrorEventArgs> DBusError;
		
		IBus ibus;
		Bus bus;
		
		string[] avalaibleBusNames = null;
		
		static bool dump = Environment.CommandLine.Contains("--dump");

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
				return avalaibleBusNames == null ? 
					(avalaibleBusNames = ibus.ListNames()) : avalaibleBusNames;
			}
		}
		
		public Bus BusUsed {
			get {
				return bus;
			}
		}

		public static DBusExplorator SessionExplorator {
			get {
				return sessionExplorator;
			}
		}

		public static DBusExplorator SystemExplorator {
			get {
				return systemExplorator;
			}
		}
		
		delegate IEnumerable<PathContainer> UpdateDelegate(string busName);
		UpdateDelegate updater;
		
		public IAsyncResult BeginGetElementsFromBus(string busName, AsyncCallback callback)
		{
			return updater.BeginInvoke(busName, callback, null);
		}
		
		public IEnumerable<PathContainer> EndGetElementsFromBus(IAsyncResult result)
		{
			return updater.EndInvoke(result);
		}
		
		delegate Introspectable IntrospectableGetter(string path);
		
		public IEnumerable<PathContainer> GetElementsFromBus(string busName)
		{
			IntrospectableGetter getter = delegate (string path) {
				return bus.GetObject<Introspectable>(busName, new ObjectPath(path));
			};
			
			List<PathContainer> paths = new List<PathContainer>(7);

			ParseIntrospectable(rootPath, getter, paths);
			
			return (IEnumerable<PathContainer>)paths;
		}
		
		void ParseIntrospectable(string currentPath, IntrospectableGetter getter,
		                                    List<PathContainer> paths)
		{
			Introspectable intr = null;
			string intrData = null;
			
			try {
				intr = getter(currentPath);
				intrData = intr.Introspect();
			} catch (Exception e) {
				string error = "Managed D-Bus error on path : " + currentPath + Environment.NewLine +
						"Error : " + e.Message;
				if (DBusError != null) {
					DBusError(this, new DBusErrorEventArgs(error));
				}
				Console.WriteLine(error);
				return;
			}
			
			if (dump) {
				Console.WriteLine("On path : " + currentPath);
				Console.WriteLine(intrData ?? "Nothing to be parsed");
				Console.WriteLine();
			}
			
			if (string.IsNullOrEmpty(intrData))
				return;
			
			List<Interface> interfaces = null;
			
			using (XmlTextReader reader = new XmlTextReader(new StringReader(intrData))) {
				reader.XmlResolver = null;
				reader.ReadToFollowing("node");
				
				while (reader.Read()) {
					if (reader.NodeType == XmlNodeType.EndElement)
						continue;
					
					if (reader.Name == "interface") {
						if (interfaces == null)
							interfaces = new List<Interface>(5);
						interfaces.Add(MakeInterfaceFromNode(reader.ReadSubtree(), reader["name"]));
					} else if (reader.Name == "node") {
						if (reader["name"] != null) {
							string newPath = JoinPath(currentPath, reader["name"]);
							ParseIntrospectable(newPath, getter, paths);
						}
					}
				}
			}
			
			if (interfaces != null)
				paths.Add(new PathContainer(currentPath, (IEnumerable<Interface>)interfaces));
		}
		
		Interface MakeInterfaceFromNode(XmlReader reader, string name)
		{
			List<IElement> entries = new List<IElement>(50);
			
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
			entries.RemoveAll(delegate (IElement elem) { return elem == null; });
			
			return new Interface(name, (IEnumerable<IElement>)entries);
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
			
			return elementFactory.FromMethodDefinition(returnArg, name, args != null ? (IEnumerable<Argument>)args : null);
		}
		
		IElement ParseSignal(XmlReader signal)
		{
			if (signal == null)
				return null;
			
			signal.Read();
			string name = signal["name"];
			
			List<Argument> args = null;
			while (signal.ReadToFollowing("arg")) {	
				if (args == null)
					args = new List<Argument>(3);
				args.Add(new Argument(signal["type"], null));
			}
			
			signal.Close();
			
			return elementFactory.FromSignalDefinition(name, args == null ? null : (IEnumerable<Argument>)args);
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
			
			return elementFactory.FromPropertyDefinition(name, type, access);
		}
		
		static string JoinPath(string path1, string path2)
		{
			string path = path1.EndsWith("/") ? path1 : path1 + "/";
			path += path2;
			return path;
		}
	}
}
