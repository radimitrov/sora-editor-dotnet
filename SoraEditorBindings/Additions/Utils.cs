using Android.Content;
using Android.Util;
using AndroidX.Core.Content.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoraEditorBindings.Additions
{
	public static class Utils
	{
		public static int DipToPx(Context context, float dip)
		{
			return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, context.Resources.DisplayMetrics);
		}

		public static int SpToPx(Context context, float sp)
		{
			return (int)TypedValue.ApplyDimension(ComplexUnitType.Sp, sp, context.Resources.DisplayMetrics);
		}
	}
}
