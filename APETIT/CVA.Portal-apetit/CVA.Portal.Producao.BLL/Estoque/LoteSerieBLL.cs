using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Estoque
{
    public class LoteSerieBLL : BaseBLL
    {
        public List<LoteSerieModel> GetDisponivel(string itemCode, string controle, string deposito)
        {
            string sql;
            if (controle == "L")
            {
                sql = String.Format(Commands.Resource.GetString("Lote_GetDisponiveis"), BaseBLL.Database, itemCode, deposito);
            }
            else
            {
                sql = String.Format(Commands.Resource.GetString("Serie_GetDisponiveis"), BaseBLL.Database, itemCode, deposito);
            }

            return DAO.FillListFromCommand<LoteSerieModel>(sql);
        }

        public LoteSerieModel GetSerialInfo(string itemCode, string serial)
        {
            string sql = string.Format(Commands.Resource.GetString("Serie_GetSerialInfo"), BaseBLL.Database, itemCode, serial);
            var result = DAO.FillModelFromCommand<LoteSerieModel>(sql);
            return result;
        }
    }
}
