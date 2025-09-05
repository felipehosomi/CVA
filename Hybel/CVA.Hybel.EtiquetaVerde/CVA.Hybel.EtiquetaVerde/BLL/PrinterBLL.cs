using CVA.Hybel.EtiquetaVerde.HELPER;
using CVA.Hybel.EtiquetaVerde.MODEL;
//using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hybel.EtiquetaVerde.BLL
{
    public class PrinterBLL
    {
        public static string GetText(ItemModel itemModel)
        {
            string layout = Path.Combine(System.Windows.Forms.Application.StartupPath, ConfigurationManager.AppSettings["LayoutVerde"]);
            List<KeyValuePair<ItemModel, string>> etiquetasRTF = new List<KeyValuePair<ItemModel, string>>();
            StreamReader sr = new StreamReader(layout);
            string layoutText = sr.ReadToEnd();
            sr.Close();

            StringBuilder sb = new StringBuilder();
            string[] lines = layoutText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                string s = line;
                s = s.Replace("#ItemCode", itemModel.ItemCode);
                s = s.Replace("#ItemName", itemModel.ItemName);
                s = s.Replace("#NrNota", itemModel.NrNF.ToString());
                s = s.Replace("#NrPedido", itemModel.NrPedido.ToString());
                s = s.Replace("#Transportadora", itemModel.Transportadora);
                s = s.Replace("#Cliente", itemModel.Cliente);

                s = s.Replace("#Endereco", itemModel.Endereco);
                sb.AppendLine(s);
            }
            return sb.ToString();
        }

        public void Imprimir(List<ItemModel> list, string printerName)
        {
            string layout = Path.Combine(System.Windows.Forms.Application.StartupPath, ConfigurationManager.AppSettings["LayoutVerde"]);
            List<KeyValuePair<ItemModel, string>> etiquetasRTF = new List<KeyValuePair<ItemModel, string>>();
            StreamReader sr = new StreamReader(layout);
            string layoutText = sr.ReadToEnd();
            sr.Close();

            string[] lines = layoutText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (ItemModel item in list)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var line in lines)
                {
                    string s = line;
                    s = s.Replace("#ItemCode", item.ItemCode);
                    s = s.Replace("#ItemName", item.ItemName);
                    s = s.Replace("#NrNota", item.NrNF.ToString());
                    s = s.Replace("#NrPedido", item.NrPedido.ToString());
                    s = s.Replace("#Transportadora", item.Transportadora);
                    s = s.Replace("#Cliente", item.Cliente);

                    s = s.Replace("#Endereco", item.Endereco);
                    sb.AppendLine(s);
                }

                string tempFile = System.Environment.GetEnvironmentVariable("TEMP");
                if (!tempFile.EndsWith(@"\"))
                {
                    tempFile += @"\";
                }

                for (int printCount = 1; printCount <= item.Quantidade; printCount++)
                {
                    tempFile += printCount + "_" + item.NrNF + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".rtf";

                    StreamWriter sw = new StreamWriter(tempFile, false, Encoding.GetEncoding("iso-8859-15"));
                    sw.Write(sb);
                    sw.Close();

                    //etiquetasRTF.Add(new KeyValuePair<ItemModel, string>(item, tempFile));

                    PrinterHelper.SendFileToPrinter(printerName, tempFile);
                }
            }

            //this.PrintRTF(etiquetasRTF, printerName);

        }

        private void PrintRTF(List<KeyValuePair<ItemModel, string>> EtiquetasRTF, string printerName)
        {
            //Application ac = new Application();
            //_Application app = ac.Application;

            ////app.Visible = true;
            //// I'm setting all of the alerts to be off            
            //app.DisplayAlerts = WdAlertLevel.wdAlertsMessageBox;

            //// Open the document to print...
            //object filename = "";
            //object missingValue = Type.Missing;

            //_Document document = null;

            //int i = 0;
            //foreach (var etiqueta in EtiquetasRTF)
            //{
            //    filename = etiqueta.Value;

            //    if (i == 0)
            //    {
            //        // Using OpenOld so as to be compatible with other versions of Word
            //        document = (_Document)app.Documents.OpenOld(ref filename,
            //                                                    ref missingValue, ref missingValue, ref missingValue, ref missingValue, ref missingValue,
            //                                                    ref missingValue, ref missingValue, ref missingValue, ref missingValue);
            //    }
            //    else
            //    {
            //        app.Selection.EndKey(6);                             /* Posiciona cursor no final do arquivo */
            //        app.Selection.InsertBreak(7);                        /* Quebra pagina antes de inserir arquivo */
            //        app.Selection.InsertFile(filename.ToString());
            //    }
            //    i++;
            //}

            //// Set the active printer
            ////app.ActivePrinter = _printerName;
            //this.SetPrinter(ac, printerName);

            //object myTrue = true;
            //object myFalse = false;
            //object nrCopias = 1;

            //// Using PrintOutOld to be version independent
            //app.ActiveDocument.PrintOutOld(ref myFalse,// Print in background
            //                               ref myFalse, ref missingValue, ref missingValue, ref missingValue, ref missingValue,
            //                               ref missingValue, ref nrCopias, ref missingValue, ref missingValue, ref myFalse,
            //                               ref missingValue, ref missingValue, ref missingValue);

            //object saveOption = Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;

            //// Make sure all of the documents are gone from the queue
            //while (app.BackgroundPrintingStatus > 0)
            //{
            //    System.Threading.Thread.Sleep(250);
            //}

            //app.NormalTemplate.Saved = true;

            //document.Close(ref saveOption, ref missingValue, ref missingValue);

            //app.Quit(ref saveOption, ref missingValue, ref missingValue);

            //foreach (var etiqueta in EtiquetasRTF)
            //{
            //    filename = etiqueta.Value;
            //    File.Delete(filename.ToString());
            //}
        }

        //private void SetPrinter(Microsoft.Office.Interop.Word.Application objWord, string printerName)
        //{
        //    Type wordBasic = objWord.WordBasic.GetType();
        //    // Imprimir na Impressora Selecionada ... 
        //    // Seleciona impressora SEM MUDAR a impressora default do sistema ... 
        //    wordBasic.InvokeMember("FilePrintSetup",
        //        BindingFlags.DeclaredOnly |
        //        BindingFlags.Public |
        //        BindingFlags.NonPublic |
        //        BindingFlags.Instance |
        //        BindingFlags.InvokeMethod |
        //        BindingFlags.OptionalParamBinding,
        //        null, objWord.WordBasic,
        //        new object[] { printerName, 1 }, null, null, new string[] { "Printer", "DoNotSetAsSysDefault" });
        //}

        public static List<string> GetPrinters()
        {
            var modelList = new List<string>();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                modelList.Add(printer);
            }
            return modelList;
        }
    }
}
