using System;
using System.Collections.Generic;
using System.Linq;
using PackIndicator.Controllers;
using PackIndicator.Models;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using SAPbobsCOM;
using PackIndicator.DAO;
using System.Runtime.InteropServices;
using System.Globalization;

namespace PackIndicator.Views
{
    [FormAttribute("PackIndicator.Views.PickingIndicator", "Views/PickingIndicator.b1f")]
    class PickingIndicator : UserFormBase
    {
        private List<PickingData> PickingData { get; set; }
        private List<Packages> Packages { get; set; }
        private static int _menuRow;

        public static IForm MotherForm { get; set; }

        public PickingIndicator()
        {
            SetPickingSuggestions();
        }

        public PickingIndicator(List<PickingData> pickingData, IForm motherForm)
        {
            MotherForm = motherForm;
            SetPickingSuggestions(pickingData);
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtDocs = ((SAPbouiCOM.Matrix)(this.GetItem("mtDocs").Specific));
            this.mtDocs.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtDocs_LinkPressedBefore);
            this.btApply = ((SAPbouiCOM.Button)(this.GetItem("btApply").Specific));
            this.btApply.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btApply_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);
            this.RightClickBefore += new RightClickBeforeHandler(this.Form_RightClickBefore);
            this.RightClickAfter += new RightClickAfterHandler(this.Form_RightClickAfter);

        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += MenuEvent;
        }

        #region MenuEvents
        private void MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.MenuUID != "RemoveSuggestion" && pVal.MenuUID != "SetSuggestion") return;
            if (UIAPIRawForm == null) return;
            if (UIAPIRawForm.UniqueID != SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.UniqueID) return;

            if (!pVal.BeforeAction)
            {
                switch (pVal.MenuUID)
                {
                    case "RemoveSuggestion":
                        RemoveSuggestion();
                        break;

                    case "SetSuggestion":
                        SetLineSuggestion();
                        break;
                }
            }
        }
        #endregion

        private void mtDocs_LinkPressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            switch (pVal.ColUID)
            {
                case "DocNum":
                    BubbleEvent = false;

                    try
                    {
                        var dtDocs = UIAPIRawForm.DataSources.DataTables.Item("dtDocs");
                        SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(BoFormObjectEnum.fo_Order, "", dtDocs.GetValue("DocEntry", pVal.Row - 1).ToString());
                    }
                    catch (Exception ex)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
                    }
                    break;

                case "SuggUom":
                    BubbleEvent = false;

                    try
                    {
                        CommonController.MotherForm = UIAPIRawForm;

                        var packageData = new PackageData(PickingData.Where(x => x.VisOrder == pVal.Row).ToList(), pVal.Row - 1, Packages);
                        packageData.Show();
                    }
                    catch (Exception ex)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
                    }
                    break;
            }
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtDocs.AutoResizeColumns();
        }

        private void btApply_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Aplicando sugestões nos pedidos de venda.", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

            GC.Collect();

            PickingData.RemoveAll(x => x.DocLineType == LineType.Remove);

            var counter = 1;
            var total = PickingData.Where(x => x.Packages.Count > 0).OrderBy(x => x.VisOrder).Select(x => x.DocEntry).Distinct().ToList().Count;
            var progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", total, false);

            //// Se está em uma trasação
            //if (CommonController.Company.InTransaction)
            //{
            //    // Então
            //    // Finaliza a transação realizando um rollback para uma nova ser aberta
            //    CommonController.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            //}
            //// Abre uma nova transação
            //CommonController.Company.StartTransaction();

            // TODO: incrementar a quantidade de saldo caso esteja incorreto




            try
            {
                foreach (var document in PickingData.Where(x => x.Packages.Count > 0).OrderBy(x => x.VisOrder).Select(x => x.DocEntry).Distinct())
                {
                    progessBar.Text = $"Aplicando sugestão de embalagem {counter}/{total}.";
                    progessBar.Value += 1;
                    counter++;

                    try
                    {
                        PickingController.UpdateDocument(PickingData.Where(x => x.DocEntry == document && (x.Packages.Count > 0 || x.DocLineType == LineType.Balance)).ToList());
                    }
                    catch (Exception ex)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
                        return;
                    }
                }

                if (UIAPIRawForm.DataSources.UserDataSources.Item("NewWay").Value == "N")
                {
                    CommonController.InProcess = true;
                    CommonController.TransitionObject = PickingData;
                }
                else
                {
                    PickingController.SetPickList(PickingData.Where(x => x.Packages.Count > 0).ToList());
                }

                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Sugestões aplicadas nos pedidos de venda com sucesso.", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                UIAPIRawForm.Close();

                if (MotherForm != null) MotherForm.Close();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);

                //// Se está em uma trasação
                //if (CommonController.Company.InTransaction)
                //{
                //    // Então
                //    // Finaliza a transação realizando um rollback para uma nova ser aberta
                //    CommonController.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                //}
            }
            finally
            {
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);

                //if (CommonController.Company.InTransaction)
                //{
                //    // Finaliza a transação realizando um commit
                //    CommonController.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                //}
            }
        }

        private void Form_RightClickBefore(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (eventInfo.ItemUID == "mtDocs")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveSuggestion").Enabled = true;
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("SetSuggestion").Enabled = true;
                _menuRow = eventInfo.Row - 1;
            }
        }

        private void Form_RightClickAfter(ref ContextMenuInfo eventInfo)
        {
            if (eventInfo.ItemUID == "mtDocs")
            {
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("RemoveSuggestion").Enabled = false;
                SAPbouiCOM.Framework.Application.SBO_Application.Menus.Item("1280").SubMenus.Item("SetSuggestion").Enabled = false;
                _menuRow = -1;
            }
        }

        private void SetPickingSuggestions(List<PickingData> pickingData)
        {
            UIAPIRawForm.Visible = false;
            UIAPIRawForm.DataSources.UserDataSources.Item("NewWay").Value = "Y";

            GC.Collect();

            var progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", 0, false);
            var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");

            try
            {
                var dtDocs = UIAPIRawForm.DataSources.DataTables.Item("dtDocs");
                var packageController = new PackageController();

                PickingData = pickingData;
                Packages = new List<Packages>();

                #region [ Obtenção das embalagens de todos os itens reservados para análise ]
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", PickingData.Count, false);

                foreach (var data in PickingData.GroupBy(x => x.ItemCode).Select(y => y.First()))
                {
                    progessBar.Text = $"Obtendo dados das embalagens para o item {data.ItemCode}.";
                    progessBar.Value += 1;

                    var package = packageController.GetPackages(data.ItemCode).Result;

                    if (package == null) continue;

                    foreach (var pack in package)
                    {
                        pack.Quantidade -= PackageController.SetReservedQuantity(pack.Codigocliente.Replace("\n", ""), pack.Validade);
                        pack.Volume = pack.Quantidade * pack.Fatorconversao;
                    }

                    Packages.AddRange(package);
                }
                #endregion

                #region [ Análise de sugestão de embalagem ]
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", PickingData.Count, false);

                // Realiza análise de acordo com a priorização dos PNs (do menor para o maior), data de vencimento (do menor para o maior), quantidade (do maior para menor).
                foreach (var data in PickingData.OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty))
                {
                    progessBar.Text = $"Realizando sugestões para {data.ItemCode} itens.";
                    progessBar.Value += 1;

                    SetSuggestion(data);
                }
                #endregion

                #region [ Inserção dos dados no DataTable ]
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", PickingData.Where(x => x.DocLineType != LineType.New).OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty).ToList().Count, false);

                dtDocs.Rows.Add(PickingData.Where(x => !x.Suggested && x.DocLineType != LineType.New).OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty).ToList().Count);
                var row = 0;

                foreach (var data in PickingData.Where(x => !x.Suggested && x.DocLineType != LineType.New && x.DocLineType != LineType.Balance).OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty))
                {
                    progessBar.Value += 1;

                    // Cód. do PN
                    dtDocs.SetValue("CardCode", row, data.CardCode);
                    // Nome do PN
                    dtDocs.SetValue("CardName", row, data.CardName);
                    // Nome do Endereço de Entrega do PN
                    dtDocs.SetValue("Address", row, data.AddressName);
                    // DocEntry
                    dtDocs.SetValue("DocEntry", row, data.DocEntry);
                    // Nº do documento
                    dtDocs.SetValue("DocNum", row, data.DocNum);
                    // Linha do Documento
                    dtDocs.SetValue("LineNum", row, data.DocVisOrder);
                    // Data de Entrega
                    dtDocs.SetValue("ShipDate", row, data.ShipDate);
                    // Cód.Do Item
                    dtDocs.SetValue("ItemCode", row, data.ItemCode);
                    // Descrição do Item
                    dtDocs.SetValue("ItemName", row, data.ItemName);
                    // UM Original
                    dtDocs.SetValue("OrigUom", row, data.OriginalUom);
                    // Unidade de Medida Sugerida
                    dtDocs.SetValue("SuggUom", row, data.Packages.Count == 0 ? "\u00A0" : String.Join(", ", data.Packages.Select(x => x.Embalagem).ToArray()));
                    // Qtd Original
                    dtDocs.SetValue("OrigQty", row, data.OriginalQty);
                    // Diferença
                    dtDocs.SetValue("Balance", row, data.Packages.Count == 0 || data.BalanceQty == 0 ? 0 : data.BalanceQty * -1);
                    // Qtd Sugerida
                    dtDocs.SetValue("SuggQty", row, String.Join(", ", data.Packages.Select(x => x.Quantidade).ToArray()));
                    // Qtd Total
                    dtDocs.SetValue("TotalQty", row, data.Packages.Count == 0 ? 0 : data.OriginalQty + (data.BalanceQty * -1));
                    // Vencimento
                    dtDocs.SetValue("DueDate", row, String.Join(", ", data.Packages.Select(x => x.Validade.ToString("dd/MM/yyy")).ToArray()));

                    // Armazena o número da linha após ordenação dos dados
                    data.VisOrder = row + 1;

                    foreach (var newLine in PickingData.Where(x => (x.DocLineType == LineType.New || x.DocLineType == LineType.Balance) && x.DocEntry == data.DocEntry && x.DocLineNum == data.DocLineNum))
                    {
                        newLine.VisOrder = data.VisOrder;
                    }

                    row++;
                }
                #endregion

                mtDocs.LoadFromDataSourceEx();
                mtDocs.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                UIAPIRawForm.State = BoFormStateEnum.fs_Maximized;
                UIAPIRawForm.Visible = true;
            }
        }

        private void SetPickingSuggestions()
        {
            UIAPIRawForm.Visible = false;
            UIAPIRawForm.DataSources.UserDataSources.Item("NewWay").Value = "N";

            GC.Collect();

            var mtPickingData = (Matrix)CommonController.MotherForm.Items.Item("10").Specific;
            var progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", mtPickingData.RowCount, false);

            Recordset rstWhs = (Recordset)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            Recordset rstBP = (Recordset)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            Recordset rstSalesOrder = (Recordset)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

            try
            {
                CommonController.MotherForm.Freeze(true);

                var dtDocs = UIAPIRawForm.DataSources.DataTables.Item("dtDocs");
                var packageController = new PackageController();

                PickingData = new List<PickingData>();
                Packages = new List<Packages>();

                string warehouse = "";
                string cardCode = "";

                #region [ Obtenção dos dados contidos no picking ]
                // Percorre por todas as linhas do picking, procurando linhas que devem passar pela recomendação de embalagem
                for (var i = 1; i <= mtPickingData.RowCount; i++)
                {
                    progessBar.Text = $"Realizando análise da linha {i}/{mtPickingData.RowCount}.";
                    progessBar.Value += 1;

                    if (((CheckBox)mtPickingData.Columns.Item("1").Cells.Item(i).Specific).Checked)
                    {
                        ((CheckBox)mtPickingData.Columns.Item("1").Cells.Item(i).Specific).Checked = false;
                    }
                    #region [ Verificação se a linha do picking será tratada ]

                    string quantity = ((EditText)mtPickingData.Columns.Item("2").Cells.Item(i).Specific).Value.Replace(".", "");
                    // Caso não exista quantidade disponível, ignorar
                    if (String.IsNullOrEmpty(quantity) || double.Parse(quantity) <= 0.0) continue;

                    // Caso não seja um pedido de venda e não haja quantidade em estoque, ignorar
                    if (((EditText)mtPickingData.Columns.Item("34").Cells.Item(i).Specific).Value != "PV" &&
                        String.IsNullOrEmpty(((EditText)mtPickingData.Columns.Item("2").Cells.Item(i).Specific).Value)) continue;

                    if (warehouse != ((EditText)mtPickingData.Columns.Item("6").Cells.Item(i).Specific).Value)
                    {
                        rstWhs.DoQuery(String.Format(Hana.Warehouse_Get, ((EditText)mtPickingData.Columns.Item("6").Cells.Item(i).Specific).Value));
                        warehouse = rstWhs.Fields.Item("WhsCode").Value.ToString();
                    }

                    //var warehouse = (SAPbobsCOM.Warehouses)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses);
                    //warehouse.GetByKey(((EditText)mtPickingData.Columns.Item("6").Cells.Item(i).Specific).Value);

                    // Caso o depósito não tenha controle de UM, ignorar
                    if (rstWhs.Fields.Item("U_CVA_UomControl").Value.ToString() != "Y") continue;

                    // Caso a linha já tenha uma UM original, então já ocorreu a sugestão na linha, porém algum erro fez com que ela não tenha sido
                    // liberada para picking. Sendo assim, seleciona a linha, mas não realiza nova sugestão
                    if (!String.IsNullOrEmpty(((EditText)mtPickingData.Columns.Item("U_CVA_OriginalUom").Cells.Item(i).Specific).Value)) continue;
                    #endregion

                    if (cardCode != ((EditText)mtPickingData.Columns.Item("10").Cells.Item(i).Specific).Value)
                    {
                        rstBP.DoQuery(String.Format(Hana.BusinessPartner_Get, ((EditText)mtPickingData.Columns.Item("10").Cells.Item(i).Specific).Value));
                        cardCode = rstBP.Fields.Item("CardCode").Value.ToString();
                    }

                    #region [ Obtenção da priorização do PN ]
                    var bpPriority = UIAPIRawForm.DataSources.DBDataSources.Item("@CVA_BPP1");
                    var conditions = new SAPbouiCOM.Conditions();
                    var condition = conditions.Add();
                    condition.Alias = "U_CardCode";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = cardCode;
                    condition.Relationship = BoConditionRelationship.cr_AND;
                    condition = conditions.Add();
                    condition.Alias = "U_WhsCode";
                    condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    condition.CondVal = warehouse;
                    bpPriority.Query(conditions);
                    #endregion

                    rstSalesOrder.DoQuery(String.Format(Hana.SalesOrder_GetLine, ((EditText)mtPickingData.Columns.Item("23").Cells.Item(i).Specific).Value, int.Parse(((EditText)mtPickingData.Columns.Item("12").Cells.Item(i).Specific).Value) - 1));

                    // Armazena em uma lista as informações da linha que deve passar pela recomendação de embalagem
                    PickingData.Add(new PickingData
                    {
                        Suggested = false,
                        DocEntry = int.Parse(((EditText)mtPickingData.GetCellSpecific("23", i)).Value),
                        DocNum = int.Parse(((EditText)mtPickingData.GetCellSpecific("11", i)).Value),
                        LineNum = i,
                        CardCode = cardCode,
                        CardName = rstBP.Fields.Item("CardName").Value.ToString(),
                        AddressName = rstBP.Fields.Item("ShipToDef").Value.ToString(),
                        DocLineType = LineType.Normal,
                        DocVisOrder = int.Parse(((EditText)mtPickingData.GetCellSpecific("12", i)).Value),
                        DocLineNum = int.Parse(((EditText)mtPickingData.GetCellSpecific("31", i)).Value),
                        ShipDate = DateTime.ParseExact(((EditText)mtPickingData.GetCellSpecific("5", i)).Value, "yyyyMMdd", null),
                        ItemCode = ((EditText)mtPickingData.GetCellSpecific("8", i)).Value,
                        ItemName = ((EditText)mtPickingData.GetCellSpecific("7", i)).Value,
                        Price = (double)rstSalesOrder.Fields.Item("Price").Value,
                        OriginalUom = ((EditText)mtPickingData.GetCellSpecific("10000062", i)).Value,
                        OriginalQty = double.Parse(((EditText)mtPickingData.GetCellSpecific("4", i)).Value),
                        BalanceQty = double.Parse(((EditText)mtPickingData.GetCellSpecific("4", i)).Value),
                        BPPriority = bpPriority.Size == 0 ? 0 : int.Parse(bpPriority.GetValue("U_Priority", 0).ToString()),
                        DueDaysLimit = int.Parse(rstWhs.Fields.Item("U_CVA_DueDateLimit").Value.ToString())
                    });
                }
                #endregion

                #region [ Obtenção das embalagens de todos os itens reservados para análise ]
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", PickingData.Count, false);

                foreach (var data in PickingData.GroupBy(x => x.ItemCode).Select(y => y.First()))
                {
                    try
                    {
                        progessBar.Text = $"Obtendo dados das embalagens para o item {data.ItemCode}.";
                        progessBar.Value += 1;
                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText($"data.ItemCode {data.ItemCode}", BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None);
                        var package = packageController.GetPackages(data.ItemCode).Result;
                        if (package == null) continue;

                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText($"Descricao {package.FirstOrDefault()?.Descricao}", BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None);
                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText($"Embalagem {package.FirstOrDefault()?.Embalagem}", BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None);

                        foreach (var pack in package)
                        {
                            pack.Quantidade -= PackageController.SetReservedQuantity(pack.Codigocliente.Replace("\n", ""), pack.Validade);
                            pack.Volume = pack.Quantidade * pack.Fatorconversao;
                        }

                        Packages.AddRange(package);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                #endregion

                #region [ Análise de sugestão de embalagem ]
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", PickingData.Count, false);

                // Realiza análise de acordo com a priorização dos PNs (do menor para o maior), data de vencimento (do menor para o maior), quantidade (do maior para menor).
                foreach (var data in PickingData.OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty))
                {
                    progessBar.Text = $"Realizando sugestões para {data.ItemCode} itens.";
                    progessBar.Value += 1;

                    SetSuggestion(data);
                }
                #endregion

                #region [ Inserção dos dados no DataTable ]
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", PickingData.Where(x => x.DocLineType != LineType.New && x.DocLineType != LineType.Balance).OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty).ToList().Count, false);

                dtDocs.Rows.Add(PickingData.Where(x => !x.Suggested && x.DocLineType != LineType.New && x.DocLineType != LineType.Balance).OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty).ToList().Count);
                var row = 0;

                foreach (var data in PickingData.Where(x => !x.Suggested && x.DocLineType != LineType.New && x.DocLineType != LineType.Balance).OrderByDescending(x => x.BPPriority).ThenBy(x => x.ShipDate).ThenByDescending(x => x.OriginalQty))
                {
                    progessBar.Value += 1;

                    // Cód. do PN
                    dtDocs.SetValue("CardCode", row, data.CardCode);
                    // Nome do PN
                    dtDocs.SetValue("CardName", row, data.CardName);
                    // Nome do Endereço de Entrega do PN
                    dtDocs.SetValue("Address", row, data.AddressName);
                    // DocEntry
                    dtDocs.SetValue("DocEntry", row, data.DocEntry);
                    // Nº do documento
                    dtDocs.SetValue("DocNum", row, data.DocNum);
                    // Linha do Documento
                    dtDocs.SetValue("LineNum", row, data.DocVisOrder);
                    // Data de Entrega
                    dtDocs.SetValue("ShipDate", row, data.ShipDate);
                    // Cód.Do Item
                    dtDocs.SetValue("ItemCode", row, data.ItemCode);
                    // Descrição do Item
                    dtDocs.SetValue("ItemName", row, data.ItemName);
                    // UM Original
                    dtDocs.SetValue("OrigUom", row, data.OriginalUom);
                    // Unidade de Medida Sugerida
                    dtDocs.SetValue("SuggUom", row, data.Packages.Count == 0 ? "\u00A0" : String.Join(", ", data.Packages.Select(x => x.Embalagem).ToArray()));
                    // Qtd Original
                    dtDocs.SetValue("OrigQty", row, data.OriginalQty);
                    // Diferença
                    dtDocs.SetValue("Balance", row, data.Packages.Count == 0 || data.BalanceQty == 0 ? 0 : data.BalanceQty * -1);
                    // Qtd Sugerida
                    dtDocs.SetValue("SuggQty", row, String.Join(", ", data.Packages.Select(x => x.Quantidade).ToArray()));
                    // Qtd Total
                    dtDocs.SetValue("TotalQty", row, data.Packages.Count == 0 ? 0 : data.OriginalQty + (data.BalanceQty * -1));
                    // Vencimento
                    dtDocs.SetValue("DueDate", row, String.Join(", ", data.Packages.Select(x => x.Validade.ToString("dd/MM/yyy")).ToArray()));

                    // Armazena o número da linha após ordenação dos dados
                    data.VisOrder = row + 1;

                    foreach (var newLine in PickingData.Where(x => (x.DocLineType == LineType.New || x.DocLineType == LineType.Balance) && x.DocEntry == data.DocEntry && x.DocLineNum == data.DocLineNum))
                    {
                        newLine.VisOrder = data.VisOrder;
                    }

                    row++;
                }
                #endregion

                mtDocs.LoadFromDataSourceEx();
                mtDocs.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                CommonController.MotherForm.Freeze(false);
                progessBar.Stop();
                Marshal.ReleaseComObject(progessBar);
                Marshal.ReleaseComObject(rstBP);
                Marshal.ReleaseComObject(rstWhs);
                Marshal.ReleaseComObject(rstSalesOrder);

                progessBar = null;
                rstBP = null;
                rstWhs = null;
                rstSalesOrder = null;

                UIAPIRawForm.State = BoFormStateEnum.fs_Maximized;
                UIAPIRawForm.Visible = true;
            }
        }

        private void SetSuggestion(PickingData data)
        {
            while (data.BalanceQty > 0.0)
            {
                var analisysPacks = Packages.Select(package => new
                {
                    Package = package,
                    BalanceQty = package.Quantidade - package.AttributedQuantity - Math.Ceiling(data.BalanceQty / package.Fatorconversao) < 0 ? (Math.Ceiling(data.BalanceQty / package.Fatorconversao) + ((package.Quantidade - package.AttributedQuantity) - Math.Ceiling(data.BalanceQty / package.Fatorconversao))) * package.Fatorconversao - data.BalanceQty
                                                                                                                                              : Math.Ceiling(data.BalanceQty / package.Fatorconversao) * package.Fatorconversao - data.BalanceQty,
                    UsedQty = (int)(package.Quantidade - package.AttributedQuantity - Math.Ceiling(data.BalanceQty / package.Fatorconversao) < 0 ? (Math.Ceiling(data.BalanceQty / package.Fatorconversao) + ((package.Quantidade - package.AttributedQuantity) - Math.Ceiling(data.BalanceQty / package.Fatorconversao))) * package.Fatorconversao - data.BalanceQty
                                                                                                                                                 : Math.Ceiling(data.BalanceQty / package.Fatorconversao))
                })
                                        .Where(x => x.Package.Codigocliente.StartsWith(data.ItemCode) &&
                                               x.Package.AttributedQuantity < x.Package.Volume)
                                        // Regra 1: análise de vencimento
                                        .OrderBy(x => (x.Package.Validade - DateTime.Now).TotalDays <= data.DueDaysLimit ? (x.Package.Validade - DateTime.Now).TotalDays : int.MaxValue)
                                        // Regra 2: análise de quantidade residual
                                        .ThenBy(x => x.BalanceQty < 0 ? Math.Abs(x.BalanceQty) + data.BalanceQty : x.BalanceQty)
                                        // Regra 3: análise de menor quantidade em estoque
                                        .ThenBy(x => x.Package.Volume - x.Package.AttributedQuantity)
                                        // Regra 4: análise de menor vencimento
                                        .ThenBy(x => x.Package.Validade)
                                        // Regra 5: análise da menor quantidade usada
                                        .ThenBy(x => x.UsedQty)
                                        .Select(x => x.Package).ToList();

                if (analisysPacks.Count == 0)
                {
                    // Caso não haja mais embalagens disponíveis, porém foram usadas algumas na linha,
                    // é preciso quebrá-la em duas linhas, sendo a outra contendo o saldo residual sem embalagem
                    if (data.BalanceQty > 0.0 && data.Packages.Count > 0 && data.OriginalQty > data.Packages.Sum(x => x.Volume))
                    {
                        PickingData.Add(new PickingData
                        {
                            DocEntry = data.DocEntry,
                            CardCode = data.CardCode,
                            CardName = data.CardName,
                            AddressName = data.AddressName,
                            DocLineType = LineType.Balance,
                            OriginalDocLineNum = data.DocLineNum,
                            ShipDate = data.ShipDate,
                            ItemCode = data.ItemCode,
                            ItemName = data.ItemName,
                            Price = data.Price,
                            OriginalUom = data.OriginalUom,
                            OriginalQty = data.OriginalQty - data.Packages.Sum(x => x.Volume),
                            BalanceQty = data.BalanceQty,
                            BPPriority = data.BPPriority,
                            DueDaysLimit = data.DueDaysLimit,
                            ReleasePicking = false,
                            Packages = new List<Packages>()
                        });
                    }

                    break;
                }

                #region [ Cálculo do volume ]
                var availableQty = Math.Ceiling(data.BalanceQty / analisysPacks.FirstOrDefault().Fatorconversao);
                var balanceQty = analisysPacks.FirstOrDefault().Quantidade - analisysPacks.FirstOrDefault().AttributedQuantity - availableQty;
                var calculatedQty = (int)balanceQty < 0 ? availableQty + balanceQty : availableQty;

                if (calculatedQty <= 0)
                {
                    // Caso não haja mais embalagens disponíveis, porém foram usadas algumas na linha,
                    // é preciso quebrá-la em duas linhas, sendo a outra contendo o saldo residual sem embalagem
                    if (data.BalanceQty > 0.0 && data.Packages.Count > 0 && data.OriginalQty > data.Packages.Sum(x => x.Volume))
                    {
                        PickingData.Add(new PickingData
                        {
                            DocEntry = data.DocEntry,
                            CardCode = data.CardCode,
                            CardName = data.CardName,
                            AddressName = data.AddressName,
                            DocLineType = LineType.Balance,
                            OriginalDocLineNum = data.DocLineNum,
                            ShipDate = data.ShipDate,
                            ItemCode = data.ItemCode,
                            ItemName = data.ItemName,
                            Price = data.Price,
                            OriginalUom = data.OriginalUom,
                            OriginalQty = data.OriginalQty - data.Packages.Sum(x => x.Volume),
                            BalanceQty = data.BalanceQty,
                            BPPriority = data.BPPriority,
                            DueDaysLimit = data.DueDaysLimit,
                            ReleasePicking = true,
                            Packages = new List<Packages>()
                        });
                    }

                    break;
                }
                #endregion

                data.Packages.Add(new Packages
                {
                    Codigocliente = analisysPacks.FirstOrDefault().Codigocliente,
                    Embalagem = analisysPacks.FirstOrDefault().Embalagem,
                    Descricao = analisysPacks.FirstOrDefault().Descricao,
                    Fatorconversao = analisysPacks.FirstOrDefault().Fatorconversao,
                    Volume = calculatedQty * analisysPacks.FirstOrDefault().Fatorconversao,
                    Quantidade = calculatedQty,
                    Validade = analisysPacks.FirstOrDefault().Validade,
                    Valor = analisysPacks.FirstOrDefault().Valor,
                    UoMEntry = PackageController.GetUomEntry(analisysPacks.FirstOrDefault().Codigocliente.Replace("\n", ""), analisysPacks.FirstOrDefault().Embalagem)
                });

                // Caso tenha sido utilizado mais um tipo de embalagem para atender a linha do picking,
                // adiciona outro elemento da lista, que representa uma nova linha que será gerada do pedido de venda
                if (data.Packages.Count > 1)
                {
                    PickingData.Add(new PickingData
                    {
                        DocEntry = data.DocEntry,
                        CardCode = data.CardCode,
                        CardName = data.CardName,
                        AddressName = data.AddressName,
                        DocLineType = LineType.New,
                        OriginalDocLineNum = data.DocLineNum,
                        VisOrder = data.VisOrder,
                        ShipDate = data.ShipDate,
                        ItemCode = data.ItemCode,
                        ItemName = data.ItemName,
                        Price = data.Price,
                        DocLineNum = data.DocLineNum,
                        OriginalUom = data.OriginalUom,
                        OriginalQty = calculatedQty * analisysPacks.FirstOrDefault().Fatorconversao < data.BalanceQty ? calculatedQty * analisysPacks.FirstOrDefault().Fatorconversao : data.BalanceQty,
                        BalanceQty = data.BalanceQty - (calculatedQty * analisysPacks.FirstOrDefault().Fatorconversao),
                        BPPriority = data.BPPriority,
                        DueDaysLimit = data.DueDaysLimit,
                        ReleasePicking = true,
                        Packages = new List<Packages>
                                                       {
                                                            new Packages
                                                            {
                                                                Codigocliente = analisysPacks.FirstOrDefault().Codigocliente,
                                                                Embalagem = analisysPacks.FirstOrDefault().Embalagem,
                                                                Descricao = analisysPacks.FirstOrDefault().Descricao,
                                                                Fatorconversao = analisysPacks.FirstOrDefault().Fatorconversao,
                                                                Volume =  calculatedQty * analisysPacks.FirstOrDefault().Fatorconversao,
                                                                Quantidade = calculatedQty,
                                                                Validade = analisysPacks.FirstOrDefault().Validade,
                                                                Valor = analisysPacks.FirstOrDefault().Valor,
                                                                UoMEntry = PackageController.GetUomEntry(analisysPacks.FirstOrDefault().Codigocliente.Replace("\n", ""), analisysPacks.FirstOrDefault().Embalagem)
                                                            }
                                                      }
                    });

                    data.DocLineType = LineType.Old;
                    //data.Packages.RemoveAll(x => x.Codigocliente.StartsWith(analisysPacks.FirstOrDefault().Codigocliente) && x.Validade == analisysPacks.FirstOrDefault().Validade);
                }

                Packages pack = Packages.FirstOrDefault(x => x.Codigocliente == analisysPacks.FirstOrDefault().Codigocliente && x.Validade == analisysPacks.FirstOrDefault().Validade && x.Quantidade == analisysPacks.FirstOrDefault().Quantidade);

                // Atualiza a quantidade utilizada
                if (pack.AttributedQuantity + calculatedQty > pack.Quantidade)
                {
                    calculatedQty += (int)pack.Quantidade - calculatedQty;
                }

                data.BalanceQty -= calculatedQty * analisysPacks.FirstOrDefault().Fatorconversao;

                pack.AttributedQuantity += calculatedQty;
                pack.AttributedLines.Add(data.LineNum);

                if (pack.AttributedQuantity == pack.Quantidade)
                {
                    Packages.Remove(pack);
                }
            }
        }

        private void SetLineSuggestion()
        {
            try
            {
                UIAPIRawForm.Freeze(true);

                #region [ Análise de sugestão de embalagem ]
                var packageController = new PackageController();

                // Realiza análise na linha selecionada
                foreach (var data in PickingData.Where(x => x.VisOrder == _menuRow + 1).ToList())
                {
                    SetSuggestion(data);
                }
                #endregion

                #region [ Inserção dos dados no DataTable ]
                var dtDocs = UIAPIRawForm.DataSources.DataTables.Item("dtDocs");
                var lineNum = PickingData.Where(x => x.LineNum == _menuRow + 1).FirstOrDefault().VisOrder - 1;

                // Unidade de Medida Sugerida
                dtDocs.SetValue("SuggUom", _menuRow, String.Join(", ", PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().Packages.Where(x => x.Volume > 0).Select(x => x.Embalagem).ToArray()));
                // Qtd Original
                dtDocs.SetValue("OrigQty", _menuRow, PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().OriginalQty);
                // Diferença
                dtDocs.SetValue("Balance", _menuRow, PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().Packages.Count(x => x.Volume > 0) == 0 || PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().BalanceQty == 0 ? 0 : PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().BalanceQty * -1);
                // Qtd Sugerida
                dtDocs.SetValue("SuggQty", _menuRow, String.Join(", ", PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().Packages.Where(x => x.Quantidade > 0).Select(x => x.Quantidade).ToArray()));
                // Qtd Total
                dtDocs.SetValue("TotalQty", _menuRow, PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().Packages.Count(x => x.Volume > 0) == 0 ? 0 : PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().OriginalQty + (PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().BalanceQty * -1));
                // Vencimento
                dtDocs.SetValue("DueDate", _menuRow, String.Join(", ", PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().Packages.Where(x => x.Volume > 0).Select(x => x.Validade.ToString("dd/MM/yyy")).ToArray()));

                // Armazena o número da linha após ordenação dos dados
                PickingData.Where(x => x.VisOrder == _menuRow + 1).FirstOrDefault().VisOrder = _menuRow + 1;

                mtDocs.LoadFromDataSourceEx();
                mtDocs.AutoResizeColumns();
                #endregion
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void RemoveSuggestion()
        {
            try
            {
                UIAPIRawForm.Freeze(true);

                var dtDocs = UIAPIRawForm.DataSources.DataTables.Item("dtDocs");
                var docEntry = PickingData.Where(x => x.VisOrder == _menuRow + 1 && x.DocLineType != LineType.New).FirstOrDefault().DocEntry;
                var docLineNum = PickingData.Where(x => x.VisOrder == _menuRow + 1 && x.DocLineType != LineType.New).FirstOrDefault().DocLineNum;

                foreach (var data in PickingData.Where(x => x.VisOrder == _menuRow + 1).ToList())
                {
                    data.BalanceQty = data.OriginalQty;

                    foreach (var pack in data.Packages)
                    {
                        Packages.Where(x => x.Codigocliente == pack.Codigocliente).FirstOrDefault().AttributedQuantity -= (int)pack.Volume;
                    }

                    data.Packages.Clear();
                    if (data.DocLineType != LineType.New) continue;
                    PickingData.Remove(data);
                }

                // Unidade de Medida Sugerida
                dtDocs.SetValue("SuggUom", _menuRow, "");
                // Diferença
                dtDocs.SetValue("Balance", _menuRow, 0.0);
                // Qtd Sugerida
                dtDocs.SetValue("SuggQty", _menuRow, 0);
                // Qtd Total
                dtDocs.SetValue("TotalQty", _menuRow, 0.0);
                // Vencimento
                dtDocs.SetValue("DueDate", _menuRow, "");
                // Unidade de Medida Sugerida
                dtDocs.SetValue("SuggUom", _menuRow, "\u00A0");

                mtDocs.LoadFromDataSourceEx();
                mtDocs.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Short);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private Button btApply;

        private Matrix mtDocs;
    }
}
