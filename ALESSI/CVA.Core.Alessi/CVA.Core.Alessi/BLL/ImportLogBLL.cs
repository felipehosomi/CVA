using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.MODEL;
using System;

namespace CVA.Core.Alessi.BLL
{
    public class ImportLogBLL
    {
        public static void AddLog(ImportLogModel model)
        {
            LogBLL.Logger.Info($"Linha {model.Line}: {model.Description}");

            model.User = SBOApp.Company.UserName;
            model.Date = DateTime.Today;
            //model.Hour = Convert.ToInt32(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString());
            model.Hour = DateTime.Now;

            CrudController crudController = new CrudController("@CVA_IMPORT_LOG");
            crudController.UserTableType = SAPbobsCOM.BoUTBTableType.bott_NoObject;
            crudController.Model = model;
            crudController.CreateModel();
        }
    }
}
