using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Lang.Diagnostic
{
	public partial class DiagnosticRegion
	{
		[Register("compareTo", "(Lio/github/rosemoe/sora/lang/diagnostic/DiagnosticRegion;)I", "")]
		public unsafe int CompareTo(global::Java.Lang.Object? o)
		{
			
			return CompareTo(o?.JavaCast<DiagnosticRegion>());
		}
	}
}
