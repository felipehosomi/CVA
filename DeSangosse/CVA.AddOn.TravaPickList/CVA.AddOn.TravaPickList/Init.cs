using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company = SAPbobsCOM.Company;
using SAPbouiCOM;
using SAPbobsCOM;

namespace CVA.AddOn.TravaPickList
{
    public class Init
    {
        Company Company { get; set; }
        Application Application { get; set; }
        EventFilters Filters { get; set; }

        public Init()
        {
            SetApplication();
            Company = (Company)Application.Company.GetDICompany();
            Filters = new EventFilters();

            AddFilter("139", BoEventTypes.et_COMBO_SELECT);
            AddFilter("139", BoEventTypes.et_ITEM_PRESSED);
            //AddFilter("-9876", BoEventTypes.et_MENU_CLICK);

            Application.AppEvent += AppEvents;
            Application.ItemEvent += ItemEvents;
            Application.SetStatusBarMessage("OK", BoMessageTime.bmt_Short, false);
        }

        private void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            var ret = true;

            try
            {
                if(pVal.FormTypeEx == "139" && pVal.EventType == BoEventTypes.et_COMBO_SELECT && pVal.ItemUID == "10000329" && pVal.BeforeAction)
                {
                    if (pVal.PopUpIndicator == 1)
                    {
                        var Form = Application.Forms.ActiveForm;
                        var ds = Form.DataSources.DBDataSources.Item("ORDR");
                        ret = ValidaLotes(ds.GetValue("DocEntry", 0));
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                Application.SetStatusBarMessage(ex.Message);
            }

            BubbleEvent = ret;
        }

        private bool ValidaLotes(string docEntry)
        {
            var ret = false;

            var oRecordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var builder = new StringBuilder();
            builder.AppendLine("SELECT RDR1.VisOrder FROM RDR1 ");
            builder.AppendLine("INNER JOIN OITM ON OITM.ItemCode = RDR1.ItemCode AND OITM.ManBtchNum = 'Y' ");
            builder.AppendLine("LEFT JOIN (SELECT ITemCode, WhsCode, BaseType, BaseEntry, BaseLinNum, SUM(Quantity) Quantity ");
            builder.AppendLine("FROM IBT1 GROUP BY ItemCode, WhsCode, BaseType, BaseEntry, BaseLinNum) IBT1 ");
            builder.AppendLine("ON IBT1.ItemCode = RDR1.ItemCode ");
            builder.AppendLine("AND IBT1.WhsCode = RDR1.WhsCode ");
            builder.AppendLine("AND IBT1.BaseEntry = RDR1.DocEntry ");
            builder.AppendLine("AND IBT1.BaseLinNum = RDR1.LineNum ");
            builder.AppendLine("AND IBT1.BaseType = 17 ");
            builder.AppendLine("INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry AND ORDR.Confirmed = 'Y' ");
            builder.AppendLine($"WHERE RDR1.DocEntry = {docEntry} ");
            builder.AppendLine("AND RDR1.Quantity <> ISNULL(IBT1.Quantity, 0) ");

            oRecordset.DoQuery(builder.ToString());

            if(oRecordset.RecordCount > 0)
            {
                var msg = new StringBuilder();
                while (!oRecordset.EoF)
                {
                    var i = int.Parse(oRecordset.Fields.Item("VisOrder").Value.ToString())+1;
                    msg.AppendLine($"Linha {i}: Informar número(s) de lote do item");
                    oRecordset.MoveNext();
                }

                if(!string.IsNullOrEmpty(msg.ToString()))
                {
                    Application.MessageBox(msg.ToString());
                }
            }
            else
            {
                ret = true;
            }

            return ret;
            /*
             IF @object_type = '17' AND @transaction_type IN ('U')
BEGIN
 DECLARE @Line INT
 SELECT @Line = RDR1.VisOrder FROM RDR1
  INNER JOIN OITM
   ON OITM.ItemCode = RDR1.ItemCode
   AND OITM.ManBtchNum = 'Y'
  LEFT JOIN (SELECT ItemCode, WhsCode, BaseType, BaseEntry, BaseLinNum, SUM(Quantity) Quantity
     FROM IBT1 GROUP BY ItemCode, WhsCode, BaseType, BaseEntry, BaseLinNum) IBT1
   ON  IBT1.ItemCode = RDR1.ItemCode
   AND IBT1.WhsCode = RDR1.WhsCode
   AND IBT1.BaseEntry = RDR1.DocEntry
   AND IBT1.BaseLinNum = RDR1.LineNum
   AND IBT1.BaseType = 17
   INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry AND ORDR.Confirmed = 'Y'
 WHERE RDR1.DocEntry = @list_of_cols_val_tab_del
 AND RDR1.Quantity <> ISNULL(IBT1.Quantity, 0)

 IF @Line IS NOT NULL
 BEGIN
  SET @error = @Line
  SET @error_message = 'Linha ' + CAST(@Line AS VARCHAR(10)) + ': Informar número(s) de lote do item'
 END
END
             */
        }

        private void AppEvents(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                case BoAppEventTypes.aet_FontChanged:
                case BoAppEventTypes.aet_LanguageChanged:
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    Environment.Exit(-1);
                    break;
            }
        }

        private void SetApplication()
        {
            var sboGuiApi = new SboGuiApi();
            var str = Environment.GetCommandLineArgs().GetValue(1).ToString();
            sboGuiApi.Connect(str);
            Application = sboGuiApi.GetApplication();
        }

        private void AddFilter(string containerUid, BoEventTypes eventType)
        {
            int pos;
            if (!FilterExists(eventType, out pos))
            {
                EventFilter oFilter = Filters.Add(eventType);
                oFilter.AddEx(containerUid);
                Application.SetFilter(Filters as EventFilters);
            }
            else
            {
                Filters.Item(pos).AddEx(containerUid);
                Application.SetFilter(Filters as EventFilters);
            }
        }

        private bool FilterExists(BoEventTypes type, out int position)
        {
            var ret = false;
            position = -1;

            try
            {
                for (var i = 0; i < Filters.Count; i++)
                {
                    var f = Filters.Item(i);
                    if (type.Equals(f.EventType))
                    {
                        ret = true;
                        position = i;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }


    }
}
