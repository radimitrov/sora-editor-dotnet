using Android.Text;
using Android.Views;
using AndroidX.AppCompat.Widget;
using IO.Github.Rosemoe.Sora;
using IO.Github.Rosemoe.Sora.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace SoraEditorBindings.Additions
{
	public class CustomSearchActionMode : Java.Lang.Object, ActionMode.ICallback
	{
		private readonly CodeEditor editor;
		private static int? actionModeNone;
		private static int? actionModeSearchText;

		public string Query { get; set; }
		public string QueryReplace { get; set; }
		public bool CaseInsensitive { get; set; }
		public bool UseRegex { get; set; }

		public const int CaseInsensitiveId = 0;
		public const int UseRegexId = 1;
		public const int PreviousId = 2;
		public const int NextId = 3;
		public const int ReplaceId = 4;
		public const int ReplaceAllId = 5;

		public event EventHandler ActionModeCreated;
		public event EventHandler<ActionMode> DismissedActionMode;

		public CustomSearchActionMode(CodeEditor editor) 
		{
			this.editor = editor;
		}

		public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
		{
			if (!editor.Searcher.HasQuery)
			{
				switch (item.ItemId)
				{
					case CaseInsensitiveId: CaseInsensitive = !CaseInsensitive; break;
					case UseRegexId: UseRegex = !UseRegex; break;
				}
				return false;
			}

			switch (item.ItemId)
			{
				case CaseInsensitiveId:CaseInsensitive = !CaseInsensitive; break;
				case UseRegexId:UseRegex = !UseRegex; break;
				case PreviousId: editor.Searcher.GotoPrevious(); break;
				case NextId: editor.Searcher.GotoNext(); break;
				case ReplaceId:
				case ReplaceAllId:
					var text = new AppCompatEditText(editor.Context);
					text.Text = QueryReplace;
					new AndroidX.AppCompat.App.AlertDialog.Builder(editor.Context)
						.SetTitle(item.TitleFormatted)
						.SetView(text)
						.SetNegativeButton(Android.Resource.String.Cancel, delegate { })
						.SetPositiveButton(item.TitleFormatted, delegate
						{
							try
							{
								if (item.ItemId == ReplaceAllId)
								{
									editor.Searcher.ReplaceAll(text.Text, new Runnable(() => mode.Finish()));
								}
								else
								{
									editor.Searcher.ReplaceThis(text.Text);
								}
							}
							catch(System.Exception ex)
							{
								new AndroidX.AppCompat.App.AlertDialog.Builder(editor.Context)
									.SetTitle(ex.GetType().Name)
									.SetMessage(ex.Message)
									.SetPositiveButton(Android.Resource.String.Ok, delegate { })
									.Show();
							}
						})
						.Show();
					break;
				default:
					break;
			}

			return false;
		}

		public ActionMode ActionMode { get; private set; }
		public LinearLayout Root { get; private set; }

		public bool OnCreateActionMode(ActionMode mode, IMenu menu)
		{
			ActionMode = mode;
			var startedActionMode = editor.Class.GetDeclaredField("startedActionMode");
			startedActionMode.Accessible = true;
			startedActionMode.SetInt(editor, ActionModeSearchText());

			menu.Add(0, CaseInsensitiveId, 0, "Case insensitive")
				.SetShowAsActionFlags(ShowAsAction.Never)
				.SetCheckable(true)
				.SetChecked(CaseInsensitive);
			menu.Add(0, UseRegexId, 0, "Regex")
				.SetShowAsActionFlags(ShowAsAction.Never)
				.SetCheckable(true)
				.SetChecked(UseRegex);
			menu.Add(1, PreviousId, 0, "Previous").SetShowAsActionFlags(ShowAsAction.Never);
			menu.Add(1, NextId, 0, "Next").SetShowAsActionFlags(ShowAsAction.IfRoom);
			menu.Add(1, ReplaceId, 0, "Replace").SetShowAsActionFlags(ShowAsAction.Never);
			menu.Add(1, ReplaceAllId, 0, "Replace all").SetShowAsActionFlags(ShowAsAction.Never);

			var sv = new SearchViewNoExtractUi(editor.Context);
			var lin = new LinearLayout(editor.Context);
			var cnt = new AppCompatTextView(editor.Context);
			try
			{
				cnt.SetTextAppearance(editor.Context, Resource.Style.TextAppearance_AppCompat_Small);
			}
			catch
			{

			}
			lin.Orientation = Orientation.Vertical;
			lin.AddView(sv, LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			lin.AddView(cnt, LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			Root = lin;
			sv.SetQuery(Query, false);
			
			sv.QueryTextChange += (s, e) =>
			{
				Query = e.NewText;
				if (string.IsNullOrEmpty(e.NewText))
				{
					editor.Searcher.StopSearch();
				}
				try
				{
					editor.Searcher.Search(e.NewText, new EditorSearcher.SearchOptions(CaseInsensitive, UseRegex));
				}
				catch(System.Exception ex)
				{
					cnt.Text = string.Empty;
				}

				cnt.Text = editor.Searcher.LastResultsCount.ToString();
			};
			sv.QueryTextSubmit += (s, e) => editor.Searcher.GotoNext();
			mode.CustomView = lin;
			sv.PerformClick();
			sv.SetIconifiedByDefault(false);
			sv.Iconified = false;
			ActionModeCreated?.Invoke(this, EventArgs.Empty);
			return true;
		}


		public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
		{
			mode.Menu.FindItem(CaseInsensitiveId).SetChecked(CaseInsensitive);
			mode.Menu.FindItem(UseRegexId).SetChecked(UseRegex);
			return true;
		}


		//Yeah, reflection... We need the field constants here.
		//Better than harcoding. Probably.
		private int ActionModeNone()
		{
			if (actionModeNone.HasValue)
			{
				return actionModeNone.Value;
			}
			var field = editor.Class.GetDeclaredField("ACTION_MODE_NONE");
			field.Accessible = true;
			actionModeNone = field.GetInt(null);
			return actionModeNone.Value;
		}

		private int ActionModeSearchText()
		{
			if (actionModeSearchText.HasValue)
			{
				return actionModeSearchText.Value;
			}
			var field = editor.Class.GetDeclaredField("ACTION_MODE_SEARCH_TEXT");
			field.Accessible = true;
			actionModeSearchText = field.GetInt(null);
			return actionModeSearchText.Value;
		}

		public void OnDestroyActionMode(ActionMode mode)
		{
			//Yes, reflection again.
			var startedActionMode = editor.Class.GetDeclaredField("startedActionMode");
			startedActionMode.Accessible = true;
			startedActionMode.SetInt(editor, ActionModeNone());
			DismissedActionMode?.Invoke(this, mode);
		}
	}
}
