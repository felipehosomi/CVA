using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.DataBase;
using CVA.View.EDoc.DAO;
using System;
using System.IO;

namespace CVA.View.EDoc.BLL
{
    public class SprocBLL
    {
        public static void CreateSprocs()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "DAO", "Sproc");
            foreach (string file in Directory.GetFiles(path, "*.sql"))
            {
                string sprocName = Path.GetFileNameWithoutExtension(file);
                if (CrudController.ExecuteScalar(String.Format(SQL.Sproc_Exists, sprocName)).ToString() == "0")
                {
                    StoredProcedureController.CreateProcedure(file);
                }
            }
        }
    }
}
