
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Repository.Services;

namespace App.ApplicationServices
{
    public class UserTables : IDisposable
    {
        private UserTablesMD _vTable = null;
        private bool _vTabelaExiste = false;

        public string Descricao { get; set; }

        public List<UserFields> _Fields { get; set; }

        public UserTables()
        {

        }

        public UserTables(string Table)
        {
            try
            {
                _vTable = (SAPbobsCOM.UserTablesMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
                _vTabelaExiste = _vTable.GetByKey(Table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove()
        {
            try
            {
                int vRet = 0;

                if (_vTabelaExiste)
                {
                    vRet = _vTable.Remove();

                    if (vRet != 0)
                    {
                        throw new Exception(AddonService.diCompany.GetLastErrorDescription());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void AddUserTable(string Descricao, BoUTBTableType TableType = BoUTBTableType.bott_NoObject)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserFields AddUserField(string Campo, string Descricao, SAPbobsCOM.BoFieldTypes Tipo, SAPbobsCOM.BoFldSubTypes SubTipo = BoFldSubTypes.st_None, int Tamanho = 254)
        {
            return new UserFields();
        }

        public void Dispose()
        {
            try
            {
                AddonService.diCompany.ReleaseComObject(_vTable);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
