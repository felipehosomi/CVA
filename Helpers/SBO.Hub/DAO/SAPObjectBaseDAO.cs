using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;

namespace SBO.Hub.DAO
{
    public class SAPObjectBaseDAO
    {
        #region Properties
        private CrudDAO crudController;
        private string tableName;
        #endregion Properties

        #region Constructor
        public SAPObjectBaseDAO(string tableName)
        {
            crudController = new CrudDAO(tableName);
            this.tableName = tableName;
        }
        #endregion Constructor

        #region Retrieve
        public virtual string RetrieveModelSql(Type modelType, string where, string orderBy, bool getValidValues)
        {
            return crudController.RetrieveModelSql(modelType, where, orderBy, getValidValues);
        }

        public virtual string RetrieveSqlModel(Type modelType, string where, bool getValidValues)
        {
            return this.RetrieveModelSql(modelType, where, String.Empty, getValidValues);
        }

        public virtual string RetrieveSqlModel(Type modelType, bool getValidValues)
        {
            return this.RetrieveModelSql(modelType, String.Empty, String.Empty, getValidValues);
        }

        public virtual T RetrieveModel<T>(string where)
        {
            return crudController.RetrieveModel<T>(where);
        }

        public virtual List<T> RetrieveModelList<T>(string where)
        {
            return crudController.RetrieveModelList<T>(where);
        }

        public virtual List<T> RetrieveModelList<T>(string where, string orderBy)
        {
            return crudController.RetrieveModelList<T>(where, orderBy);
        }
        #endregion

        #region Util
        public virtual string Exists(string where)
        {
            return crudController.Exists(where);
        }

        public virtual string Exists(string returnColumn, string where)
        {
            return crudController.Exists(returnColumn, where);
        }

        public virtual List<T> FillModelList<T>(string sql)
        {
            return crudController.FillModelList<T>(sql);
        }

        public string GetNextCode()
        {
            return CrudDAO.GetNextCode(tableName);
        }

        public string GetNextCode(string fieldName)
        {
            return CrudDAO.GetNextCode(fieldName, tableName);
        }
        #endregion
    }
}
