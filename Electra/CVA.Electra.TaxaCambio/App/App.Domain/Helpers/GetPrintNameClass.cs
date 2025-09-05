using EtiquetaWpf;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.Domain.Helpers
{
    /// <summary>
    /// Wrapper for OpenFileDialog
    /// </summary>
    public class GetPrintNameClass
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow(); 

        private readonly System.Windows.Forms.PrintDialog _PrintDialog;
        private String _Conteudo;
        private String _pkInstalledPrinters;
        private String _NomeImpressora;

        public String NomeImpressora
        {
            set { _NomeImpressora = value; }
        }

        public String Conteudo
        {
            set { _Conteudo = value; }
        }

        public String pkInstalledPrinters
        {
            set { _pkInstalledPrinters = value; }
        }

        // Constructor
        public GetPrintNameClass()
        {
            _PrintDialog = new System.Windows.Forms.PrintDialog();
        }

        // Methods
        public void GetPrintName()
        {
            IntPtr ptr = GetForegroundWindow();
            WindowWrapper oWindow = new WindowWrapper(ptr);
            PrinterSettings ps = new PrinterSettings();

            //-- Abre a caixa para escolher a impressora
            if (String.IsNullOrEmpty(_NomeImpressora))
            {
                if (_PrintDialog.ShowDialog(oWindow) == DialogResult.OK)
                {
                    // Send a printer-specific to the printer.
                    _NomeImpressora = _PrintDialog.PrinterSettings.PrinterName;
                    RawPrinterHelper.SendStringToPrinter(_PrintDialog.PrinterSettings.PrinterName, _Conteudo);
                }
            }
            else
                RawPrinterHelper.SendStringToPrinter(_NomeImpressora, _Conteudo);

            /*
            //-- Pega a impressora padrão
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                String pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                RawPrinterHelper.SendStringToPrinter(pkInstalledPrinters, _Conteudo);
                break;
            }
            */

            /*
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                String pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                if (pkInstalledPrinters.Contains(_pkInstalledPrinters))
                {
                    RawPrinterHelper.SendStringToPrinter(pkInstalledPrinters, _Conteudo);
                }
            }
            */

            //foreach (String printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    if (printer == ps.PrinterName)
            //        if (ps.IsDefaultPrinter) //Just trying with default printer for now. It will change later.
            //            RawPrinterHelper.SendStringToPrinter(printer, _Conteudo);
            //}
        }
    }

}
