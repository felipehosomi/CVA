using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Reflection;
using System.Drawing.Printing;
using System.Xml;

namespace CVA.Electra.ImportaFolha
{
    static public class Support
    {


        static public CultureInfo pt_BR = new CultureInfo("pt-BR");

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();


        public static void SaveLogFile(string FileName, string Text)
        {
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(FileName, true);
                file.WriteLine(Text);
                file.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void CloseActiveForm()
        {
            try
            {
                SAPbouiCOM.Form oForm = ImportaFolha.oApplication.Forms.ActiveForm;
                oForm.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtem valor do ChooseFromList
        /// </summary>
        /// <param name="evento">SAPbouiCOM.ItemEvent</param>
        /// <param name="ChooseFromList_ID"></param>
        /// <param name="Coluna"></param>
        /// <param name="Linha"></param>
        /// <returns></returns>
        public static string GetValChooseFromList(SAPbouiCOM.ItemEvent evento, string ChooseFromList_ID, string Coluna, int Linha)
        {
            try
            {
                string sCFL_ID = null;
                SAPbouiCOM.IChooseFromListEvent oCFLEvento = null;
                SAPbouiCOM.DataTable oDataTable = null;
                SAPbouiCOM.Form oForm = ImportaFolha.oApplication.Forms.GetFormByTypeAndCount(evento.FormType, evento.FormTypeCount);

                oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)evento;
                sCFL_ID = oCFLEvento.ChooseFromListUID;

                oDataTable = oCFLEvento.SelectedObjects;
                if (oDataTable != null)
                {
                    if (sCFL_ID == ChooseFromList_ID)
                    {
                        return oDataTable.GetValue(Coluna, Linha).ToString();
                    }
                }
                return "";
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Obter Impressora
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="initialDirectory"></param>
        /// <param name="dialogTitle"></param>
        /// <returns></returns>
        static public void GetPrintViaOFD(String s)
        {
            GetPrintNameClass oGetPrintName = new GetPrintNameClass();
            oGetPrintName.Conteudo = s;
            Thread threadGetPrint = new Thread(oGetPrintName.GetPrintName); 
            threadGetPrint.SetApartmentState(ApartmentState.STA);

            try
            {
                threadGetPrint.Start();

                // Wait a milisec more.
                Thread.Sleep(1);

                // Wait for thread to end.
                threadGetPrint.Join();


            }
            catch (Exception ex_GetFileNameViaOFD)
            {
                // Simply rethrow the exception.
                throw (ex_GetFileNameViaOFD);
            }
        }

        /// <summary>
        /// Wrapper for OpenFileDialog
        /// </summary>
        private class GetPrintNameClass
        {
            private readonly PrintDialog _PrintDialog;
            private String _Conteudo;


            public String Conteudo
            {
                set { _Conteudo = value; }
            }
            // Constructor
            public GetPrintNameClass()
            {
                _PrintDialog = new PrintDialog();
            }

            // Methods

            public void GetPrintName()
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);
                _PrintDialog.PrinterSettings = new PrinterSettings();
                if (_PrintDialog.ShowDialog(oWindow) == DialogResult.OK)
                {

                    // Send a printer-specific to the printer.
                    RawPrinterHelper.SendStringToPrinter(_PrintDialog.PrinterSettings.PrinterName, _Conteudo);
                }
            }
        }

        /// <summary>
        /// Obter nome do arquivo
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="initialDirectory"></param>
        /// <param name="dialogTitle"></param>
        /// <returns></returns>
        static public String GetFileNameViaOFD(String filter, String initialDirectory, String dialogTitle, bool CheckFileExists)
        {
            GetFileNameClass oGetFileName = new GetFileNameClass();
            oGetFileName.Filter = filter;
            oGetFileName.InitialDirectory = initialDirectory;
            oGetFileName.DialogTitle = dialogTitle;
            oGetFileName.CheckFileExists = CheckFileExists;
            Thread threadGetFileName = new Thread(oGetFileName.GetFileName);
            threadGetFileName.SetApartmentState(ApartmentState.STA);

            try
            {
                threadGetFileName.Start();

                // Wait for thread to get started.
                while (!threadGetFileName.IsAlive)
                {
                }

                // Wait a milisec more.
                Thread.Sleep(1);

                // Wait for thread to end.
                threadGetFileName.Join();

                // Use file name as you will here
                return oGetFileName.FileName;

            }
            catch (Exception ex_GetFileNameViaOFD)
            {
                // Simply rethrow the exception.
                throw (ex_GetFileNameViaOFD);
            }
        }

        #region Private supporting classes

        /// <summary>
        /// Wrapper for OpenFileDialog
        /// </summary>
        private class GetFileNameClass
        {
            private readonly OpenFileDialog _oFileDialog;

            // Properties
            public String DialogTitle
            {
                set { _oFileDialog.Title = value; }
            }

            public String FileName
            {
                get { return _oFileDialog.FileName; }
            }

            public String Filter
            {
                set { _oFileDialog.Filter = value; }
            }

            public String InitialDirectory
            {
                set { _oFileDialog.InitialDirectory = value; }
            }
            public bool CheckFileExists
            {
                set { _oFileDialog.CheckFileExists = value; }
            }


            // Constructor
            public GetFileNameClass()
            {
                _oFileDialog = new OpenFileDialog();
            }

            // Methods

            public void GetFileName()
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);
                if (_oFileDialog.ShowDialog(oWindow) != DialogResult.OK)
                {
                    _oFileDialog.FileName = string.Empty;
                }
            }
        }

        private class WindowWrapper : IWin32Window
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

        #endregion Private supporting classes

        static public string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            return sb.ToString();
        }

        private class GetFolderNameClass
        {
            private readonly FolderBrowserDialog _oFolderDialog;

            // Properties
            public String SelectedPath
            {
                get { return _oFolderDialog.SelectedPath; }
            }


            // Constructor
            public GetFolderNameClass()
            {
                _oFolderDialog = new FolderBrowserDialog();
            }

            // Methods

            public void GetFolderName()
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);
                if (_oFolderDialog.ShowDialog(oWindow) != DialogResult.OK)
                {
                    _oFolderDialog.SelectedPath = string.Empty;
                }
            }
        }

        static public String GetFolderNameViaOFD()
        {
            GetFolderNameClass oGetFolderName = new GetFolderNameClass();
            Thread threadGetFolderName = new Thread(oGetFolderName.GetFolderName);
            threadGetFolderName.SetApartmentState(ApartmentState.STA);

            try
            {
                threadGetFolderName.Start();

                // Wait for thread to get started.
                while (!threadGetFolderName.IsAlive)
                {
                }

                // Wait a milisec more.
                Thread.Sleep(1);

                // Wait for thread to end.
                threadGetFolderName.Join();

                // Use file name as you will here
                return oGetFolderName.SelectedPath;

            }
            catch (Exception ex_GetFolderNameViaOFD)
            {
                // Simply rethrow the exception.
                throw (ex_GetFolderNameViaOFD);
            }
        }

    }
}
