using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CVA.View.Dctf.BLL
{
    public class DctfFileBLL
    {
        public string GenerateFile(DBDataSource dt_Dctf)
        {
            string path = dt_Dctf.GetValue("U_Dir", 0);
            if (!Directory.Exists(path))
            {
                return $"Diretório {path} não encontrado!";
            }
            StreamWriter sw = new StreamWriter(Path.Combine(path, $"DCTF_{DateTime.Now.ToString("ddMMyyyy_HHmm")}.txt"));
            try
            {
                Regex digitsOnly = new Regex(@"[^\d]");
                ///////////
                /// R01 ///
                ///////////
                string line = "R01";
                line += digitsOnly.Replace(dt_Dctf.GetValue("U_CNPJ", 0), "");
                line += dt_Dctf.GetValue("U_Ano", 0);
                line += dt_Dctf.GetValue("U_Mes", 0).PadLeft(2, '0');
                line += dt_Dctf.GetValue("U_Evento", 0);
                line += !String.IsNullOrEmpty(dt_Dctf.GetValue("U_DtEvento", 0)) ? dt_Dctf.GetValue("U_DtEvento", 0) : new String(' ', 8);
                line += dt_Dctf.GetValue("U_DtDe", 0).Substring(4);
                line += dt_Dctf.GetValue("U_DtAte", 0).Substring(4);
                line += dt_Dctf.GetValue("U_Retific", 0) == "Y" ? "1" : "0";
                line += !String.IsNullOrEmpty(dt_Dctf.GetValue("U_NrRetific", 0))  ? dt_Dctf.GetValue("U_NrRetific", 0) : new String('0', 12);
                line += dt_Dctf.GetValue("U_Tributacao", 0);
                line += dt_Dctf.GetValue("U_QualPJ", 0);
                line += dt_Dctf.GetValue("U_Balanco", 0) == "Y" ? "1" : "0";
                line += dt_Dctf.GetValue("U_SCP", 0) == "Y" ? "1" : "0";
                line += dt_Dctf.GetValue("U_SimplesNacional", 0) == "Y" ? "1" : "0";
                line += dt_Dctf.GetValue("U_CPRB", 0) == "Y" ? "1" : "0";
                line += dt_Dctf.GetValue("U_VarMon", 0);
                line += new String('0', 12);
                line += dt_Dctf.GetValue("U_PIS", 0);
                line += dt_Dctf.GetValue("U_SitPJ", 0);
                line += dt_Dctf.GetValue("U_Lei", 0);
                line += new String(' ', 10);

                sw.WriteLine(line);

                ///////////
                /// R02 ///
                ///////////
                line = "R02";
                line += digitsOnly.Replace(dt_Dctf.GetValue("U_CNPJ", 0), "");
                line += dt_Dctf.GetValue("U_Ano", 0);
                line += dt_Dctf.GetValue("U_Mes", 0).PadLeft(2, '0');
                line += dt_Dctf.GetValue("U_Evento", 0);
                line += !String.IsNullOrEmpty(dt_Dctf.GetValue("U_DtEvento", 0)) ? dt_Dctf.GetValue("U_DtEvento", 0) : new String(' ', 8);
                line += dt_Dctf.GetValue("U_NomeEmp", 0).PadRight(115, ' ');
                line += new String('0', 4);
                line += dt_Dctf.GetValue("U_Rua", 0).PadRight(40, ' ');
                line += dt_Dctf.GetValue("U_RuaNr", 0).PadRight(6, ' ');
                line += dt_Dctf.GetValue("U_Complemento", 0).PadRight(21, ' ');
                line += dt_Dctf.GetValue("U_Bairro", 0).PadRight(20, ' ');
                line += dt_Dctf.GetValue("U_Municipio", 0).PadRight(50, ' ');
                line += dt_Dctf.GetValue("U_UF", 0).PadRight(2, ' ');
                line += dt_Dctf.GetValue("U_CEP", 0).PadRight(8, ' ');
                line += dt_Dctf.GetValue("U_DDDTel", 0).PadRight(4, ' ');
                line += dt_Dctf.GetValue("U_Telefone", 0).PadRight(9, ' ');
                line += dt_Dctf.GetValue("U_DDDFax", 0).PadRight(4, ' ');
                line += dt_Dctf.GetValue("U_Fax", 0).PadRight(9, ' ');
                line += dt_Dctf.GetValue("U_CaixaPostal", 0).PadRight(6, ' ');
                line += dt_Dctf.GetValue("U_UF_CP", 0).PadRight(2, ' ');
                line += dt_Dctf.GetValue("U_CEP_CP", 0).PadRight(8, ' ');
                line += dt_Dctf.GetValue("U_Email", 0).PadRight(40, ' ');

                sw.WriteLine(line);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                sw.Close();
            }
            return String.Empty;
        }
    }
}
