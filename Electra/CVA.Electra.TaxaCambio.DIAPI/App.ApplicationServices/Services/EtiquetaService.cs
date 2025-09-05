using App.ApplicationServices.Model;
using App.ApplicationServices.Services;
using App.Domain.Helpers;
using EtiquetaWpf;
using Sap.Data.Hana;
using SAPB1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App.ApplicationServices.Services
{
    static public class EtiquetaService
    {
        static public void Recebimento(ObservableCollection<ItemLoteModel> obj)
        {
            try
            {
                GetPrintNameClass oGetPrintName = new GetPrintNameClass();

                //itens
                foreach (var item in obj)
                {
                    if (item.Etiquetas > 0)
                    {
                        //obter dados do item
                        string expDate = "";
                        var itemCode = item.ItemCode;// "PAPOSFT0060100000AAA";
                        var distNmber = item.Lote;
                        var codigoBarras = $"{itemCode}{distNmber}";
                        if (item.LoteValidade.HasValue)
                        {
                            expDate = item.LoteValidade.Value.ToString("dd/MM/yyyy");
                        }


                        //obter arquivo Etiqueta OITM.U_Caminho
                        //string sCaminhoArquivo = @"D:\ETIQUETALISTAZPL.txt";
                        string sCaminhoArquivo = getCaminhoArquivo(itemCode);
                        if (string.IsNullOrEmpty(sCaminhoArquivo))
                        {
                            continue;
                        }

                        //montar layout
                        string sEtiqueta = System.IO.File.ReadAllText(sCaminhoArquivo);
                        sEtiqueta = sEtiqueta.Replace("{0}", itemCode).Replace("{1}", distNmber).Replace("{2}", "").Replace("{3}", "").Replace("{4}", expDate).Replace("{5}", codigoBarras);
                        sEtiqueta = sEtiqueta.Replace("^PQ1", String.Format(@"^PQ{0}", item.Etiquetas));

                        //imprimir
                        printViaOFD(ref oGetPrintName, sEtiqueta);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        static public void printViaOFD(ref GetPrintNameClass oGetPrintName, String textFile) //static public void GetPrintViaOFD(String s, string Impressora)
        {
            string sErro = String.Empty;

            try
            {
                sErro = "Passando o conteúdo para 'oGetPrintName.Conteudo'";
                oGetPrintName.Conteudo = textFile;
                //oGetPrintName.pkInstalledPrinters = Impressora;

                sErro = "Passando o conteúdo para 'new Thread(oGetPrintName.GetPrintName)'";
                Thread threadGetPrint = new Thread(oGetPrintName.GetPrintName);

                sErro = "Passando o conteúdo para 'new Thread(ApartmentState.STA)'";
                threadGetPrint.SetApartmentState(ApartmentState.STA);

                try
                {
                    sErro = "Passando o conteúdo para 'threadGetPrint.Start()'";
                    threadGetPrint.Start();

                    // Wait a milisec more.
                    sErro = "Passando o conteúdo para 'Thread.Sleep(1)'";
                    Thread.Sleep(1);

                    // Wait for thread to end.
                    sErro = "Passando o conteúdo para 'threadGetPrint.Join()'";
                    threadGetPrint.Join();
                }
                catch (Exception)
                {
                    throw;
                    // Simply rethrow the exception.
                    //throw (ex_GetFileNameViaOFD);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ocorreu um erro ao imprimir as etiquetas. Detalhes: " + sErro + " - " + ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                //throw;
            }

        }

        public static string getCaminhoArquivo(string sCodigoItem)
        {
            try
            {
                HanaService hanaService = new HanaService();
                var Database = hanaService.Database;

                string sCaminho = String.Empty;
                string sQuery = String.Empty;
                sQuery = String.Format($@"SELECT TOP 1 ""U_Caminho"" FROM ""{ Database}"".""OITM"" WHERE ""ItemCode"" = '{sCodigoItem}'");
                sCaminho = Convert.ToString(hanaService.ExecuteScalar(sQuery));

                return sCaminho;
            }
            catch (Exception)
            {

                throw;
            }
        }




    }
}
