using BLL;
using DAL.Connection;
using DAL.Data;
using MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTROLLER
{
    public class CancelaDocController
    {
        public static void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent, Form form)
        {
            BubbleEvent = true;
            if (!pVal.BeforeAction)
            {
                if (pVal.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    //form.DataSources.UserDataSources.Item("ud_Order").Value = "Y";
                    //form.DataSources.UserDataSources.Item("ud_Inv").Value = "Y";
                }
                if (pVal.EventType == BoEventTypes.et_CLICK)
                {
                    if (pVal.ItemUID == "bt_Search")
                    {
                        form.Freeze(true);
                        EditText et_DtFrom = form.Items.Item("et_DtFrom").Specific as EditText;
                        EditText et_DtTo = form.Items.Item("et_DtTo").Specific as EditText;

                        string sql = Query.ImpPedido_Get;
                        UserDataSource ud_Order = form.DataSources.UserDataSources.Item("ud_Order");
                        UserDataSource ud_Inv = form.DataSources.UserDataSources.Item("ud_Inv");

                        if (ud_Order.Value == "Y" && ud_Order.Value == "Y")
                        {
                            sql += " WHERE PED1.U_STATUS IN (1, 3, 4, 6, 8) ";
                        }
                        else if (ud_Order.Value == "Y")
                        {
                            sql += " WHERE PED1.U_STATUS IN (1, 4, 6) ";
                        }
                        else if (ud_Inv.Value == "Y")
                        {
                            sql += " WHERE PED1.U_STATUS IN (3, 8) ";
                        }
                        else
                        {
                            return;
                        }
                        if (!String.IsNullOrEmpty(et_DtFrom.Value))
                        {
                            sql += $" AND U_DATA >= CAST('{et_DtFrom.Value}' AS DATE) ";
                        }
                        if (!String.IsNullOrEmpty(et_DtTo.Value))
                        {
                            sql += $" AND U_DATA <= CAST('{et_DtTo.Value}' AS DATE) ";
                        }

                        form.DataSources.DataTables.Item("dt_Doc").ExecuteQuery(sql);
                        Grid gr_doc = form.Items.Item("gr_Doc").Specific as Grid;
                        gr_doc.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;
                        gr_doc.Columns.Item("Code").Visible = false;
                        gr_doc.Columns.Item("Row").Visible = false;
                        gr_doc.Columns.Item("Base").Visible = false;
                        gr_doc.Columns.Item("Arquivo").Editable = false;
                        gr_doc.Columns.Item("Linha").Editable = false;
                        gr_doc.Columns.Item("Empresa").Editable = false;
                        gr_doc.Columns.Item("Status").Editable = false;
                        gr_doc.Columns.Item("Pedido").Editable = false;
                        gr_doc.Columns.Item("Cód. PN").Editable = false;
                        gr_doc.Columns.Item("Cód. Item").Editable = false;
                        gr_doc.Columns.Item("Qtde.").Editable = false;
                        gr_doc.Columns.Item("Valor").Editable = false;
                        gr_doc.Columns.Item("Log").Editable = false;

                        form.Items.Item("bt_Cancel").Visible = true;
                        form.Freeze(false);
                    }
                    if (pVal.ItemUID == "bt_Cancel")
                    {
                        try
                        {
                            Grid gr_doc = form.Items.Item("gr_Doc").Specific as Grid;

                            DataTable dt_Doc = form.DataSources.DataTables.Item("dt_Doc");

                            Arquivo arquivoPedido = new Arquivo();
                            arquivoPedido.LINHAS = new List<ArquivoLinha>();

                            Arquivo arquivoNF = new Arquivo();
                            arquivoNF.LINHAS = new List<ArquivoLinha>();
                            for (int i = 0; i < dt_Doc.Rows.Count; i++)
                            {
                                if (dt_Doc.GetValue("#", i).ToString() == "Y")
                                {
                                    ArquivoLinha linha = new ArquivoLinha();
                                    linha.LINHAGRID = Convert.ToInt32(dt_Doc.GetValue("Row", i));
                                    linha.CODE = dt_Doc.GetValue("Code", i).ToString();
                                    linha.BASE = dt_Doc.GetValue("Base", i).ToString();
                                    linha.LINHA = Convert.ToInt32(dt_Doc.GetValue("Linha", i));
                                    linha.NUMEROPEDIDOSAP = Convert.ToInt32(dt_Doc.GetValue("Pedido", i));
                                    string status = dt_Doc.GetValue("Status", i).ToString();
                                    if (status == "NF Gerada" || status == "Erro ao cancelar NF")
                                    {
                                        arquivoNF.LINHAS.Add(linha);
                                    }
                                    else
                                    {
                                        arquivoPedido.LINHAS.Add(linha);
                                    }
                                }
                            }

                            PedidoCompraBlo PedidoCompraBlo = new PedidoCompraBlo();
                            NotaFiscalEntradaBlo notaFiscalEntradaBlo = new NotaFiscalEntradaBlo();

                            arquivoPedido = PedidoCompraBlo.Cancel(arquivoPedido, dt_Doc, form);
                            arquivoNF = notaFiscalEntradaBlo.Cancel(arquivoPedido, dt_Doc, form);
                            if (!String.IsNullOrEmpty(arquivoPedido.MENSAGEMSTATUS))
                            {
                                ConnectionDao.Instance.Application.SetStatusBarMessage(arquivoPedido.MENSAGEMSTATUS);
                            }
                            else if (!String.IsNullOrEmpty(arquivoNF.MENSAGEMSTATUS))
                            {
                                ConnectionDao.Instance.Application.SetStatusBarMessage(arquivoPedido.MENSAGEMSTATUS);
                            }
                            else
                            {
                                form.Items.Item("bt_Cancel").Visible = false;

                                ConnectionDao.Instance.Application.MessageBox("Geração finalizada!");
                            }
                        }
                        catch (Exception ex)
                        {
                            ConnectionDao.Instance.Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            form.Freeze(false);
                        }
                    }
                }
            }
        }
    }
}
