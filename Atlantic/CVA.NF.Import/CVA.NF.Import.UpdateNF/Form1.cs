using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.NF.Import.UpdateNF
{
    public partial class Form1 : Form
    {
        public Company oCompany = new Company();

        public Form1()
        {
            InitializeComponent();

            //this.txtServer.Text = "FELIPE-PC";
            //this.txtUserDB.Text = "sa";
            //this.txtPasswDB.Text = "sa@123";
            //this.txtUserSAP.Text = "manager";
            //this.txtPasswSAP.Text = "manager";

            this.txtServer.Text = "SERVERSAP";
            this.txtUserDB.Text = "sa";
            this.txtPasswDB.Text = "sa@#Atlantic";
            this.txtUserSAP.Text = "manager";
            this.txtPasswSAP.Text = "CVA$sap16";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!oCompany.Connected)
            {
                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
                oCompany.Server = txtServer.Text;
                oCompany.DbUserName = txtUserDB.Text;
                oCompany.DbPassword = txtPasswDB.Text;
                oCompany.UserName = txtUserSAP.Text;
                oCompany.Password = txtPasswSAP.Text;
                oCompany.CompanyDB = txtBanco.Text;

                int err = oCompany.Connect();

                if (err != 0)
                {
                    string MsgErro;
                    oCompany.GetLastError(out err, out MsgErro);
                    MessageBox.Show(MsgErro);
                    return;
                }

                SBOApp.Company = oCompany;

                Recordset rst = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                rst.DoQuery(@"SELECT MAX(VPM2.DocNum) DocNum, MAX(VPM2.DocEntry) DocEntry, MAX(OBOE.BoeNum) BoeNum
                                FROM VPM2
                                    INNER JOIN OVPM
                                        ON OVPM.DocNum = VPM2.DocNum
	                                LEFT JOIN OBOE
		                                ON OBOE.PmntNum = OVPM.DocNum
                                WHERE  OVPM.DocDueDate = '2018-04-25'
                                --AND OVPM.CANCELED = 'N'
                                GROUP BY
                                    OVPM.CardCode,
                                    OVPM.DocDate,
                                    VPM2.SumApplied
                                HAVING COUNT(SumApplied) > 1
                                ORDER BY MAX(VPM2.DocEntry)");

                int i = 0;
                while (!rst.EoF)
                {
                    CrudController.ExecuteNonQuery($"UPDATE OVPM SET U_IsAuto = 0, U_IsBSPmnt = 0 WHERE DocNum = {(int)rst.Fields.Item("DocNum").Value}");

                    if ((int)rst.Fields.Item("BoeNum").Value != 0)
                    {
                        BillOfExchangeTransaction boe = SBOApp.Company.GetBusinessObject(BoObjectTypes.oBillOfExchangeTransactions) as BillOfExchangeTransaction;
                        try
                        {
                            boe.PostingDate = DateTime.Today;

                            boe.StatusFrom = BoBOTFromStatus.btfs_Generated;
                            boe.StatusTo = BoBOTToStatus.btts_Canceled;

                            boe.Deposits.PostingType = BoDepositPostingTypes.dpt_Collection;
                            boe.IsBoeReconciled = BoYesNoEnum.tNO;

                            boe.Lines.BillOfExchangeNo = (int)rst.Fields.Item("BoeNum").Value;
                            boe.Lines.BillOfExchangeType = BoBOETypes.bobt_Outgoing;

                            if (boe.Add() != 0)
                            {
                                //MessageBox.Show("Erro ao cancelar boleto: " + SBOApp.Company.GetLastErrorDescription());
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Erro ao cancelar boleto: " + ex.Message);
                            rst.MoveNext();
                            continue;
                        }
                        finally
                        {

                            Marshal.ReleaseComObject(boe);
                            boe = null;
                        }
                    }
                    else
                    {
                        Payments payment = (Payments)SBOApp.Company.GetBusinessObject(BoObjectTypes.oVendorPayments);
                        payment.GetByKey((int)rst.Fields.Item("DocNum").Value);
                        if (payment.Cancel() != 0)
                        {
                            //MessageBox.Show("Erro ao cancelar Pagamento: " + SBOApp.Company.GetLastErrorDescription());
                            rst.MoveNext();
                            continue;
                        }
                    }

                    JournalEntries lcm = (JournalEntries)SBOApp.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
                    lcm.GetByKey((int)rst.Fields.Item("DocEntry").Value);
                    if (lcm.Cancel() != 0)
                    {
                        //MessageBox.Show("Erro ao cancelar LCM: " + SBOApp.Company.GetLastErrorDescription());
                        rst.MoveNext();
                        continue;
                    }
                    
                    i++;
                }
                MessageBox.Show("Itens cancelados: " + i);

                oCompany.Disconnect();
            }
        }
    }
}
