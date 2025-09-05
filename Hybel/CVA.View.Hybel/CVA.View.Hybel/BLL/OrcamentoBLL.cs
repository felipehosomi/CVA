using ClosedXML.Excel;
using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using SAPbobsCOM;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace CVA.View.Hybel.BLL
{
    public class OrcamentoBLL
    {
        public static string GerarPlanilha(TaxaConversaoFiltroModel filtroModel)
        {
            string msg = String.Empty;
            try
            {
                if (!Directory.Exists(filtroModel.Diretorio))
                {
                    return "Diretório não encontrado!";
                }

                string currentPath = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.Combine(currentPath, "Reports", "TaxaConversaoBase.xlsx");
                var wb = new XLWorkbook(path);

                var ws = wb.Worksheet(1);
                //ws.SetShowGridLines(false);

                ws.Cell(1, "K").Value = filtroModel.DataDe.ToString("dd/MM/yyyy") + " - " + filtroModel.DataAte.ToString("dd/MM/yyyy");
                ws.Cell(2, "K").Value = filtroModel.Filial;
                ws.Cell(3, "K").Value = filtroModel.DescMotivo;

                Recordset rst = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                string sql = String.Format(SQL.RelatorioVendasCancelas, filtroModel.DataDe.ToString("yyyyMMdd"), filtroModel.DataAte.ToString("yyyyMMdd"), filtroModel.CodMotivo, filtroModel.CodFilial);
                rst.DoQuery(sql);

                int currentRow = 6;

                DataTable dtbVendas = new DataTable("dtbVendas");
                dtbVendas.Columns.Add("Filial", typeof(string));

                // Cabeçalho
                for (int i = 1; i < rst.Fields.Count; i++)
                {
                    dtbVendas.Columns.Add(rst.Fields.Item(i).Name, typeof(decimal));
                }

                DataRow rowTotal = dtbVendas.NewRow();
                for (int i = 1; i < rowTotal.ItemArray.Length; i++)
                {
                    rowTotal[i] = 0;
                }

                //Valores
                while (!rst.EoF)
                {
                    DataRow rowVendas = dtbVendas.NewRow();
                    for (int i = 0; i < rst.Fields.Count; i++)
                    {
                        rowVendas[i] = rst.Fields.Item(i).Value;
                        if (i > 0)
                        {
                            rowTotal[i] = Convert.ToDouble(rowTotal[i]) + (double)rst.Fields.Item(i).Value;
                        }
                    }

                    dtbVendas.Rows.Add(rowVendas);
                    rst.MoveNext();
                }
                dtbVendas.Rows.Add(rowTotal);
                
                ws.Cell(currentRow, "A").InsertTable(dtbVendas);
                currentRow += dtbVendas.Rows.Count;
                currentRow += 3;

                // Análises
                sql = String.Format(SQL.RelatorioTaxaConversaoVenda, filtroModel.DataDe.ToString("yyyyMMdd"), filtroModel.DataAte.ToString("yyyyMMdd"), filtroModel.CodMotivo, filtroModel.CodFilial);
                TaxaConversaoVendaModel model = new CrudController().FillModelAccordingToSql<TaxaConversaoVendaModel>(sql);

                ws.Range($"A{currentRow}:C{currentRow}").Merge();

                DataTable dtbAnalise = new DataTable("dtbAnalise");
                dtbAnalise.Columns.Add("Análise");
                dtbAnalise.Columns.Add("Quantidade", typeof(decimal));
                dtbAnalise.Columns.Add("Valor Total", typeof(decimal));

                DataRow row = dtbAnalise.NewRow();
                row["Análise"] = "Orçamentos Cancelados";
                row["Quantidade"] = model.CanceladosCount;
                row["Valor Total"] = model.Cancelados;
                dtbAnalise.Rows.Add(row);

                row = dtbAnalise.NewRow();
                row["Análise"] = "Notas Faturadas";
                row["Quantidade"] = model.FaturadosCount;
                row["Valor Total"] = model.Faturados;
                dtbAnalise.Rows.Add(row);

                row = dtbAnalise.NewRow();
                row["Análise"] = "Atrasados";
                row["Quantidade"] = model.AtrasadosCount;
                row["Valor Total"] = model.Atrasados;
                dtbAnalise.Rows.Add(row);

                row = dtbAnalise.NewRow();
                row["Análise"] = "Orçamentos Não Finalizados";
                row["Quantidade"] = model.NaoFinalizadosCount;
                row["Valor Total"] = model.NaoFinalizados;
                dtbAnalise.Rows.Add(row);

                row = dtbAnalise.NewRow();
                row["Análise"] = "Total(100 %)";
                row["Quantidade"] = model.TotalCount;
                row["Valor Total"] = model.Total;
                dtbAnalise.Rows.Add(row);

                row = dtbAnalise.NewRow();
                row["Análise"] = "% Taxa Conversão Venda";
                row["Quantidade"] = model.PorcCount;
                row["Valor Total"] = model.Porc;
                dtbAnalise.Rows.Add(row);

                ws.Cell(currentRow, "A").InsertTable(dtbAnalise);
                for (int i = currentRow; i < currentRow + dtbAnalise.Rows.Count; i++)
                {
                    ws.Cell(i, "C").Style.NumberFormat.Format = "R$ #,##0.00";
                }
                //ws.Columns().AdjustToContents();

                string fileName = Path.Combine(filtroModel.Diretorio, $"TaxaConversaoBase_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx");
                wb.SaveAs(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
    }
}
