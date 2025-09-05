using App.ApplicationServices.Services;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class OPService
    {
        public System.Data.DataTable ObterConcluidos(int? codeOrdemDeProducao)
        {
            try
            {
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;

                //var sQuery = $"Select 1 as \"teste\" FROM \"{Database}\".\"@SD_OPSIM\" WHERE \"Code\" = '{codeOrdemDeProducao}_{CodePosicao}' and \"U_OPERADOR\" <> {Operador}";
                var sQuery = $@"
SELECT T1.""U_ITEMCODE"",SUM(T1.""U_QTYP"") as ""U_QTYP"",T1.""U_POSICAO"" ,T1.""U_ITEMCODE2""
FROM  ""{ Database }"".""@SD_OP""  T0 
inner join   ""{ Database }"".""@SD_OP1""  T1 ON T0.""DocEntry"" = T1.""DocEntry""
WHERE T0.""U_STATUSOP"" = '1' AND T0.""U_OPCODE"" = {codeOrdemDeProducao}
GROUP BY  T1.""U_ITEMCODE"",T1.""U_POSICAO"",T1.""U_ITEMCODE2""
; ";

                DataTable dt = new DataTable("Table1");
                HanaDataAdapter da = new HanaDataAdapter(sQuery, _conn);
                da.Fill(dt);
                _conn.Close();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public double ObterDiferencaItem(string itemcode)
        {
            try
            {
                double ret = 0;
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;

                var sQuery = $@"SELECT T0.""U_SD_PercDif"" FROM ""{ Database }"".""OITM""  T0 
WHERE T0.""ItemCode"" = '{itemcode}' ";

                var cmd = new HanaCommand(sQuery, _conn);
                var result = Convert.ToString(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
                _conn.Close();

                if (!string.IsNullOrEmpty(result))
                {
                    ret = Convert.ToDouble(result);
                }

                return ret;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static double ObterSemiAcabdoPesado(int opCode, string itemcode, int posicao)
        {
            try
            {
                double ret = 0;
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;

                var sQuery = $@"SELECT SUM(T1.""U_QTYP"") as ""U_QTYP""
FROM  ""{ Database }"".""@SD_OP""  T0
INNER JOIN  ""{ Database }"".""@SD_OP1"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
Where T1.""U_POSICAO"" = {posicao}-- Etiqueta
AND T1.""U_ITEMCODE2"" = '{itemcode}'--Etiqueta
AND T0.""U_OPCODE"" = {opCode}";

                var cmd = new HanaCommand(sQuery, _conn);
                var result = Convert.ToString(cmd.ExecuteScalar());
                _conn.Close();

                if (!string.IsNullOrEmpty(result))
                {
                    ret = Convert.ToDouble(result);
                }

                return ret;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool ValidarVariação(double? totalEsperado, double? totalPesado)
        {
            try
            {
                bool valido = true;
                double variacao = 0;
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;

                var sQuery = $@"SELECT T0.""U_SD_VARIACAO"" FROM  ""{ Database }"".""OADM""  T0";

                var cmd = new HanaCommand(sQuery, _conn);
                var result = Convert.ToString(cmd.ExecuteScalar());
                _conn.Close();

                if (!string.IsNullOrEmpty(result))
                {
                    variacao = Convert.ToDouble(result);
                }
                if (variacao != 0)
                {
                    var valorVar = totalEsperado * (variacao / 100);
                    if (totalPesado > totalEsperado + valorVar ||
                        totalPesado < totalEsperado - valorVar)
                    {
                        valido = false;
                    }
                }

                return valido;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
