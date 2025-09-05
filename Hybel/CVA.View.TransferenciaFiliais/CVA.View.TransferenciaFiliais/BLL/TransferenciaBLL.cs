using CVA.AddOn.Common;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.TransferenciaFiliais.BLL
{
    public class TransferenciaBLL
    {
        public static void GeraEsbocoDevolucaoNFSaida(int idSaida)
        {
            Documents oDocDev = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oDrafts);
            Documents oDocSai = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oInvoices);
            ProgressBar pbLinhas = SBOApp.Application.StatusBar.CreateProgressBar("Aguarde... Gerando Linhas", oDocSai.Lines.Count, false);
            try
            {
                if (!oDocSai.GetByKey(idSaida))
                {
                    goto IL_0f23;
                }

                string chaveAcesso = string.Empty;
                Recordset oRsChaveNFE = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                oRsChaveNFE.DoQuery($"select U_ChaveAcesso from [@SKL25NFE] where U_DocEntry = '{oDocSai.DocEntry}' and U_inStatus = 3 and U_cdErro = 100 and U_tipoDocumento = 'NS' ");
                chaveAcesso = (string)oRsChaveNFE.Fields.Item("U_chaveacesso").Value;

                int num = oDocSai.SequenceSerial;
                Recordset oRs = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                oRs.DoQuery(string.Format(@"select top 1 * from (select 'Tipo' = 'E', DocEntry from ODRF where  u_cva_nf_custo = '{0}' and BPLId = {1} and U_chaveacesso='{2}'
                                            union all
                                            select 'Tipo' = 'D', DocEntry from ORIN where u_cva_nf_custo = '{0}' and BPLId = {1} and U_chaveacesso='{2}') as dados
                                            order by Tipo, DocEntry desc", num.ToString(), oDocSai.BPL_IDAssignedToInvoice, chaveAcesso.Trim()));
                if (oRs.RecordCount <= 0)
                {
                    Recordset oRsPn = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                    oRsPn.DoQuery($"select top 1 CardCode from CRD7 where TaxId0 = (select TaxIdNum from obpl where BPLId = '{oDocSai.BPL_IDAssignedToInvoice}') and CardCode like 'C%'");

                    Recordset oRsFilial = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                    oRsFilial.DoQuery($"select top 1 BPLId from OBPL where County = (select County from INV12 where DocEntry = '{oDocSai.DocEntry}') ");

                    // Campo chave de acrsso NFE
                    oDocDev.CardCode = (string)oRsPn.Fields.Item("CardCode").Value; 
                    oDocDev.UserFields.Fields.Item("U_CVA_NF_CUSTO").Value = oDocSai.SequenceSerial.ToString();
                    oDocDev.UserFields.Fields.Item("U_chaveacesso").Value = chaveAcesso.Trim();
                    oDocDev.BPL_IDAssignedToInvoice = (int)oRsFilial.Fields.Item("BPLId").Value;
                    oDocDev.DocDueDate = DateTime.Now;
                    oDocDev.TaxDate = oDocSai.TaxDate;
                    oDocDev.DocDate = DateTime.Now;
                    oDocDev.SequenceCode = -2;
                    oDocDev.SequenceSerial = num;
                    oDocDev.SeriesString = oDocSai.SeriesString;
                    oDocDev.SequenceModel = oDocSai.SequenceModel;
                    oDocDev.SalesPersonCode = oDocSai.SalesPersonCode;
                    oDocDev.DocObjectCode = BoObjectTypes.oCreditNotes;
                 
                    for (int k = 0; k < oDocSai.Lines.Count; k++)
                    {
                        num = pbLinhas.Value++;
                        oDocSai.Lines.SetCurrentLine(k);
                        if (k > 0)
                        {
                            oDocDev.Lines.Add();
                        }
                        oDocDev.Lines.ItemCode = oDocSai.Lines.ItemCode;
                        oDocDev.Lines.Quantity = oDocSai.Lines.Quantity;
                        oDocDev.Lines.Price = oDocSai.Lines.Price;
                        oDocDev.Lines.PriceAfterVAT = oDocSai.Lines.PriceAfterVAT;
                        oDocDev.Lines.UnitPrice = oDocSai.Lines.UnitPrice;
                        oDocDev.Lines.EnableReturnCost = BoYesNoEnum.tYES;
                        Recordset oRsRc = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                        oRsRc.DoQuery($"select top 1 StockPrice from INV1 where DocEntry = '{oDocSai.DocEntry}' and LineNum = '{oDocSai.Lines.LineNum}'");
                        oDocDev.Lines.ReturnCost = (double)oRsRc.Fields.Item("StockPrice").Value;

                        oDocDev.Lines.DiscountPercent = oDocSai.Lines.DiscountPercent;
                        oDocDev.Lines.Usage = "5";
                        for (int j = 0; j < oDocSai.Lines.SerialNumbers.Count; j++)
                        {
                            oDocSai.Lines.SerialNumbers.SetCurrentLine(j);
                            if (!string.IsNullOrEmpty(oDocSai.Lines.SerialNumbers.InternalSerialNumber))
                            {
                                if (j > 0)
                                {
                                    oDocDev.Lines.SerialNumbers.Add();
                                }
                                oDocDev.Lines.SerialNumbers.InternalSerialNumber = oDocSai.Lines.SerialNumbers.InternalSerialNumber;
                                oDocDev.Lines.SerialNumbers.ManufacturerSerialNumber = oDocSai.Lines.SerialNumbers.ManufacturerSerialNumber;
                            }
                        }
                        for (int i = 0; i < oDocSai.Lines.BatchNumbers.Count; i++)
                        {
                            oDocSai.Lines.BatchNumbers.SetCurrentLine(i);
                            if (!string.IsNullOrEmpty(oDocSai.Lines.BatchNumbers.BatchNumber))
                            {
                                if (i > 0)
                                {
                                    oDocDev.Lines.BatchNumbers.Add();
                                }
                                oDocDev.Lines.BatchNumbers.BatchNumber = oDocSai.Lines.BatchNumbers.BatchNumber;
                                oDocDev.Lines.BatchNumbers.InternalSerialNumber = oDocSai.Lines.BatchNumbers.InternalSerialNumber;
                                oDocDev.Lines.BatchNumbers.Quantity = oDocSai.Lines.BatchNumbers.Quantity;
                            }
                        }
                    }
                    goto IL_0f23;
                }
                try
                {
                    pbLinhas.Stop();
                }
                catch
                {
                }
                if ((dynamic)oRs.Fields.Item("Tipo").Value == "E")
                {
                    Form esb = (Form)SBOApp.Application.OpenForm((BoFormObjectEnum)112, "", (dynamic)oRs.Fields.Item("DocEntry").Value);
                    esb.Select();
                }
                else if ((dynamic)oRs.Fields.Item("Tipo").Value == "D")
                {
                    Form esb2 = (Form)SBOApp.Application.OpenForm(BoFormObjectEnum.fo_InvoiceCreditMemo, "", (dynamic)oRs.Fields.Item("DocEntry").Value);
                    esb2.Select();
                }
                goto end_IL_00be;
                IL_0f23:
                try
                {
                    pbLinhas.Stop();
                }
                catch
                {
                }
                string errMsg = "";
                int error = oDocDev.Add();
                if (error != 0)
                {
                    SBOApp.Company.GetLastError(out error, out errMsg);
                    SBOApp.Application.MessageBox(error.ToString() + " :: " + errMsg, 1, "Ok", "", "");
                }
                else
                {
                    SBOApp.Application.MessageBox("Esboço devolução criado com sucesso!", 1, "Ok", "", "");
                    SBOApp.Application.OpenForm((BoFormObjectEnum)112, "", SBOApp.Company.GetNewObjectKey());
                }
                end_IL_00be:;
            }
            catch (Exception e)
            {
                try
                {
                    pbLinhas.Stop();
                }
                catch
                {
                }
                SBOApp.Application.MessageBox(e.Message, 1, "Ok", "", "");
            }
            finally
            {
                Marshal.ReleaseComObject(pbLinhas);
                pbLinhas = null;
            }
        }

        public static void GeraDevolucaoNFSaida(int idSaida, Form formDev, Form formSaida)
        {
            try
            {
                formDev.Freeze(true);
                ((EditText)formDev.Items.Item("4").Specific).Value = ((EditText)formSaida.Items.Item("4").Specific).Value;
                ((EditText)formDev.Items.Item("U_CVA_NF_CUSTO").Specific).Value = ((EditText)formSaida.Items.Item("2036").Specific).Value;
                DBDataSource oDbLinhaDev = formDev.DataSources.DBDataSources.Item("RIN1");
                formDev.Items.Item("14").Click(BoCellClickType.ct_Regular);
                Matrix oMatrix = (Matrix)formDev.Items.Item("38").Specific;
                DBDataSource oDbLinhaSaida = formSaida.DataSources.DBDataSources.Item("INV1");
                ProgressBar pbLinhas = SBOApp.Application.StatusBar.CreateProgressBar("Aguarde... Gerando Linhas", oDbLinhaSaida.Size, false);
                for (int i = 0; i < oDbLinhaSaida.Size; i++)
                {
                    try
                    {
                        pbLinhas.Value++;
                    }
                    catch
                    {
                    }
                    string codigoItem = oDbLinhaSaida.GetValue("ItemCode", i).Trim();
                    string quantidade = oDbLinhaSaida.GetValue("Quantity", i);
                    string preco = oDbLinhaSaida.GetValue("Price", i);
                    ((EditText)oMatrix.Columns.Item("1").Cells.Item(i + 1).Specific).Value = codigoItem;
                    EditText obj2 = (EditText)oMatrix.Columns.Item("11").Cells.Item(i + 1).Specific;
                    double num = Convert.ToDouble(quantidade.Replace(".", ","));
                    obj2.Value = num.ToString();
                    EditText obj3 = (EditText)oMatrix.Columns.Item("14").Cells.Item(i + 1).Specific;
                    num = Convert.ToDouble(preco.Replace(".", ","));
                    obj3.Value = num.ToString("#0.00");
                    ((CheckBox)oMatrix.Columns.Item("1470002169").Cells.Item(i + 1).Specific).Checked = true;
                    ((EditText)oMatrix.Columns.Item("1470002171").Cells.Item(i + 1).Specific).Value = oDbLinhaSaida.GetValue("StockPrice", i);
                }
                try
                {
                    pbLinhas.Stop();
                }
                catch
                {
                }
                formDev.Items.Item("2013").Click(BoCellClickType.ct_Regular);
                ((EditText)formDev.Items.Item("2021").Specific).Value = "NF n°" + idSaida.ToString();
                formDev.Items.Item("498").Click(BoCellClickType.ct_Regular);
                formDev.Items.Item("3").Click(BoCellClickType.ct_Regular);
                Form oFormRef = SBOApp.Application.Forms.ActiveForm;
                oFormRef.Freeze(true);
                Matrix oMatrixRef = (Matrix)oFormRef.Items.Item("5").Specific;
                ((ComboBox)oMatrixRef.Columns.Item("1").Cells.Item(1).Specific).Select("13", BoSearchKey.psk_ByValue);
                ((EditText)oMatrixRef.Columns.Item("3").Cells.Item(1).Specific).Value = idSaida.ToString();
                oFormRef.Freeze(false);
                oFormRef.Items.Item("540020001").Click(BoCellClickType.ct_Regular);
                formDev.Freeze(false);
            }
            catch (Exception e)
            {
                SBOApp.Application.MessageBox(e.Message, 1, "Ok", "", "");
            }
        }

    }
}
