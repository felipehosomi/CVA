using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Producao
{
    public class RecursoBLL : BaseBLL
    {
        public RecursoBLL()
        {
            DAO.TableName = "ORSC";
        }

        public List<RecursoModel> Get(string tipo = "")
        {
            string where = String.Empty;
            if (!string.IsNullOrEmpty(tipo))
            {
                where = $" \"ResType\" = '{tipo}' ";
            }

            return DAO.RetrieveModelList<RecursoModel>(where);
        }
    }
}
