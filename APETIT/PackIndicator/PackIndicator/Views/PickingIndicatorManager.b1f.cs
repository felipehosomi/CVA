using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using PackIndicator.Controllers;
using PackIndicator.Models;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace PackIndicator.Views
{
    [FormAttribute("PackIndicator.Views.PickingIndicatorManager", "Views/PickingIndicatorManager.b1f")]
    class PickingIndicatorManager : UserFormBase
    {
        public double PreviousReleaseQty { get; set; }
        private bool MyBubbleEvent { get; set; }

        public PickingIndicatorManager()
        {
        }

        public PickingIndicatorManager(string docNumFrom, string docNumTo, string docDateFrom, string docDateTo, string cardCodeFrom, string cardCodeTo, string itemCodeFrom, string itemCodeTo, string stockCategoryFrom, string stockCategoryTo, string dueDateFrom, string dueDateTo, string routeFrom, string routeTo, List<string> whsCodes)
        {
            GetOrdersLines(docNumFrom, docNumTo, docDateFrom, docDateTo, cardCodeFrom, cardCodeTo, itemCodeFrom, itemCodeTo, stockCategoryFrom, stockCategoryTo, dueDateFrom, dueDateTo, routeFrom, routeTo, whsCodes);
            UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized;
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtOrders = ((SAPbouiCOM.Matrix)(this.GetItem("mtOrders").Specific));
            this.mtOrders.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtOrders_ValidateBefore);
            this.mtOrders.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtOrders_ValidateAfter);
            this.mtOrders.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtOrders_LinkPressedBefore);
            this.btOK = ((SAPbouiCOM.Button)(this.GetItem("btOK").Specific));
            this.btOK.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btOK_PressedBefore);
            this.btOK.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btOK_PressedAfter);
            this.btCancel = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.btCancel.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btCancel_PressedBefore);
            this.btGoBack = ((SAPbouiCOM.Button)(this.GetItem("Item_3").Specific));
            this.btGoBack.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btGoBack_PressedAfter);
            this.OnCustomInitialize();

        }

        

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
        }

        private void GetOrdersLines(string docNumFrom, string docNumTo, string docDateFrom, string docDateTo, string cardCodeFrom, string cardCodeTo, string itemCodeFrom, string itemCodeTo, string stockCategoryFrom, string stockCategoryTo, string dueDateFrom, string dueDateTo, string routeFrom, string routeTo, List<string> whsCodes)
        {
            var dtOrders = UIAPIRawForm.DataSources.DataTables.Item("dtOrders");

            string sql = String.Format(
                                  @"do 
                                        begin
	                                        pick = 
	                                        select row_number()over(partition by RDR1.""ItemCode"", RDR1.""WhsCode"" order by RDR1.""ShipDate"", RDR1.""VisOrder"", ORDR.""DocNum"") as ""RowNum"", 
	 	                                           ORDR.""DocEntry"", ORDR.""DocNum"", ORDR.""CardCode"", ORDR.""CardName"", OCRD.""ShipToDef"",
	 	                                           RDR1.""ItemCode"", RDR1.""Dscription"" as ""ItemName"", RDR1.""LineNum"" ""LineNum"", RDR1.""VisOrder"" + 1 as ""VisOrder"", RDR1.""WhsCode"",
	 	                                           RDR1.""ShipDate"", RDR1.""Price"", 
	 	                                           RDR1.""Quantity"" - RDR1.""ReleasQtty"" as ""Quantity"", RDR1.""NumPerMsr"", RDR1.""U_CVA_OriginalUom"" as ""OriginalUom"",
	 	                                           OUOM.""UomCode"",
                                                   OITW.""OnHand"" - round((select sum((""ReleasQtty"" + ""PickOty"") * ""NumPerMsr"") from RDR1 as A0 where A0.""ItemCode"" = RDR1.""ItemCode"" and A0.""WhsCode"" = RDR1.""WhsCode""), 4) as ""WhsQty"",
	 	                                           OWHS.""U_CVA_DueDateLimit"",
	 	                                           coalesce(""@CVA_BPP1"".""U_Priority"", 0) as ""BPPriority""
	                                           from ORDR
	                                          inner join OCRD on OCRD.""CardCode"" = ORDR.""CardCode""
	                                          inner join RDR1 on ORDR.""DocEntry"" = RDR1.""DocEntry""
	                                          inner join OUOM on OUOM.""UomEntry"" = RDR1.""UomEntry""
	                                          inner join OITM on OITM.""ItemCode"" = RDR1.""ItemCode""
	                                          inner join OWHS on OWHS.""WhsCode"" = RDR1.""WhsCode""
	                                          inner join OITW on OITW.""ItemCode"" = OITM.""ItemCode""
	                                            and OITW.""WhsCode"" = RDR1.""WhsCode""	
	                                           left join ""@CVA_BPP1"" on ""@CVA_BPP1"".""U_CardCode"" = ORDR.""CardCode""
	                                            and ""@CVA_BPP1"".""U_WHsCode"" = RDR1.""WhsCode""	
	                                          where ORDR.""WddStatus"" in ('-', 'P', 'A')  
	                                            and ORDR.""Confirmed"" = 'Y' 
	                                            and OCRD.""CardType"" <> 'L' 
	                                            and RDR1.""DropShip"" <> 'Y'	                                            
	                                            and RDR1.""LineStatus"" = 'O'                                        	                                           
	                                            and RDR1.""Quantity"" >= 0	                                             
	                                            and RDR1.""OpenCreQty"" > 0
	                                            and RDR1.""PickStatus"" <> 'Y'
	                                            and RDR1.""ReleasQtty"" < RDR1.""Quantity""
	                                            and OITM.""TreeType"" <> 'S'
                                                {0}
                                                {1}
                                                {2}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {7}
                                                {8}
                                                {9}
                                                {10}
                                                {11}
                                                {12}
                                                {13}
                                                {14};
                                        
	                                        select row_number()over(order by ""ShipDate"", ""DocEntry"", ""VisOrder"") as ""RowNum"", 'PV' as ""TransType"",
                                                   Pick.""DocEntry"", Pick.""DocNum"", Pick.""CardCode"", Pick.""CardName"", Pick.""LineNum"", Pick.""VisOrder"", Pick.""ShipDate"", 
	                                       	 	   Pick.""ShipToDef"", Pick.""ItemCode"", Pick.""ItemName"", Pick.""WhsCode"", Pick.""Price"", Pick.""Quantity"", Pick.""UomCode"", cast(Pick.""NumPerMsr"" as int) as ""NumPerMsr"", 
		                                           case when (""WhsQty"" - coalesce((select sum(A0.""Quantity"" * ""NumPerMsr"") from :pick A0 where A0.""ItemCode"" = Pick.""ItemCode"" and A0.""WhsCode"" = Pick.""WhsCode"" and A0.""RowNum"" < Pick.""RowNum""), 0)) / ""NumPerMsr"" > 0 and
		   			                                         (""WhsQty"" - coalesce((select sum(A0.""Quantity"" * ""NumPerMsr"") from :pick A0 where A0.""ItemCode"" = Pick.""ItemCode"" and A0.""WhsCode"" = Pick.""WhsCode"" and A0.""RowNum"" < Pick.""RowNum""), 0)) / ""NumPerMsr"" < Pick.""Quantity"" then		   			                                         
		   			                               		cast((""WhsQty"" - coalesce((select sum(A0.""Quantity"" * ""NumPerMsr"") from :pick A0 where A0.""ItemCode"" = Pick.""ItemCode"" and A0.""WhsCode"" = Pick.""WhsCode"" and A0.""RowNum"" < Pick.""RowNum""), 0)) / ""NumPerMsr"" as decimal(16,5))
		   			                                         
		                                           else case when (""WhsQty"" - coalesce((select sum(A0.""Quantity"" * ""NumPerMsr"") from :pick A0 where A0.""ItemCode"" = Pick.""ItemCode"" and A0.""WhsCode"" = Pick.""WhsCode"" and A0.""RowNum"" < Pick.""RowNum""), 0)) / ""NumPerMsr"" > 0 and
		   			 	                                          (""WhsQty"" - coalesce((select sum(A0.""Quantity"" * ""NumPerMsr"") from :pick A0 where A0.""ItemCode"" = Pick.""ItemCode"" and A0.""WhsCode"" = Pick.""WhsCode"" and A0.""RowNum"" < Pick.""RowNum""), 0)) / ""NumPerMsr"" >= Pick.""Quantity"" then		   		                                        
		   		                                   		cast(Pick.""Quantity"" as decimal(16,5))
		                                           else
		   		                                        0
		                                           end end as ""ReleaseQty"",		
		                                           	                                                                                   
		                                           case when ""WhsQty"" - coalesce((select sum(A0.""Quantity"" * ""NumPerMsr"") from :pick A0 where A0.""ItemCode"" = Pick.""ItemCode"" and A0.""WhsCode"" = Pick.""WhsCode"" and A0.""RowNum"" < Pick.""RowNum""), 0) > 0 then		   		                                        	   		                                        
		   		                                   		cast((""WhsQty"" - coalesce((select sum(A0.""Quantity"" * ""NumPerMsr"") from :pick A0 where A0.""ItemCode"" = Pick.""ItemCode"" and A0.""WhsCode"" = Pick.""WhsCode"" and A0.""RowNum"" < Pick.""RowNum""), 0)) / ""NumPerMsr"" as decimal(16,5))
		                                           else
		   		                                        0
		                                           end as ""WhsQty"",
                                                   Pick.""U_CVA_DueDateLimit"" as ""DateLimit"", Pick.""BPPriority"", Pick.""OriginalUom""
	                                          from :pick as Pick
										 	 order by ""ShipDate"", ""DocEntry"", ""VisOrder"";
                                    end", whsCodes.Count > 0 ? @"and RDR1.""WhsCode"" in ('" + String.Join("','", whsCodes) + "')" : "",
                                          !String.IsNullOrEmpty(docNumFrom) ? @"and ORDR.""DocNum"" >= " + docNumFrom : "",
                                          !String.IsNullOrEmpty(docNumTo) ? @"and ORDR.""DocNum"" <= " + docNumTo : "",
                                          !String.IsNullOrEmpty(docDateFrom) ? @"and ORDR.""DocDate"" >= '" + docDateFrom + "'" : "",
                                          !String.IsNullOrEmpty(docDateTo) ? @"and ORDR.""DocDate"" <= '" + docDateTo + "'" : "",
                                          !String.IsNullOrEmpty(cardCodeFrom) ? @"and ORDR.""CardCode"" >= '" + cardCodeFrom + "'" : "",
                                          !String.IsNullOrEmpty(cardCodeTo) ? @"and ORDR.""CardCode"" <= '" + cardCodeTo + "'" : "",
                                          !String.IsNullOrEmpty(itemCodeFrom) ? @"and RDR1.""ItemCode"" >= '" + itemCodeFrom + "'" : "",
                                          !String.IsNullOrEmpty(itemCodeTo) ? @"and RDR1.""ItemCode"" <= '" + itemCodeTo + "'" : "",

                                          !String.IsNullOrEmpty(stockCategoryFrom) ? @"and RDR1.""U_CVA_CatEstoque"" >= '" + stockCategoryFrom + "'" : "",
                                          !String.IsNullOrEmpty(stockCategoryTo) ? @"and RDR1.""U_CVA_CatEstoque"" <= '" + stockCategoryTo + "'" : "",

                                          !String.IsNullOrEmpty(dueDateFrom) ? @"and RDR1.""U_CVA_PrchDueDate"" >= '" + dueDateFrom + "'" : "",
                                          !String.IsNullOrEmpty(dueDateTo) ? @"and RDR1.""U_CVA_PrchDueDate"" <= '" + dueDateTo + "'" : "",

                                          !String.IsNullOrEmpty(routeFrom) ? @"and RDR1.""U_CVA_RotaEntrega"" >= '" + routeFrom + "'" : "",
                                          !String.IsNullOrEmpty(routeTo) ? @"and RDR1.""U_CVA_RotaEntrega"" <= '" + routeTo + "'" : "");

            dtOrders.ExecuteQuery(sql);

            mtOrders.Columns.Item("RowNum").DataBind.Bind("dtOrders", "RowNum");
            mtOrders.Columns.Item("TransType").DataBind.Bind("dtOrders", "TransType");
            mtOrders.Columns.Item("DocEntry").DataBind.Bind("dtOrders", "DocEntry");
            mtOrders.Columns.Item("DocNum").DataBind.Bind("dtOrders", "DocNum");
            mtOrders.Columns.Item("CardCode").DataBind.Bind("dtOrders", "CardCode");
            mtOrders.Columns.Item("CardName").DataBind.Bind("dtOrders", "CardName");
            mtOrders.Columns.Item("LineNum").DataBind.Bind("dtOrders", "LineNum");
            mtOrders.Columns.Item("VisOrder").DataBind.Bind("dtOrders", "VisOrder");
            mtOrders.Columns.Item("ShipDate").DataBind.Bind("dtOrders", "ShipDate");
            mtOrders.Columns.Item("ShipToDef").DataBind.Bind("dtOrders", "ShipToDef");
            mtOrders.Columns.Item("ItemCode").DataBind.Bind("dtOrders", "ItemCode");
            mtOrders.Columns.Item("ItemName").DataBind.Bind("dtOrders", "ItemName");
            mtOrders.Columns.Item("WhsCode").DataBind.Bind("dtOrders", "WhsCode");
            mtOrders.Columns.Item("Price").DataBind.Bind("dtOrders", "Price");
            mtOrders.Columns.Item("Quantity").DataBind.Bind("dtOrders", "Quantity");
            mtOrders.Columns.Item("UomCode").DataBind.Bind("dtOrders", "UomCode");
            mtOrders.Columns.Item("NumPerMsr").DataBind.Bind("dtOrders", "NumPerMsr");
            mtOrders.Columns.Item("ReleaseQty").DataBind.Bind("dtOrders", "ReleaseQty");
            mtOrders.Columns.Item("WhsQty").DataBind.Bind("dtOrders", "WhsQty");
            mtOrders.Columns.Item("DateLimit").DataBind.Bind("dtOrders", "DateLimit");
            mtOrders.Columns.Item("BPPriority").DataBind.Bind("dtOrders", "BPPriority");
            mtOrders.Columns.Item("OrigUom").DataBind.Bind("dtOrders", "OriginalUom");
            mtOrders.LoadFromDataSourceEx();

            for (var i = 1; i <= mtOrders.RowCount; i++)
            {
                if (double.Parse(dtOrders.GetValue("WhsQty", i - 1).ToString()) > 0) continue;

                mtOrders.CommonSetting.SetCellFontColor(i, 17, Color.Red.R | (Color.Red.G << 8) | (Color.Red.B << 16));
                mtOrders.CommonSetting.SetCellFontColor(i, 18, Color.Red.R | (Color.Red.G << 8) | (Color.Red.B << 16));
            }
        }

        private void mtOrders_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            switch (pVal.ColUID)
            {
                case "DocNum":
                    BubbleEvent = false;

                    try
                    {
                        var dtOrders = UIAPIRawForm.DataSources.DataTables.Item("dtOrders");
                        SAPbouiCOM.Framework.Application.SBO_Application.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_Order, "", dtOrders.GetValue("DocEntry", pVal.Row - 1).ToString());
                    }
                    catch (Exception ex)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
                    }
                    break;
            }
        }

        private void mtOrders_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID != "ReleaseQty") return;

            MyBubbleEvent = true;
            var dtOrders = UIAPIRawForm.DataSources.DataTables.Item("dtOrders");
            var prevReleaseQty = double.Parse(dtOrders.GetValue("ReleaseQty", pVal.Row - 1).ToString());

            mtOrders.FlushToDataSource();

            if (double.Parse(dtOrders.GetValue("ReleaseQty", pVal.Row - 1).ToString()) > double.Parse(dtOrders.GetValue("WhsQty", pVal.Row - 1).ToString()))
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Impossível entrar valor que provoque queda da quantidade em estoque abaixo do nível mínimo definido", SAPbouiCOM.BoMessageTime.bmt_Short);
                BubbleEvent = false;
                MyBubbleEvent = false;
                dtOrders.SetValue("ReleaseQty", pVal.Row - 1, prevReleaseQty);
                return;
            }

            PreviousReleaseQty = prevReleaseQty;
        }

        private void mtOrders_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!MyBubbleEvent) return;
            if (!pVal.ItemChanged) return;
            if (pVal.ColUID != "ReleaseQty") return;

            try
            {
                UIAPIRawForm.Freeze(true);

                var dtOrders = UIAPIRawForm.DataSources.DataTables.Item("dtOrders");
                var releaseQty = double.Parse(dtOrders.GetValue("ReleaseQty", pVal.Row - 1).ToString());
                var whsQty = double.Parse(dtOrders.GetValue("WhsQty", pVal.Row - 1).ToString());

                if (releaseQty > 0)
                {
                    mtOrders.CommonSetting.SetCellFontColor(pVal.Row, 17, Color.Black.R | (Color.Black.G << 8) | (Color.Black.B << 16));
                }
                else
                {
                    mtOrders.CommonSetting.SetCellFontColor(pVal.Row, 17, Color.Red.R | (Color.Red.G << 8) | (Color.Red.B << 16));
                }

                var itemCode = dtOrders.GetValue("ItemCode", pVal.Row - 1).ToString();
                var whsCode = dtOrders.GetValue("WhsCode", pVal.Row - 1).ToString();
                var rowNum = int.Parse(dtOrders.GetValue("RowNum", pVal.Row - 1).ToString());
                var balance = PreviousReleaseQty - releaseQty;
                var xml = dtOrders.SerializeAsXML(SAPbouiCOM.BoDataTableXmlSelect.dxs_DataOnly);
                var doc = XDocument.Parse(xml);
                var rows = doc.Descendants().Where(d => d.Name == "Cell" &&
                                                        d.Descendants().Any(e => e.Name == "ColumnUid" &&
                                                                                 e.Value == "ItemCode" &&
                                                                                 d.Descendants("Value").Single().Value == itemCode) &&
                                                        (d.Parent.Descendants().Where(e => e.Name == "ColumnUid" &&
                                                                                      e.Value == "WhsCode").FirstOrDefault().NextNode as XElement).Value == whsCode);

                foreach (var row in rows)
                {
                    var lineRowNum = int.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "RowNum").FirstOrDefault().NextNode as XElement).Value);
                    if (lineRowNum <= rowNum) continue;

                    var lineWhsQty = double.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "WhsQty").FirstOrDefault().NextNode as XElement).Value, CultureInfo.InvariantCulture);
                    (row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "WhsQty").FirstOrDefault().NextNode as XElement).Value = (lineWhsQty + balance).ToString();

                    if (lineWhsQty + balance > 0)
                    {
                        mtOrders.CommonSetting.SetCellFontColor(lineRowNum, 18, Color.Black.R | (Color.Black.G << 8) | (Color.Black.B << 16));
                    }
                    else
                    {
                        mtOrders.CommonSetting.SetCellFontColor(lineRowNum, 18, Color.Red.R | (Color.Red.G << 8) | (Color.Red.B << 16));
                    }
                }

                dtOrders.LoadSerializedXML(SAPbouiCOM.BoDataTableXmlSelect.dxs_DataOnly, doc.ToString());
                mtOrders.LoadFromDataSourceEx();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }

        private void btOK_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dtOrders = UIAPIRawForm.DataSources.DataTables.Item("dtOrders");
            var xml = dtOrders.SerializeAsXML(SAPbouiCOM.BoDataTableXmlSelect.dxs_DataOnly);
            var doc = XDocument.Parse(xml);
            var rows = doc.Descendants().Where(d => d.Name == "Cell" &&
                                                    d.Descendants().Any(e => e.Name == "ColumnUid" &&
                                                                             e.Value == "ReleaseQty" &&
                                                                             float.Parse((d.NextNode as XElement).Descendants("Value").Single().Value) > float.Parse((d.Parent.Descendants().Where(x => x.Name == "ColumnUid" &&
                                                                                                                                                                                                        x.Value == "WhsQty").FirstOrDefault().NextNode as XElement).Value)));

            if (rows.Count() > 0)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Impossível entrar valor que provoque queda da quantidade em estoque abaixo do nível mínimo definido.", SAPbouiCOM.BoMessageTime.bmt_Short);
                BubbleEvent = false;
            }
        }

        private void btOK_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                var pickingData = new List<Models.PickingData>();
                var dtOrders = UIAPIRawForm.DataSources.DataTables.Item("dtOrders");

                var xml = dtOrders.SerializeAsXML(SAPbouiCOM.BoDataTableXmlSelect.dxs_DataOnly);
                var doc = XDocument.Parse(xml);
                var rows = doc.Descendants().Where(d => d.Name == "Cell" &&
                                                        d.Descendants().Any(e => e.Name == "ColumnUid" &&
                                                                                 e.Value == "ReleaseQty" &&
                                                                                 float.Parse((d.NextNode as XElement).Descendants("Value").Single().Value) > 0));

                foreach (var row in rows)
                {
                    // Armazena em uma lista as informações da linha que deve passar pela recomendação de embalagem
                    pickingData.Add(new PickingData
                    {
                        Suggested = !String.IsNullOrEmpty((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "OriginalUom").FirstOrDefault().NextNode as XElement).Value),
                        DocEntry = int.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "DocEntry").FirstOrDefault().NextNode as XElement).Value),
                        DocNum = int.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "DocNum").FirstOrDefault().NextNode as XElement).Value),
                        LineNum = 0,
                        CardCode = (row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "CardCode").FirstOrDefault().NextNode as XElement).Value,
                        CardName = (row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "CardName").FirstOrDefault().NextNode as XElement).Value,
                        AddressName = (row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "ShipToDef").FirstOrDefault().NextNode as XElement).Value,
                        DocLineType = LineType.Normal,
                        DocVisOrder = int.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "VisOrder").FirstOrDefault().NextNode as XElement).Value),
                        DocLineNum = int.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "LineNum").FirstOrDefault().NextNode as XElement).Value),
                        ShipDate = DateTime.ParseExact((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "ShipDate").FirstOrDefault().NextNode as XElement).Value, "yyyyMMdd", null),
                        ItemCode = (row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "ItemCode").FirstOrDefault().NextNode as XElement).Value,
                        ItemName = (row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "ItemName").FirstOrDefault().NextNode as XElement).Value,
                        Price = double.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "Price").FirstOrDefault().NextNode as XElement).Value, CultureInfo.InvariantCulture),
                        OriginalUom = (row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "UomCode").FirstOrDefault().NextNode as XElement).Value,
                        OriginalQty = double.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "Quantity").FirstOrDefault().NextNode as XElement).Value, CultureInfo.InvariantCulture),
                        BalanceQty = double.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "Quantity").FirstOrDefault().NextNode as XElement).Value, CultureInfo.InvariantCulture),
                        BPPriority = int.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "BPPriority").FirstOrDefault().NextNode as XElement).Value),
                        DueDaysLimit = int.Parse((row.Parent.Descendants().Where(x => x.Name == "ColumnUid" && x.Value == "DateLimit").FirstOrDefault().NextNode as XElement).Value),
                        ReleasePicking = true,
                    });
                }

                var pickingIndicator = new PickingIndicator(pickingData, UIAPIRawForm);
                pickingIndicator.Show();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
        }

        private void btGoBack_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            UIAPIRawForm.Close();
        }

        private void btCancel_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (CommonController.MotherForm != null)
            {
                CommonController.MotherForm.Close();
            }
        }

        private SAPbouiCOM.Matrix mtOrders;
        private SAPbouiCOM.Button btOK;
        private SAPbouiCOM.Button btCancel;
        private SAPbouiCOM.Button btGoBack;
    }
}
