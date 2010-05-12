// 
// MethodInvokeDialog.cs
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

	public partial class MethodInvokeDialog : Gtk.Dialog
	{
		MethodCaller caller;
		Func<object>[] entries;
		uint rowIndex;
		Window parent;
		
		const string errMessage
			= "An error occured while converting parameter, check that the type parsing is supported"; 
		
		public MethodInvokeDialog (Window parent, Bus bus, string busName, ObjectPath path, IElement element)
			: base (element.Name, parent, DialogFlags.DestroyWithParent | DialogFlags.Modal)
		{
			this.Build ();
			this.SetDefaultSize (-1, -1);
			this.methodName.Text = element.Name;
			this.parent = parent;
			this.TransientFor = parent;
			
			try {
				BuildInterface (element);
				caller = new MethodCaller(bus, busName, path, element.Parent.Name, element.Name, element.Data);
			} catch (Exception e) {
				Logging.Error ("Error while creating the invocation proxy", e, parent);
				buttonExecute.Sensitive = false;
			}
			
			this.ShowAll ();
			
		}
		
		void BuildInterface (IElement element)
		{
			IEnumerable<Argument> args = element.Data.Args;
			uint count = 0;
			
			if (args == null || (count = (uint)args.Count ()) == 0) {
				argAlign.HideAll ();
				argFrame.HideAll ();
				containerVbox.Remove (argAlign);
				return;
			}
			
			argAlign.ShowAll ();
			argumentTable.Resize (count, 2);
			
			entries = args.Select<Argument,Func<object>>(BuildArgumentEntry).ToArray();
		}
		
		Func<object> BuildArgumentEntry (Argument a)
		{
			Label lbl = new Label ();
			Entry ety = new Entry ();

			DType t = Mapper.DTypeFromString(a.Type);
			
			lbl.Text = string.Format ("{0} ({1}) : ", a.Name, Mapper.DTypeToStr (t));
			lbl.Xalign = 0;
			
			argumentTable.Attach (lbl, 0, 1, rowIndex, rowIndex + 1);
			argumentTable.Attach (ety, 1, 2, rowIndex, rowIndex + 1);
			rowIndex++;
			
			lbl.Show ();
			ety.Show ();
			argumentTable.ShowAll ();
			
			return (Func<object>)delegate {
				object result = Mapper.Convert(t, ety.Text);
				
				return result;
			};
		}
		
		protected virtual void OnButtonExecuteClicked (object sender, System.EventArgs e)
		{
			object[] ps = null;
			
			try {
				ps = (entries == null) ? null : entries
					.Select((f) => f()).ToArray();
			} catch (Exception ex) {
				Logging.Error ("Parsing error, check that you entered correct values", ex, parent);
				return;
			}
			
			object result = null;
			
			try {
				result = caller.Invoke (ps);
			} catch (Exception ex) {
				result = "Error";
				Logging.Error ("Error while invoking method", ex, parent);
			}
			
			resultLabel.Text = result != null ? result.ToString () : "nil";
		}
		
		protected virtual void OnButtonCloseClicked (object sender, System.EventArgs e)
		{
			HideAll ();
			Destroy ();
		}
	}
}
