using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailAutorizacao.MODEL;
using EmailAutorizacao.SERVICE.Portal.Resource;
using EmailAutorizacao.HELPER;
using SAPbobsCOM;

namespace EmailAutorizacao.SERVICE.UserTables
{
    public class ConfigDAO
    {
        public static ConfigModel GetConfig(int tipo)
        {
            var configModel = new ConfigModel();
            var oRecordset = (Recordset)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordset.DoQuery(string.Format(Query.Config_Get, tipo));

            while (!oRecordset.EoF)
            {
                configModel.Banco = oRecordset.Fields.Item("Banco").Value.ToString();
                configModel.Senha = oRecordset.Fields.Item("Senha").Value.ToString();
                configModel.Servidor = oRecordset.Fields.Item("Servidor").Value.ToString();
                configModel.Tipo = int.Parse(oRecordset.Fields.Item("Tipo").Value.ToString());
                configModel.Usuario = oRecordset.Fields.Item("Usuario").Value.ToString();
                oRecordset.MoveNext();
            }

            return configModel;
        }
    }
}
