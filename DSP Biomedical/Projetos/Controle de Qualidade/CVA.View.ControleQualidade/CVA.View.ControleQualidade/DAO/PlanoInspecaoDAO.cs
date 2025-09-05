using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.Resources.Query;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.DAO
{
    public partial class PlanoInspecaoDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
        private SAPbobsCOM.Company _company { get; set; }
    }

    public partial class PlanoInspecaoDAO
    {
        public PlanoInspecaoDAO(BusinessOneDAO businessOneDAO, SAPbobsCOM.Company company)
        {
            _businessOneDAO = businessOneDAO;
            _company = company;
        }

        public string GetMaxCode_Quality1()
        {
            var query = Select.GetMaxCodeQuality1;
            return _businessOneDAO.ExecuteSqlForObject<string>(query);
        }

        public bool VerifyExistence(string code)
        {
            var query = String.Format(Select.GetCodeFromPlan, code);
            return string.IsNullOrEmpty(_businessOneDAO.ExecuteSqlForObject<string>(query));
        }

        public bool VerifyExistence_Quantiy1(string code)
        {
            var query = string.Format(Select.GetCodeFromPlan1, code);
            return string.IsNullOrEmpty(_businessOneDAO.ExecuteSqlForObject<string>(query));
        }

        public PlanoInspecao GetPlano(string code)
        {
            var query = String.Format(Select.GetPlanoInspecao, code);
            return _businessOneDAO.ExecuteSqlForObject<PlanoInspecao>(query);
        }

        public List<PlanoInspecaoLinha> GetPlanoInspecaoLinha(string code, string tipoInspecao)
        {
            var query = String.Format(Select.GetPlanoInspecaoLinha, code, tipoInspecao);
            return _businessOneDAO.ExecuteSqlForList<PlanoInspecaoLinha>(query);
        }

        public DuplicarPlanoModel RetornaPlano(string code)
        {
            var query = String.Format(Select.RetornaPlano, code);
            return _businessOneDAO.ExecuteSqlForObject<DuplicarPlanoModel>(query);
        }

        public List<DuplicarPlanoModelLnha> RetornaPlanoLinha(string code)
        {
            var query = String.Format(Select.RetornaPlanoLinhas, code);
            return _businessOneDAO.ExecuteSqlForList<DuplicarPlanoModelLnha>(query);
        }

    }
}
