using Android.Content;
using Android.Drm;
using Android.Graphics.Drawables;
using Android.Views;
using IO.Github.Rosemoe.Sora.Event;
using IO.Github.Rosemoe.Sora.Lang.Styling.InlayHint;
using IO.Github.Rosemoe.Sora.Lang.Styling.Line;
using IO.Github.Rosemoe.Sora.Widget.Base;
using IO.Github.Rosemoe.Sora.Widget.Component;
using Java.Interop;
using Java.Lang;
using Microsoft.Maui.ApplicationModel;
using SoraEditorBindings;
using SoraEditorBindings.Additions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace IO.Github.Rosemoe.Sora.Widget
{
    public partial class CodeEditor
	{
		private string currentLanguage; 
		private string currentTheme;
		private HashSet<int> linesWithSideIcons = new HashSet<int>();
		private HashSet<int> linesWithBreakpointSideIcons = new HashSet<int>();




        public Event.SubscriptionReceipt SubscribeEvent<T>(IEventReceiver receiver)
		{
			var eventType = Java.Lang.Class.FromType(typeof(T));
			return this.SubscribeEvent(eventType, receiver);
		}

		public Event.SubscriptionReceipt SubscribeEvent<T>(Action<Java.Lang.Object, Unsubscribe> callback)
		{
			if (callback== null)
			{
				throw new ArgumentNullException(nameof(callback));
			}
			var receiver = new EventReceiver();
			receiver.Receive += (s, e) =>
			{
				callback.Invoke(e.Result, e.Unsubscribe);
			};
			return SubscribeEvent<T>(receiver);
		}

		
		public void PostDelayedInLifecycle(Action action, long delayMs)
		{
			var runnable = new Runnable(action);
			this.PostDelayedInLifecycle(runnable, delayMs);
		}

		public void SetText(string text, bool clearUndoRedoStack=true)
		{
			if (clearUndoRedoStack)
			{
				SetText(text);
				return;
			}
			this.Text.Replace(0, this.Text.Length() - 1, text);
		}


		/*public virtual void InitSideIconsSubscriber()
		{
            SubscribeEvent<SideIconClickEvent>((val, unsub) =>
            {
                var sideClick = val.JavaCast<SideIconClickEvent>();
                if (sideClick.ClickedIcon.CustomData == null || sideClick.ClickedIcon.CustomData.Class != Java.Lang.Class.FromType(typeof(SideIconErrorMessage)))
                {
                    return;
                }

                Post(() =>
                {
                    var msgs = sideClick.ClickedIcon.CustomData.JavaCast<SideIconErrorMessage>();
                    new AndroidX.AppCompat.App.AlertDialog.Builder(Context)
                        .SetTitle("Line " + sideClick.ClickedIcon.Line)
                        .SetCancelable(true)
                        .SetPositiveButton("OK", delegate { })
                        .SetMessage(string.Join("\n\n", msgs.Messages))
                        .Show();
                });
            });
        }

		public virtual void SetLineSideIcon(int line, Drawable icon)
		{
			var lineStyle = new LineSideIcon(line, icon);
			Styles.EraseLineStyle(line, Java.Lang.Class.FromType(typeof(LineSideIcon)));
			Styles.AddLineStyle(lineStyle);
			linesWithSideIcons.Add(line);
		}

		public virtual void SetLineSideIcon(int line, Drawable icon, string clickText)
		{
			var lineStyle = new LineSideIcon(line, icon);
			lineStyle.CustomData = new SideIconErrorMessage(clickText);
			Styles.EraseLineStyle(line, Java.Lang.Class.FromType(typeof(LineSideIcon)));
			Styles.AddLineStyle(lineStyle);
			linesWithSideIcons.Add(line);
		}

		public virtual void SetLineSideIcon(int line, Drawable icon, SideIconErrorMessage clickMsg)
		{
			var lineStyle = new LineSideIcon(line, icon);
			lineStyle.CustomData = clickMsg;
			Styles.EraseLineStyle(line, Java.Lang.Class.FromType(typeof(LineSideIcon)));
			Styles.AddLineStyle(lineStyle);
			linesWithSideIcons.Add(line);
		}



		public virtual void RemoveAllSideLineIcons()
		{
			foreach (var line in linesWithSideIcons)
			{
				Styles.EraseLineStyle(line, Java.Lang.Class.FromType(typeof(LineSideIcon)));
			}
			linesWithSideIcons.Clear();
		}


		public virtual void RemoveLineSideIcon(int line)
		{
			Styles.EraseLineStyle(line, Java.Lang.Class.FromType(typeof(LineSideIcon)));
			linesWithSideIcons.Remove(line);
		}

		public virtual bool HasLineSideIcon(int line)
		{
			return linesWithSideIcons.Contains(line);
		}


		//TODO
		public virtual void AddBreakpoint(int line)
		{
			linesWithBreakpointSideIcons.Add(line);
			

		}*/

		public virtual EditorDiagnosticTooltipWindow DiagnosticsTooltipWindow
		{
			get
			{
				return GetComponent(Java.Lang.Class.FromType(typeof(EditorDiagnosticTooltipWindow)))
						?.JavaCast<EditorDiagnosticTooltipWindow>();
			}
		}

	
		public CustomSearchActionMode CustomBeginSearchMode()
		{
			var am = new CustomSearchActionMode(this);
			StartActionMode(am);
			return am;
		}


	}
}
