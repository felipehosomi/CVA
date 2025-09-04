using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Control.Logic.DAO.Resources;
using CVA.AddOn.Control.Logic.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.DAO.UserTables
{
    public class ConfigDAO
    {
        public ConfigModel GetConfig(int tipo)
        {
            CrudController crud = new CrudController();
            return crud.FillModelAccordingToSql<ConfigModel>(String.Format(Query.Config_Get, tipo));
        }
    }
}
