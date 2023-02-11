using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Github.Rosemoe.Sora.Widget.Base
{
	public partial class EditorPopupWindow
	{

		protected void ApplyWindowAttributes(bool state)
		{
			unsafe
			{
				const string __id = "applyWindowAttributes.(Z)V";
				try
				{
					JniArgumentValue* __args = stackalloc JniArgumentValue[1];
					__args[0] = new JniArgumentValue(state);
					_members.InstanceMethods.InvokeVirtualVoidMethod(__id, this, __args);
				}
				finally
				{
				}
			}
		}
	}
}
