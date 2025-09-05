using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Electra.ImportaFolha
{

    class ImportaFolha
    {
        public static SAPbouiCOM.Application oApplication;
        private SAPbobsCOM.Company oCompany;

        public ImportaFolha()
        {
            SetApplication();
            AddMenuItems();
            oApplication.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_MenuEvent);
            oApplication.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            oApplication.AppEvent += new _IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
        }

        private void SetApplication()
        {

            // *******************************************************************
            //  Use an SboGuiApi object to establish connection
            //  with the SAP Business One application and return an
            //  initialized appliction object
            // *******************************************************************

            SAPbouiCOM.SboGuiApi SboGuiApi = null;
            string sConnectionString = null;

            SboGuiApi = new SAPbouiCOM.SboGuiApi();

            //  by following the steps specified above, the following
            //  statment should be suficient for either development or run mode

            sConnectionString = System.Convert.ToString(Environment.GetCommandLineArgs().GetValue(1));

            //  connect to a running SBO Application

            SboGuiApi.Connect(sConnectionString);

            //  get an initialized application object

            oApplication = SboGuiApi.GetApplication(-1);
            oCompany = (SAPbobsCOM.Company) oApplication.Company.GetDICompany();

            LogInfoFormat("O Add-on Importador de Folha de Pagamento esta conectado.");

        }

        private void LogErrorFormat(string sMsg)
        {
            oApplication.SetStatusBarMessage(sMsg);
        }

        private void LogInfoFormat(string sMsg)
        {
            oApplication.SetStatusBarMessage(sMsg, BoMessageTime.bmt_Long, false);
        }

        private void load()
        {
            string strTmp = "form.srf";
            LoadFromXML(ref strTmp);
            var oForm = oApplication.Forms.Item("UBK000006");
            oForm.Visible = true;

        }

        private void LoadFromXML(ref string FileName)
        {
            System.Xml.XmlDocument oXmlDoc = null;
            oXmlDoc = new System.Xml.XmlDocument();
            string sPath = null;
            sPath = GetExecutingDirectoryName();// System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath).ToString()).ToString();
            oXmlDoc.Load(sPath + @"\" + FileName);
            string strXML = oXmlDoc.InnerXml.ToString();
            try
            {
                oApplication.LoadBatchActions(ref strXML);
            }
            catch (Exception ex)
            {
                oApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }

        }

        public static string GetExecutingDirectoryName()
        {

            return System.Windows.Forms.Application.ExecutablePath.Replace("CVA.Electra.ImportaFolha.EXE", "").Replace("CVA.Electra.ImportaFolha.exe", "");
           //var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
           // return new FileInfo(location.AbsolutePath).Directory.FullName;
        }

        private void AddMenuItems()
        {

            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = oApplication.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = oApplication.Menus.Item("1536");


            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            oCreationPackage.UniqueID = "FOLHAPGTO";
            oCreationPackage.String = "CVA: Importar Folha de Pagamento";
            oCreationPackage.Enabled = true;

            oCreationPackage.Position = 1;

            oMenus = oMenuItem.SubMenus;

            try
            {
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { 
                LogErrorFormat("Menu Already Exists");
            }
        }

        public void SBO_Application_AppEvent(BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case BoAppEventTypes.aet_CompanyChanged:
                    System.Windows.Forms.Application.Exit();
                    Environment.Exit(0);
                    break;
                case BoAppEventTypes.aet_ServerTerminition:
                case BoAppEventTypes.aet_ShutDown:
                    //Events.KillProcess();
                    System.Windows.Forms.Application.Exit();
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        private SAPbouiCOM.Form oForm;
        private SAPbouiCOM.EditText txtLancamento;
        private SAPbouiCOM.EditText txtVencimento;
        private SAPbouiCOM.Button btnLocaliza;
        private SAPbouiCOM.ComboBox cbFilial;
        
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {            
            if (pVal.FormType == 60004)
            {
                SAPbouiCOM.BoEventTypes EventEnum = 0;
                EventEnum = pVal.EventType;
                if (FormUID.Equals("UBK000006") & (EventEnum == SAPbouiCOM.BoEventTypes.et_FORM_VISIBLE) & pVal.BeforeAction==false)
                {
                    //oForm = oApplication.Forms.Item(FormUID);
                    //if (this.txtLancamento==null)
                    //{
                    try
                    {
                        
                        oForm = oApplication.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
                        //oForm.Freeze(true);

                        this.txtLancamento = ((SAPbouiCOM.EditText)(oForm.Items.Item("txtLanca")).Specific);
                        this.txtVencimento = ((SAPbouiCOM.EditText)(oForm.Items.Item("txtVenc")).Specific);
                        this.btnLocaliza = ((SAPbouiCOM.Button)(oForm.Items.Item("btnLocaliz")).Specific);
                        this.cbFilial = ((SAPbouiCOM.ComboBox)(oForm.Items.Item("6")).Specific);
                        oForm.DataSources.UserDataSources.Add("DateDS", SAPbouiCOM.BoDataType.dt_DATE, 10);
                        oForm.DataSources.UserDataSources.Add("DateDS1", SAPbouiCOM.BoDataType.dt_DATE, 10);
                        txtLancamento.DataBind.SetBound(true, "", "DateDS");
                        txtVencimento.DataBind.SetBound(true, "", "DateDS1");

                        SAPbobsCOM.Recordset oRecordset =
                        (SAPbobsCOM.Recordset)
                            oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordset.DoQuery("select  \"BPLId\",\"BPLName\"  from \"OBPL\" where \"Disabled\"='N' order by 2");
                        oRecordset.MoveFirst();

                        while (oRecordset.EoF == false) //
                        {
                            cbFilial.ValidValues.Add(
                                Convert.ToString(oRecordset.Fields.Item("BPLId").Value)
                                , Convert.ToString(oRecordset.Fields.Item("BPLName").Value)
                                );


                            oRecordset.MoveNext();
                        }
                        oRecordset.MoveFirst();
                        cbFilial.Item.DisplayDesc = true;
                        cbFilial.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                        cbFilial.Item.Width = 200;

                        //oForm.Freeze(false);
                    }
                    catch (Exception)
                    {

                        //throw;
                    }

                    //}

                    //oForm.Items.Item("");
                }
                else if (FormUID.Equals("UBK000006") & (pVal.ItemUID.Equals("btnLocaliz")) & pVal.BeforeAction == false)
                {
                    if (txtLancamento.Value.Equals("") && txtVencimento.Value.Equals(""))
                    {
                        oApplication.MessageBox("Os campos Data de Lançamento e Data de Vencimento não estão preenchidos.");
                    }
                    else if (txtLancamento.Value.Equals(""))
                    {
                        oApplication.MessageBox("O campo Data de Lançamento não está preenchido.");
                    }
                    else if (txtVencimento.Value.Equals(""))
                    {
                        oApplication.MessageBox("O campo Data de Vencimento não está preenchido.");
                    }
                    else
                    {
                        SelecionaArquivo();
                        //var fileDialog = new SelectFileDialog("c:\\", "", "Arquivo CSV|*.csv|Todos os arquivos|*.*", DialogType.OPEN);
                        //fileDialog.Open();
                        //string file = fileDialog.SelectedFile;

                        //OpenFileDialog openFileDialog1 = new OpenFileDialog();
                        //openFileDialog1.Filter = "Arquivo CSV|*.csv|Todos os arquivos|*.*";
                        //openFileDialog1.InitialDirectory = "c:\\";
                        //openFileDialog1.Title = "Selecione o Arquivo";


                        //if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        //{
                        //    // Assign the cursor in the Stream to the Form's Cursor property.  
                        //    //this.Cursor = new Cursor(openFileDialog1.OpenFile());
                        //}
                        //if (openFileDialog1.FileName==null)
                        //{

                        //}
                        //if (file.ToUpper().Contains(".CSV"))
                        //{
                        //    try
                        //    {
                        //        FileCSV arquivo = new FileCSV();
                        //        Boolean flag = arquivo.lerArquivo(file);
                        //        if (flag == false)
                        //        {
                        //            Application.MessageBox("O arquivo selecionado está vazio.");
                        //            return;
                        //        }
                        //        else //validações
                        //        {

                        //            //Validação das Contas do arquivo .csv

                        //            var lstErros = fileCSVService.validaConta(arquivo.query, Application);

                        //            if (lstErros.Count() > 0)
                        //            {
                        //                string msg = string.Format("Contas/PN {0} não existente.", string.Join(", ", lstErros));
                        //                Application.MessageBox(msg);
                        //                return;
                        //            }

                        //            //Validação dos Centros de Custo do arquivo .csv

                        //            lstErros = fileCSVService.validaCentroCusto(arquivo.query, Application);

                        //            if (lstErros.Count() > 0)
                        //            {
                        //                string msg = string.Format("Centro de Custo {0} não existente.", string.Join(", ", lstErros));
                        //                Application.MessageBox(msg);
                        //                return;
                        //            }

                        //            //Somatória Débito e Crédito

                        //            //if (arquivo.query.Sum(a => a.ValorCredito) != arquivo.query.Sum(a => a.ValorDebito)) 
                        //            //{
                        //            //    Application.MessageBox("A somatória das colunas Débito e Crédito devem ser Iguais.");
                        //            //    return;
                        //            //}

                        //            //Chama método geraLancamento

                        //            string codigo = fileCSVService.geraLancamento(arquivo.query, txtLancamento, txtVencimento, Convert.ToInt32(cbFilial.Value), Application);

                        //            if (codigo != "")
                        //            {
                        //                Application.MessageBox("O arquivo foi carregado com sucesso. Criado o lançamento " + codigo + ".");
                        //            }

                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Application.MessageBox("Ocorreu um problema na importação do arquivo selecionado:" + ex.Message);
                        //    }
                        //}
                        //else if (!file.Equals(""))
                        //{
                        //    Application.MessageBox("Arquivo inválido.");
                        //}
                    }
                }
                //
                //if ((EventEnum != SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE) & (EventEnum != SAPbouiCOM.BoEventTypes.et_FORM_LOAD))
                //{
                //    oApplication.SetStatusBarMessage("An " + EventEnum.ToString() + " has been sent by a form with the unique ID: " + FormUID, SAPbouiCOM.BoMessageTime.bmt_Short, false);
                //}
            }
            BubbleEvent = true;
        }
        //[STAThread]
        private void SelecionaArquivo()
        {
            //Thread t = new Thread(() =>
            //{
                //OpenFileDialog openFileDialog1 = new OpenFileDialog();
                //openFileDialog1.Filter = "Arquivo CSV|*.csv|Todos os arquivos|*.*";
                //openFileDialog1.InitialDirectory = "c:\\";
                //openFileDialog1.Title = "Selecione o Arquivo";

                string ret = Support.GetFileNameViaOFD("Arquivo CSV|*.csv|Todos os arquivos|*.*", "c:\\", "Selecione o Arquivo", true);
                if (!string.IsNullOrEmpty(ret))
                {
                    //openFileDialog1.SafeFileName

                    if (ret.ToUpper().Contains(".CSV"))
                    {
                        try
                        {
                            FileCSV arquivo = new FileCSV();
                            Boolean flag = arquivo.lerArquivo(ret);
                            if (flag == false)
                            {
                                oApplication.MessageBox("O arquivo selecionado está vazio.");
                                return;
                            }
                            else //validações
                            {
                                FileCSVService fileCSVService = new FileCSVService(oCompany, oApplication);
                                //Validação das Contas do arquivo .csv

                                var lstErros = fileCSVService.validaConta(arquivo.query);

                                if (lstErros.Count() > 0)
                                {
                                    string msg = string.Format("Contas/PN {0} não existente.", string.Join(", ", lstErros));
                                    oApplication.MessageBox(msg);
                                    return;
                                }

                                //Validação dos Centros de Custo do arquivo .csv

                                lstErros = fileCSVService.validaCentroCusto(arquivo.query);

                                if (lstErros.Count() > 0)
                                {
                                    string msg = string.Format("Centro de Custo {0} não existente.", string.Join(", ", lstErros));
                                    oApplication.MessageBox(msg);
                                    return;
                                }

                                string codigo = fileCSVService.geraLancamento(arquivo.query, txtLancamento, txtVencimento, Convert.ToInt32(cbFilial.Value));

                                if (codigo != "")
                                {
                                    oApplication.MessageBox("O arquivo foi carregado com sucesso. Criado o lançamento " + codigo + ".");
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            oApplication.MessageBox("Ocorreu um problema na importação do arquivo selecionado:" + ex.Message);
                        }
                    }
                    //else if (!openFileDialog1.SafeFileName.Equals(""))
                    //{
                    //    oApplication.MessageBox("Arquivo inválido.");
                    //}
                    //((SAPbouiCOM.Button)(oForm.Items.Item("btnLocaliz")).Specific).Caption = "asd";
                    // Assign the cursor in the Stream to the Form's Cursor property.  
                    //this.Cursor = new Cursor(openFileDialog1.OpenFile());
                }
            //});// Kick off a new thread
            //t.IsBackground = true;
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "Arquivo CSV|*.csv|Todos os arquivos|*.*";
            //openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Title = "Selecione o Arquivo";


            //if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    // Assign the cursor in the Stream to the Form's Cursor property.  
            //    //this.Cursor = new Cursor(openFileDialog1.OpenFile());
            //}
            //if (openFileDialog1.FileName == null)
            //{

            //}
        }

        private void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {                
                if (pVal.BeforeAction == true)
                {
                    if (pVal.MenuUID.Equals("FOLHAPGTO"))
                    {
                        load();
                    }
                }
            }
            catch (Exception ex)
            {

                oApplication.MessageBox(ex.Message);
            }

        }
    }
}
