﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;

namespace ResourceEditor
{
		
	class SelectAllCommand : AbstractMenuCommand
	{
		public override void Run()
		{
			ResourceEditWrapper editor = (ResourceEditWrapper)SD.Workbench.ActiveViewContent;
			
			editor.SelectAll();
		}
	}
	
}
