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
    public class GeradorNFController
    {
        public static void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent, Form form)
        {
            BubbleEvent = true;
            if (!pVal.BeforeAction)
            {
                if (pVal.EventType == BoEventTypes.et_DOUBLE_CLICK && pVal.ItemUID == "gr_Doc" && pVal.ColUID == "#")
                {
                    form.Freeze(true);
                    DataTable dt_Doc = form.DataSources.DataTables.Item("dt_Doc");
                    if (dt_Doc.Rows.Count > 0)
                    {
                        string selected = dt_Doc.GetValue("#", 0).ToString() == "Y" ? "N" : "Y";

                        for (int i = 0; i < dt_Doc.Rows.Count; i++)
                        {
                            dt_Doc.SetValue("#", i, selected);
                        }
                    }
                    form.Freeze(false);
                }

                if (pVal.EventType == BoEventTypes.et_CLICK)
                {
                    if (pVal.ItemUID == "bt_Search")
                    {
                        try
                        {


                            form.Freeze(true);
                            EditText et_DtFrom = form.Items.Item("et_DtFrom").Specific as EditText;
                            EditText et_DtTo = form.Items.Item("et_DtTo").Specific as EditText;

                            EditText et_DtFromP = form.Items.Item("et_DtPCDe").Specific as EditText;
                            EditText et_DtToP = form.Items.Item("et_DtPCAte").Specific as EditText;

                            EditText et_NrPedido = form.Items.Item("et_NrPC").Specific as EditText;

                            DataTable dtBases = form.DataSources.DataTables.Item("dtBases");
                        
                            dtBases.ExecuteQuery("select distinct PED1.U_BASE from [@CVA_IMP_PED1] PED1 WITH(NOLOCK) order by 1");
                            if (dtBases.IsEmpty)
                            {
                                ConnectionDao.Instance.Application.SetStatusBarMessage("Nenhum registro encontrado");
                                return;
                            }

                            string sSqlConsulta = string.Empty;
                            for (int i = 0; i < dtBases.Rows.Count; i++)
                            {
                                if (i > 0)
                                {
                                    sSqlConsulta = sSqlConsulta + " UNION ALL ";
                                }
                                sSqlConsulta = sSqlConsulta + string.Format(Query.ImpPedido_GetByBase, dtBases.GetValue("U_BASE", i));

                                if (!String.IsNullOrEmpty(et_DtFrom.Value))
                                {
                                    sSqlConsulta = sSqlConsulta + $" AND U_DATA >= CAST('{et_DtFrom.Value}' AS DATE) ";
                                }
                                if (!String.IsNullOrEmpty(et_DtTo.Value))
                                {
                                    sSqlConsulta = sSqlConsulta + $" AND U_DATA <= CAST('{et_DtTo.Value}' AS DATE) ";
                                }
                                if (!String.IsNullOrEmpty(et_DtFromP.Value))
                                {
                                    sSqlConsulta += $" AND OPOR.TaxDate >= CAST('{et_DtFromP.Value}' AS DATE) ";
                                }
                                if (!String.IsNullOrEmpty(et_DtToP.Value))
                                {
                                    sSqlConsulta += $" AND OPOR.TaxDate <= CAST('{et_DtToP.Value}' AS DATE) ";
                                }
                                if (!String.IsNullOrEmpty(et_NrPedido.Value))
                                {
                                    sSqlConsulta += $" AND OPOR.DocNum =  " + et_NrPedido.Value;
                                }

                            }
                            sSqlConsulta = string.Format("select ROW_NUMBER() OVER(ORDER BY TB.[Linha] ASC) AS [Row],* from ({0}) as TB order by 6,10,9", sSqlConsulta);
                            //sSqlConsulta = sSqlConsulta.Replace("  ", "");
                            form.DataSources.DataTables.Item("dt_Doc").ExecuteQuery(sSqlConsulta);
                            Grid gr_doc = form.Items.Item("gr_Doc").Specific as Grid;
                            gr_doc.Columns.Item("#").Type = BoGridColumnType.gct_CheckBox;
                            gr_doc.Columns.Item("Code").Visible = false;
                            //gr_doc.Columns.Item("Code").sor

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

                            gr_doc.Columns.Item("Data Importação").Editable = false;
                            gr_doc.Columns.Item("Data Do Documento").Editable = false;
                            gr_doc.Columns.Item("Data Do Lançamento").Editable = false;

                            form.Items.Item("bt_Gen").Visible = true;

                        }
                        catch (Exception ex) {
                            ConnectionDao.Instance.Application.MessageBox(ex.Message);
                        }
                        finally
                        {
                            form.Freeze(false);
                        }
                    }
                    if (pVal.ItemUID == "bt_Gen")
                    {
                        try
                        {
                            Grid gr_doc = form.Items.Item("gr_Doc").Specific as Grid;

                            NotaFiscalEntradaBlo notaFiscalEntradaBlo = new NotaFiscalEntradaBlo();
                            DataTable dt_Doc = form.DataSources.DataTables.Item("dt_Doc");
                            
                            Arquivo arquivo = new Arquivo();
                            arquivo.LINHAS = new List<ArquivoLinha>();
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
                                    arquivo.LINHAS.Add(linha);
                                }
                            }

                            arquivo = notaFiscalEntradaBlo.Generate(arquivo, dt_Doc, form, ConnectionDao.Instance.Application);
                            if (!String.IsNullOrEmpty(arquivo.MENSAGEMSTATUS))
                            {
                                ConnectionDao.Instance.Application.SetStatusBarMessage(arquivo.MENSAGEMSTATUS);
                            }
                            else
                            {
                                form.Items.Item("bt_Gen").Visible = false;

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
