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
		
		public InvokeDialog (Bus bus, string busName, ObjectPath path, IElement element)
		{
			this.Build ();
			this.methodName.Text = element.Name;
			Console.WriteLine (path.ToString());
			caller = new MethodCaller(bus, busName, path, element.Parent.Name, element.Name, element.Data);
			BuildInterface (element);
			this.buttonExecute.Clicked += OnButtonExecuteActivated;
		}
		
		void BuildInterface (IElement element)
		{
			if (element.Data.Args == null)
				return;
			entries = element.Data.Args.Select((a) => {
				Label lbl = new Label (a.Name + " : ");
				Entry ety = new Entry ();
				HBox box = new HBox ();
				box.Add(lbl);
				box.Add(ety);
				box.ShowAll ();
				argsVb.Add (box);
				argsVb.ShowAll ();
				
				return (Func<object>)delegate {
					object result = null;
					try {
						result = Mapper.Convert(Mapper.DTypeFromString(a.Type), ety.Text);
					} catch (Exception e) {
						MessageDialog diag = new MessageDialog(this, DialogFlags.DestroyWithParent,
						                                       MessageType.Error, ButtonsType.Ok,
						                                       e.Message);
						diag.Run();
						diag.Destroy();
						return null;
					}
					
					return result;
				};
			}).ToArray();
		}
		
		protected virtual void OnButtonExecuteActivated (object sender, System.EventArgs e)
		{
			Console.WriteLine ("Meuh");
			bool success = true;
			object[] ps = (entries == null) ? null : entries
				.Select((f) => { object tmp = f(); if (tmp == null) success = false; return tmp; })
					.ToArray();
			if (!success)
				return;
			
			object result = caller.Invoke (ps);
			resultLabel.Text = result.ToString ();
			Console.WriteLine("Result is " + result.ToString());
		}
	}
}
