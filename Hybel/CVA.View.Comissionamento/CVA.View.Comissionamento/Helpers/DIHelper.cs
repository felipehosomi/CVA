using System;
using System.Text;
using SAPbobsCOM;

namespace CVA.View.Comissionamento.Helpers
{
    public class DIHelper
    {
        private readonly SapFactory _factory;

        public DIHelper(SapFactory factory)
        {
            _factory = factory;
        }

        public string Format_MoneyToString(double valor)
        {
            var oSboBob = (SBObob)_factory.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_MoneyToString(valor, BoMoneyPrecisionTypes.mpt_Sum);
            return oRecordset.Fields.Item(0).Value;
        }

        public string Format_DateToString(DateTime valor)
        {
            var oSboBob = (SBObob)_factory.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset = oSboBob.Format_DateToString(valor);
            return oRecordset.Fields.Item(0).Value;
        }

        public DateTime Format_StringToDate(string valor)
        {
            var oSboBob = (SBObob)_factory.Company.GetBusinessObject(BoObjectTypes.BoBridge);
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            var sValor = string.Empty;

            if (valor.Contains(" 00:00:00"))
                sValor = valor.Length > 10 ? valor.Substring(0, 10) : valor;
            else
                sValor = valor;

            oRecordset = oSboBob.Format_StringToDate(sValor);
            return Convert.ToDateTime(oRecordset.Fields.Item(0).Value);
        }

        public double Format_StringToDouble(string s)
        {
            double d = 0;
            try
            {
                d = Convert.ToDouble(s);
                d = Math.Round(d, 6);
                return d;
            }
            catch
            {
            }
            try
            {
                var nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                var oCompanyService = _factory.Company.GetCompanyService();
                var oAdminInfo = oCompanyService.GetAdminInfo();
                var sbodsep = oAdminInfo.DecimalSeparator;
                var sbotsep = oAdminInfo.ThousandsSeparator;
                if (s.IndexOf("".PadLeft(1)) > 0)
                {
                    s = oAdminInfo.DisplayCurrencyontheRight == BoYesNoEnum.tYES
                        ? s.Substring(0, s.IndexOf("".PadLeft(1)))
                        : s.Substring(s.IndexOf("".PadLeft(1)), s.Length - s.IndexOf("".PadLeft(1)));
                }
                var s1 = s.Replace(sbotsep, nfi.NumberGroupSeparator);

                s1 = s1.Replace(sbodsep, nfi.NumberDecimalSeparator);
                d = Convert.ToDouble(s);
                d = Math.Round(d, 6);
                return d;
            }
            catch
            {
                return 0;
            }
        }

        public int GetNextCode(string tableName)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT COALESCE(MAX(CAST(Code AS INT)), 0) + 1 FROM [@{tableName}]");

            var retorno = (int)oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public int GetNextDocEntry(string tableName)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT COALESCE(MAX(DocEntry), 0) + 1 FROM [@{tableName}]");

            var retorno = (int)oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public int GetCalcCode(string docEntry, string objType, string lineNum, string instId)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT DISTINCT Code FROM [@CVA_CALC_COMISSAO] WHERE U_DOCENTRY = {docEntry} AND U_OBJTYPE = {objType} AND U_PARCELA = {instId} AND U_LINENUM = {lineNum}");

            var retorno = (int)oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public string GetCardName(string cardCode)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT CardName FROM OCRD WHERE CardCode = '{cardCode}'");

            var retorno = oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public string GetGroupName(string groupCode)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT GroupName FROM OCRG WHERE GroupCode = {groupCode}");

            var retorno = oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public string GetItemName(string itemCode)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT ItemName FROM OITM WHERE ItemCode = '{itemCode}'");

            var retorno = oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public string GetCounty(string countyCode)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT Name FROM OCNT WHERE AbsId = {countyCode}");

            var retorno = oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public string GetItmsGrpNam(string groupCode)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT ItmsGrpNam FROM OITB WHERE ItmsGrpCod = {groupCode}");

            var retorno = oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public string GetPrcName(string prcCode)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT PrcName FROM OPRC WHERE PrcCode = '{prcCode}'");

            var retorno = oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public string GetSlpName(string slpCode)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT SlpName FROM OSLP WHERE SlpCode = {slpCode}");

            var retorno = oRecordset.Fields.Item(0).Value;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);

            return retorno;
        }

        public int GetCriteria(int vendedor = 0, int grupoItens = 0, string cliente = "", string item = "", string centroCusto = "", string estado = "", int cidade = 0, int fabricante = 0, int setor = 0, int grupoCliente = 0)
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT [dbo].[fn_CVA_PositionCriteria]({vendedor}, {grupoItens}, '{cliente}', '{item}', '{centroCusto}', '{estado}', {cidade}, {fabricante}, {setor}, {grupoCliente})");
            var lRet = oRecordset.Fields.Item(0).Value.ToString();
            return int.Parse(lRet) == 0 ? 1 : int.Parse(lRet);
        }

        public void AtualizaRegras()
        {
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var builder = new StringBuilder();
            builder.AppendLine("SELECT Code, ISNULL(U_COMISSIONADO, 0) AS U_COMISSIONADO, ");
            builder.AppendLine("ISNULL(U_VENDEDOR, 0) AS U_VENDEDOR, ");
            builder.AppendLine("ISNULL(U_GRUPO, 0) AS U_GRUPO, ");
            builder.AppendLine("ISNULL(U_CLIENTE, '') AS U_CLIENTE, ");
            builder.AppendLine("ISNULL(U_ITEM, '') AS U_ITEM, ");
            builder.AppendLine("ISNULL(U_CENTROCUSTO, '') AS U_CENTROCUSTO, ");
            builder.AppendLine("ISNULL(U_ESTADO, '') AS U_ESTADO, ");
            builder.AppendLine("ISNULL(U_CIDADE, 0) AS U_CIDADE, ");
            builder.AppendLine("ISNULL(U_FABRICANTE, 0) AS U_FABRICANTE, ");
            builder.AppendLine("ISNULL(U_SETOR, 0) AS U_SETOR, ");
            builder.AppendLine("ISNULL(U_GROUP, 0) AS U_GROUP, ");
            builder.AppendLine("ISNULL(U_PRIORIDADE, 0) AS U_PRIORIDADE, ");
            builder.AppendLine("ISNULL(U_COMNAME, '') AS U_COMNAME, ");
            builder.AppendLine("ISNULL(U_VNDNAME, '') AS U_VNDNAME, ");
            builder.AppendLine("ISNULL(U_ITEMNAME, '') AS U_ITEMNAME, ");
            builder.AppendLine("ISNULL(U_ITMSGRPNAM, '') AS U_ITMSGRPNAM, ");
            builder.AppendLine("ISNULL(U_PRCNAME, '') AS U_PRCNAME, ");
            builder.AppendLine("ISNULL(U_CARDNAME, '') AS U_CARDNAME, ");
            builder.AppendLine("ISNULL(U_COUNTY, '') AS U_COUNTY, ");
            builder.AppendLine("ISNULL(U_GROUPNAME, '') AS U_GROUPNAME ");
            builder.AppendLine("FROM [@CVA_REGR_COMISSAO]");
            oRecordset.DoQuery(builder.ToString());

            var oProgress = _factory.Application.StatusBar.CreateProgressBar($"Total de {oRecordset.RecordCount} regras para atualizar", oRecordset.RecordCount, true);

            try
            {
                while (!oRecordset.EoF)
                {
                    string code = oRecordset.Fields.Item("Code").Value.ToString();
                    int comissionado = int.Parse(oRecordset.Fields.Item("U_COMISSIONADO").Value.ToString());
                    int vendedor = int.Parse(oRecordset.Fields.Item("U_VENDEDOR").Value.ToString());
                    int grupoItem = int.Parse(oRecordset.Fields.Item("U_GRUPO").Value.ToString());
                    string cliente = oRecordset.Fields.Item("U_CLIENTE").Value.ToString();
                    string item = oRecordset.Fields.Item("U_ITEM").Value.ToString();
                    string centroCusto = oRecordset.Fields.Item("U_CENTROCUSTO").Value.ToString();
                    string estado = oRecordset.Fields.Item("U_ESTADO").Value.ToString();
                    int cidade = int.Parse(oRecordset.Fields.Item("U_CIDADE").Value.ToString());
                    int fabricante = int.Parse(oRecordset.Fields.Item("U_FABRICANTE").Value.ToString());
                    int setor = int.Parse(oRecordset.Fields.Item("U_SETOR").Value.ToString());
                    int grupoCliente = int.Parse(oRecordset.Fields.Item("U_GROUP").Value.ToString());
                    int prioridade = int.Parse(oRecordset.Fields.Item("U_PRIORIDADE").Value.ToString());
                    string nomeComissionado = oRecordset.Fields.Item("U_COMNAME").Value.ToString();
                    string nomeVendedor = oRecordset.Fields.Item("U_VNDNAME").Value.ToString();
                    string nomeItem = oRecordset.Fields.Item("U_ITEMNAME").Value.ToString();
                    string nomeGrupoItem = oRecordset.Fields.Item("U_ITMSGRPNAM").Value.ToString();
                    string nomeCentroCusto = oRecordset.Fields.Item("U_PRCNAME").Value.ToString();
                    string nomeCliente = oRecordset.Fields.Item("U_CARDNAME").Value.ToString();
                    string nomeCidade = oRecordset.Fields.Item("U_COUNTY").Value.ToString();
                    string nomeGrupoCliente = oRecordset.Fields.Item("U_GROUPNAME").Value.ToString();

                    int novaPrioridade = GetCriteria(comissionado, grupoItem, cliente, item, centroCusto, estado, cidade, fabricante, setor, grupoCliente);

                    if (novaPrioridade != prioridade)
                        AtualizaPrioridade(code, novaPrioridade);

                    if (comissionado != 0 && nomeComissionado == "")
                        AtualizaNomes(Nomes.Comissionado, code, comissionado);

                    if (vendedor != 0 && nomeVendedor == "")
                        AtualizaNomes(Nomes.Vendedor, code, vendedor);

                    if (item != "" && nomeItem == "")
                        AtualizaNomes(Nomes.Item, code, item);

                    if (grupoItem != 0 && nomeGrupoItem == "")
                        AtualizaNomes(Nomes.GrupoItem, code, grupoItem);

                    if (centroCusto != "" && nomeCentroCusto == "")
                        AtualizaNomes(Nomes.CentroCusto, code, centroCusto);

                    if (cliente != "" && nomeCliente == "")
                        AtualizaNomes(Nomes.Cliente, code, cliente);

                    if (cidade != 0 && nomeCidade == "")
                        AtualizaNomes(Nomes.Cidade, code, cidade);

                    if (grupoCliente != 0 && nomeGrupoCliente == "")
                        AtualizaNomes(Nomes.GrupoCliente, code, grupoCliente);

                    oProgress.Value++;
                    oRecordset.MoveNext();
                }

                try
                {
                    oProgress.Stop();
                    oProgress = null;
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                oProgress.Stop();
                oProgress = null;
                _factory.Application.SetStatusBarMessage(ex.Message);
                throw;
            }
        }

        private void AtualizaNomes(Nomes nome, string code, object value)
        {
            var oCompanyService = _factory.Company.GetCompanyService();
            var oGeneralService = oCompanyService.GetGeneralService("UDOREGR");
            var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

            oGeneralParams.SetProperty("Code", code);
            var oGeneralData = oGeneralService.GetByParams(oGeneralParams);

            switch (nome)
            {
                case Nomes.Comissionado:
                    oGeneralData.SetProperty("U_COMNAME", GetSlpName(value.ToString()));
                    break;
                case Nomes.Vendedor:
                    oGeneralData.SetProperty("U_VNDNAME", GetSlpName(value.ToString()));
                    break;
                case Nomes.Item:
                    oGeneralData.SetProperty("U_ITEMNAME", GetItemName(value.ToString()));
                    break;
                case Nomes.GrupoItem:
                    oGeneralData.SetProperty("U_ITMSGRPNAM", GetItmsGrpNam(value.ToString()));
                    break;
                case Nomes.Cidade:
                    oGeneralData.SetProperty("U_COUNTY", GetCounty(value.ToString()));
                    break;
                case Nomes.GrupoCliente:
                    oGeneralData.SetProperty("U_GROUPNAME", GetGroupName(value.ToString()));
                    break;
                case Nomes.Cliente:
                    oGeneralData.SetProperty("U_CARDNAME", GetCardName(value.ToString()));
                    break;
                case Nomes.CentroCusto:
                    oGeneralData.SetProperty("U_PRCNAME", GetPrcName(value.ToString()));
                    break;
            }

            oGeneralService.Update(oGeneralData);
        }

        private void AtualizaPrioridade(string code, int novaPrioridade)
        {
            var oCompanyService = _factory.Company.GetCompanyService();
            var oGeneralService = oCompanyService.GetGeneralService("UDOREGR");
            var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

            oGeneralParams.SetProperty("Code", code);
            var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
            oGeneralData.SetProperty("U_PRIORIDADE", novaPrioridade);
            oGeneralService.Update(oGeneralData);
        }

        private enum Nomes
        {
            Comissionado,
            Vendedor,
            Item,
            GrupoItem,
            Cidade,
            GrupoCliente,
            Cliente,
            CentroCusto
        }
    }
}
