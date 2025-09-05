
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Repository.Services;

namespace App.ApplicationServices
{
    public class UserUDO : IDisposable
    {
        private UserObjectsMD _vUserUDO = null;
        private bool _vUDOExiste = false;

        public UserUDO(string UDO)
        {
            _vUserUDO = (UserObjectsMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
            _vUDOExiste = _vUserUDO.GetByKey(UDO);
        }
        
        public void Remove()
        {
            try
            {
                if (_vUDOExiste)
                {
                    

                    List<string> vTabelasUDO = new List<string>();

                    for (int i = 0; i < _vUserUDO.ChildTables.Count; i++)
                    {
                        _vUserUDO.ChildTables.SetCurrentLine(i);

                        if(!_vUserUDO.ChildTables.TableName.IsNullOrEmpty())
                            vTabelasUDO.Add(_vUserUDO.ChildTables.TableName);
                    }

                    vTabelasUDO.Add(_vUserUDO.TableName);

                    int iRet = _vUserUDO.Remove();

                    if (iRet == 0)
                    {
                        _vUserUDO.Release();

                        foreach (string vTable in vTabelasUDO)
                        {
                            UserTables vTabelaSAP = new UserTables(vTable);
                            vTabelaSAP.Remove();
                        }
                    }
                    else
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

        public void Dispose()
        {
            try
            {
                AddonService.diCompany.ReleaseComObject(_vUserUDO);

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
