using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Widget
{
	public partial class EditorSearcher
	{
		public virtual int LastResultsCount => LastResults == null ? 0 : LastResults.Size();
	}
}
