using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Configuracoes;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class InspecaoMPBLL : BaseBLL
    {
        public List<DocumentoModel> GetRecebimentos(DateTime? dataDe, DateTime? dataAte, string status, int? nf, string itemCode)
        {
            ParametrosBLL parametrosBLL = new ParametrosBLL();
            ParametrosModel parametrosModel = parametrosBLL.Get("0001");
            if (String.IsNullOrEmpty(parametrosModel.InspecaoMP))
            {
                parametrosModel.InspecaoMP = "PDN";
            }

            string where = String.Empty;
            if (status == "P")
            {
                where = "\"INSP\".\"Code\" IS NULL";
            }
            else
            {
                where = $"\"INSP\".\"U_Status\" = '{status}'";
            }
            if (dataDe.HasValue)
            {
                where += $" AND  \"O{parametrosModel.InspecaoMP}\".\"DocDate\" >= '{dataDe.Value.ToString("yyyyMMdd")}'";
            }
            if (dataAte.HasValue)
            {
                where += $" AND  \"O{parametrosModel.InspecaoMP}\".\"DocDate\" <= '{dataAte.Value.ToString("yyyyMMdd")}'";
            }
            if (nf.HasValue)
            {
                where += $" AND  \"O{parametrosModel.InspecaoMP}\".\"Serial\" = {nf.Value}";
            }
            if (!string.IsNullOrEmpty(itemCode))
            {
                if (parametrosModel.InspecaoMP == "PDN")
                {
                    where += $" AND \"PDN1\".\"ItemCode\" = '{itemCode}'";
                }
                else
                {
                    where += $" AND \"PCH1\".\"ItemCode\" = '{itemCode}'";
                }
            }

            string commandName;
            if (parametrosModel.InspecaoMP == "PDN")
            {
                commandName = "InspecaoMP_GetByRecebimentoMercadorias";
            }
            else
            {
                commandName = "InspecaoMP_GetByNF";
            }

            return DAO.FillListFromCommand<DocumentoModel>(String.Format(Commands.Resource.GetString(commandName), BaseBLL.Database, where));
        }
    }
}
