
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AUXILIAR
{
    public class Printer
    {
        public void Print(List<EtiquetaModel> etiquetaList, string impressora, string tipoEtiqueta, int qtdeImpressao = 0)
        {
            try
            {
                var arquivo = new ReportDocument();
                arquivo.Load(tipoEtiqueta);
                foreach (InternalConnectionInfo internalConnectionInfo in arquivo.DataSourceConnections)
                    internalConnectionInfo.SetConnection(ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["Database"], ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]);

                arquivo.PrintOptions.PrinterName = impressora;

                foreach (var etiqueta in etiquetaList)
                {
                    if (qtdeImpressao != 0)
                    {
                        etiqueta.Quantidade = qtdeImpressao;
                    }
                    for (int i = 0; i < etiqueta.Quantidade; i++)
                    {
                        string quantidade = $"{i + 1} de {etiqueta.Quantidade.ToString("f0")}";

                        //arquivo.SetParameterValue("@ItemCode", "AAA");
                        //arquivo.SetParameterValue("@ItemName", "BBB");
                        //arquivo.SetParameterValue("@Pedido", "CCC");
                        //arquivo.SetParameterValue("@Serie", "DDD");
                        //arquivo.SetParameterValue("@QtdeDeAte", "EEE");

                        if (String.IsNullOrEmpty(etiqueta.ItemName))
                        {
                            etiqueta.ItemName = "";
                        }
                        if (String.IsNullOrEmpty(etiqueta.Serie))
                        {
                            etiqueta.Serie = "";
                        }
                        if (String.IsNullOrEmpty(etiqueta.Pedido))
                        {
                            etiqueta.Pedido = "";
                        }

                        arquivo.SetParameterValue("@ItemCode", etiqueta.ItemCode);
                        arquivo.SetParameterValue("@ItemName", etiqueta.ItemName);
                        arquivo.SetParameterValue("@Pedido", etiqueta.Pedido);
                        arquivo.SetParameterValue("@Serie", etiqueta.Serie);
                        arquivo.SetParameterValue("@QtdeDeAte", quantidade);

                        arquivo.PrintToPrinter(1, true, 1, 1);
                    }
                }

                arquivo.Close();
                arquivo.Dispose();

                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
                throw ex;
            }
        }
    }
}
