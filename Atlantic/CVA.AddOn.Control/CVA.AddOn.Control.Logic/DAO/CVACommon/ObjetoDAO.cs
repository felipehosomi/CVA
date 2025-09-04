using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.DAO.CVACommon
{
    public class ObjetoDAO
    {
        public Objeto Get(CVAObjectEnum objectType)
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            Objeto model = sqlHelper.FillModel<Objeto>(String.Format(Resources.Query.Objeto_GetById, (int)objectType));
            return model;
        }
    }
}
