using SAPbouiCOM;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ApplicationServices
{
    public static class CompanyExtension
    {
        public static void ReleaseComObject(this SAPbobsCOM.Company vCompany, params object[] Componentes)
        {
            try
            {
                object vComponenteAux = null;

                foreach (object vComponente in Componentes)
                {
                    vComponenteAux = vComponente;
                    ReleaseComObject(ref vComponenteAux);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ReleaseComObject(ref object Componente)
        {
            try
            {
                Componente.Release();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void StartTransatcion_Verified(this SAPbobsCOM.Company vCompany)
        {
            try
            {
                if (!vCompany.InTransaction)
                    vCompany.StartTransaction();
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public static void EndTransatcion_Verified(this SAPbobsCOM.Company vCompany, BoWfTransOpt endType)
        {
            try
            {
                if (vCompany.InTransaction)
                    vCompany.EndTransaction(endType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EndTransationCommit_Verified(this SAPbobsCOM.Company vCompany)
        {
            try
            {
                if (vCompany.InTransaction)
                    vCompany.EndTransaction(BoWfTransOpt.wf_Commit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EndTransationRollBack_Verified(this SAPbobsCOM.Company vCompany)
        {
            try
            {
                if (vCompany.InTransaction)
                    vCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
