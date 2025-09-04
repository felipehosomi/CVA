using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.View.AdicionaTabelaUsuario
{
    public partial class Form1 : Form
    {
        public Company oCompany = new Company();

        public Form1()
        {
            InitializeComponent();

            this.txtServer.Text = "SAPQAS";
            this.txtUserDB.Text = "sa";
            this.txtPasswDB.Text = "sa@#Atlantic";
            this.txtUserSAP.Text = "manager";
            this.txtPasswSAP.Text = "CVA$sap16";
            this.cbDbType.SelectedIndex = 6;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CVAApp.DatabaseName = "CVA_ATL_CON";
            CVAApp.ServerName = this.txtServer.Text;
            CVAApp.DBPassword = this.txtPasswDB.Text;
            CVAApp.DBUserName = this.txtUserDB.Text;

            SqlController sqlController = new SqlController();


            List<DocsModel> list = new List<DocsModel>();
            List<string> bases = sqlController.FillStringList("SELECT BASE FROM CVA_BASES");
            foreach (var banco in bases)
            {
                string sqlDocs = $@"SELECT '{banco}' Banco, OBPL.BPLName, OPOR.DocEntry, OPOR.DocDate, OPOR.DocTotal, OPOR.CardCode, OBPL.TaxIdNum, POR1.ItemCode
                                FROM [{banco}]..[OPOR]

                                    INNER JOIN [{banco}]..[POR1]
                                        ON POR1.DocEntry = OPOR.DocEntry

                                    INNER JOIN [{banco}]..OBPL
		                                ON OBPL.BPLid = OPOR.BPLid

                                WHERE OPOR.DocStatus = 'O'
                                AND OPOR.DataSource = 'O'
                                AND OPOR.DocEntry NOT IN
                                (
	                                SELECT ISNULL(U_NUMSAP, 0) FROM [SBO_PRD_ATL0001]..[@CVA_IMP_PED1]
	                                WHERE U_BASE = '{banco}'
                                )";

                list.AddRange(sqlController.FillModelListAccordingToSql<DocsModel>(sqlDocs));
            }
            if (!oCompany.Connected)
            {
                oCompany.LicenseServer = tbxLicenceServer.Text;
                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
                oCompany.Server = txtServer.Text;
                oCompany.DbUserName = txtUserDB.Text;
                oCompany.DbPassword = txtPasswDB.Text;
                oCompany.UserName = txtUserSAP.Text;
                oCompany.Password = txtPasswSAP.Text;
                oCompany.CompanyDB = "SBO_PRD_ATL0001";

                int err = oCompany.Connect();

                if (err != 0)
                {
                    string MsgErro;
                    oCompany.GetLastError(out err, out MsgErro);
                    MessageBox.Show(MsgErro);
                    return;
                }
                SBOApp.Company = oCompany;

                if (list.Count > 0)
                {
                    Insert(list);
                }
                oCompany.Disconnect();
            }

        }

        private void Insert(List<DocsModel> list)
        {
            var oCompanyService = SBOApp.Company.GetCompanyService();
            var oGeneralService = oCompanyService.GetGeneralService("CVA_IMP_PED");
            var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
            oGeneralData.SetProperty("Code", CrudController.GetNextCode("@CVA_IMP_PED").ToString().PadLeft(8, '0'));
            oGeneralData.SetProperty("U_ARQUIVO", "Manual");
            oGeneralData.SetProperty("U_DATA", DateTime.Today);
            oGeneralData.SetProperty("U_STATUS", "0");
            oGeneralData.SetProperty("U_LINHAS", list.Count);

            var oChildren = oGeneralData.Child("CVA_IMP_PED1");
            foreach (var linha in list)
            {
                var oChild = oChildren.Add();
                oChild.SetProperty("U_CNPJ", linha.TaxIdNum);
                oChild.SetProperty("U_EMPRESA", linha.BPLName);
                oChild.SetProperty("U_BASE", linha.Banco);
                oChild.SetProperty("U_STATUS", 1);
                oChild.SetProperty("U_LINHA", 0);
                oChild.SetProperty("U_LOG", "Pedido gerado com sucesso!");
                oChild.SetProperty("U_PN", linha.CardCode);
                oChild.SetProperty("U_LCTO", linha.DocDate);
                oChild.SetProperty("U_ITEM", linha.ItemCode);
                oChild.SetProperty("U_QTD", 1);
                oChild.SetProperty("U_VALOR", (double)linha.DocTotal);
                oChild.SetProperty("U_NUMSAP", linha.DocEntry.ToString());
            }

            oGeneralService.Add(oGeneralData);

            Marshal.ReleaseComObject(oChildren);
            Marshal.ReleaseComObject(oCompanyService);
            Marshal.ReleaseComObject(oGeneralService);
            Marshal.ReleaseComObject(oGeneralData);

            oCompanyService = null;
            oGeneralService = null;
            oGeneralData = null;
            oChildren = null;
        }
    }
}
