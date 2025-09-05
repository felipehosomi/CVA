using SAPbobsCOM;
using Application = SAPbouiCOM.Framework.Application;

namespace CVA.Escoteiros.Magento.AddOn.Controller
{
    public class CommonController
    {
        public static void GetCompany()
        {
            Company = (Company)Application.SBO_Application.Company.GetDICompany();
            CompanyService = Company.GetCompanyService();
        }

        public static Company Company { get; set; }

        public static CompanyService CompanyService { get; set; }

        public static int FormFatherCount { get; set; }

        public static string FormFatherType { get; set; }
    }
}
