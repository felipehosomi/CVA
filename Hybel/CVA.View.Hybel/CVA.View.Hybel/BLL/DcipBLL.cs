using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Util;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CVA.View.Hybel.BLL
{
    public class DcipBLL
    {
        public static string GerarArquivo(string diretorio, int filial, int tipo, DateTime dataDe)
        {
            DateTime dataAte = new DateTime(dataDe.Year, dataDe.Month, DateTime.DaysInMonth(dataDe.Year, dataDe.Month));

            if (tipo == 1)
            {
                return GerarArquivoEnergiaEletrica(diretorio, filial, dataDe, dataAte);
            }
            else
            {
                return GerarArquivoSimplesNacional(diretorio, filial, dataDe, dataAte);
            }
        }

        public static string GerarArquivoEnergiaEletrica(string diretorio, int filial, DateTime dataDe, DateTime dataAte)
        {
            string msg = String.Empty;

            Recordset rst = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            try
            {
                rst.DoQuery(String.Format(SQL.Dcip_GetEnergiaEletrica, filial, dataDe.ToString("yyyy-MM-dd"), dataAte.ToString("yyyy-MM-dd")));
                if (rst.RecordCount == 0)
                {
                    return "Nenhum dado encontrado para geração do arquivo";
                }

                StreamWriter sw = new StreamWriter(Path.Combine(diretorio, $"DCIP_EnergiaEletrica_{dataDe.ToString("MMyyyy")}.txt"));
                
                string linha = Regex.Replace(rst.Fields.Item("IE").Value.ToString(), @"[^\d]", "").PadLeft(9, '0');
                linha += dataDe.ToString("yyyyMM");
                linha += "040";
                linha += "017";
                linha += rst.Fields.Item("ICMS").Value.ToString().Replace(",", "").PadLeft(17, '0');
                linha += new string('0', 15);
                sw.WriteLine(linha);

                linha = Regex.Replace(rst.Fields.Item("IE").Value.ToString(), @"[^\d]", "").PadLeft(9, '0');
                linha += dataDe.ToString("yyyyMM");
                linha += "050";
                linha += rst.Fields.Item("ICMS").Value.ToString().Replace(",", "").PadLeft(17, '0');
                sw.WriteLine(linha);

                linha = "9" + "3".PadLeft(9, '0');
                sw.WriteLine(linha);

                sw.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(rst);
                rst = null;
            }
            return msg;
        }

        public static string GerarArquivoSimplesNacional(string diretorio, int filial, DateTime dataDe, DateTime dataAte)
        {
            string msg = String.Empty;
            try
            {
                string sql = String.Format(SQL.Dcip_GetSimplesNacional, filial, dataDe.ToString("yyyy-MM-dd"), dataAte.ToString("yyyy-MM-dd"));
                List<DcipSimplesNacionalModel> list = new CrudController().FillModelListAccordingToSql<DcipSimplesNacionalModel>(sql);
                if (list.Count == 0)
                {
                    return "Nenhum dado encontrado para geração do arquivo";
                }

                StreamWriter sw = new StreamWriter(Path.Combine(diretorio, $"DCIP_SimplesNacional_{dataDe.ToString("MMyyyy")}.txt"));
                FileWriterUtil fileWriterUtil = new FileWriterUtil();

                string line;
                foreach (var item in list)
                {
                    line = fileWriterUtil.WriteLine(item);
                    sw.WriteLine(line);
                }

                // Registro totais
                line = Regex.Replace(list[0].IEFilial.PadLeft(9, '0'), @"[^\d]", "").PadLeft(9, '0');
                line += list[0].Periodo.ToString("yyyyMM");
                line += "130";
                line += list.Sum(l => l.ValorTotal).ToString().Replace(",", "").PadLeft(17, '0');
                line += list.Sum(l => l.BaseCalculo).ToString().Replace(",", "").PadLeft(17, '0');
                sw.WriteLine(line);

                line = "9" + (list.Count + 2).ToString().PadLeft(9, '0');
                sw.WriteLine(line);

                sw.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
    }
}
