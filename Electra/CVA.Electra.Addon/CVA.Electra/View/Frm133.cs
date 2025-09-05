using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.View.CRCP.BLL;
using CVA.View.CRCP.Model;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Electra
{
    public class f133 : BaseForm
    {
        Form Form;

        #region Constructor
        public f133()
        {
            FormCount++;
        }

        public f133(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f133(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f133(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.ActionSuccess && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    Form = SBOApp.Application.Forms.ActiveForm;
                    var canceled = Form.DataSources.DBDataSources.Item("OINV").GetValue("CANCELED", 0);
                    if (canceled == "N")
                    {
                        var docEntry = Form.DataSources.DBDataSources.Item("OINV").GetValue("DocEntry", 0);
                        var cardCode = Form.DataSources.DBDataSources.Item("OINV").GetValue("CardCode", 0);
                        string strSql = $"Select TOP 1 \"DocDueDate\" From \"ORDR\" T0 INNER JOIN \"RDR1\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" Where T1.\"TrgetEntry\"={docEntry} and T1.\"TargetType\" = 13 ";
                        Recordset record = (Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        record.DoQuery(strSql);
                        record.MoveFirst();
                        if (record.RecordCount > 0)
                        {
                            Documents doc = (Documents)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                            doc.GetByKey(Convert.ToInt32(docEntry));
                            doc.DocDueDate = DateTime.Parse(record.Fields.Item(0).Value.ToString(), CultureInfo.CreateSpecificCulture("pt-BR"));

                            strSql = $"SELECT T0.\"E_MailL\" FROM OCPR T0 WHERE T0.\"NFeRcpn\" = 'Y' AND T0.\"Active\" = 'Y' AND T0.\"E_MailL\" <> '' AND T0.\"CardCode\" = '{cardCode}'";
                            record.DoQuery(strSql);
                            int count = 0;
                            if (record.RecordCount > 0)
                                while (!record.EoF)
                                {
                                    if(count == 0)
                                        doc.UserFields.Fields.Item("U_EmailEnvDanfe").Value = record.Fields.Item(0).Value.ToString();
                                    else
                                        doc.UserFields.Fields.Item("U_EmailEnvDanfe").Value = doc.UserFields.Fields.Item("U_EmailEnvDanfe").Value + ";" + record.Fields.Item(0).Value.ToString();
                                    record.MoveNext();
                                    count++;
                                }
                            
                            doc.NumAtCard = doc.SequenceSerial.ToString();
                            doc.JournalMemo = "NF " + doc.SequenceSerial + " " + (doc.CardName.Length > 30 ? doc.CardName.Substring(0, 29) : doc.CardName);
                            strSql = $"SELECT TOP 1 SUBSTRING(OCFP.\"Descrip\",1,100)  FROM \"OINV\" INNER JOIN \"INV1\" on OINV.\"DocEntry\" = INV1.\"DocEntry\" INNER JOIN \"OCFP\" on OCFP.\"Code\" = INV1.\"CFOPCode\" WHERE OINV.\"DocEntry\" = {docEntry}";
                            record.DoQuery(strSql);
                            if (record.RecordCount > 0)
                                doc.UserFields.Fields.Item("U_EENatOp").Value = record.Fields.Item(0).Value.ToString();
                            int erro = 0;
                            string msg = "";
                            erro = doc.Update();
                            if (erro != 0)
                            {
                                SBOApp.Company.GetLastError(out erro, out msg);
                                SBOApp.Application.StatusBar.SetText($"Não foi possível atualizar a data de vencimento. {msg}", BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error);
                            }
                        }
                    }
                }
            }
            return true;
        }


    }
}
