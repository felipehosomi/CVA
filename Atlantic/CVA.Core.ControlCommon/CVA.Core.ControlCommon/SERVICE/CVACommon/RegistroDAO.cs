using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL;
using CVA.Core.ControlCommon.MODEL.CVACommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.SERVICE.CVACommon
{
    public class RegistroDAO
    {
        public void Insert(Registro registro)
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConnectionString);
            sqlHelper.CreateModel<Registro>("CVA_REG", registro);
        }

        public Registro Get(string objectCode, CVAObjectEnum objectType)
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConnectionString);
            Registro model = sqlHelper.FillModelAccordingToSql<Registro>(String.Format(Resource.Query.Registro_Get, objectCode, (int)objectType));
            return model;
        }

        public void UpdateError(string objectCode, CVAObjectEnum objectType)
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConnectionString);
            string sql = String.Format(Resource.Query.Registro_UpdateError, objectCode, (int)objectType);
            sqlHelper.ExecuteNonQuery(sql);
        }
    }
}
