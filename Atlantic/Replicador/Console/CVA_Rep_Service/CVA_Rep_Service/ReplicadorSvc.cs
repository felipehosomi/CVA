using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CVA_Obj_Shared.Interfaces;
using CVA_Obj_Shared.Sap;
using CVA_Obj_Shared.Extensions;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;
using SAPbobsCOM;
using System.Threading.Tasks;

// ReSharper disable PossibleInvalidOperationException

namespace CVA_Rep_Service
{
    public class ReplicadorSvc
    {
        private readonly CVA_TIM cvaTimer;

        private readonly ILogger logger;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly UnitOfWork oUnitOfWork;

        public ReplicadorSvc()
        {
            try
            {
                oUnitOfWork = new UnitOfWork();
                cvaTimer = oUnitOfWork.CvaTimRepository.GetByID(1);

                logService.Configure(
                    new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
                logger = logService.GetLogger<ReplicadorSvc>();

                if (cvaTimer.STU == 2)
                    Process();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Process()
        {
            try
            {
                UpdateTimer(6);
                if(Directory.Exists($@"{Path.GetTempPath()}\SM_OBS_DLL"))
                    DeleteDirectory($@"{Path.GetTempPath()}\SM_OBS_DLL");
                var listaRegistros = oUnitOfWork.CvaRegRepository.Get(r => r.STU == 3).Take(Convert.ToInt32(cvaTimer.NUM_OBJ)).OrderBy(o => o.ID).ToArray();
                if (listaRegistros.Length > 0)
                {
                    var baseOrigem = oUnitOfWork.CvaBasRepository.GetByID(1);
                    var provider = new PluginProvider();
                    provider.Initialize();
                    var dict = new Dictionary<int, string>();
                    var dictUsuario = new Dictionary<int, UsuarioService>();
                    var dictUtilizacao = new Dictionary<int, UtilizacaoService>();
                    var dictCodigoImposto = new Dictionary<int, string>();
                    var dictTipoImposto = new Dictionary<int, string>();
                    var dictAliquotaImposto = new Dictionary<string, string>();
                    var dictCentroCusto = new Dictionary<int, CentroCustoService>();
                    var lista = new List<int>();

                    using (var origem = new OrigemBLL(baseOrigem))
                    {
                        foreach (var cvaReg in listaRegistros.Where(b => b.BAS == baseOrigem.ID).ToArray())
                        {
                            var cvaObj = oUnitOfWork.CvaObjRepository.Get(o => o.ID == cvaReg.OBJ && o.STU == 2).FirstOrDefault();

                            if (cvaObj != null)
                            {
                                if (cvaObj.ID == 1)
                                {
                                    origem.CreateGrupoParceiroNegocio(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 2)
                                {
                                    origem.CreateGrupoItem(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 3)
                                {
                                    origem.CreateUsuario(cvaReg.ID, ref dictUsuario);
                                    lista.Add(cvaReg.ID);
                                    if (dictUsuario != null && dictUsuario.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 4 && cvaReg.FUNC != 2)
                                {
                                    origem.CreateFormaPagamento(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 5)
                                {
                                    origem.CreateCondicaoPagamento(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 6)
                                {
                                    origem.CreateCentroCusto(cvaReg.ID, ref dictCentroCusto);
                                    lista.Add(cvaReg.ID);
                                    if (dictCentroCusto != null && dictCentroCusto.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 7)
                                {
                                    origem.CreateIndicador(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 8)
                                {
                                    origem.CreateUtilizacao(cvaReg.ID, ref dictUtilizacao);
                                    lista.Add(cvaReg.ID);
                                    if (dictUtilizacao != null && dictUtilizacao.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 9)
                                {
                                    origem.CreatePlanoContas(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 11)
                                {
                                    origem.CreateItem(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 12)
                                {
                                    origem.CreateParceiroNegocio(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 13)
                                {
                                    dictAliquotaImposto.Clear();
                                    dictTipoImposto.Clear();

                                    origem.CreateTipoImposto(ref dictTipoImposto);
                                    origem.CreateAliquotaImposto(ref dictAliquotaImposto);
                                    origem.CreateCodigoImposto(cvaReg.ID, ref dictCodigoImposto);
                                    lista.Add(cvaReg.ID);
                                    if (dictTipoImposto != null && dictAliquotaImposto != null && dictTipoImposto.Count > 0 && dictAliquotaImposto.Count > 0 && dictCodigoImposto != null && dictCodigoImposto != null)
                                        continue;
                                }
                                else if (cvaObj.ID == 14)
                                {
                                    origem.CreateContasBancarias(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                }
                                else if (cvaObj.ID == 15)
                                {
                                    origem.CreateVendedoresCompradores(cvaReg.ID, ref dict);
                                    lista.Add(cvaReg.ID);
                                    if (dict != null && dict.Count > 0)
                                        continue;
                                } 
                            }
                        }
                    }

                    if(lista != null && lista.Count > 0)
                    {
                        Replicador.Replica(oUnitOfWork, provider, lista, dict, dictUsuario, dictCentroCusto, dictUtilizacao, dictTipoImposto, dictAliquotaImposto, dictCodigoImposto);
                    }

                    //if (dictUsuario != null && dictUsuario.Count > 0)
                    //{
                    //    Replicador.ReplicaUsuario(oUnitOfWork, dictUsuario, provider);
                    //}

                    //if (dictCentroCusto != null && dictCentroCusto.Count > 0)
                    //{
                    //    Replicador.ReplicaCentroCusto(oUnitOfWork, dictCentroCusto, provider);
                    //}

                    //if (dictUtilizacao != null && dictUtilizacao.Count > 0)
                    //{
                    //    Replicador.ReplicaUtilizacao(oUnitOfWork, dictUtilizacao, provider);
                    //}

                    //if (dict != null && dict.Count > 0)
                    //{
                    //    Replicador.Replica(oUnitOfWork, dict, provider);
                    //}

                    //if (dictTipoImposto != null && dictAliquotaImposto != null && dictTipoImposto.Count > 0 && dictAliquotaImposto.Count > 0 && dictCodigoImposto != null && dictCodigoImposto != null)
                    //{
                    //    Replicador.ReplicaImposto(oUnitOfWork, dictCodigoImposto, provider, dictTipoImposto, dictAliquotaImposto);
                    //}
                }

                UpdateTimer(2);
                var tim = Convert.ToInt32(cvaTimer.TIM);
                var slp = Convert.ToInt32(TimeSpan.FromMinutes(Convert.ToDouble(tim)).TotalMilliseconds);
                System.Threading.Thread.Sleep(slp);
            }
            catch (ReplicadorException ex)
            {
                if (!ex.Message.Contains("-8037"))
                {
                    if (!ex.Message.Contains("Internal error"))
                    {
                        UpdateTimer(5);
                        logger.Error(ex.Message + Environment.NewLine + ex.StackTrace, ex);
                    }
                    else
                    {
                        AtualizaReg();
                        UpdateTimer(2);
                    }
                }
                else
                {
                    AtualizaReg();
                    UpdateTimer(2);

                }

                throw;
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("-8037"))
                {
                    if (!ex.Message.Contains("Internal error"))
                    {
                        UpdateTimer(5);
                        logger.Error(ex.Message + Environment.NewLine + ex.StackTrace, ex);
                    }
                    else
                    {
                        AtualizaReg();
                        UpdateTimer(2);
                    }
                }
                else
                {
                    AtualizaReg();
                    UpdateTimer(2);

                }

                throw;
            }
            finally
            {
                Stop();
            }
        }
        
        public void Start()
        {
            if (cvaTimer.TIM == null) Stop();
            if (cvaTimer.STU != 2) Stop();
            if (cvaTimer.NUM_OBJ == null) Stop();
        }

        public void Stop()
        {
            oUnitOfWork.Dispose();
            Environment.Exit(0);
        }

        public void Shutdown()
        {
            oUnitOfWork.Dispose();
            Environment.Exit(0);
        }

        private void UpdateTimer(int stu)
        {
            cvaTimer.STU = stu;
            oUnitOfWork.CvaTimRepository.Update(cvaTimer);
            oUnitOfWork.Save();
        }

        private void AtualizaReg()
        {
            var cvaRegLog = oUnitOfWork.CvaRegLogRepository.Get(r => r.MSG.Contains("-8037")).OrderByDescending(o => o.ID).FirstOrDefault();
            if (cvaRegLog != null)
            {
                var cvaRegs = oUnitOfWork.CvaRegRepository.Get(r => r.STU == 5 && r.ID >= cvaRegLog.REG).ToList();
                foreach (var reg in cvaRegs)
                {
                    reg.STU = 3;
                    oUnitOfWork.CvaRegRepository.Update(reg);
                    oUnitOfWork.Save();
                } 
            }

            cvaRegLog = oUnitOfWork.CvaRegLogRepository.Get(r => r.MSG.Contains("Internal error")).OrderByDescending(o => o.ID).FirstOrDefault();
            if (cvaRegLog != null)
            {
                var cvaRegs = oUnitOfWork.CvaRegRepository.Get(r => r.STU == 5 && r.ID >= cvaRegLog.REG).ToList();
                foreach (var reg in cvaRegs)
                {
                    reg.STU = 3;
                    oUnitOfWork.CvaRegRepository.Update(reg);
                    oUnitOfWork.Save();
                }
            }
        }

        /// <summary>
        /// Depth-first recursive delete, with handling for descendant 
        /// directories open in Windows Explorer.
        /// </summary>
        public static void DeleteDirectory(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }
    }
}