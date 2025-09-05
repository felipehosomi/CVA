using ClosedXML.Excel;
using CVA.AddOn.Common;
using CVA.View.EDoc.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EDoc.BLL
{
    public class EDocExcelBLL
    {
        public static string GenerateExcel(EDocFilterModel filterModel, EDocModel eDocModel)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layout", "EDoc.xlsx");
                var wb = new XLWorkbook(path);
                var ws = wb.Worksheet(1);

                int currentRow = 3;

                //ws.Cell(currentRow, "B").Value = model.EmpresaModel.Inscricao;
                string fileName = Path.Combine(filterModel.Diretorio, $"EDoc{DateTime.Now.ToString("ddMMyyyy")}.xlsx");
                wb.SaveAs(fileName);
                int retorno = SBOApp.Application.MessageBox("Deseja abrir o layout do arquivo gerado?", 1, "Sim", "Não");
                if (retorno == 1)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }
    }
}
