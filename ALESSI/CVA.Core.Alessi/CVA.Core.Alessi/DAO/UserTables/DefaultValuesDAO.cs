using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.DAO.Resources;
using CVA.Core.Alessi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.DAO.UserTables
{
    public class DefaultValuesDAO
    {
        public List<DefaultValuesModel> GetDefaultValues(int objType, string layout)
        {
            CrudController crudController = new CrudController();
            string sql = String.Format(Query.DefaultValues_Get, objType, layout);
            List<DefaultValuesModel> list = crudController.FillModelList<DefaultValuesModel>(sql);
            return list;
        }
    }
}
