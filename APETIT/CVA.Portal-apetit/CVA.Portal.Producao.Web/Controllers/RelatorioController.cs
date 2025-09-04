using ClosedXML.Excel;
using ClosedXML.Extensions;
using CVA.Portal.Producao.Model.Configuracoes;
using CVA.Portal.Producao.Model.Qualidade;
using CVA.Portal.Producao.Web.Helper;
using CVA.Portal.Producao.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Controllers
{
    [CvaAuthorize("Relatorio")]
    public class RelatorioController : Controller
    {
        APICallUtil api = new APICallUtil();

        [HttpGet]
        public ActionResult InspecaoMP()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> InspecaoMP(DateTime? startDate, DateTime? endDate, int? nf = null)
        {
            var model = await api.GetListAsync<FichaInspecaoModel>("FichaInspecao", $"startDate={startDate}&endDate={endDate}&nf={nf}&codEtapa=recebimento de matéria-prima&tipoDoc=RE");
            Session["InspecaoReportResult"] = model;
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> InspecaoOP()
        {
            var etapaList = await api.GetListAsync<UsuarioEtapaModel>("UsuarioEtapa", $"codUsuario={CvaSession.Usuario.Code}");
            etapaList = etapaList.Where(s => s.SelectedInt == 1).ToList();
            ViewBag.EtapaSelectList = new SelectList(etapaList, "CodEtapa", "CodEtapa");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> InspecaoOP(DateTime? startDate, DateTime? endDate, string codEtapa = null)
        {
            var etapaList = await api.GetListAsync<UsuarioEtapaModel>("UsuarioEtapa", $"codUsuario={CvaSession.Usuario.Code}");
            etapaList = etapaList.Where(s => s.SelectedInt == 1).ToList();
            ViewBag.EtapaSelectList = new SelectList(etapaList, "CodEtapa", "CodEtapa");

            var model = await api.GetListAsync<FichaInspecaoModel>("FichaInspecao", $"startDate={startDate}&endDate={endDate}&codEtapa={codEtapa.ToLower()}&tipoDoc=OP");
            Session["InspecaoReportResult"] = model;
            return View(model);
        }

        public ActionResult DownloadPlanilha(string tipoInspec, DateTime? startDate, DateTime? endDate, string codEtapa = null)
        {
            var model = Session["InspecaoReportResult"] as List<FichaInspecaoModel>;

            if (model != null)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Inspeções");
                    worksheet.ShowGridLines = false;

                    worksheet.Cell("A1").Value = "TOTAL DE INSPEÇÕES";
                    worksheet.Cell("A2").Value = "TOTAL DE ITENS APROVADOS";
                    worksheet.Cell("A3").Value = "TOTAL DE ITENS REPROVADOS";
                    worksheet.Cell("A4").Value = "EFICIÊNCIA";
                    worksheet.Range("A1:A4").Style.Font.Bold = true;
                    worksheet.Cell("B1").Value = Session["InspecaoTotal"];
                    worksheet.Cell("B2").Value = Session["InspecaoTotalAprovado"];
                    worksheet.Cell("B3").Value = Session["InspecaoTotalReprovado"];
                    worksheet.Cell("B4").Value = Session["InspecaoEficiencia"];
                    worksheet.Cell("B4").Style.NumberFormat.NumberFormatId = 10;

                    int line = 6;

                    foreach (var inspecao in model)
                    {
                        worksheet.Cell($"A{line}").Value = tipoInspec == "MP" ? "Nota Fiscal" : "Nr. OP";
                        worksheet.Cell($"B{line}").Value = tipoInspec == "MP" ? inspecao.NrNF : inspecao.DocNum;
                        worksheet.Cell($"B{line}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        line++;
                        worksheet.Cell($"A{line}").Value = "Item";
                        worksheet.Cell($"B{line}").Value = inspecao.Item;
                        worksheet.Range($"B{line}:K{line}").Merge(false);
                        worksheet.Range($"A{line - 1}:K{line}").Style.Fill.BackgroundColor = XLColor.FromHtml(inspecao.StatusLote == "A" ? "#dff0d8" : "#f2dede");
                        line++;
                        int tableStartLine = line;
                        worksheet.Cell($"A{line}").Value = "ID";
                        worksheet.Cell($"B{line}").Value = "Especificação";
                        worksheet.Cell($"C{line}").Value = "Vlr.Nominal";
                        worksheet.Cell($"D{line}").Value = "Padrão de";
                        worksheet.Cell($"E{line}").Value = "Padrão até";
                        worksheet.Cell($"F{line}").Value = "Análise";
                        worksheet.Cell($"G{line}").Value = "Observação";
                        worksheet.Cell($"H{line}").Value = "Método";
                        worksheet.Cell($"I{line}").Value = "Amostragem";
                        worksheet.Cell($"J{line}").Value = "Máquina";
                        worksheet.Cell($"K{line}").Value = "Resultado";
                        worksheet.Range($"A{line - 2}:K{line}").Style.Font.Bold = true;

                        if (inspecao.ItemList != null)
                        {
                            foreach (var item in inspecao.ItemList)
                            {
                                line++;
                                worksheet.Cell($"A{line}").Value = item.ID;
                                worksheet.Cell($"B{line}").Value = item.DescEspec;
                                worksheet.Cell($"C{line}").Value = item.VlrNominalStr;
                                worksheet.Cell($"D{line}").Value = item.PadraoDeStr;
                                worksheet.Cell($"E{line}").Value = item.PadraoAteStr;
                                worksheet.Cell($"F{line}").Value = item.Analise;
                                worksheet.Cell($"G{line}").Value = item.Observacao;
                                worksheet.Cell($"H{line}").Value = item.Metodo;
                                worksheet.Cell($"I{line}").Value = item.Amostragem;
                                worksheet.Cell($"J{line}").Value = item.CodRecurso;
                                worksheet.Cell($"K{line}").Value = item.Resultado;
                            }

                            var excelTable = worksheet.Range($"A{tableStartLine}:K{tableStartLine + inspecao.ItemList.Count}").CreateTable();
                            worksheet.Range($"A{tableStartLine - 2}:K{tableStartLine + inspecao.ItemList.Count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                        line = line + 2;
                    }

                    worksheet.Columns().AdjustToContents();
                    return workbook.Deliver("RelatorioDeInspecao.xlsx");
                }
            }

            return null;
        }
    }
}