using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CVA.Electra.ImportaFolha
{
    public class FileCSVService
    {
        private SAPbobsCOM.Company oCompany;
        private SAPbouiCOM.Application oApplication;


        private SAPbobsCOM.Recordset oRecordSet;
        public FileCSVService(SAPbobsCOM.Company pCompany, SAPbouiCOM.Application pApplication)
        {
            oCompany = pCompany;
            oApplication = pApplication;
        }
        public List<string> validaConta(IEnumerable<LinhaCSV> query)
        {

            oApplication.SetStatusBarMessage("Inciando a Importação do arquivo...",BoMessageTime.bmt_Short,false);

            SAPbouiCOM.ProgressBar oProgBar;
            oProgBar = oApplication.StatusBar.CreateProgressBar("Folha de Pagamento - Validando Contas Contábeis...", 27, false);

            oProgBar.Maximum = query.Count() - 1;

            var lstResult = new List<string>();
            foreach (var item in query)
            {
                oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                var sql = string.Format("select Sum(\"contador\") as contador from (select count(*) \"contador\" from \"OACT\" where \"LocManTran\" = 'N' and  \"AcctCode\" = '{0}' union all select count(*) from \"OCRD\" where \"CardCode\" = '{0}' ) TB", item.ContaContabil);
                oRecordSet.DoQuery(sql);
                //int registroConta = b1DAO.ExecuteSqlForObject<int>(string.Format(@"select Sum(contador) as contador from (select count(*) 'contador' from oact where AcctCode='{0}' union all select count(*) from OCRD where cardcode='{0}') TB", item.ContaContabil));
                int registroConta = 0;
                if (oRecordSet.RecordCount > 0)
                {
                    registroConta = oRecordSet.Fields.Item("contador").Value;

                }

                if (registroConta == 0)
                {
                    lstResult.Add(item.ContaContabil);
                }
                try
                {
                    oProgBar.Value += 1;
                }
                catch { }

            }

            //oProgBar.Stop();


            System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgBar);
            oProgBar = null;
            SetCOMObjectFree(oRecordSet);
            return lstResult;
        }

        public List<string> validaCentroCusto(IEnumerable<LinhaCSV> query)
        {

            SAPbouiCOM.ProgressBar oProgBar;
            oProgBar = oApplication.StatusBar.CreateProgressBar("Folha de Pagamento - Validando Centro de Custo...", 27, false);

            oProgBar.Maximum = query.Count();

            var lstResult = new List<string>();
            foreach (var item in query)
            {
                if (item.CentroCusto == "")
                {
                    continue;
                }
                else
                {
                    int registroCentro = 0;
                    //b1DAO.ExecuteSqlForObject<int>(string.Format("select count(*) from OOCR where OcrCode = '{0}'", item.CentroCusto));
                    oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    oRecordSet.DoQuery(string.Format("select count(*) \"count\" from \"OOCR\" where \"OcrCode\" = '{0}'", item.CentroCusto));
                    if (oRecordSet.RecordCount > 0)
                    {
                        registroCentro = oRecordSet.Fields.Item("count").Value;

                    }
                    if (registroCentro == 0)
                    {
                        lstResult.Add(item.CentroCusto);
                    }
                }
                try
                {
                    oProgBar.Value += 1;
                }
                catch { }
            }
            //oProgBar.Stop();
            //oProgBar.Stop();


            System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgBar);
            oProgBar = null;
            SetCOMObjectFree(oRecordSet);
            return lstResult;
        }

        public string geraLancamento(IEnumerable<LinhaCSV> query, SAPbouiCOM.EditText dtLanca, SAPbouiCOM.EditText dtVenc, Int32 iBPLID)
            {
            var lcto = (JournalEntries)oCompany.GetBusinessObject(BoObjectTypes.oJournalEntries);

            SAPbouiCOM.ProgressBar oProgBar;
            oProgBar = oApplication.StatusBar.CreateProgressBar("Folha de Pagamento - Gerando linhas do LCM...", 27, false);

            oProgBar.Maximum = lcto.Lines.Count;


            try
            {
                lcto.ReferenceDate = DateTime.ParseExact(dtLanca.String, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                lcto.DueDate = DateTime.ParseExact(dtVenc.String, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                lcto.TaxDate = DateTime.ParseExact(dtLanca.String, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var item = lcto.Lines;

                foreach (var linha in query)
                {

                    item.Debit = Math.Round(Convert.ToDouble(linha.ValorDebito), 2);
                    item.ShortName = linha.ContaContabil;
                    //item.AccountCode = linha.ContaContabil;
                    item.BPLID = iBPLID;
                    item.CostingCode = linha.CentroCusto;
                    item.LineMemo = linha.Observacao;
                    item.Credit = Math.Round(Convert.ToDouble(linha.ValorCredito), 2);
                    item.ProjectCode = linha.Projeto;


                    item.Add();
                    try
                    {
                        oProgBar.Value += 1;
                    }
                    catch { }
                }

                int result = lcto.Add();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgBar);
                oProgBar = null;

                if (result != 0)
                {
                    LogErrorFormat(oCompany.GetLastErrorDescription());
                    return "";
                }

                return oCompany.GetNewObjectKey().ToString();
            }
            catch (Exception ex)
            {
                LogErrorFormat(ex.Message);
                return "";
            }
            finally
            {
                lcto = null;
            }
        }

        public void SetCOMObjectFree(object objeto)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objeto);
                objeto = null;
                GC.Collect();
            }
            catch (Exception)
            {
            }
        }


        private void LogErrorFormat(string sMsg)
        {
            oApplication.SetStatusBarMessage(sMsg);
        }

        private void LogInfoFormat(string sMsg)
        {
            oApplication.SetStatusBarMessage(sMsg, BoMessageTime.bmt_Long, false);
        }
    }
}
