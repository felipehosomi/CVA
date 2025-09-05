using System;
using System.Windows.Forms;
using SAPbouiCOM;
using SAPbobsCOM;
using System.Collections.Generic;
using System.Xml;
using System.Collections;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Conferencia.Arquivo;

namespace Conferencia
{
    public class CrystalReport
    {
        private FolderBrowserDialog folderDialog;
        private FileDialog openDialog;
        private PrintDialog printDialog;
        private DialogResult resultDialog;
        private ConnectionInfo ReportConnectionInfo;
        public string CrystalDirectory;

        public void CreateReportConnectionInfo()
        {
            //string serverName = SBOApp.Company.Server;
            //if (serverName.ToLower().Contains("(local)"))
            //{
            //    serverName = serverName.ToLower().Replace("(local)", Environment.MachineName);
            //}


            //var UserID = ConfigurationSettings.AppSettings["UserID"];
            //var Passwrod = ConfigurationSettings.AppSettings["Password"];
            //var Server = ConfigurationSettings.AppSettings["Server"];
            //var DataBase = ConfigurationSettings.AppSettings["DataBase"];

            //ReportConnectionInfo = new ConnectionInfo();
            //ReportConnectionInfo.ServerName = Server;
            //ReportConnectionInfo.DatabaseName = DataBase;
            //ReportConnectionInfo.UserID = UserID;
            //ReportConnectionInfo.Password = Passwrod;
        }

        public void ExecuteCrystalReport(string fileRpt, string pedidoIni, string pedidoFim, DateTime dataIni, DateTime dataFim, string impresso)
        {
            try
            {
                var xml = new XMLReader();
                ReportDocument report = new ReportDocument();

                report.Load(fileRpt);

                var UserID = xml.UserID();
                var Passwrod = xml.Password();
                var Server = xml.Server();
                var DataBase = xml.Base();


                string strConnection = $"DRIVER={{HDBODBC}};UID={UserID};PWD={Passwrod};SERVERNODE={Server};DATABASE={DataBase}";

                NameValuePairs2 logonProps2 = report.DataSourceConnections[0].LogonProperties;

                logonProps2.Set("Connection String", strConnection);

                report.DataSourceConnections[0].SetLogonProperties(logonProps2);

                report.SetParameterValue("PedidoIni", pedidoIni);
                report.SetParameterValue("PedidoFim", pedidoFim);
                report.SetParameterValue("DataInicio", dataIni);
                report.SetParameterValue("DataFinal", dataFim);
                report.SetParameterValue("Impresso", impresso);
                report.SetParameterValue("Schema@", ConexaoSAP.Conexao.oCompany.CompanyDB);


                frmRelatorios frm = new frmRelatorios();
                frm.crystalReportViewer1.ReportSource = report;
                frm.crystalReportViewer1.Refresh();
                frm.Text = report.SummaryInfo.ReportTitle;
                frm.Show();
                //ShowCrystalReport(frm);
            }
            catch (Exception e)
            {
                //SBOApp.Application.StatusBar.SetText(e.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
        }

        public void SetReportDBLogon(CrystalDecisions.CrystalReports.Engine.ReportDocument report)
        {
            Sections sections = report.ReportDefinition.Sections;
            foreach (CrystalDecisions.CrystalReports.Engine.Section section in sections)
            {
                ReportObjects reportObjects = section.ReportObjects;
                foreach (ReportObject reportObject in reportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subreportObject = (SubreportObject)reportObject;
                        ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);

                        SetReportDBLogon(subReportDocument);
                    }
                }
            }

            foreach (CrystalDecisions.CrystalReports.Engine.Table table in report.Database.Tables)
            {
                TableLogOnInfo tableLogonInfo = table.LogOnInfo;
                tableLogonInfo.ConnectionInfo = ReportConnectionInfo;
                table.ApplyLogOnInfo(tableLogonInfo);
            }

            report.VerifyDatabase();
        }

        private static void SetReportParameters(ReportDocument report, Hashtable reportParams)
        {
            if (reportParams != null)
            {
                foreach (string key in reportParams.Keys)
                {
                    ParameterField param = report.ParameterFields[key];
                    if (param != null)
                    {
                        if (reportParams[key] is IList)
                        {
                            foreach (object value in (IList)reportParams[key])
                            {
                                ParameterDiscreteValue paramValue = new ParameterDiscreteValue();
                                paramValue.Value = value;
                                param.CurrentValues.Add(paramValue);
                            }
                        }
                        else
                        {
                            ParameterDiscreteValue paramValue = new ParameterDiscreteValue();
                            paramValue.Value = reportParams[key];
                            param.CurrentValues.Add(paramValue);
                        }
                    }

                }
            }
        }

        private void ShowCrystalReport(frmRelatorios frm)
        {
            System.Threading.Thread thrCrystalReport = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InternalShowCrystalReport));
            try
            {
                if (thrCrystalReport.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    thrCrystalReport.SetApartmentState(System.Threading.ApartmentState.STA);
                    thrCrystalReport.Start(frm);
                }
                else if (thrCrystalReport.ThreadState == System.Threading.ThreadState.Stopped)
                {
                    thrCrystalReport.Start(frm);
                    thrCrystalReport.Join();
                }

                while (thrCrystalReport.ThreadState == System.Threading.ThreadState.Running)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            catch (Exception e)
            {
                //SBOApp.Application.StatusBar.SetText(e.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                System.Windows.Forms.Application.ExitThread();
            }
        }

        internal class WindowWrapper : System.Windows.Forms.IWin32Window
        {
            private IntPtr _hwnd;

            public virtual IntPtr Handle
            {
                get { return _hwnd; }
            }

            public WindowWrapper(IntPtr handle)
            {
                _hwnd = handle;
            }
        }

        #region 'Dialog'
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private void InternalFolderBrowserDialog()
        {
            IntPtr ptr = GetForegroundWindow();
            WindowWrapper oWindow = new WindowWrapper(ptr);
            resultDialog = folderDialog.ShowDialog(oWindow);
            oWindow = null;
        }

        /// <summary>
        /// Run a System.Windows.Forms.FolderBrowserDialog by a specific thread
        /// </summary>
        /// <param name="dlg">FolderBrowserDialog object with the necessary settings.</param>
        /// <returns>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box; otherwise, System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        public DialogResult SAPFolderBrowserDialog(FolderBrowserDialog dlg)
        {
            System.Threading.Thread thrFolderBrowser = new System.Threading.Thread(new System.Threading.ThreadStart(InternalFolderBrowserDialog));
            thrFolderBrowser.SetApartmentState(System.Threading.ApartmentState.STA);

            try
            {
                folderDialog = dlg;

                thrFolderBrowser.Start();
                while (!thrFolderBrowser.IsAlive)
                {
                    System.Threading.Thread.Sleep(1);
                }
                thrFolderBrowser.Join();
            }
            catch (Exception e)
            {
                //SBOApp.Application.StatusBar.SetText(e.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                thrFolderBrowser.Abort();
                thrFolderBrowser = null;
            }

            return resultDialog;
        }

        private void InternalOpenFile()
        {
            IntPtr ptr = GetForegroundWindow();
            WindowWrapper oWindow = new WindowWrapper(ptr);
            resultDialog = openDialog.ShowDialog(oWindow);
            oWindow = null;
        }

        /// <summary>
        /// Run a System.Windows.Forms.FileDialog by a specific thread
        /// </summary>
        /// <param name="dlg">FileDialog object with the necessary settings.</param>
        /// <returns>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box; otherwise, System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        public DialogResult SAPFileDialog(FileDialog dlg)
        {
            System.Threading.Thread thrOpenFile = new System.Threading.Thread(new System.Threading.ThreadStart(InternalOpenFile));
            thrOpenFile.SetApartmentState(System.Threading.ApartmentState.STA);

            try
            {
                openDialog = dlg;

                thrOpenFile.Start();
                while (!thrOpenFile.IsAlive)
                {
                    System.Threading.Thread.Sleep(1);
                }
                thrOpenFile.Join();
            }
            catch (Exception e)
            {
                //SBOApp.Application.StatusBar.SetText(e.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                thrOpenFile.Abort();
                thrOpenFile = null;
            }

            return resultDialog;
        }

        private void InternalOpenPrint()
        {
            IntPtr ptr = GetForegroundWindow();
            WindowWrapper oWindow = new WindowWrapper(ptr);
            resultDialog = printDialog.ShowDialog(oWindow);
            oWindow = null;
        }

        /// <summary>
        /// Run a System.Windows.Forms.FileDialog by a specific thread
        /// </summary>
        /// <param name="dlg">FileDialog object with the necessary settings.</param>
        /// <returns>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box; otherwise, System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        public DialogResult SAPPrintDialog(PrintDialog dlg)
        {
            System.Threading.Thread thrOpenPrint = new System.Threading.Thread(new System.Threading.ThreadStart(InternalOpenPrint));
            thrOpenPrint.SetApartmentState(System.Threading.ApartmentState.STA);

            try
            {
                printDialog = dlg;

                thrOpenPrint.Start();
                while (!thrOpenPrint.IsAlive)
                {
                    System.Threading.Thread.Sleep(1);
                }
                thrOpenPrint.Join();
            }
            catch (Exception e)
            {
                //SBOApp.Application.StatusBar.SetText(e.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                thrOpenPrint.Abort();
                thrOpenPrint = null;
            }

            return resultDialog;
        }

        //private void ShowCrystalReport(frmRelatorios frm)
        //{
        //    System.Threading.Thread thrCrystalReport = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(SAPAddOn.InternalShowCrystalReport));
        //    thrCrystalReport.SetApartmentState(System.Threading.ApartmentState.STA);

        //    try
        //    {
        //        thrCrystalReport.Start(frm);

        //        while (!thrCrystalReport.IsAlive)
        //        {
        //            System.Threading.Thread.Sleep(1);
        //        }
        //        thrCrystalReport.Join();
        //    }
        //    catch (Exception e)
        //    {
        //        StatusBarMessage(e.Message, BoStatusBarMessageType.smt_Error);
        //    }
        //    finally
        //    {
        //        thrCrystalReport.Abort();
        //        thrCrystalReport = null;
        //    }
        //}

        private static void InternalShowCrystalReport(object frm)
        {
            if (frm != null)
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);

                //TODO: Acertar
                frmRelatorios frmCrystal = new frmRelatorios();
                frmCrystal.crystalReportViewer1.ReportSource = (frm as frmRelatorios).crystalReportViewer1.ReportSource;
                frmCrystal.crystalReportViewer1.ParameterFieldInfo = (frm as frmRelatorios).crystalReportViewer1.ParameterFieldInfo;
                //frmCrystal.Text = (frm as frmRelatorios).Text;
                frmCrystal.ShowDialog(oWindow);
                oWindow = null;
            }
        }

        #endregion
    }
}
