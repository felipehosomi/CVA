using DAL.Connection;
using DAL.Data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataInterface
{
    public class DatabaseConfigDao
    {
        public static string GetConciliadoraConnectionString()
        {
            string connectionString = "data source={0};initial catalog={1};user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework";

            var rst = (Recordset)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            rst.DoQuery("SELECT * FROM [@CVA_CONFIG_DB] WHERE U_Tipo = 2");

            connectionString = String.Format(connectionString, rst.Fields.Item("U_Servidor").Value, rst.Fields.Item("U_Banco").Value, rst.Fields.Item("U_Usuario").Value, rst.Fields.Item("U_Senha").Value);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(rst);

            return connectionString;
        }

        public static BaseModel GetBaseByCnpj(string cnpj)
        {
            ConciliadoraEDM edm = new ConciliadoraEDM();
            edm.Database.Connection.ConnectionString = DatabaseConfigDao.GetConciliadoraConnectionString();

            if (!cnpj.Contains("."))
            {
                cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            }

            BaseDeParaModel baseDeParaModel = edm.CVA_BASES_DE_PARA.FirstOrDefault(b => b.CNPJ_FILIAL_DE == cnpj);
            if (baseDeParaModel == null)
            {
                throw new Exception($"Dados de conexão para CNPJ {cnpj} não encontrados!");
            }
            BaseModel baseModel = edm.CVA_BASES.FirstOrDefault(b => b.ID == baseDeParaModel.BASE_DE);
            return baseModel;
        }

        public static BaseModel GetBaseByName(string name)
        {
            ConciliadoraEDM edm = new ConciliadoraEDM();
            edm.Database.Connection.ConnectionString = DatabaseConfigDao.GetConciliadoraConnectionString();
            BaseModel baseModel = edm.CVA_BASES.FirstOrDefault(b => b.BASE == name);
            return baseModel;
        }
    }
}
