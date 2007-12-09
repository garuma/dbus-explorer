/***************************************************************************
 *  ImageAnimation.cs
 *
 *  Copyright (C) 2005 Novell
 *  Written by Aaron Bockover (aaron@aaronbock.net)
 ****************************************************************************/

/*  THIS FILE IS LICENSED UNDER THE MIT LICENSE AS OUTLINED IMMEDIATELY BELOW: 
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a
 *  copy of this software and associated documentation files (the "Software"),  
 *  to deal in the Software without restriction, including without limitation  
 *  the rights to use, copy, modify, merge, publish, distribute, sublicense,  
 *  and/or sell copies of the Software, and to permit persons to whom the  
 *  Software is furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 *  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 *  DEALINGS IN THE SOFTWARE.
 */
 
using System;
using Gtk;
using Gdk;

namespace DBusExplorer
{
    public class ImageAnimation : Gtk.Image
    {
	   	Pixbuf sourcePixbuf;
	   	Pixbuf inactivePixbuf = null;
	   	int frameWidth, frameHeight, maxFrames, currentFrame;
		uint refreshRate;
	   	Pixbuf [] frames;
	   	bool active = true;
		uint timeout_id;
		bool isFirstRun = true;
	   	
	   	protected ImageAnimation() : base()
	   	{
	   		
	   	}
	   	
	   	public ImageAnimation(Pixbuf sourcePixbuf, uint refreshRate,
		                      int frameWidth, int frameHeight)
			: this(sourcePixbuf, refreshRate, frameWidth, frameHeight, 0)
	   	{
	   	}
	   	
	   	public ImageAnimation (Pixbuf sourcePixbuf, uint refreshRate,
		                       int frameWidth, int frameHeight, int maxFrames) 
			: base()
	   	{
			this.refreshRate = refreshRate;
			Load(sourcePixbuf, frameWidth, frameHeight, maxFrames);
			Active = true;
	   	}
	   	
	   	public void Load(Pixbuf sourcePixbuf, int frameWidth, 
		                 int frameHeight, int maxFrames)
	   	{
	   		this.sourcePixbuf = sourcePixbuf;
	   		this.frameWidth = frameWidth;
	   		this.frameHeight = frameHeight;
	   		this.maxFrames = maxFrames;
	   		SpliceImage();
	   	}

	   	public Pixbuf InactivePixbuf {
	   		set {
	   			inactivePixbuf = value;
	   			if(!active)
	   				Pixbuf = value;
	   		}
	   	}
	   	
	   	public bool Active
	   	{
			get {
				return active;
			}
			set
			{
        		active = value;
			
				if (active)
				{
					if (!isFirstRun)
						GLib.Source.Remove (timeout_id);
					else
						isFirstRun = false;
				
					timeout_id = GLib.Timeout.Add(refreshRate, 
					                 new GLib.TimeoutHandler(OnTimeout));
				}
			}
	   	}
	   	
	   	void SpliceImage()
	   	{
	   		int width, height, rows, cols, frameCount;
	   		
	   		if(sourcePixbuf == null)
	   			throw new Exception("No source pixbuf specified");
	   			
	   		width = sourcePixbuf.Width;
	   		height = sourcePixbuf.Height;
	   		
	   		if(width % frameWidth != 0 || height % frameHeight != 0)
	   			throw new Exception("Invalid frame dimensions");
	   			
	   		rows = height / frameHeight;
	   		cols = width / frameWidth;
	   		frameCount = rows * cols;
	   		
	   		frames = new Pixbuf[maxFrames > 0 ? maxFrames : frameCount];
	   		
	   		bool doBreak = false;
	   		
	   		for(int y = 0, n = 0; y < rows; y++) {
	   			for(int x = 0; x < cols; x++, n++) {
	   				frames[n] = new Pixbuf(sourcePixbuf,
	   					x * frameWidth,
	   					y * frameHeight,
	   					frameWidth,
	   					frameHeight
	   				);
	   				
	   				if(maxFrames > 0 && n >= maxFrames - 1) {
	   					doBreak = true;
	   					break;
	   				}
	   			}
	   			
	   			if(doBreak)
	   				break;
	   		}
	   		
	   		currentFrame = 0;
	   	}
	   	
	   	bool OnTimeout()
	   	{
	   		if(!active)
	   		{
	   			if (inactivePixbuf != null && Pixbuf != inactivePixbuf)
	   				Pixbuf = inactivePixbuf;
	   			else if (currentFrame != 0)
	   				Pixbuf = frames[0];
	   				
	   			return false;
	   		}
	   	
	   		if(frames == null || frames.Length == 0)
	   			return false;
	   		
	   		
	   		if (currentFrame < 32)
	   			Pixbuf = frames[currentFrame++];
	   		else
	   		{
	   			currentFrame = 1;
	   			Pixbuf = frames[currentFrame];
	   		}
	   			
	   		return true;
	   	}
	}
}
