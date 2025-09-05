using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Helpers
{
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        private readonly IntPtr _hwnd;

        // Property
        public virtual IntPtr Handle
        {
            get { return _hwnd; }
        }

        // Constructor
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }
    }
}
