using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.DAO.Resources;
using CVA.Core.Alessi.MODEL;
using System;
using System.Collections.Generic;

namespace CVA.Core.Alessi.DAO.UserTables
{
    public class DocumentMappingDAO
    {
        public List<DocumentMappingModel> GetMapping(int objType, string layout)
        {
            CrudController crudController = new CrudController();
            string sql = String.Format(Query.DocumentMapping_Get, objType, layout);
            List<DocumentMappingModel> list = crudController.FillModelList<DocumentMappingModel>(sql);
            foreach (var item in list)
            {
                sql = String.Format(Query.DocumentMappingItem_GetByCode, item.Code);
                item.FieldsList = crudController.FillModelList<DocumentMappingItemModel>(sql);
            }
            return list;
        }

        public List<string> GetLayouts(int objType)
        {
            CrudController crudController = new CrudController();
            string sql = String.Format(Query.DocumentMappingLayout_GetByObjType, objType);
            List<string> list = crudController.FillStringList(sql);
            return list;
        }
    }
}
