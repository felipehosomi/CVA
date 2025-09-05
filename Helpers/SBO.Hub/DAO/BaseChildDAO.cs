using SBO.Hub.DAO;
using System;
using System.Collections.Generic;

namespace SBO.Hub.DAO
{
    public class BaseChildDAO
    {
        #region Properties
        private CrudDAO CrudDAO;
        private CrudChildDAO CrudChildDAO;
        private string tableName;
        #endregion Properties

        #region Constructor
        public BaseChildDAO(string parentTable, string tableName)
        {
            CrudDAO = new CrudDAO(tableName);
            CrudChildDAO = new CrudChildDAO(parentTable, tableName);
            this.tableName = tableName;
        }
        #endregion Constructor

        #region CRUD
        public virtual string RetrieveModelSql(Type modelType, string where, string orderBy, bool getValidValues)
        {
            return CrudDAO.RetrieveModelSql(modelType, where, orderBy, getValidValues);
        }

        public virtual string RetrieveSqlModel(Type modelType, string where, bool getValidValues)
        {
            return this.RetrieveModelSql(modelType, where, String.Empty, getValidValues);
        }

        public virtual string RetrieveSqlModel(Type modelType, bool getValidValues)
        {
            return this.RetrieveModelSql(modelType, String.Empty, String.Empty, getValidValues);
        }

        public virtual void CreateModel(object model, object parentCode)
        {
            CrudChildDAO.Model = model;
            CrudChildDAO.CreateModel(parentCode);
        }

        public virtual void CreateModelList(object[] modelList, object parentCode)
        {
            CrudChildDAO.ModelList = new List<object>();
            CrudChildDAO.ModelList.AddRange(modelList);
            CrudChildDAO.CreateModelList(parentCode);
        }

        public virtual void UpdateModel(object model, object parentCode)
        {
            CrudChildDAO.Model = model;
            CrudChildDAO.UpdateModel(parentCode);
        }

        public virtual void UpdateModelList(object[] modelList, object parentCode)
        {
            CrudChildDAO.ModelList = new List<object>();
            CrudChildDAO.ModelList.AddRange(modelList);
            CrudChildDAO.UpdateModelList(parentCode);
        }

        public virtual T RetrieveModel<T>(string where)
        {
            return CrudDAO.RetrieveModel<T>(where);
        }

        public virtual List<T> RetrieveModelList<T>(string where)
        {
            return CrudDAO.RetrieveModelList<T>(where);
        }

        public virtual List<T> RetrieveModelList<T>(string where, string orderBy)
        {
            return CrudDAO.RetrieveModelList<T>(where, orderBy);
        }

        //public virtual void UpdateModel(object model)
        //{
        //    CrudController.Model = model;
        //    CrudController.UpdateModel();
        //}

        //public virtual void UpdateModel(object model, string where)
        //{
        //    CrudController.Model = model;
        //    CrudController.UpdateModel(where);
        //}

        //public virtual void DeleteModel(string where)
        //{
        //    CrudController.DeleteModel(tableName, where);
        //}

        #endregion

        #region Util
        public virtual string Exists(string where)
        {
            return CrudDAO.Exists(where);
        }

        public virtual string Exists(string returnColumn, string where)
        {
            return CrudDAO.Exists(returnColumn, where);
        }

        public virtual List<T> FillModelList<T>(string sql)
        {
            return CrudDAO.FillModelList<T>(sql);
        }

        public string GetNextCode()
        {
            return CrudDAO.GetNextCode(tableName);
        }

        public string GetNextCode(string fieldName)
        {
            return CrudDAO.GetNextCode(tableName, fieldName);
        }
        #endregion
    }
}
