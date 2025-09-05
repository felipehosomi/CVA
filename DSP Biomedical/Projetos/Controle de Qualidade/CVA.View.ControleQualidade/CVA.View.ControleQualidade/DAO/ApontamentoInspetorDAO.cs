using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.Resources.Query;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.DAO
{
    public class ApontamentoInspetorDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
        private SAPbobsCOM.Company _company { get; set; }

        public ApontamentoInspetorDAO(BusinessOneDAO businessOneDAO, SAPbobsCOM.Company company)
        {
            _businessOneDAO = businessOneDAO;
            _company = company;
        }

        public string GetNextCode()
        {
            return _businessOneDAO.GetNextCode("CVA_QUALITY_INS");
        }

        public ApontamentoInspetor Get(string user, string date, string op)
        {
            string query = String.Format(Select.GetApontamentoAtual, date, op);
            return _businessOneDAO.ExecuteSqlForObject<ApontamentoInspetor>(query);
        }

        public string Save(ApontamentoInspetor apontamento)
        {
            CompanyService oCompanyService = _company.GetCompanyService();
            GeneralService oGeneralService = oCompanyService.GetGeneralService("CVAQualityIns");
            GeneralData oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
            GeneralDataCollection oChildren = null;
            GeneralData oChild = null;

            GeneralDataParams generalParam = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

            string exists = _businessOneDAO.ExecuteSqlForObject<string>(String.Format(Select.GenericExists, "@CVA_QUALITY_INS", apontamento.Code));

            try
            {
                if (!String.IsNullOrEmpty(exists))
                {
                    generalParam.SetProperty("Code", apontamento.Code);
                    oGeneralData = oGeneralService.GetByParams(generalParam);
                }
                else
                {
                    oGeneralData.SetProperty("Code", apontamento.Code);
                    oGeneralData.SetProperty("U_Usuario", apontamento.Usuario);
                    oGeneralData.SetProperty("U_PlanoInsp", apontamento.PlanoInsp);
                    oGeneralData.SetProperty("U_Data", apontamento.Data);
                    oGeneralData.SetProperty("U_OP", apontamento.OP);
                    oGeneralData.SetProperty("U_ItemCode", apontamento.ItemCode);
                }
               
                oChildren = oGeneralData.Child("CVA_QUALITY_INS1");

                int index = 0;
                for (; index < oChildren.Count && index < apontamento.Linhas.Count; index++)
                {
                    oChild = oChildren.Item(index);
                    oChild.SetProperty("U_Valor", apontamento.Linhas[index].Valor);
                }

                for (; index < apontamento.Linhas.Count; index++)
                {
                    oChild = oChildren.Add();

                    oChild.SetProperty("U_InspLinha", apontamento.Linhas[index].InspLinha);
                    oChild.SetProperty("U_Hora", apontamento.Linhas[index].Hora);
                    oChild.SetProperty("U_Valor", apontamento.Linhas[index].Valor);
                    oChild.SetProperty("U_Nome", apontamento.Linhas[index].Nome);
                }

                if (!String.IsNullOrEmpty(exists))
                {
                    oGeneralService.Update(oGeneralData);
                }
                else
                {
                    oGeneralService.Add(oGeneralData);
                }
                return String.Empty;
            }
            catch (Exception e)
            {
                return "Erro ao salvar objeto: " + e.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(oGeneralService);
                Marshal.ReleaseComObject(oGeneralData);
                Marshal.ReleaseComObject(oCompanyService);

                oGeneralService = null;
                oGeneralData = null;
                oCompanyService = null;

                Marshal.ReleaseComObject(oChildren);
                oChildren = null;

                Marshal.ReleaseComObject(oChild);
                oChild = null;
            }
        }
    }
}
