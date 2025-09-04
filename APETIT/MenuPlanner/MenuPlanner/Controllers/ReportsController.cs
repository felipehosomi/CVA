using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MenuPlanner.Views;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace MenuPlanner.Controllers
{
    class ReportsController
    {
        private FolderBrowserDialog folderDialog;
        private FileDialog openDialog;
        private PrintDialog printDialog;
        private DialogResult resultDialog;
        private ConnectionInfo ReportConnectionInfo;
        public string CrystalDirectory;

        public void ExecuteCrystalReport(string fileName, string idPlan)
        {
            try
            {
                var reportDocument = new ReportDocument();
                reportDocument.Load(Path.Combine(Environment.CurrentDirectory, "Reports", fileName));

                var userID = CommonController.Company.DbUserName;
                var password = ConfigurationManager.AppSettings["Password"];
                var server = CommonController.Company.Server;
                var database = CommonController.Company.CompanyDB;

                var strConnection = $"DRIVER={{HDBODBC}};UID={userID};PWD={password};SERVERNODE={server};CS={database}";

                var logonProps = reportDocument.DataSourceConnections[0].LogonProperties;
                reportDocument.DataSourceConnections[0].SetConnection(server, database, userID, password);

                logonProps.Set("Connection String", strConnection);
                reportDocument.DataSourceConnections[0].SetLogonProperties(logonProps);

                reportDocument.SetParameterValue("schema@", CommonController.Company.CompanyDB);
                reportDocument.SetParameterValue("idPlanejamento", idPlan);

                var frm = new ReportViewer();
                frm.crystalReportViewer1.ReportSource = reportDocument;
                frm.Text = reportDocument.SummaryInfo.ReportTitle;
                ShowCrystalReport(frm);

                reportDocument.Close();
                reportDocument.Dispose();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        private void CreateReportConnectionInfo()
        {
            string serverName = CommonController.Company.Server;
            if (serverName.ToLower().Contains("(local)"))
            {
                serverName = serverName.ToLower().Replace("(local)", Environment.MachineName);
            }

            //var userName = ConfigurationSettings.AppSettings["UserID"];
            var Password = ConfigurationSettings.AppSettings["Password"];

            string userName = CommonController.Company.DbUserName;

            string dataBase = CommonController.Company.CompanyDB;
            ReportConnectionInfo = new ConnectionInfo();
            ReportConnectionInfo.ServerName = serverName;
            ReportConnectionInfo.DatabaseName = dataBase;
            ReportConnectionInfo.UserID = userName;
            ReportConnectionInfo.Password = Password;
        }

        public void ExecuteCrystalReport(string fileName, Hashtable reportParams)
        {
            var report = new ReportDocument();

            try
            {

                CreateReportConnectionInfo();

                string dataBase = CommonController.Company.CompanyDB;
                reportParams.Add("schema@", dataBase);

                var filePath = Path.Combine(Environment.CurrentDirectory, "Reports", fileName);

                report.FileName = filePath;

                //SetReportDBLogon(report);
                SetReportParameters(report, reportParams);


                //report.SetParameterValue("schema@", dataBase);
                //report.SetParameterValue("idPlanejamento", "94");


                //var frm = new ReportViewer();
                //frm.crystalReportViewer1.ReportSource = report;
                //ShowCrystalReport(frm);
            }
            catch (Exception ex)
            {
                var t = ex.InnerException;
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                report.Dispose();
                report = null;
                reportParams = null;
                ReportConnectionInfo = null;
                GC.Collect();
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
                    param.CurrentValues.Clear();

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

        //private void ShowCrystalReport(frmRelatorios frm)
        //{
        //    System.Threading.Thread thrCrystalReport = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InternalShowCrystalReport));
        //    try
        //    {
        //        if (thrCrystalReport.ThreadState == System.Threading.ThreadState.Unstarted)
        //        {
        //            thrCrystalReport.SetApartmentState(System.Threading.ApartmentState.STA);
        //            thrCrystalReport.Start(frm);
        //        }
        //        else if (thrCrystalReport.ThreadState == System.Threading.ThreadState.Stopped)
        //        {
        //            thrCrystalReport.Start(frm);
        //            thrCrystalReport.Join();
        //        }

        //        while (thrCrystalReport.ThreadState == System.Threading.ThreadState.Running)
        //        {
        //            System.Windows.Forms.Application.DoEvents();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
        //    }
        //    finally
        //    {
        //        System.Windows.Forms.Application.ExitThread();
        //    }
        //}

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
        public static extern IntPtr GetForegroundWindow();

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
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
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
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
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
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                thrOpenPrint.Abort();
                thrOpenPrint = null;
            }

            return resultDialog;
        }

        private void ShowCrystalReport_1(ReportViewer frm)
        {
            System.Threading.Thread thrCrystalReport = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InternalShowCrystalReport));
            thrCrystalReport.SetApartmentState(System.Threading.ApartmentState.STA);

            try
            {
                thrCrystalReport.Start(frm);

                while (!thrCrystalReport.IsAlive)
                {
                    System.Threading.Thread.Sleep(1);
                }

                thrCrystalReport.Join();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                thrCrystalReport.Abort();
                thrCrystalReport = null;
            }
        }

        private static void InternalShow(object frm)
        {
            if (frm != null)
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);

                //TODO: Acertar
                var frmCrystal = new ReportViewer();
                frmCrystal.crystalReportViewer1.ReportSource = (frm as ReportViewer).crystalReportViewer1.ReportSource;
                frmCrystal.crystalReportViewer1.ParameterFieldInfo = (frm as ReportViewer).crystalReportViewer1.ParameterFieldInfo;
                frmCrystal.Text = ((ReportDocument)(frm as ReportViewer).crystalReportViewer1.ReportSource).SummaryInfo.ReportTitle;
                //frmCrystal.crystalReportViewer1.PrintReport();
                frmCrystal.ShowDialog(oWindow);
                //frmCrystal.Close();
                oWindow = null;
            }
        }

        private void ShowCrystalReport(ReportViewer frm)
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

                while (!thrCrystalReport.IsAlive)
                {
                    System.Threading.Thread.Sleep(1);
                }

                thrCrystalReport.Join();

                //while (thrCrystalReport.ThreadState == System.Threading.ThreadState.Running)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //}
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                System.Windows.Forms.Application.ExitThread();
            }
        }

        private static void InternalShowCrystalReport(object frm)
        {
            if (frm != null)
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);

                //TODO: Acertar
                var frmCrystal = new ReportViewer();
                frmCrystal.crystalReportViewer1.ReportSource = (frm as ReportViewer).crystalReportViewer1.ReportSource;
                frmCrystal.crystalReportViewer1.ParameterFieldInfo = (frm as ReportViewer).crystalReportViewer1.ParameterFieldInfo;
                frmCrystal.Text = ((ReportDocument)(frm as ReportViewer).crystalReportViewer1.ReportSource).SummaryInfo.ReportTitle;
                //frmCrystal.crystalReportViewer1.PrintReport();
                frmCrystal.ShowDialog(oWindow);
                //frmCrystal.Close();
                oWindow = null;
            }
        }

        #endregion
    }
}
