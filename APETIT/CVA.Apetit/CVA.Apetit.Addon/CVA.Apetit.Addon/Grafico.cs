using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Apetit.Addon
{
    public class Grafico
    {
        public Grafico()
        {

        }
            
        public string GerarGrafico(DateTime dataRef, int contrato, int servico, int modelo)
        {
            string arqOut = "", msg = "";

            try
            {
                arqOut = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Planilha.xlsx";
                FileInfo newFile = new FileInfo(arqOut);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(arqOut);
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    CriarPlanilha(package);
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }

        //==================================================================================================================================//
        private void CriarPlanilha(ExcelPackage package)
        //==================================================================================================================================//
        {
            int numLinha, dia;
            string aba1 = "Planilha", vigencia;
            double soma;

            try
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add(aba1);
                //FormataPlanilhaInicial(worksheet, caso, numMeses);
                numLinha = 3;
                vigencia = "202004";     

                System.Data.DataTable oDT = RetornaDtGrafico(vigencia);

                if (oDT.Rows.Count > 0)
                {
                    sheet.Cells[numLinha - 1, 1].Value = "Dia";
                    sheet.Cells[numLinha - 1, 2].Value = "Padrão";
                    sheet.Cells[numLinha - 1, 3].Value = "Planejado";

                    foreach (System.Data.DataRow linha in oDT.Rows)
                    {
                        dia = Convert.ToDateTime(linha["DataPlanej"].ToString()).Day;
                        Double.TryParse(linha["Custo"].ToString(), out soma);

                        sheet.Cells[numLinha, 1].Value = dia;
                        if (soma > 0)
                        {
                            sheet.Cells[numLinha, 2].Value = (soma - 1) > 0 ? (soma - 1) : 0;
                            sheet.Cells[numLinha, 3].Value = soma;
                        }
                        numLinha++;
                    }
                }

                //var range = sheet.Cells["A3"].LoadFromText(Utils.GetFileInfo(csvDir, "Sample9-1.txt", false), format, TableStyles.Medium27, true);
                var range = sheet.Cells["A2:C33"];
                //var range = sheet.Cells[1, 1, 1, 5]

                //var tbl = sheet.Tables[0];

                var dateStyle = package.Workbook.Styles.CreateNamedStyle("TableDate");
                dateStyle.Style.Numberformat.Format = "YYYY-MM";
                var numStyleDia = package.Workbook.Styles.CreateNamedStyle("TableDIa");
                numStyleDia.Style.Numberformat.Format = "#,##0";
                var numStyleCusto = package.Workbook.Styles.CreateNamedStyle("TableCusto");
                numStyleCusto.Style.Numberformat.Format = "#,##0.00";



                var tbl = sheet.Tables.Add(range.Offset(0, 0, range.End.Row - range.Start.Row + 1, range.End.Column - range.Start.Column + 1), "Table");

                tbl.ShowTotal = true;
                tbl.Columns[0].TotalsRowLabel = "Média";
                tbl.Columns[0].DataCellStyleName = "TableDIa";           //"TableDate";
                tbl.Columns[1].TotalsRowFunction = RowFunctions.Average;    //      RowFunctions.Sum;
                tbl.Columns[1].DataCellStyleName = "TableCusto";
                tbl.Columns[2].TotalsRowFunction = RowFunctions.Average;
                tbl.Columns[2].DataCellStyleName = "TableCusto";

                var chart = sheet.Drawings.AddChart("chart1", eChartType.ColumnClustered3D);
                chart.Title.Text = "Custo de Projeção do Cardápio para FEV/2020";
                chart.XAxis.Title.Text = "Dia";
                chart.YAxis.Title.Text = "R$";

                //Column3D -> not implemented
                chart.SetPosition(10, 330);
                chart.SetSize(800, 600);

                //Create one series for each column...
                for (int col = 1; col < 3; col++)
                {
                    var ser = chart.Series.Add(range.Offset(1, col, range.End.Row - 1, 1), range.Offset(1, 0, range.End.Row - 1, 1));
                    ser.HeaderAddress = range.Offset(0, col, 1, 1);
                }

                chart.Style = eChartStyle.Style26;      //eChartStyle.Style27;
                sheet.View.ShowGridLines = false;
                sheet.Calculate();
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();







                /*
                //Add a Line series
                var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineStacked);
                chartType2.UseSecondaryAxis = true;
                var serie3 = chartType2.Series.Add(range.Offset(1, 2, range.End.Row - 1, 1), range.Offset(1, 0, range.End.Row - 1, 1));
                serie3.Header = "Items in stock";

                //By default the secondary XAxis is not visible, but we want to show it...
                chartType2.XAxis.Deleted = false;
                chartType2.XAxis.TickLabelPosition = eTickLabelPosition.High;

                //Set the max value for the Y axis...
                chartType2.YAxis.MaxValue = 50;

                chart.Style = eChartStyle.Style26;
                sheet.View.ShowGridLines = false;
                sheet.Calculate();
                */

                /*
                ExcelPackage pck = new ExcelPackage();
                ExcelRange r1, r2;

                var sheet1 = pck.Workbook.Worksheets.Add("data_sheet");
                var sheet2 = pck.Workbook.Worksheets.Add("chart_sheet");
                var chart = (OfficeOpenXml.Drawing.Chart.ExcelBarChart)sheet2.Drawings.AddChart("some_name", OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
                chart.Legend.Position = OfficeOpenXml.Drawing.Chart.eLegendPosition.Right;
                chart.Legend.Add();
                chart.SetPosition(1, 0, 1, 0);
                chart.SetSize(600, 400);
                chart.DataLabel.ShowValue = true;

                r1 = sheet1.Cells["A3:A10"];
                r2 = sheet1.Cells["B3:B10"];
                chart.Series.Add(r2, r1);

                chart.Style = OfficeOpenXml.Drawing.Chart.eChartStyle.Style21;
                chart.Title.Text = "Some title";
                chart.XAxis.Title.Text = "X axis name";
                chart.YAxis.Title.Text = "Y axis name";
                */




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private System.Data.DataTable RetornaDtGrafico(string vigencia)
        //==================================================================================================================================//
        {
            string sql, sDataIni, sDataFim;
            DateTime dtAux;
            System.Data.DataTable oDT = null;

            try
            {
                sDataIni = vigencia + "01";
                dtAux = DateTime.ParseExact(sDataIni, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                dtAux = dtAux.AddMonths(1);
                sDataFim = dtAux.ToString("yyyyMMdd");

                sql = string.Format(@"
SELECT 
	ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
    , SUM(T1.""U_CVA_CUSTO_ITEM"") AS ""Custo""
FROM ""@CVA_PLANEJAMENTO"" T0
    INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T0.""Code"" = T1.""U_CVA_PLAN_ID""
WHERE ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{0}'
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) < '{1}'
GROUP BY ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1)
", sDataIni, sDataFim);
                oDT = Class.Conexao.ExecuteSqlDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oDT;
        }

    }
}
