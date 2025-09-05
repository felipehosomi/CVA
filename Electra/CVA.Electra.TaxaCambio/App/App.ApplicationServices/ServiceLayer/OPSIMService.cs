using App.ApplicationServices.Services;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class OPSIMService
    {
        //private readonly IServiceLayerRepository<SAPB1.SD_OPSIM> _repository;

        public OPSIMService()
        {
            //_repository = new ServiceLayerRepositories<SAPB1.SD_OPSIM>("SD_OPSIM");
        }

        public void Inserir(int? OPCode, List<int> list, int? Operador)
        {
            try
            {
                //var teste = _repository.Get("");
                //SAPB1.SD_OPSIM oSD_OPSIM;
                //foreach (var posicao in list)
                //{
                //    oSD_OPSIM = new SAPB1.SD_OPSIM();
                //    var code = $"{OPCode}_{posicao}";
                //    oSD_OPSIM.Code = code;
                //    oSD_OPSIM.Name = code;
                //    oSD_OPSIM.U_OPERACAO = OPCode.ToString();
                //    oSD_OPSIM.U_POSICAO = posicao;
                //    _repository.Add(oSD_OPSIM);
                //}
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;
                foreach (var posicao in list)
                {
                    var code = $"{OPCode}_{posicao}";
                    var sQuery = $"insert into \"{Database}\".\"@SD_OPSIM\" (\"Code\",\"Name\",\"U_OPERACAO\",\"U_POSICAO\",\"U_OPERADOR\")" +
                    $" Values('{code}','{code}','{OPCode}',{posicao},{Operador})";
                    var cmd = new HanaCommand(sQuery, _conn);
                    var ret = cmd.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Remover(int? OPCode, List<int> list)
        {
            try
            {
                //SAPB1.SD_OPSIM oSD_OPSIM;
                //foreach (var posicao in list)
                //{
                //    oSD_OPSIM = new SAPB1.SD_OPSIM();
                //    var code = $"{OPCode}_{posicao}";
                //    oSD_OPSIM.Code = code;
                //    oSD_OPSIM.Name = code;
                //    oSD_OPSIM.U_OPERACAO = OPCode.ToString();
                //    oSD_OPSIM.U_POSICAO = posicao;
                //    _repository.Delete(oSD_OPSIM, "Code");
                //}
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;
                foreach (var posicao in list)
                {
                    var code = $"{OPCode}_{posicao}";
                    var sQuery = $"DELETE FROM \"{Database}\".\"@SD_OPSIM\" WHERE \"Code\" = '{code}'";
                    var cmd = new HanaCommand(sQuery, _conn);
                    var ret = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool EstaTravado(int? codeOrdemDeProducao,int? CodePosicao, int? Operador)
        {
            try
            {
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;
                var sQuery = $"Select 1 as \"teste\" FROM \"{Database}\".\"@SD_OPSIM\" WHERE \"Code\" = '{codeOrdemDeProducao}_{CodePosicao}' and \"U_OPERADOR\" <> {Operador}";
                var cmd = new HanaCommand(sQuery, _conn);
                var ret = Convert.ToString( cmd.ExecuteScalar());
                if (ret == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Inserir(int? OPCode, int posicao, int? Operador)
        {
            try
            {
                if (ExisteRegisto(OPCode,  posicao,   Operador))
                {
                    return;
                }
                HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                var Database = FrameworkService.Database;
              
                    var code = $"{OPCode}_{posicao}";
                    var sQuery = $"insert into \"{Database}\".\"@SD_OPSIM\" (\"Code\",\"Name\",\"U_OPERACAO\",\"U_POSICAO\",\"U_OPERADOR\")" +
                    $" Values('{code}','{code}','{OPCode}',{posicao},{Operador})";
                    var cmd = new HanaCommand(sQuery, _conn);
                    var ret = cmd.ExecuteNonQuery();
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ExisteRegisto(int? codeOrdemDeProducao, int CodePosicao, int? operador)
        {
            try
            {
                try
                {
                    HanaConnection _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                    _conn.Open();
                    var Database = FrameworkService.Database;
                    var sQuery = $"Select 1 as \"teste\" FROM \"{Database}\".\"@SD_OPSIM\" WHERE \"Code\" = '{codeOrdemDeProducao}_{CodePosicao}' and \"U_OPERADOR\" = {operador}";
                    var cmd = new HanaCommand(sQuery, _conn);
                    var ret = Convert.ToString(cmd.ExecuteScalar());
                    if (ret == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
