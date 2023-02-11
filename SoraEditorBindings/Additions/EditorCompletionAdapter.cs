using IO.Github.Rosemoe.Sora.Lang.Completion;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Widget.Component
{
	public partial class EditorCompletionAdapter
	{
		public CompletionItem GetCompletionItem(int position)
		{
			return this.GetItem(position)?.JavaCast<CompletionItem>();
		}
	}
}
