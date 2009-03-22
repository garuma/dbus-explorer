// 
// InvokeDialog.cs
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
using Gtk;
using NDesk.DBus;
using System.Collections.Generic;
using System.Linq;

namespace DBusExplorer
{
	
	
	public partial class InvokeDialog : Gtk.Dialog
	{
		MethodCaller caller;
		Func<object>[] entries;
		
		const string message = "An error occured while converting parameter {0} which has type {1}\n\n{2}"; 
		
		public InvokeDialog (Bus bus, string busName, ObjectPath path, IElement element)
		{
			this.Build ();
			this.methodName.Text = element.Name;
			
			caller = new MethodCaller(bus, busName, path, element.Parent.Name, element.Name, element.Data);
			BuildInterface (element);
			this.buttonExecute.Clicked += OnButtonExecuteActivated;
			this.ShowAll ();
		}
		
		void BuildInterface (IElement element)
		{
			IEnumerable<Argument> args = element.Data.Args;
			
			if (args == null)
				return;
			
			entries = args.Select<Argument,Func<object>>(BuildArgumentEntry).ToArray();
		}
		
		Func<object> BuildArgumentEntry (Argument a)
		{
			Label lbl = new Label ();
			Entry ety = new Entry ();
			HBox box = new HBox ();
			DType t = Mapper.DTypeFromString(a.Type);
			lbl.Text = string.Format ("{0} ({1}) : ", a.Name, Mapper.DTypeToStr (t));
			
			box.Add(lbl);
			box.Add(ety);
			argsVb.Add (box);
			box.ShowAll ();
			argsVb.ShowAll ();
			
			return (Func<object>)delegate {
				object result = null;
				try {
					result = Mapper.Convert(t, ety.Text);
				} catch (Exception e) {
					ShowErrorDialog (a, e.Message);
					return null;
				}
				
				return result;
			};
		}
		
		void ShowErrorDialog (Argument a, string error)
		{
			MessageDialog diag = new MessageDialog(this, DialogFlags.DestroyWithParent,
			                                       MessageType.Error, ButtonsType.Ok,
			                                       string.Format(message, a.Name, a.Type, error));
			diag.Run();
			diag.Destroy();
		}
		
		protected virtual void OnButtonExecuteActivated (object sender, System.EventArgs e)
		{
			bool success = true;
			object[] ps = (entries == null) ? null : entries
				.Select((f) => { object tmp = f(); if (tmp == null) success = false; return tmp; })
					.ToArray();
			if (!success)
				return;
			
			object result = caller.Invoke (ps);
			resultLabel.Text = result != null ? result.ToString () : "nil";
		}
	}
}
