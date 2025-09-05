using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.MODEL;
using SAPbobsCOM;
using System.Collections.Generic;
using System.Linq;
using System;
using CVA.Core.DSP.Controle.HELPER;

namespace CVA.Core.DSP.Controle.BLL
{
    public class FerramentasBLL
    {
        private FerramentaDAO _ferramentaDAO { get; set; }
        private Company _company { get; set; }

        public FerramentasBLL(FerramentaDAO ferramentaDAO, Company company)
        {
            _ferramentaDAO = ferramentaDAO;
            _company = company;
        }
        
        public List<Ferramenta> Get()
        {
            BusinessOneLog.Add("Iniciando recuperação de ferramentas");
            try
            {
                var ferrementas = _ferramentaDAO.Get();
                if (ferrementas != null)
                    return ferrementas.OrderBy(m => m.Nome).ToList();
                return null;
            }
            catch (Exception ex)
            {
                BusinessOneLog.Add("Falha ao recuperar Ferramentas:  " + ex.Message, true);
                return null;
            }
        }

        public Ferramenta GetByItemCode(string itemCode)
        {
            try
            {
                BusinessOneLog.Add("Iniciando recuperação de ferramenta por ItemCode");
                return _ferramentaDAO.GetItemCode(itemCode);
            }
            catch (System.Exception ex)
            {
                BusinessOneLog.Add("Falha ao recuperar ferramenta: " + ex.Message, true);
                throw;
            }
        }

        public bool Add(string toolCode, int counter)
        {
            try
            {
                BusinessOneLog.Add("Iniciando processo de inserção de ferramenta");
                var isValid = ValidateFields(toolCode, counter);

                BusinessOneLog.Add("Verificação de validade do formulário");
                if (isValid)
                    return _ferramentaDAO.Insert(toolCode, counter);
                return isValid;
            }
            catch (Exception ex)
            {
                BusinessOneLog.Add("Erro ao adicionar ferramenta :" +ex.Message, true);
                return false;
            }
        }

        public bool Update(string toolCode, int counter)
        {
            try
            {
                BusinessOneLog.Add("Iniciando processo de atualização de ferramenta");
                return _ferramentaDAO.Update(toolCode, counter);
            }
            catch (Exception ex)
            {
                BusinessOneLog.Add("Erro ao atualizar ferramenta :" + ex.Message, true);
                return false;
            }
        }

        public string GetCounter(string toolCode)
        {
            BusinessOneLog.Add("Iniciando recuperação do contador da ferramenta");
            return _ferramentaDAO.GetCounter(toolCode);
        }

        private bool ValidateFields(string itemCode, int counter)
        {
            BusinessOneLog.Add("Validando dados de formulário");
            return !string.IsNullOrEmpty(itemCode) && counter != 0;
        }
    }
}