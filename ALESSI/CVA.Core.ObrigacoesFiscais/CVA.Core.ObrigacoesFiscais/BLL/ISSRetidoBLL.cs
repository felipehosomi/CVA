using CVA.Core.ObrigacoesFiscais.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ObrigacoesFiscais.BLL
{
    public class ISSRetidoBLL
    {
        public static string GetSQL(FileFilterModel filterModel)
        {
            string sproc;
            if (filterModel.Layout == "R")
            {
                sproc = "SP_CVA_RESTGO";
            }
            else
            {
                sproc = "SP_CVA_GISS";
            }

            string sql = $@"DECLARE @DateFrom DATETIME = CAST('{filterModel.DateFrom.ToString("yyyyMMdd")}' AS datetime)
                            DECLARE @DateTo DATETIME = CAST('{filterModel.DateTo.ToString("yyyyMMdd")}' AS datetime)

                            exec {sproc} {filterModel.BranchId}, @DateFrom, @DateTo";

            return sql;
        }
    }
}
