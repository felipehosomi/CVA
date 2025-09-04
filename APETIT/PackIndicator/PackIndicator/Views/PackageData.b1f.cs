using System.Collections.Generic;
using SAPbouiCOM.Framework;
using PackIndicator.Models;
using System.Globalization;
using System.Linq;
using PackIndicator.Controllers;
using System;

namespace PackIndicator.Views
{
    [FormAttribute("PackIndicator.Views.PackageData", "Views/PackageData.b1f")]
    class PackageData : UserFormBase
    {
        private static List<PickingData> PickingData { get; set; }
        private static List<Packages> AllPackages { get; set; }
        private int RowNumber { get; set; }

        public PackageData()
        {

        }

        public PackageData(List<PickingData> pickingData, int row, List<Packages> allPackages)
        {
            var dtPacks = UIAPIRawForm.DataSources.DataTables.Item("dtPacks");

            foreach (var pack in allPackages.Where(x => x.Codigocliente.StartsWith(pickingData.FirstOrDefault().ItemCode) && x.AttributedLines.Contains(pickingData.FirstOrDefault().LineNum))
                                 .Union(allPackages.Where(x => x.Codigocliente.StartsWith(pickingData.FirstOrDefault().ItemCode) && x.AttributedQuantity < x.Volume && !x.AttributedLines.Contains(pickingData.FirstOrDefault().LineNum))))
            {
                dtPacks.Rows.Add();

                dtPacks.SetValue("Suggested", dtPacks.Rows.Count - 1, pack.AttributedLines.Contains(pickingData.FirstOrDefault().LineNum) ? "Y" : "N");
                dtPacks.SetValue("PackCode", dtPacks.Rows.Count - 1, pack.Codigocliente);
                dtPacks.SetValue("Package", dtPacks.Rows.Count - 1, pack.Embalagem);
                dtPacks.SetValue("PackName", dtPacks.Rows.Count - 1, pack.Descricao);
                dtPacks.SetValue("Factor", dtPacks.Rows.Count - 1, pack.Fatorconversao);
                dtPacks.SetValue("Volume", dtPacks.Rows.Count - 1, pack.AttributedLines.Contains(pickingData.FirstOrDefault().LineNum) ? pack.Volume : pack.Volume - pack.AttributedQuantity);
                dtPacks.SetValue("Qty", dtPacks.Rows.Count - 1, pack.AttributedLines.Contains(pickingData.FirstOrDefault().LineNum) ? pickingData.FirstOrDefault().Packages.Where(x => x.Codigocliente == pack.Codigocliente && x.Validade == pack.Validade).FirstOrDefault().Quantidade : 0);
                dtPacks.SetValue("Balance", dtPacks.Rows.Count - 1, pack.Quantidade - pack.AttributedQuantity);
                dtPacks.SetValue("DueDate", dtPacks.Rows.Count - 1, pack.Validade);
            }

            mtPacks.LoadFromDataSourceEx();
            mtPacks.AutoResizeColumns();

            PickingData = pickingData;
            AllPackages = allPackages;
            RowNumber = row;
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtPacks = ((SAPbouiCOM.Matrix)(this.GetItem("mtPacks").Specific));
            this.mtPacks.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtPacks_ValidateAfter);
            this.mtPacks.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtPacks_ValidateBefore);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.VisibleAfter += new SAPbouiCOM.Framework.FormBase.VisibleAfterHandler(this.Form_VisibleAfter);
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {

        }

        private void Form_VisibleAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            var formWidth = UIAPIRawForm.ClientWidth;
            var formHeight = UIAPIRawForm.ClientHeight;

            // Centralização do form
            UIAPIRawForm.Left = int.Parse(((Application.SBO_Application.Desktop.Width - formWidth) / 2).ToString(CultureInfo.InvariantCulture));
            UIAPIRawForm.Top = int.Parse(((Application.SBO_Application.Desktop.Height - formHeight) / 3).ToString(CultureInfo.InvariantCulture));
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtPacks.AutoResizeColumns();
        }

        private void mtPacks_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dtPacks = UIAPIRawForm.DataSources.DataTables.Item("dtPacks");
            mtPacks.FlushToDataSource();

            if (int.Parse(dtPacks.GetValue("Qty", pVal.Row - 1).ToString()) > int.Parse(dtPacks.GetValue("Volume", pVal.Row - 1).ToString()))
            {
                Application.SBO_Application.StatusBar.SetText("Quantidade indicada maior que a disponível", SAPbouiCOM.BoMessageTime.bmt_Short);
                BubbleEvent = false;
            }
        }

        private void mtPacks_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var dtPacks = UIAPIRawForm.DataSources.DataTables.Item("dtPacks");
            mtPacks.FlushToDataSource();

            if (int.Parse(dtPacks.GetValue("Qty", pVal.Row - 1).ToString()) > int.Parse(dtPacks.GetValue("Volume", pVal.Row - 1).ToString()))
            {
                return;
            }

            //dtPacks.SetValue("Balance", pVal.Row - 1, double.Parse(dtPacks.GetValue("Volume", pVal.Row - 1).ToString()) / double.Parse(dtPacks.GetValue("Factor", pVal.Row - 1).ToString()) - double.Parse(dtPacks.GetValue("Qty", pVal.Row - 1).ToString()) );
            dtPacks.SetValue("Balance", pVal.Row - 1, double.Parse(dtPacks.GetValue("Volume", pVal.Row - 1).ToString()) - double.Parse(dtPacks.GetValue("Qty", pVal.Row - 1).ToString()));

            mtPacks.LoadFromDataSourceEx();
        }

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                var dtPacks = UIAPIRawForm.DataSources.DataTables.Item("dtPacks");
                var dtDocs = CommonController.MotherForm.DataSources.DataTables.Item("dtDocs");
                var mtDocs = (SAPbouiCOM.Matrix)CommonController.MotherForm.Items.Item("mtDocs").Specific;

                mtPacks.FlushToDataSource();

                dtDocs.SetValue("SuggUom", RowNumber, String.Empty);
                dtDocs.SetValue("SuggQty", RowNumber, String.Empty);
                dtDocs.SetValue("DueDate", RowNumber, String.Empty);

                for (var i = 0; i < dtPacks.Rows.Count; i++)
                {
                    var packCode = dtPacks.GetValue("PackCode", i).ToString();
                    var dueDate = DateTime.Parse(dtPacks.GetValue("DueDate", i).ToString());

                    if (int.Parse(dtPacks.GetValue("Qty", i).ToString()) == 0)
                    {
                        if (dtPacks.GetValue("Suggested", i).ToString() == "Y")
                        {
                            var packQty = PickingData.FirstOrDefault().Packages.Where(y => y.Codigocliente.StartsWith(packCode)).FirstOrDefault().Quantidade;

                            // Atualiza a quantidade utilizada
                            AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().AttributedQuantity -= (int)packQty;
                            AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().AttributedLines.Remove(PickingData.FirstOrDefault().LineNum);

                            foreach (var pickData in PickingData)
                            {
                                pickData.Packages.RemoveAll(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate);

                                if (pickData.Packages.Count == 0)
                                {
                                    pickData.DocLineType = LineType.Remove;
                                }
                            }
                        }

                        continue;
                    }
                    else
                    {
                        if (dtPacks.GetValue("Suggested", i).ToString() == "Y")
                        {
                            PickingData.FirstOrDefault().Packages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Quantidade = int.Parse(dtPacks.GetValue("Qty", i).ToString());
                            PickingData.FirstOrDefault().Packages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Volume = int.Parse(dtPacks.GetValue("Qty", i).ToString()) * AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Fatorconversao;
                        }
                        else
                        {
                            var newPack = new Packages
                            {
                                Codigocliente = AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Codigocliente,
                                Embalagem = AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Embalagem,
                                Descricao = AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Descricao,
                                Fatorconversao = AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Fatorconversao,
                                Volume = int.Parse(dtPacks.GetValue("Qty", i).ToString()) * AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Fatorconversao,
                                Quantidade = int.Parse(dtPacks.GetValue("Qty", i).ToString()),
                                Validade = AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Validade,
                                Valor = AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().Valor,
                                UoMEntry = AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().UoMEntry
                            };

                            PickingData.FirstOrDefault().Packages.Add(newPack);
                            AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().AttributedLines.Add(PickingData.FirstOrDefault().LineNum);
                            AllPackages.Where(x => x.Codigocliente.StartsWith(packCode) && x.Validade == dueDate).FirstOrDefault().AttributedQuantity += int.Parse(dtPacks.GetValue("Qty", i).ToString());
                        }
                    }
                }

                var originalQty = double.Parse(dtDocs.GetValue("OrigQty", RowNumber).ToString(), CultureInfo.InvariantCulture);

                dtDocs.SetValue("SuggUom", RowNumber, String.Join(", ", PickingData.FirstOrDefault().Packages.Select(x => x.Embalagem).ToArray()));
                dtDocs.SetValue("SuggQty", RowNumber, String.Join(", ", PickingData.FirstOrDefault().Packages.Select(x => x.Quantidade).ToArray()));
                dtDocs.SetValue("Balance", RowNumber, PickingData.FirstOrDefault().Packages.Count(x => x.Volume > 0) == 0 ? 0 : PickingData.FirstOrDefault().Packages.Sum(x => x.Volume) - originalQty);
                dtDocs.SetValue("TotalQty", RowNumber, PickingData.FirstOrDefault().Packages.Count(x => x.Volume > 0) == 0 ? 0 : PickingData.FirstOrDefault().Packages.Sum(x => x.Volume));
                dtDocs.SetValue("DueDate", RowNumber, String.Join(", ", PickingData.FirstOrDefault().Packages.Where(x => x.Volume > 0).Select(x => x.Validade.ToString("dd/MM/yyy")).ToArray()));

                // Corrige o saldo
                PickingData.FirstOrDefault().BalanceQty = PickingData.FirstOrDefault().OriginalQty - PickingData.FirstOrDefault().Packages.Sum(x => x.Quantidade * x.Fatorconversao);

                mtDocs.LoadFromDataSourceEx();
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;

        private SAPbouiCOM.Matrix mtPacks;
    }
}
