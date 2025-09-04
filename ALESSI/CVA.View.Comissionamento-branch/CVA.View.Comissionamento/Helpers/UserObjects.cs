using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class UserObjects
    {
        public static bool Exists(string udoCode)
        {
            UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var ret = udo.GetByKey(udoCode);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            return ret;
        }

        public static void Create(string xmlFilePath)
        {
            B1Connection.Instance.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            udo.Browser.ReadXml(xmlFilePath, 0);
            var udoCode = udo.Code;

            if (udo.Add() != 0)
            {
                int errCode;
                string errMsg;

                B1Connection.Instance.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

                throw new Exception($"Erro ao criar objeto definido pelo usuário: ({errCode}) {errMsg}");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            AddData(udoCode);
        }

        private static void AddData(string udoCode)
        {
            var oCompanyService = B1Connection.Instance.Company.GetCompanyService();

            if (udoCode == "UDOTIPO")
            {
                var oGeneralService = oCompanyService.GetGeneralService("UDOTIPO");
                var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "1");
                oGeneralData.SetProperty("Name", "REPRESENTANTE");
                oGeneralData.SetProperty("U_TIPO", "N");
                var oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOTIPO");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "2");
                oGeneralData.SetProperty("Name", "VENDEDOR");
                oGeneralData.SetProperty("U_TIPO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData); 
            }

            if (udoCode == "UDOCRIT")
            {
                var oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "1");
                oGeneralData.SetProperty("Name", "Vendedor (obrigatório)");
                oGeneralData.SetProperty("U_POS", "1");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                var oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "2");
                oGeneralData.SetProperty("Name", "Item");
                oGeneralData.SetProperty("U_POS", "2");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "3");
                oGeneralData.SetProperty("Name", "Grupo de itens");
                oGeneralData.SetProperty("U_POS", "3");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "4");
                oGeneralData.SetProperty("Name", "Centro de custo");
                oGeneralData.SetProperty("U_POS", "4");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "5");
                oGeneralData.SetProperty("Name", "Fabricante");
                oGeneralData.SetProperty("U_POS", "5");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "6");
                oGeneralData.SetProperty("Name", "Cliente");
                oGeneralData.SetProperty("U_POS", "6");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "7");
                oGeneralData.SetProperty("Name", "Cidade");
                oGeneralData.SetProperty("U_POS", "7");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "8");
                oGeneralData.SetProperty("Name", "Estado");
                oGeneralData.SetProperty("U_POS", "8");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "9");
                oGeneralData.SetProperty("Name", "Setor");
                oGeneralData.SetProperty("U_POS", "9");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "10");
                oGeneralData.SetProperty("Name", "Grupo de cliente");
                oGeneralData.SetProperty("U_POS", "10");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);
            }
        }

        public static void ExportXml(string udoCode)
        {
            B1Connection.Instance.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            UserObjectsMD udo = (UserObjectsMD)B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var filename = $"{AppDomain.CurrentDomain.BaseDirectory}\\{udoCode}.xml";

            if (udo.GetByKey(udoCode))
            {
                udo.SaveXML(ref filename);                
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);
        }

    }
}
