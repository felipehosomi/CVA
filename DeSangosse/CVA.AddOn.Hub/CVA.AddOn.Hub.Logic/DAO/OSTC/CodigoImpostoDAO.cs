using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using System;
using System.Collections.Generic;

namespace CVA.Hub.DAO.OSTC
{
    public class CodigoImpostoDAO
    {
        public List<string> GetObsNFList(List<string> codeList)
        {
            if (codeList.Count == 0)
            {
                return new List<string>();
            }

            string where = String.Empty;

            foreach (var item in codeList)
            {
                where += $", '{item}'";
            }
            where = where.Substring(2);
            CrudController crudController = new CrudController();
            List<string> listObs = crudController.FillStringList(String.Format(Query.CodigoImposto_GetObsNF, where));
            where = String.Empty;
            foreach (var item in listObs)
            {
                if (item.Contains("\r"))
                {
                    foreach (var splitted in item.Split('\r'))
                    {
                        where += $", '{splitted}'";
                    }
                }
                else if (!String.IsNullOrEmpty(item))
                {
                    where += $", '{item}'";
                }
            }
            if (!String.IsNullOrEmpty(where))
            {
                where = where.Substring(2);
                listObs = crudController.FillStringList(String.Format(Query.TextoPredefinido_GetInCode, where));
            }
            else
            {
                listObs = new List<string>();
            }

            return listObs;
        }
    }
}
