using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Control.Logic.DAO.CVACommon
{
    public class RegistroDAO
    {
        public void Insert(Registro registro)
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            sqlHelper.TableName = "CVA_REG";
            sqlHelper.Model = registro;
            sqlHelper.CreateModel();
        }

        public Registro Get(string objectCode, CVAObjectEnum objectType)
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            Registro model = sqlHelper.FillModelAccordingToSql<Registro>(String.Format(Resources.Query.Registro_Get, objectCode, (int)objectType));
            return model;
        }

        public bool IsFirstErrorObject(string objectCode, CVAObjectEnum objectType)
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            List<Registro> list = sqlHelper.FillModelListAccordingToSql<Registro>(String.Format(Resources.Query.Registro_GetErrorList, (int)objectType));
            if (list.Count > 0)
            {
                return list[0].Codigo == objectCode;
            }
            else
            {
                return false;
            }
        }

        public void UpdateError(CVAObjectEnum objectType)
        {
            SqlController sqlHelper = new SqlController(StaticKeys.ConnectionString);
            string sql = String.Format(Resources.Query.Registro_UpdateError, (int)objectType);
            sqlHelper.ExecuteNonQuery(sql);
        }
    }
}
