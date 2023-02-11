using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Lang.Styling.Line;

public sealed partial class LineSideIcon
{
	public override int CompareTo(Java.Lang.Object obj)
	{
		return Line.CompareTo(obj?.JavaCast<LineBackground>()?.Line);
	}
}
