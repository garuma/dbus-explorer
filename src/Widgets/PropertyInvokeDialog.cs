// 
// PropertyInvokeDialog.cs
//  
// Author:
//       Jérémie Laval <jeremie.laval@gmail.com>
// 
// Copyright (c) 2010 Jérémie "Garuma" Laval
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

namespace DBusExplorer
{
	public partial class PropertyInvokeDialog : Gtk.Dialog
	{
		Window parent;
		PropertyCaller caller;
		DType propertyType;
		
		public PropertyInvokeDialog (Window parent, Bus bus, string busName, ObjectPath path, IElement element)
			: base (element.Name, parent, DialogFlags.DestroyWithParent | DialogFlags.Modal)
		{
			this.Build ();
			this.parent = parent;
			this.setAlign.HideAll ();
			this.WidthRequest = 250;
			this.HeightRequest = 150;
			this.propertyName.Text = element.Name;
			this.propertyType = Mapper.DTypeFromString (element.Data.ReturnType);
			
			try {
				this.caller = new PropertyCaller (bus, busName, path, element.Parent.Name, element.Name, element.Data);
			} catch (Exception e) {
				Logging.Error ("Error while creating the invocation proxy", e, parent);
				buttonExecute.Sensitive = false;
			}
		}
		
		protected virtual void OnGetBtnToggled (object sender, System.EventArgs e)
		{
			if (!getBtn.Active)
				return;
			
			setAlign.HideAll ();
			getAlign.ShowAll ();
		}
		
		protected virtual void OnSetBtnToggled (object sender, System.EventArgs e)
		{
			if (!setBtn.Active)
				return;
			
			getAlign.HideAll ();
			setAlign.ShowAll ();
		}
		
		protected virtual void OnButtonExecuteClicked (object sender, System.EventArgs evt)
		{
			try {
				object result = caller.Invoke (setBtn.Active ? new[] { Mapper.Convert (propertyType, setEntry.Text) } : null);
				if (result != null)
					getLbl.Text = result.ToString ();
			} catch (Exception e) {
				Logging.Error ("Error while calling property", e, parent);
				Console.WriteLine (e.ToString ());
			}
		}		
	}
}

