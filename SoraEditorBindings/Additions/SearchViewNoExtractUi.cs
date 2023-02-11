using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views.InputMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoraEditorBindings.Additions
{
	public class SearchViewNoExtractUi : AndroidX.AppCompat.Widget.SearchView
	{
		public SearchViewNoExtractUi(Context context) : base(context)
		{
		}

		public SearchViewNoExtractUi(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public SearchViewNoExtractUi(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

		protected SearchViewNoExtractUi(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public override IInputConnection OnCreateInputConnection(EditorInfo outAttrs)
		{
			outAttrs.ImeOptions = ImeFlags.NoExtractUi | ImeFlags.NoFullscreen;
			return base.OnCreateInputConnection(outAttrs);
		}
	}
}
