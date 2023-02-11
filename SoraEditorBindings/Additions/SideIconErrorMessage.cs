using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoraEditorBindings
{
    public class SideIconErrorMessage : Java.Lang.Object
    {

        public SideIconErrorMessage()
        {
        }

        public SideIconErrorMessage(string message)
        {
            Messages.Add(message);
        }

        public SideIconErrorMessage(params string[] messages)
        {
            Messages.AddRange(messages);
        }

        public SideIconErrorMessage(IEnumerable<string> messages)
        {
            Messages.AddRange(messages);
        }

        public List<string> Messages { get; } = new List<string>();
    }
}
