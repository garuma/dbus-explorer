// Logging.cs
// Copyright (c) 2007-2009 Jérémie Laval <jeremie.laval@gmail.com>
//
// See COPYING file for license information.
// 

using System;
using Gtk;
using System.Collections.Generic;

namespace DBusExplorer
{
	public enum LogType {
		Warning,
		Error
	}
	
	public delegate void WatcherDelegate(LogType type, string message, Exception ex, Window parent);
	
	public static class Logging
	{
		static List<WatcherDelegate> watchers = new List<WatcherDelegate>();
		
		public static void AddWatcher(WatcherDelegate watcher)
		{
			watchers.Add (watcher);
		}
		
		static void Propagate (LogType type, string message, Exception ex, Window parent)
		{
			watchers.ForEach (delegate (WatcherDelegate watcher) {
				watcher(type, Mono.Unix.Catalog.GetString(message), ex, parent);
			});
		}
		
		public static void Warning (string message)
		{
			Propagate(LogType.Warning, message, null, null);
		}
		
		public static void Error (string message)
		{
			Propagate (LogType.Error, message, null, null);
		}
		
		public static void Error (string message, Exception ex)
		{
			Propagate (LogType.Error, message, ex, null);
		}
		
		public static void Error (string message, Exception ex, Window parent)
		{
			Propagate (LogType.Error, message, ex, parent);
		}
	}
}
