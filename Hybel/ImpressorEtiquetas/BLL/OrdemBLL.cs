using AUXILIAR;
using DAO;
using MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OrdemBLL
    {
        public List<EtiquetaModel> GetEtiqueta(int ordem)
        {
            return new EtiquetaDAO().GetEtiquetaList(ordem);
        }

        public List<OrdemProducaoModel> GetOPList(OPFiltroModel filtroModel)
        {
            return new OrdemDAO().GetOPList(filtroModel);
        }

        #region GenerateFileToPrintNF
        public static void GenerateFileToPrintPRN(List<EtiquetaModel> etiquetasModelList, string layoutEtiqueta, string impressora, int? qtdeImpressao)
        {
            int quantidadeDe = 0;
            
            foreach (EtiquetaModel etiqueta in etiquetasModelList)
            {
                if (etiqueta.Quantidade != 1)
                {
                    quantidadeDe = 0;
                }

                if (etiqueta.ItemName == null)
                {
                    etiqueta.ItemName = "";
                }
                if (etiqueta.Serie == null)
                {
                    etiqueta.Serie = "";
                }
                if (etiqueta.Pedido == null)
                {
                    etiqueta.Pedido = "";
                }

                for (int i = 0; i < etiqueta.Quantidade && (!qtdeImpressao.HasValue || i < qtdeImpressao); i++)
                {
                    StringBuilder sb = new StringBuilder();

                    qtdeImpressao--;
                    StreamReader sr = new StreamReader(layoutEtiqueta, Encoding.Default);
                    quantidadeDe++;
                    if (etiqueta.Quantidade == 1)
                    {
                        etiqueta.QtdeEtiqueta = $"{quantidadeDe} de {etiquetasModelList.Count}";
                    }
                    else
                    {
                        etiqueta.QtdeEtiqueta = $"{quantidadeDe} de {etiqueta.Quantidade}";
                    }
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();

                        s = s.Replace("-10000000000-", etiqueta.ItemCode);
                        s = s.Replace("-90000000000-", etiqueta.Serie);
                        s = s.Replace("#12", etiqueta.ItemCode.Substring(0, 3));
                        s = s.Replace("#C", etiqueta.ItemCode.Substring(3, 1));
                        s = s.Replace("#4567890", etiqueta.ItemCode.Substring(4));
                        s = s.Replace("#ItemName", etiqueta.ItemName);
                        s = s.Replace("#Pedido", etiqueta.Pedido);
                        s = s.Replace("#Ordem", etiqueta.Ordem.ToString());
                        s = s.Replace("#Serie", etiqueta.Serie);
                        s = s.Replace("#Qtde", etiqueta.QtdeEtiqueta);
                        s = s.Replace("#UF", etiqueta.UF);
                        sb.AppendLine(s);
                    }

                    sr.Close();

                    //string tempFile = System.Environment.GetEnvironmentVariable("Temp");

                    //if (!tempFile.EndsWith(@"\"))
                    //{
                    //    tempFile += @"\";
                    //}

                    //tempFile += etiqueta.Ordem.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + ".prn";

                    //StreamWriter sw = new StreamWriter(tempFile, false, Encoding.Default);
                    //sw.Write(sb);
                    //sw.Close();
                    //System.Windows.Forms.MessageBox.Show(sb.ToString());

                    LabelPrinterHelper.SendStringToPrinter(impressora, sb.ToString());
                }
            }
        }
        #endregion GenerateFileToPrintNF
    }
}
