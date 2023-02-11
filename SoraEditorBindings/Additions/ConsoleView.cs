using Android.Content;
using IO.Github.Rosemoe.Sora.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoraEditorBindings.Additions
{
	/// <summary>
	/// An abstraction of CodeEditor meant to be used as a Console view.
	/// </summary>
	public class ConsoleView : CodeEditor
	{
		public ConsoleView(Context context) : base(context)
		{
			Init();
		}

		private void Init()
		{

		}
	}
}
