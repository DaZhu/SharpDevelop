﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace ICSharpCode.WpfDesign.XamlDom
{
	public class DesignInstanceExtension : MarkupExtension
	{
		public DesignInstanceExtension(Type type)
		{
			this.Type = type;
		}

		public Type Type { get; set; }

		public bool IsDesignTimeCreatable { get; set; }

		public bool CreateList { get; set; }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return null;
		}
	}
}
