using App.ApplicationServices.Services;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class SacoService
    {

        public SacoService()
        {
        }
           

        public System.Data.DataTable Obter(int? codeOrdemDeProducao)
        {
            try
            {
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;
                //var sQuery = $"Select 1 as \"teste\" FROM \"{Database}\".\"@SD_OPSIM\" WHERE \"Code\" = '{codeOrdemDeProducao}_{CodePosicao}' and \"U_OPERADOR\" <> {Operador}";
                var sQuery = $@"Select distinct
 t0.""BELPOS_ID"",
 (select ""U_OPERADOR"" from ""{ Database }"".""@SD_OPSIM"" t9

    where t9.""U_POSICAO"" = t0.""BELPOS_ID"" and  t9.""U_OPERACAO"" = t0.""BELNR_ID"") as ""OPERADOR""

from ""{ Database }"".beas_ftpos t0    
inner join ""{ Database }"".beas_FTSTL t1 on t0.""BELNR_ID"" = t1.""BELNR_ID"" and t0.""BELPOS_ID"" = t1.""BELPOS_ID""

inner join ""{ Database }"".OITM t3 on t0.""ItemCode"" = t3.""ItemCode""

inner join ""{ Database }"".OMTP t4 on t3.""MatType"" = t4.""AbsEntry""

inner join ""{ Database }"".OITM t5 on t1.""ART1_ID"" = t5.""ItemCode""

inner join ""{ Database }"".OMTP t6 on t5.""MatType"" = t6.""AbsEntry""

where t6.""MatType"" != 'Embalagem'
and t4.""AbsEntry"" = 6
and t0.""BELNR_ID"" = {codeOrdemDeProducao}; ";

                DataTable dt = new DataTable("Table1");
                HanaDataAdapter da = new HanaDataAdapter(sQuery, _conn);
                //da.FillSchema(dt, SchemaType.Source);
                da.Fill(dt);
                //DataSet ds = new DataSet();
                //ds.Merge(dt);


                //var cmd = new HanaCommand(sQuery, _conn);
                //var reader =  cmd.ExecuteReader();

                //return ret.();

                _conn.Close();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
