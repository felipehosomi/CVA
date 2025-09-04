using ClosedXML.Excel;
using CVA.AddOn.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CVA.Core.ObrigacoesFiscais.BLL
{
    public class ExcelBLL
    {
        #region GenerateExcel
        public static string GenerateExcel(string diretorio, string layout, Dictionary<string, DataTable> tablesList)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layout", "DIME.xlsx");
                var wb = new XLWorkbook();
                var ws = wb.AddWorksheet(layout);

                int currentRow = 1;

                foreach (var table in tablesList)
                {
                    ws.Cell(currentRow, "A").Style.Font.Bold = true;
                    ws.Cell(currentRow, "A").Value = table.Value.TableName;
                    currentRow++;
                    ws.Cell(currentRow, "A").InsertTable(table.Value);
                    currentRow += table.Value.Rows.Count + 2;
                }

                string fileName = Path.Combine(diretorio, $"{layout}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx");
                wb.SaveAs(fileName);
                int retorno = SBOApp.Application.MessageBox("Deseja abrir o layout do arquivo gerado?", 1, "Sim", "Não");
                if (retorno == 1)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                return "Erro ao gerar layout Excel: " + ex.Message;
            }
            return String.Empty;
        }
        #endregion
    }
}
