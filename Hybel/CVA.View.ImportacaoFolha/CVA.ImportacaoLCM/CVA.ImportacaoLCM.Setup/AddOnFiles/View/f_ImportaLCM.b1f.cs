using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM.Framework;
using CVA.ImportacaoLCM.Controller;
using System.Windows.Forms;
using System.Threading;
using SAPbouiCOM;
using CVA.ImportacaoLCM.BLL;
using System.IO;
using CVA.ImportacaoLCM.Util;

namespace CVA.ImportacaoLCM.View
{
    [FormAttribute("CVA.ImportacaoLCM", "View/f_ImportaLCM.b1f")]
    class f_ImportaLCM : UserFormBase
    {


        public f_ImportaLCM()
        {
            
        }

        
        private SAPbouiCOM.ComboBox cbFilial;
        private SAPbouiCOM.ComboBox cb_CCusto;        

        private SAPbouiCOM.Button btPesquisa;
        private SAPbouiCOM.Button btImportar;
        private SAPbouiCOM.Button btCancelar;

        private StaticText StaticText1;
        private StaticText StaticText2;
        private StaticText StaticText3;
        private StaticText StaticText5;

        private EditText edLancto;
        private EditText edVencto;
        private EditText edArquivo;

        
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.cbFilial = ((SAPbouiCOM.ComboBox)(this.GetItem("cbFilial").Specific));
            this.edArquivo = ((SAPbouiCOM.EditText)(this.GetItem("edArquivo").Specific));
            this.btPesquisa = ((SAPbouiCOM.Button)(this.GetItem("btPesquisa").Specific));
            this.btPesquisa.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btPesquisa_ClickBefore);
            this.btImportar = ((SAPbouiCOM.Button)(this.GetItem("btImportar").Specific));
            this.btImportar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btImportar_ClickBefore);
            this.btCancelar = ((SAPbouiCOM.Button)(this.GetItem("btCancelar").Specific));
            this.btCancelar.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btCancelar_ClickBefore);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.edLancto = ((SAPbouiCOM.EditText)(this.GetItem("edLancto").Specific));
            this.edVencto = ((SAPbouiCOM.EditText)(this.GetItem("edVencto").Specific));
            this.cb_CCusto = ((SAPbouiCOM.ComboBox)(this.GetItem("cb_CCusto").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadBefore += new LoadBeforeHandler(this.Form_LoadBefore);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
           
            carregaFilial();
            CarregaCentroCusto();
        }
        private void carregaFilial()
        {
            string sql = SQL.SQL.CarregaFilial;

            try
            {
                cbFilial.AddValuesFromQuery(sql);
            }
            catch
            {
                sql = SQL.SQL.CarregaFilial_Hana;
                cbFilial.AddValuesFromQuery(sql);
            }

            this.UIAPIRawForm.Items.Item("cbFilial").DisplayDesc = true;
        }

        private void CarregaCentroCusto()
        {
            string sql = SQL.SQL.CarregaCentroCusto;
            try
            {
                cb_CCusto.AddValuesFromQuery(sql);
            }
            catch 
            {
                sql = SQL.SQL.CarregaCentroCusto_Hana;
                cb_CCusto.AddValuesFromQuery(sql);
            }         
          
            cb_CCusto.Select("0", BoSearchKey.psk_ByValue);
            this.UIAPIRawForm.Items.Item("cb_CCusto").DisplayDesc = true;
        }


        private void btPesquisa_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string arquivo = new DialogUtil().OpenFileDialog();
            if (!String.IsNullOrEmpty(arquivo))
            {
                edArquivo.Value = arquivo.ToString();

            }


        }

        private void btCancelar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.UIAPIRawForm.Close();
        }

        private void btImportar_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var log = new LogErro();
            int oper;

            string Caminho = String.Format("{0}{1}", log.GetTempPath() , "CVA.LogImportação.txt");
            log.FileExists();
            try
            {
                oper = SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Deseja realmente importar LCM?", 1, "Sim", "Não", "");

                if (oper != 1)
                    return;

                if (!ValidationFields.ValidateEditText(edArquivo))
                    throw new ArgumentNullException("Arquivo", "Campo obrigatório.");

                if (!ValidationFields.ValidateComboBox(cbFilial))
                    throw new ArgumentNullException("Filial", "Campo obrigatório.");

                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage("Aguarde o término do processamento.", BoMessageTime.bmt_Long, false);

                int empresa = Convert.ToInt32(cbFilial.Selected.Value);

              

                JournalVouchersBLL jdt = new JournalVouchersBLL();
                ArquivoBLL arq = new ArquivoBLL();
                var LCM = arq.Importar(edArquivo.Value.Trim());
                

                if (LCM != null)
                {


                    File.Delete(Caminho);

                    var cbCustos = cb_CCusto.Selected.Value.ToString();
                    // Adlteração no Centro de Custo
                    int ret = jdt.Criar(LCM, edLancto.Value, edVencto.Value, empresa,cbCustos);

                    if (ret == 0)
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("LCM criado com sucesso", 1, "OK", "", "");
                    }
                    else
                    {
                        SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("Não foi possivel importar arquivo, Verificar LOG", 1, "OK", "", "");
                        System.Diagnostics.Process.Start("notepad.exe", Caminho);
                    }

                }
                else
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("verificar arquivo para importação!", 1, "OK", "", "");
                }

            }
            catch (Exception ex)
            {

                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Long, false);

            }


        }

        private void Form_LoadBefore(SBOItemEventArg pVal, out bool BubbleEvent)
            {
                BubbleEvent = true;
                this.UIAPIRawForm.SupportedModes = 0;

            }
        
        
    }
}