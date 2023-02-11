using IO.Github.Rosemoe.Sora.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Text
{
	public partial class ContentLine
	{
		public ICharSequence SubSequenceFormatted(int start, int end)
		{
			return this.SubSequence(start, end);
		}
	}
}
