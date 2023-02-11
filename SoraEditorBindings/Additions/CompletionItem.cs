using Android.Runtime;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Lang.Completion
{
	public partial class CompletionItem
	{
		// Metadata.xml XPath field reference: path="/api/package[@name='io.github.rosemoe.sora.lang.completion']/class[@name='CompletionItem']/field[@name='desc']"
		[Register("desc")]
		public virtual global::Java.Lang.ICharSequence Desc
		{
			get
			{
				const string __id = "desc.Ljava/lang/CharSequence;";

				var __v = _members.InstanceFields.GetObjectValue(__id, this);
				return global::Java.Lang.Object.GetObject<Java.Lang.ICharSequence>(__v.Handle, JniHandleOwnership.TransferLocalRef);
			}
			set
			{
				const string __id = "desc.Ljava/lang/CharSequence;";

				IntPtr native_value = CharSequence.ToLocalJniHandle(value);
				try
				{
					_members.InstanceFields.SetValue(__id, this, new JniObjectReference(native_value));
				}
				finally
				{
					JNIEnv.DeleteLocalRef(native_value);
				}
			}
		}


		// Metadata.xml XPath field reference: path="/api/package[@name='io.github.rosemoe.sora.lang.completion']/class[@name='CompletionItem']/field[@name='icon']"
		[Register("icon")]
		public virtual global::Android.Graphics.Drawables.Drawable Icon
		{
			get
			{
				const string __id = "icon.Landroid/graphics/drawable/Drawable;";

				var __v = _members.InstanceFields.GetObjectValue(__id, this);
				return global::Java.Lang.Object.GetObject<global::Android.Graphics.Drawables.Drawable>(__v.Handle, JniHandleOwnership.TransferLocalRef);
			}
			set
			{
				const string __id = "icon.Landroid/graphics/drawable/Drawable;";

				IntPtr native_value = global::Android.Runtime.JNIEnv.ToLocalJniHandle(value);
				try
				{
					_members.InstanceFields.SetValue(__id, this, new JniObjectReference(native_value));
				}
				finally
				{
					global::Android.Runtime.JNIEnv.DeleteLocalRef(native_value);
				}
			}
		}


		// Metadata.xml XPath field reference: path="/api/package[@name='io.github.rosemoe.sora.lang.completion']/class[@name='CompletionItem']/field[@name='label']"
		[Register("label")]
		public virtual global::Java.Lang.ICharSequence Label
		{
			get
			{
				const string __id = "label.Ljava/lang/CharSequence;";

				var __v = _members.InstanceFields.GetObjectValue(__id, this);
				return global::Java.Lang.Object.GetObject<Java.Lang.ICharSequence>(__v.Handle, JniHandleOwnership.TransferLocalRef);
			}
			set
			{
				const string __id = "label.Ljava/lang/CharSequence;";

				IntPtr native_value = CharSequence.ToLocalJniHandle(value);
				try
				{
					_members.InstanceFields.SetValue(__id, this, new JniObjectReference(native_value));
				}
				finally
				{
					JNIEnv.DeleteLocalRef(native_value);
				}
			}
		}
	}
}
