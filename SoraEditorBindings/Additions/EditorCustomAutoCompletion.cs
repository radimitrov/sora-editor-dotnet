using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using IO.Github.Rosemoe.Sora.Event;
using IO.Github.Rosemoe.Sora.Lang.Completion;
using IO.Github.Rosemoe.Sora.Widget;
using IO.Github.Rosemoe.Sora.Widget.Base;
using IO.Github.Rosemoe.Sora.Widget.Component;
using IO.Github.Rosemoe.Sora.Widget.Schemes;
using Java.Interop;
using Microsoft.Maui.ApplicationModel;
using Org.Apache.Http.Conn.Schemes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static AndroidX.Browser.CustomTabs.CustomTabsIntent;

namespace SoraEditorBindings.Additions
{
	public class EditorCustomAutoCompletion : EditorPopupWindow
	{
		protected ListView listView;
		protected BaseAdapter adapter;
		protected ViewGroup root;
		private int detailsTextSizeSp = 15;
		private int textSizeSp = 18;

		public List<CompletionItem> CompletionList { get; private set; }


		public bool PerformingCompletion { get; private set; }
		public int TextSizeSp
		{
			get { return textSizeSp; }
			set
			{
				if (textSizeSp != value)
				{
					adapter = null;
					listView.Adapter = null;
				}
				textSizeSp = value;
			}
		}
		public int DetailsTextSizeSp
		{
			get { return detailsTextSizeSp; }
			set
			{
				if (detailsTextSizeSp != value)
				{
					adapter = null;
					listView.Adapter = null;
				}
				detailsTextSizeSp = value;
			}
		}
		public bool EnterKeyInsertsCompletion { get; set; } = true;

		public PopupPosition PopupPosition { get; private set; }

		public EditorCustomAutoCompletion(CodeEditor editor) : base(editor, FeatureHideWhenFastScroll)
		{
			SetContentView(GetRoot());
			SubscribeToEditorEvents(editor);
		}

		public EditorCustomAutoCompletion(CodeEditor editor, int features) : base(editor, features)
		{
			SetContentView(GetRoot());
			SubscribeToEditorEvents(editor);
		}

		private void SubscribeToEditorEvents(CodeEditor editor)
		{
			editor.SubscribeEvent<EditorKeyEvent>(OnEditorKeyEvent);
			editor.SubscribeEvent<ColorSchemeUpdateEvent>(OnApplyColorScheme);
			editor.SubscribeEvent<SelectionChangeEvent>(OnEditorSelectionChange);
			editor.SubscribeEvent<ScrollEvent>(OnEditorScrollChange);
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

		private void OnEditorSelectionChange(Java.Lang.Object selectionChangeEvent, Unsubscribe unsub)
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

		protected void OnEditorKeyEvent(Java.Lang.Object editorKeyEvent, Unsubscribe unsub)
		{
			if (!EnterKeyInsertsCompletion)
			{
				return;
			}
			var ev = JavaObjectExtensions.JavaCast<EditorKeyEvent>(editorKeyEvent);
			if (ev.Action != (int)KeyEventActions.Down)
			{
				return;
			}
			if (ev.KeyCode == (int)Keycode.Enter || ev.KeyCode == (int)Keycode.NumpadEnter)
			{
				if (IsShowing && listView.Count != 0 && ev.CanIntercept())
				{
					ev.Intercept();
					try
					{
						InsertCompletion();
					}
					catch
					{

					}
				}
			}
		}


		public virtual ListView GetListView()
		{
			if (listView == null)
			{
				listView = new ListView(Editor.Context);
				listView.Adapter = GetAdapter();
				listView.ItemClick += (sender, e) => InsertCompletion(e.Position);
			}
			if (null == listView.Adapter)
			{
				listView.Adapter = GetAdapter();
			}
			return listView;
		}

		public virtual View GetRoot()
		{
			if (root != null)
			{
				return root;
			}

			var lin = new LinearLayout(Editor.Context);
			lin.AddView(GetListView(), ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			root = lin;
			lin.Background = GetBackground(Editor.ColorScheme);
			return lin;
		}

		public virtual BaseAdapter GetAdapter()
		{
			if (adapter != null)
			{
				return adapter;
			}
			CompletionList = new List<CompletionItem>();
			adapter = new EditorCustomCompletionAdapter(Editor, CompletionList)
			{
				TextSizeSp = this.TextSizeSp,
				DetailsTextSizeSp = this.DetailsTextSizeSp
			};
			return adapter;
		}




		protected virtual Drawable GetBackground(EditorColorScheme scheme)
		{
			var gd = new GradientDrawable();
			gd.SetCornerRadius(TypedValue.ApplyDimension(ComplexUnitType.Dip, 8, Editor.Context.Resources.DisplayMetrics));
			gd.SetStroke(1, new Color(scheme.GetColor(EditorColorScheme.CompletionWndCorner)));
			gd.SetColor(scheme.GetColor(EditorColorScheme.CompletionWndBackground));
			return gd;
		}

		private void OnApplyColorScheme(Java.Lang.Object colorSchemeUpdateEvent, Unsubscribe unsub)
		{
			var scheme = colorSchemeUpdateEvent.JavaCast<ColorSchemeUpdateEvent>().ColorScheme;
			GetRoot().Background = GetBackground(scheme);
		}
		public void InsertCompletion()
		{
			InsertCompletion(listView.SelectedItemPosition == -1 ? 0 : listView.SelectedItemPosition);
		}

		public virtual void InsertCompletion(int listPos)
		{
			PerformingCompletion = true;
			try
			{
				Dismiss();
				var item = GetAdapter().GetItem(listPos).JavaCast<CompletionItem>();
				Editor.RestartInput();
				Editor.Text.BeginBatchEdit();
				item.PerformCompletion(Editor, Editor.Text, Editor.Cursor.LeftLine, Editor.Cursor.LeftColumn);
				Editor.Text.EndBatchEdit();
				Editor.UpdateCursor();
				Editor.RestartInput();
			}
			finally
			{
				Editor.PostDelayedInLifecycle(() => PerformingCompletion = false, 10);
			}
		}

		public void SetCompletions(IList<CompletionItem> items, bool show = true)
		{
			
			CompletionList.Clear();
			CompletionList.AddRange(items);
			((EditorCustomCompletionAdapter)GetAdapter()).SetItems(items);
			GetAdapter().NotifyDataSetChanged();
			/*MainThread.BeginInvokeOnMainThread(()=>
			{

			});*/
			
			/*if (CompletionList.Count > 0)
			{
				GetListView().SetSelection(0);
			}*/
			if (show)
			{
				Show();
			}

		}



		public override void Show()
		{
			if (CompletionList.Count == 0)
			{
				if (IsShowing)
				{
					Dismiss();
				}
				return;
			}
			ApplyWindowSize();
			ApplyWindowPosition();
			if (IsShowing)
			{
				return;
			}
			base.Show();
		}

		protected virtual void ApplyWindowSize()
		{
			var adapter = GetAdapter();
			var itemHeight = Utils.SpToPx(Editor.Context, TextSizeSp + DetailsTextSizeSp) + 4;
			var maxHeight = (int)(itemHeight * 4.2);
			var charHeight = Utils.SpToPx(Editor.Context, TextSizeSp);
			var width = (charHeight + 2) * 10;
			var height = Math.Min(itemHeight * adapter.Count, maxHeight);
			SetSize(width, height);

		}

		protected virtual void ApplyWindowPosition()
		{
			var width = Width;
			var height = Height;
			var line = Editor.Cursor.LeftLine;
			var col = Editor.Cursor.LeftColumn;
			int x = (int)((Editor.GetOffset(line, col) - (width / 6)));
			int y = (int)(Editor.RowHeight * line) - Editor.OffsetY;
			int spansDownToRow = (y + Height) / Editor.RowHeight;
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
