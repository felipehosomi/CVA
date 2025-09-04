using System;
using System.Collections.Generic;
using System.Linq;
using CVA_Rep_DAL;
using CVA_Obj_Shared.Sap;
using SAPbobsCOM;
using CVA_Rep_Exception;
using CVA_Obj_Shared.Interfaces;

namespace CVA_Rep_Service
{
    public static class Replicador
    {
        public static void Replica(
            UnitOfWork oUnitOfWork, 
            PluginProvider provider, 
            List<int> lista, 
            Dictionary<int, string> dictRegistros,
            Dictionary<int, UsuarioService> dictUsuarios,
            Dictionary<int, CentroCustoService> dictCentroCusto,
            Dictionary<int, UtilizacaoService> dictUtilizacao,
            Dictionary<int, string> dictTipoImposto,
            Dictionary<string, string> dictAliquotaImposto,
            Dictionary<int, string> dictCodigoImposto)
        {
            foreach (var _base in GetBasesDestino(oUnitOfWork))
            {
                var _registroID = 0;

                try
                {
                    lista.Sort();

                    foreach (var l in lista)
                    {
                        var registro = oUnitOfWork.CvaRegRepository.GetByID(l);
                        _registroID = registro.ID;

                        if (_base.ID != registro.BAS && _base.ID >= (registro.BAS_ERR ?? 0))
                        {
                            var oCompany = GetCompanyDestino(_base);
                            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;

                            var cvaObj = oUnitOfWork.CvaObjRepository.GetByID(registro.OBJ);

                            try
                            {
                                if(dictRegistros.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "A")
                                {
                                    provider.Create(cvaObj.OBJ, dictRegistros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}.");
                                }
                                else if(dictRegistros.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "U")
                                {
                                    provider.Update(cvaObj.OBJ, dictRegistros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}.");
                                }
                                else if (dictRegistros.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "D")
                                {
                                    provider.Delete(cvaObj.OBJ, dictRegistros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}.");
                                }
                                else if (dictCodigoImposto.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "A")
                                {
                                    provider.Create(cvaObj.OBJ, dictCodigoImposto.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), dictTipoImposto, dictAliquotaImposto, oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}.");
                                }
                                else if (dictCodigoImposto.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "U")
                                {
                                    provider.Update(cvaObj.OBJ, dictCodigoImposto.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), dictTipoImposto, dictAliquotaImposto, oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}.");
                                }
                                else if (dictCodigoImposto.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "D")
                                {
                                    provider.Delete(cvaObj.OBJ, dictCodigoImposto.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), dictTipoImposto, dictAliquotaImposto, oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}.");
                                }
                                else if (dictCentroCusto.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "A")
                                {
                                    provider.Create(cvaObj.OBJ, dictCentroCusto.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}.");
                                }
                                else if (dictCentroCusto.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "U")
                                {
                                    provider.Update(cvaObj.OBJ, dictCentroCusto.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}.");
                                }
                                else if (dictCentroCusto.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "D")
                                {
                                    provider.Delete(cvaObj.OBJ, dictCentroCusto.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}.");
                                }
                                else if (dictUsuarios.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "A")
                                {
                                    provider.Create(cvaObj.OBJ, dictUsuarios.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}.");
                                }
                                else if (dictUsuarios.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "U")
                                {
                                    provider.Update(cvaObj.OBJ, dictUsuarios.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}.");
                                }
                                else if (dictUsuarios.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "D")
                                {
                                    provider.Delete(cvaObj.OBJ, dictUsuarios.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}.");
                                }
                                else if (dictUtilizacao.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "A")
                                {
                                    provider.Create(cvaObj.OBJ, dictUtilizacao.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}.");
                                }
                                else if (dictUtilizacao.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "U")
                                {
                                    provider.Update(cvaObj.OBJ, dictUtilizacao.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}.");
                                }
                                else if (dictUtilizacao.Keys.Contains(l) && registro.CVA_FUNC.FUNC == "D")
                                {
                                    provider.Delete(cvaObj.OBJ, dictUtilizacao.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);
                                    SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}.");
                                }
                            }
                            catch (Exception)
                            {
                                if (oCompany.Connected)
                                    oCompany.Disconnect();

                                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
                                //oCompany = null;

                                throw;
                            }

                            if (oCompany.Connected)
                                oCompany.Disconnect();

                            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
                            //oCompany = null;
                        }
                    }
                }
                catch (ReplicadorException ex)
                {
                    RollbackLog(oUnitOfWork, lista, _registroID, _base, ex.Message);

                    throw;
                }
                catch (Exception ex)
                {
                    RollbackLog(oUnitOfWork, lista, _registroID, _base, ex.Message);

                    throw;
                }
            }

            lista.Sort();
            foreach (var pair in lista)
            {
                AtualizaReg(oUnitOfWork, pair, null, 4);
            }
        }

        //public static void Replica(UnitOfWork oUnitOfWork, Dictionary<int, string> registros, PluginProvider provider)
        //{
        //    foreach (var _base in GetBasesDestino(oUnitOfWork))
        //    {
        //        var _registroID = 0;

        //        try
        //        {
        //            var lst = registros.Keys.ToList();
        //            lst.Sort();

        //            foreach (var kvpRegistro in lst)
        //            {
        //                var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //                _registroID = registro.ID;

        //                if (_base.ID != registro.BAS && _base.ID >= (registro.BAS_ERR ?? 0))
        //                {
        //                    Company oCompany = GetCompanyDestino(_base);
        //                    oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
        //                    var cvaObj = oUnitOfWork.CvaObjRepository.GetByID(registro.OBJ);

        //                    try
        //                    {
        //                        if (registro.CVA_FUNC.FUNC == "A")
        //                        {
        //                            provider.Create(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}.");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "U")
        //                        {
        //                            provider.Update(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}.");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "D")
        //                        {
        //                            provider.Delete(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}.");
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        if (oCompany.Connected)
        //                            oCompany.Disconnect();

        //                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                        //oCompany = null;

        //                        throw;
        //                    }

        //                    if (oCompany.Connected)
        //                        oCompany.Disconnect();

        //                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                    //oCompany = null;
        //                }
        //            }
        //        }
        //        catch (ReplicadorException ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //    }

        //    var lstt = registros.Keys.ToList();
        //    lstt.Sort();

        //    foreach (var pair in lstt)
        //    {
        //        AtualizaReg(oUnitOfWork, pair, null, 4);
        //    }
        //}

        //public static void ReplicaImposto(UnitOfWork oUnitOfWork, Dictionary<int, string> registros, PluginProvider provider, Dictionary<int, string> dictTipoImposto, Dictionary<string, string> dictAliquotaImposto)
        //{
        //    foreach (var _base in GetBasesDestino(oUnitOfWork))
        //    {
        //        var _registroID = 0;
        //        try
        //        {
        //            var lst = registros.Keys.ToList();
        //            lst.Sort();

        //            foreach (var kvpRegistro in lst)
        //            {
        //                var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //                _registroID = registro.ID;

        //                if (_base.ID != registro.BAS && _base.ID >= (registro.BAS_ERR ?? 0))
        //                {
        //                    Company oCompany = GetCompanyDestino(_base);
        //                    oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
        //                    var cvaObj = oUnitOfWork.CvaObjRepository.GetByID(registro.OBJ);

        //                    try
        //                    {
        //                        if (registro.CVA_FUNC.FUNC == "A")
        //                        {
        //                            provider.Create(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), dictTipoImposto, dictAliquotaImposto, oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "U")
        //                        {
        //                            provider.Update(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), dictTipoImposto, dictAliquotaImposto, oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "D")
        //                        {
        //                            provider.Delete(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), dictTipoImposto, dictAliquotaImposto, oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}");
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        if (oCompany.Connected)
        //                            oCompany.Disconnect();

        //                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                        //oCompany = null;

        //                        throw;
        //                    }

        //                    if (oCompany.Connected)
        //                        oCompany.Disconnect();

        //                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                    //oCompany = null;
        //                }
        //            }
        //        }
        //        catch (ReplicadorException ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //    }

        //    var lstt = registros.Keys.ToList();
        //    lstt.Sort();

        //    foreach (var pair in lstt)
        //    {
        //        AtualizaReg(oUnitOfWork, pair, null, 4);
        //    }
        //}

        //public static void ReplicaCentroCusto(UnitOfWork oUnitOfWork, Dictionary<int, CentroCustoService> registros, PluginProvider provider)
        //{
        //    foreach (var _base in GetBasesDestino(oUnitOfWork))
        //    {
        //        var _registroID = 0;

        //        try
        //        {
        //            var lst = registros.Keys.ToList();
        //            lst.Sort();

        //            foreach (var kvpRegistro in lst)
        //            {
        //                var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //                _registroID = registro.ID;

        //                if (_base.ID != registro.BAS && _base.ID >= (registro.BAS_ERR ?? 0))
        //                {
        //                    Company oCompany = GetCompanyDestino(_base);
        //                    oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
        //                    var cvaObj = oUnitOfWork.CvaObjRepository.GetByID(registro.OBJ);

        //                    try
        //                    {
        //                        if (registro.CVA_FUNC.FUNC == "A")
        //                        {
        //                            provider.Create(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "U")
        //                        {
        //                            provider.Update(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "D")
        //                        {
        //                            provider.Delete(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}");
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        if (oCompany.Connected)
        //                            oCompany.Disconnect();

        //                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                        //oCompany = null;

        //                        throw;
        //                    }

        //                    if (oCompany.Connected)
        //                        oCompany.Disconnect();

        //                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                    //oCompany = null;
        //                }
        //            }
        //        }
        //        catch (ReplicadorException ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //    }

        //    var lstt = registros.Keys.ToList();
        //    lstt.Sort();

        //    foreach (var kvpRegistro in lstt)
        //    {
        //        AtualizaReg(oUnitOfWork, kvpRegistro, null, 4);
        //    }
        //}

        //public static void ReplicaUsuario(UnitOfWork oUnitOfWork, Dictionary<int, UsuarioService> registros, PluginProvider provider)
        //{
        //    foreach (var _base in GetBasesDestino(oUnitOfWork))
        //    {
        //        var _registroID = 0;

        //        try
        //        {
        //            var lst = registros.Keys.ToList();
        //            lst.Sort();

        //            foreach (var kvpRegistro in lst)
        //            {
        //                var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //                _registroID = registro.ID;

        //                if (_base.ID != registro.BAS && _base.ID >= (registro.BAS_ERR ?? 0))
        //                {
        //                    Company oCompany = GetCompanyDestino(_base);
        //                    oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
        //                    var cvaObj = oUnitOfWork.CvaObjRepository.GetByID(registro.OBJ);

        //                    try
        //                    {
        //                        if (registro.CVA_FUNC.FUNC == "A")
        //                        {
        //                            provider.Create(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "U")
        //                        {
        //                            provider.Update(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "D")
        //                        {
        //                            provider.Delete(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}");
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        if (oCompany.Connected)
        //                            oCompany.Disconnect();

        //                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                        //oCompany = null;

        //                        throw;
        //                    }

        //                    if (oCompany.Connected)
        //                        oCompany.Disconnect();

        //                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                    //oCompany = null;
        //                }
        //            }
        //        }
        //        catch (ReplicadorException ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //    }

        //    var lstt = registros.Keys.ToList();
        //    lstt.Sort();

        //    foreach (var kvpRegistro in lstt)
        //    {
        //        AtualizaReg(oUnitOfWork, kvpRegistro, null, 4);
        //    }
        //}

        //public static void ReplicaUtilizacao(UnitOfWork oUnitOfWork, Dictionary<int, UtilizacaoService> registros, PluginProvider provider)
        //{
        //    foreach (var _base in GetBasesDestino(oUnitOfWork))
        //    {
        //        var _registroID = 0;

        //        try
        //        {
        //            var lst = registros.Keys.ToList();
        //            lst.Sort();

        //            foreach (var kvpRegistro in lst)
        //            {
        //                var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //                _registroID = registro.ID;

        //                if (_base.ID != registro.BAS && _base.ID >= (registro.BAS_ERR ?? 0))
        //                {
        //                    Company oCompany = GetCompanyDestino(_base);
        //                    oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
        //                    var cvaObj = oUnitOfWork.CvaObjRepository.GetByID(registro.OBJ);

        //                    try
        //                    {
        //                        if (registro.CVA_FUNC.FUNC == "A")
        //                        {
        //                            provider.Create(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto replicado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "U")
        //                        {
        //                            provider.Update(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto atualizado na base {_base.COMP}");
        //                        }
        //                        else if (registro.CVA_FUNC.FUNC == "D")
        //                        {
        //                            provider.Delete(cvaObj.OBJ, registros.Where(d => d.Key == registro.ID).ToDictionary(p => p.Key, p => p.Value), oCompany);

        //                            SalvaLog(oUnitOfWork, registro.ID, _base.ID, 4, $"{cvaObj.OBJ}: Objeto removido da base {_base.COMP}");
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        if (oCompany.Connected)
        //                            oCompany.Disconnect();

        //                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                        //oCompany = null;

        //                        throw;
        //                    }

        //                    if (oCompany.Connected)
        //                        oCompany.Disconnect();

        //                    //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oCompany);
        //                    //oCompany = null;
        //                }
        //            }
        //        }
        //        catch (ReplicadorException ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            RollbackLog(oUnitOfWork, registros, _registroID, _base, ex.Message);

        //            throw;
        //        }
        //    }

        //    var lstt = registros.Keys.ToList();
        //    lstt.Sort();

        //    foreach (var kvpRegistro in lstt)
        //    {
        //        AtualizaReg(oUnitOfWork, kvpRegistro, null, 4);
        //    }
        //}

        private static void AtualizaReg(UnitOfWork oUnitOfWork, int _reg, int? _base, int _stu, bool isError = false)
        {
            var _cvaReg = oUnitOfWork.CvaRegRepository.GetByID(_reg);
            _cvaReg.BAS_ERR = _stu == 5 ? _base : (isError ? _base : null);
            _cvaReg.STU = _stu;
            _cvaReg.UPD = DateTime.Now;

            oUnitOfWork.CvaRegRepository.Update(_cvaReg);
            oUnitOfWork.Save();
        }

        private static void SalvaLog(UnitOfWork oUnitOfWork, int _reg, int _base, int _stu, string _msg)
        {
            var _log = new CVA_REG_LOG();
            _log.BAS = _base;
            _log.REG = _reg;
            _log.STU = _stu;
            _log.INS = DateTime.Now;
            _log.MSG = _msg;

            oUnitOfWork.CvaRegLogRepository.Insert(_log);
            oUnitOfWork.Save();
        }

        private static CVA_BAS[] GetBasesDestino(UnitOfWork UoW)
        {
            return UoW.CvaBasRepository.Get(b => b.STU == 2).OrderBy(o => o.ID).ToArray();
        }

        private static Company GetCompanyDestino(CVA_BAS _base)
        {
            var conn = B1Connection.Instance;
            var oCompany = conn.Connect(_base.UNAME, _base.PAS, _base.COMP, _base.SRVR, false, _base.DB_UNAME, _base.DB_PAS, (BoDataServerTypes)_base.DB_TYP, _base.DB_SRVR);
            oCompany.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            return oCompany;
        }

        private static void RollbackLog(UnitOfWork oUnitOfWork, List<int> lista, int _registroID, CVA_BAS _base, string message)
        {
            lista.Sort();
            foreach (var kvpRegistro in lista)
            {
                var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
                var baseId = oUnitOfWork.CvaBasRepository.Get(b => b.ID > _base.ID).FirstOrDefault();
                int? int0 = null;
                if (baseId != null)
                    int0 = baseId.ID;

                if (kvpRegistro >= _registroID)
                    AtualizaReg(oUnitOfWork, registro.ID, _base.ID, 5);
                else
                    AtualizaReg(oUnitOfWork, registro.ID, int0, 3, true);
            }

            SalvaLog(oUnitOfWork, _registroID, _base.ID, 5, $"{message}.");
        }

        //private static void RollbackLog(UnitOfWork oUnitOfWork, Dictionary<int, string> registros, int _registroID, CVA_BAS _base, string message)
        //{
        //    var lst = registros.Keys.ToList();
        //    lst.Sort();
        //    foreach (var kvpRegistro in lst)
        //    {
        //        var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //        var baseId = oUnitOfWork.CvaBasRepository.Get(b => b.ID > _base.ID).FirstOrDefault();
        //        int? int0 = null;
        //        if (baseId != null)
        //            int0 = baseId.ID;

        //        if(kvpRegistro >= _registroID)
        //            AtualizaReg(oUnitOfWork, registro.ID, _base.ID, 5);
        //        else
        //            AtualizaReg(oUnitOfWork, registro.ID, int0, 3, true);
        //    }

        //    SalvaLog(oUnitOfWork, _registroID, _base.ID, 5, $"{message}.");
        //}

        //private static void RollbackLog(UnitOfWork oUnitOfWork, Dictionary<int, CentroCustoService> registros, int _registroID, CVA_BAS _base, string message)
        //{
        //    var lst = registros.Keys.Where(k => k >= _registroID).ToList();
        //    lst.Sort();

        //    foreach (var kvpRegistro in lst)
        //    {
        //        var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //        var baseId = oUnitOfWork.CvaBasRepository.Get(b => b.ID > _base.ID).FirstOrDefault();
        //        int? int0 = null;
        //        if (baseId != null)
        //            int0 = baseId.ID;

        //        if (kvpRegistro >= _registroID)
        //            AtualizaReg(oUnitOfWork, registro.ID, _base.ID, 5);
        //        else
        //            AtualizaReg(oUnitOfWork, registro.ID, int0, 3, true);
        //    }

        //    SalvaLog(oUnitOfWork, _registroID, _base.ID, 5, $"{message}.");
        //}

        //private static void RollbackLog(UnitOfWork oUnitOfWork, Dictionary<int, UsuarioService> registros, int _registroID, CVA_BAS _base, string message)
        //{
        //    var lst = registros.Keys.Where(k => k >= _registroID).ToList();
        //    lst.Sort();

        //    foreach (var kvpRegistro in lst)
        //    {
        //        var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //        var baseId = oUnitOfWork.CvaBasRepository.Get(b => b.ID > _base.ID).FirstOrDefault();
        //        int? int0 = null;
        //        if (baseId != null)
        //            int0 = baseId.ID;

        //        if (kvpRegistro >= _registroID)
        //            AtualizaReg(oUnitOfWork, registro.ID, _base.ID, 5);
        //        else
        //            AtualizaReg(oUnitOfWork, registro.ID, int0, 3, true);
        //    }

        //    SalvaLog(oUnitOfWork, _registroID, _base.ID, 5, $"{message}.");
        //}

        //private static void RollbackLog(UnitOfWork oUnitOfWork, Dictionary<int, UtilizacaoService> registros, int _registroID, CVA_BAS _base, string message)
        //{
        //    var lst = registros.Keys.Where(k => k >= _registroID).ToList();
        //    lst.Sort();

        //    foreach (var kvpRegistro in lst)
        //    {
        //        var registro = oUnitOfWork.CvaRegRepository.GetByID(kvpRegistro);
        //        var baseId = oUnitOfWork.CvaBasRepository.Get(b => b.ID > _base.ID).FirstOrDefault();
        //        int? int0 = null;
        //        if (baseId != null)
        //            int0 = baseId.ID;

        //        if (kvpRegistro >= _registroID)
        //            AtualizaReg(oUnitOfWork, registro.ID, _base.ID, 5);
        //        else
        //            AtualizaReg(oUnitOfWork, registro.ID, int0, 3, true);
        //    }

        //    SalvaLog(oUnitOfWork, _registroID, _base.ID, 5, $"{message}.");
        //}
    }
}
