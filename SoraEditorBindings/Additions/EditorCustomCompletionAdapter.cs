using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Core.Graphics.Drawable;
using AndroidX.Core.View;
using IO.Github.Rosemoe.Sora.Lang.Completion;
using IO.Github.Rosemoe.Sora.Widget;
using IO.Github.Rosemoe.Sora.Widget.Component;
using IO.Github.Rosemoe.Sora.Widget.Schemes;
using Java.Security;
using System.Runtime.CompilerServices;

namespace SoraEditorBindings.Additions;

internal class EditorCustomCompletionAdapter : BaseAdapter<CompletionItem>, IListAdapter
{
	private readonly CodeEditor editor;
	List<CompletionItem> completionItems;

	public EditorCustomCompletionAdapter(CodeEditor editor, List<CompletionItem> list)
	{
		if (list == null)
		{
			throw new ApplicationException($"{nameof(completionItems)} can't be null");
		}
		this.editor = editor;
		this.completionItems = new List<CompletionItem>(list);
	}

	public virtual void SetItems(IEnumerable<CompletionItem> items)
	{
		completionItems.Clear();
		completionItems.AddRange(items);
	}

	public override CompletionItem this[int position] => completionItems[position];

	public override int Count => completionItems.Count;

	public override long GetItemId(int position)
	{
		return position;
	}

	public int TextSizeSp { get; init; } = 20;
	public int DetailsTextSizeSp { get; init; } = 15;


	protected virtual void ConfigureNewView(View convertView, ViewHolder holder)
	{
		var img = holder.Icon;
		var text = holder.Text;
		text.TextSize = TextSizeSp;
		var details = convertView.FindViewById<TextView>(Resource.Id.cilDetails);
		details.TextSize = DetailsTextSizeSp;
		var pxHeight = Utils.SpToPx(editor.Context, TextSizeSp + DetailsTextSizeSp + 2);
		var pars = convertView.LayoutParameters;
		pars.Height = pxHeight;
		convertView.LayoutParameters = pars;
		var parsImg = img.LayoutParameters.JavaCast<ViewGroup.MarginLayoutParams>();

		var imgMargins = (int)(pxHeight * 0.25);
		parsImg.TopMargin = imgMargins;
		parsImg.BottomMargin = imgMargins;
		img.LayoutParameters = parsImg;

		text.SetTextColor(new Color(editor.ColorScheme.GetColor(EditorColorScheme.CompletionWndTextPrimary)));
		details.SetTextColor(new Color(editor.ColorScheme.GetColor(EditorColorScheme.CompletionWndTextSecondary)));

	}

	public override View GetView(int position, View convertView, ViewGroup parent)
	{
		ViewHolder holder;
		if (convertView == null)
		{
			var inflater = (LayoutInflater)editor.Context.GetSystemService(Context.LayoutInflaterService);
			convertView = inflater.Inflate(Resource.Layout.simplecompletionitemlayout, parent, false);
			holder = new ViewHolder(convertView);
			ConfigureNewView(convertView, holder);
			
			convertView.Tag = holder;
		}
		else
		{
			holder = (ViewHolder)convertView.Tag;
		}

		var item = GetItem(position).JavaCast<CompletionItem>();

		holder.Icon.SetImageDrawable(item.Icon);
		holder.Text.TextFormatted = item.Label;
		holder.Details.TextFormatted = item.Desc;
		if ((item.Desc == null || item.Desc.Length() == 0) && holder.Details.Visibility != ViewStates.Gone)
		{
			holder.Details.Visibility = ViewStates.Gone;
		}
		else if (holder.Details.Visibility != ViewStates.Visible)
		{
			holder.Details.Visibility = ViewStates.Visible;
		}

		return convertView;
	}

	public class ViewHolder : Java.Lang.Object
	{
		public ViewHolder(View root)
		{
			Icon = root.FindViewById<ImageView>(Resource.Id.cilIcon);
			Text = root.FindViewById<TextView>(Resource.Id.cilText);
			Details = root.FindViewById<TextView>(Resource.Id.cilDetails);
		}

		public ImageView Icon { get; }
		public TextView Text { get; }
		public TextView Details { get; }
	}
}
