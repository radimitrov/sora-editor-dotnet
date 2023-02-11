using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Hardware.Lights;
using Android.Util;
using IO.Github.Rosemoe.Sora.Widget.Style;
using IO.Github.Rosemoe.Sora.Widget.Style.Builtin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Icu.Text.ListFormatter;
using static Android.Media.Audiofx.AudioEffect;

namespace SoraEditorBindings.Additions
{
	public class HandleStyleSide : Java.Lang.Object, ISelectionHandleStyle
	{
		private int alpha=255;
		private float scaleFactor = 1f;
		private Drawable drawable;
		private int width;
		private int height;
		private int lastColor = 0;

		public HandleStyleSide(Context context)
		{
			drawable = context.GetDrawable(Resource.Drawable.abc_text_select_handle_right_mtrl).Mutate();
			width = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 20f, context.Resources.DisplayMetrics);
			height = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 20f, context.Resources.DisplayMetrics);
		}

		public void Draw(Canvas canvas, int handleType, float x, float y, int rowHeight, int color, ISelectionHandleStyle.HandleDescriptor descriptor)
		{
			if (lastColor != color)
			{
				lastColor = color;
				drawable.SetColorFilter(new PorterDuffColorFilter(new Color(color), PorterDuff.Mode.SrcAtop));
			}
			var left = (int)(x - (width * scaleFactor) / 2);
			var top = (int)y;
			var right = (int)(x + (width * scaleFactor) / 2);
			var bottom = (int)(y + height * scaleFactor);
			drawable.SetBounds(left, top, right, bottom);
			drawable.SetAlpha(alpha);
			drawable.Draw(canvas);
			descriptor.Set(left, top, right, bottom, ISelectionHandleStyle.AlignCenter);
		}

		public void SetAlpha(int alpha)
		{
			this.alpha = alpha;
		}

		public void SetScale(float scaleFactor)
		{
			this.scaleFactor = scaleFactor;
		}
	}
}
