using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Lang.Styling
{
	public partial class LineBackground
	{
		public int CompareTo(Java.Lang.Object obj)
		{
			throw new NotImplementedException();
			//return this.CompareTo(obj?.JavaCast<LineBackground>());
		}
	}
}

namespace IO.Github.Rosemoe.Sora.Lang.Styling.Line
{
	public partial class LineBackground
	{
		public override int CompareTo(Java.Lang.Object obj)
		{
			return base.CompareTo(obj as LineBackground);
		}
	}
}
