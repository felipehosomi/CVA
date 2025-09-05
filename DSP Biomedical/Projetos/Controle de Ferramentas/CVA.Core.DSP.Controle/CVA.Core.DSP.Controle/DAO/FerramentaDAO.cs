using CVA.Core.DSP.Controle.HELPER;
using CVA.Core.DSP.Controle.MODEL;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;

namespace CVA.Core.DSP.Controle.DAO
{
    public partial class FerramentaDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
        private Company _company { get; set; }
    }

    public partial class FerramentaDAO
    {
        public FerramentaDAO(BusinessOneDAO businessOneDAO, Company company)
        {
            _businessOneDAO = businessOneDAO;
            _company = company;
        }

        public List<Ferramenta> Get()
        {
            try
            {
                BusinessOneLog.Add("Acessando a base de dados para recuperação de ferramentas");
                var query = Resources.Select.Query.GetFerramentas;
                return _businessOneDAO.ExecuteSqlForList<Ferramenta>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Ferramenta GetItemCode(string itemCode)
        {
            try
            {
                BusinessOneLog.Add("Acessando a base de dados para recuperação de ferramenta por itemCode");
                var query = String.Format(Resources.Select.Query.GetFerramentaItemCode, itemCode);
                return _businessOneDAO.ExecuteSqlForObject<Ferramenta>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Insert(string toolCode, int counter)
        {
            try
            {
                BusinessOneLog.Add("Inserindo valores na tabela de usuário para cadastro de ferramenta");
                var table = _company.UserTables.Item("CVA_Tool");

                table.Code = toolCode.Trim();
                table.Name = toolCode.Trim();
                table.UserFields.Fields.Item("U_CVA_Tool").Value = toolCode;
                table.UserFields.Fields.Item("U_CVA_Counter").Value = counter.ToString();

                if (table.Add() != 0)
                {
                    int ErrorCode;
                    string errorMsg;

                    _company.GetLastError(out ErrorCode, out errorMsg);
                    BusinessOneLog.Add("Erro ao adicionar ferramenta : " + errorMsg, true);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update(string toolCode, int counter)
        {
            try
            {
                BusinessOneLog.Add("Atualizado valores na tabela de usuário para cadastro de ferramenta");
                var table = _company.UserTables.Item("CVA_Tool");
                table.GetByKey(toolCode);
                table.UserFields.Fields.Item("U_CVA_Counter").Value = counter.ToString();

                if (table.Update() != 0)
                {
                    int ErrorCode;
                    string errorMsg;

                    _company.GetLastError(out ErrorCode, out errorMsg);
                    BusinessOneLog.Add("Erro ao atualizar ferramenta : " + errorMsg, true);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCounter(string toolCode)
        {
            try
            {
                BusinessOneLog.Add("Acessando a base de dados para recuperar contador da ferramenta");
                var table = _company.UserTables.Item("CVA_TOOL");
                table.GetByKey(toolCode);

                return table.UserFields.Fields.Item("U_CVA_Counter").Value.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
