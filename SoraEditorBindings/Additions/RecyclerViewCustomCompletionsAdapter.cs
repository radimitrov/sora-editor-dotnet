using Android.Graphics;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using IO.Github.Rosemoe.Sora.Lang.Completion;
using IO.Github.Rosemoe.Sora.Widget;
using IO.Github.Rosemoe.Sora.Widget.Schemes;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Webkit.WebSettings;

namespace SoraEditorBindings.Additions
{
    public class RecyclerViewCustomCompletionsAdapter : RecyclerView.Adapter
    {
        private readonly CodeEditor editor;
        private readonly List<CompletionItem> items;
        public int TextSizeSp { get; init; } = 20;
        public int DetailsTextSizeSp { get; init; } = 15;

        public RecyclerViewCustomCompletionsAdapter(CodeEditor editor, List<CompletionItem> items)
        {
            this.editor = editor;
            this.items = items;
        }

        public override int ItemCount => items.Count;

        public CompletionItem GetItem(int index)
        {
            return items[index];
        }

        protected virtual void ConfigureNewView(ViewHolder holder)
        {
            var img = holder.Icon;
            var text = holder.Text;
            text.TextSize = TextSizeSp;
            var details = holder.Details;
            details.TextSize = DetailsTextSizeSp;
            var pxHeight = Utils.SpToPx(editor.Context, TextSizeSp + DetailsTextSizeSp + 2);
            var pars = holder.ItemView.LayoutParameters;
            pars.Height = pxHeight;
            holder.ItemView.LayoutParameters = pars;
            var parsImg = img.LayoutParameters.JavaCast<ViewGroup.MarginLayoutParams>();

            var imgMargins = (int)(pxHeight * 0.25);
            parsImg.TopMargin = imgMargins;
            parsImg.BottomMargin = imgMargins;
            img.LayoutParameters = parsImg;

            text.SetTextColor(new Color(editor.ColorScheme.GetColor(EditorColorScheme.CompletionWndTextPrimary)));
            details.SetTextColor(new Color(editor.ColorScheme.GetColor(EditorColorScheme.CompletionWndTextSecondary)));

        }

        public void SetItems(IEnumerable<CompletionItem> items)
        {
            this.items.Clear();
            this.items.AddRange(items);
            NotifyDataSetChanged();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layoutInflater = LayoutInflater.From(parent.Context);
            var listItem = layoutInflater.Inflate(Resource.Layout.simplecompletionitemlayout, parent, false);
            var viewHolder = new ViewHolder(listItem);
            ConfigureNewView(viewHolder);
            return viewHolder;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];
            var holder = (ViewHolder)viewHolder;
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
            holder.ItemView.SetOnClickListener(new ClickListener(() =>
            {
				ItemClick?.Invoke(holder.ItemView, new ClickEventArgs()
                {
                    Item = item,
                    Position = position
                });
            }));
			holder.ItemView.SetOnLongClickListener(new LongClickListener(() =>
			{
				ItemLongClick?.Invoke(holder.ItemView, new ClickEventArgs()
				{
					Item = item,
					Position = position
				});
			}));
		}

        public event EventHandler<ClickEventArgs> ItemClick;
		public event EventHandler<ClickEventArgs> ItemLongClick;

		public class ClickEventArgs : EventArgs
        {
            public int Position { get; set; }
            public CompletionItem Item { get; set; }    
        }

		public class ClickListener : Java.Lang.Object, View.IOnClickListener
		{
			private readonly Action a;

			public ClickListener(Action a)
            {
				this.a = a;
			}
			public void OnClick(View v)
			{
                a.Invoke();
			}
		}

		public class LongClickListener : Java.Lang.Object, View.IOnLongClickListener
		{
			private readonly Action a;

			public LongClickListener(Action a)
			{
				this.a = a;
			}
			public bool OnLongClick(View v)
			{
				a.Invoke();
                return true;
			}
		}

		public class ViewHolder : RecyclerView.ViewHolder
        {
            public ViewHolder(View root) : base(root)
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
}
