using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLL
{
    public class DialogBlo
    {
        private static string resultString;

        /// <summary>
        /// Dialog para selecionar pasta
        /// </summary>
        /// <returns>Pasta Selecionada</returns>
        public static string FolderBrowserDialog(string selectedPath = null)
        {
            Thread thread = new Thread(() => ShowFolderBrowserDialog(selectedPath));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return resultString;
        }

        /// <summary>
        /// Dialog para selecionar arquivo
        /// </summary>
        /// <returns>Arquivo Selecionado</returns>
        public static string OpenFileDialog(string filter = null)
        {
            Thread thread = new Thread(() => ShowOpenFileDialog(filter));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return resultString;
        }

        private static void ShowFolderBrowserDialog(string selectedPath = null)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(selectedPath))
            {
                fbd.SelectedPath = selectedPath;
            }

            if (fbd.ShowDialog(WindowWrapper.GetForegroundWindowWrapper()) == DialogResult.OK)
            {
                resultString = fbd.SelectedPath;
            }
            System.Threading.Thread.CurrentThread.Abort();
        }

        private static void ShowOpenFileDialog(string filter = null)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (!String.IsNullOrEmpty(filter))
            {
                ofd.Filter = filter;
            }
            if (ofd.ShowDialog(WindowWrapper.GetForegroundWindowWrapper()) == DialogResult.OK)
            {
                resultString = ofd.FileName;
            }
            System.Threading.Thread.CurrentThread.Abort();
        }
    }

    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        private IntPtr handle;

        public virtual IntPtr Handle
        {
            get
            {
                return handle;
            }
        }

        public WindowWrapper(IntPtr handle)
        {
            this.handle = handle;
        }

        public static WindowWrapper GetForegroundWindowWrapper()
        {
            return new WindowWrapper(GetForegroundWindow());
        }
    }
}
