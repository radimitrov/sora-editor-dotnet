using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Hardware.Lights;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;
using IO.Github.Rosemoe.Sora.Event;
using IO.Github.Rosemoe.Sora.Widget;
using IO.Github.Rosemoe.Sora.Widget.Base;
using IO.Github.Rosemoe.Sora.Widget.Schemes;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Icu.Text.ListFormatter;
using static Android.Provider.DocumentsContract;
using static Android.Webkit.WebSettings;

namespace SoraEditorBindings.Additions
{
	public class EditorCustomBasicTooltip : EditorPopupWindow
	{
		protected AppCompatTextView textView;
		protected int textSizeDp = 16;
		protected View root;

		public EditorCustomBasicTooltip(CodeEditor editor) : base(editor, FeatureHideWhenFastScroll)
		{
			editor.SubscribeEvent<ColorSchemeUpdateEvent>(OnApplyColorScheme);
			editor.SubscribeEvent<SelectionChangeEvent>(OnEditorSelectionChange);
			SetContentView(GetRoot());
		}
		public EditorCustomBasicTooltip(CodeEditor editor, int features) : base(editor, features)
		{
			editor.SubscribeEvent<ColorSchemeUpdateEvent>(OnApplyColorScheme);
			editor.SubscribeEvent<SelectionChangeEvent>(OnEditorSelectionChange);
			editor.SubscribeEvent<ScrollEvent>(OnEditorScrollChange);
			SetContentView(GetRoot());
		}

		protected virtual void OnEditorScrollChange(Java.Lang.Object scrollEvent, Unsubscribe arg2)
		{
			var change = scrollEvent.JavaCast<ScrollEvent>();
			if (change.Cause == ScrollEvent.CauseUserDrag || change.Cause == ScrollEvent.CauseTextSelecting)
			{
				if (IsShowing)
				{
					Dismiss();
				}
			}
		}

		protected virtual void OnEditorSelectionChange(Java.Lang.Object selectionChangeEvent, Unsubscribe unsub)
		{
			var change = selectionChangeEvent.JavaCast<SelectionChangeEvent>();
			if (change.Cause == SelectionChangeEvent.CauseTap)
			{
				if (IsShowing)
				{
					Dismiss();
				}
			}
		}

		public PopupPosition PopupPosition { get; private set; }
		public int MaxHeight { get; set; }
		public int MaxWidth { get; set; }

		public AppCompatTextView GetTextView()
		{
			if (textView == null)
			{
				textView = new AppCompatTextView(Editor.Context);
				textView.Background = GetBackground(Editor.ColorScheme);
				textView.SetTextColor(new Color(Editor.ColorScheme.GetColor(EditorColorScheme.CompletionWndTextSecondary)));
			}
			return textView;
		}

		public virtual View GetRoot()
		{
			if (root == null)
			{
				var lin = new LinearLayout(Editor.Context);
				lin.Orientation = Orientation.Vertical;
				lin.AddView(GetTextView(), ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
				root = lin;
			}
			return root;
		}

		private void OnApplyColorScheme(Java.Lang.Object colorSchemeUpdateEvent, Unsubscribe unsub)
		{
			var scheme = colorSchemeUpdateEvent.JavaCast<ColorSchemeUpdateEvent>().ColorScheme;
			GetTextView().Background = GetBackground(scheme);
			GetTextView().SetTextColor(new Color(scheme.GetColor(EditorColorScheme.CompletionWndTextSecondary)));
		}

		protected virtual Drawable GetBackground(EditorColorScheme scheme)
		{
			var gd = new GradientDrawable();
			gd.SetCornerRadius(TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Editor.Context.Resources.DisplayMetrics));
			gd.SetStroke(1, new Color(scheme.GetColor(EditorColorScheme.CompletionWndCorner)));
			gd.SetColor(scheme.GetColor(EditorColorScheme.CompletionWndBackground));
			return gd;
		}

		public int TextSizeSp
		{
			get
			{
				return textSizeDp;
			}
			set
			{
				textSizeDp = value;
				GetTextView().TextSize = value;
			}
		}

		public virtual void Show(PopupPosition? pos = null)
		{
			ApplyWindowSize();
			ApplyWindowPosition(pos);
			if (IsShowing)
			{
				return;
			}
			base.Show();
		}

		public override void Show()
		{
			Show(null);
		}

		protected virtual void ApplyWindowSize()
		{
			var view = this.GetRoot();
			if (MaxWidth == 0)
			{
				MaxWidth = Editor.Width / 2;
			}
			if (MaxHeight == 0)
			{
				MaxHeight = Editor.Height / 3;
			}
			var widthMeasureSpec = View.MeasureSpec.MakeMeasureSpec(MaxWidth, MeasureSpecMode.AtMost);
			var heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(MaxHeight, MeasureSpecMode.AtMost);
			view.Measure(widthMeasureSpec, heightMeasureSpec);

			var width = view.MeasuredWidth;
			var height = view.MeasuredHeight;
			SetSize(width, height);
		}

		protected virtual void ApplyWindowPosition(PopupPosition? pos)
		{
			var width = Width;
			var height = Height;
			var line = Editor.Cursor.LeftLine;
			var col = Editor.Cursor.LeftColumn;
			int x = (int)((Editor.GetOffset(line, col) - (width / 6)));
			int y = (int)(Editor.RowHeight * line) - Editor.OffsetY;
			int spansDownToRow = (y + Height) / Editor.RowHeight;
			if (pos == null)
			{

				if (spansDownToRow > Editor.LastVisibleRow + 1)
				{
					y -= height - 5;
					PopupPosition = PopupPosition.AboveRow;
				}
				else
				{
					y += Editor.RowHeight + 5;
					PopupPosition = PopupPosition.BelowRow;
				}
			}
			else if (pos == PopupPosition.AboveRow)
			{
				y -= height - 5;
				PopupPosition = PopupPosition.AboveRow;
			}
			else if (pos == PopupPosition.BelowRow)
			{
				y += Editor.RowHeight + 5;
				PopupPosition = PopupPosition.BelowRow;
			}

			if (x + width > Editor.Width)
			{
				x -= x + width - Editor.Width;
			}
			else if (x < 0)
			{
				x = 0;
			}

			SetLocationAbsolutely(x, y);
		}
	}
}
