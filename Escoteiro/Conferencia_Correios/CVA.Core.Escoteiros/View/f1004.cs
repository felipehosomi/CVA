using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Core.Escoteiros.DAO;
using CVA.Core.Escoteiros.Model;
using SAPbouiCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CVA.Core.Escoteiros.View
{
    //[CVA.AddOn.Common.Attributes.Form(3048)]
    public class f1004 : BaseForm
    {
        Form Form;
        Form _Form;
        private Grid _grid;
        public static string Path;

        #region Constructor
        public f1004()
        {
            FormCount++;
        }

        public f1004(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f1004(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f1004(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            try
            {
                Form = (Form)base.Show();
                ((ComboBox)Form.Items.Item("cb_desp").Specific).AddValuesFromQuery(DAO.Query.CarregaComboDesp);
                ((ComboBox)Form.Items.Item("cb_Trasnp").Specific).AddValuesFromQuery(DAO.Query.CarregaComboTrnsp);


                CarregaFormulario();

                var dtini = DateTime.Now.ToString("yyyyMMdd");

                ((EditText)Form.Items.Item("tx_dtIni").Specific).Value = dtini;
                ((EditText)Form.Items.Item("tx_dtFim").Specific).Value = dtini;

            }
            catch (Exception ex)
            {

                SBOApp.Application.SetStatusBarMessage("Show Form -> " + ex.Message);
            }
            return Form;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                SAPbouiCOM.Form oForm = SBOApp.Application.Forms.Item(ItemEventInfo.FormUID);

                switch (ItemEventInfo.EventType)
                {
                    case BoEventTypes.et_CLICK:
                        try
                        {
                            if (ItemEventInfo.ItemUID == "btn_Pesq")
                            {
                                this.CarregaGrid();
                            }
                            if (ItemEventInfo.ItemUID == "btn_IntCo")
                            {
                                ComboBox cb = (ComboBox)oForm.Items.Item("cb_desp").Specific;
                                string TipoDespacho = cb.Selected.Description;

                                if (TipoDespacho.StartsWith("SEDEX") || TipoDespacho.StartsWith("PAC"))
                                //if (Transportadora.ToUpper() == "CORREIOS")
                                {
                                    this.IntegraçãoCorreio();
                                    oForm.Items.Item("btn_ListP").Enabled = true;
                                }
                                else
                                {
                                    SBOApp.Application.SetStatusBarMessage("Funcionalidade Apenas para SEDEX ou PAC");
                                }
                            }

                            if (oForm.Items.Item("btn_ListP").Enabled == true && ItemEventInfo.ItemUID == "btn_ListP")
                            {
                                this.GerarRelatorio();
                                ComboBox cb = (ComboBox)oForm.Items.Item("cb_desp").Specific;
                                string TipoDespacho = cb.Selected.Value;
                            }

                            try
                            {
                                ComboBox _cb = (ComboBox)oForm.Items.Item("cb_desp").Specific;
                                if (_cb.Selected != null)
                                {
                                    string _TipoDespacho = _cb.Selected.Description;

                                    if (_TipoDespacho.StartsWith("TRANSPORTADORA"))
                                    {
                                        oForm.Items.Item("btn_ListP").Enabled = true;
                                        oForm.Items.Item("btn_IntCo").Enabled = false;
                                    }
                                }
                               
                            }
                            catch (Exception ex)
                            {

                                SBOApp.Application.SetStatusBarMessage("Selecione o Tipo de Despacho.",BoMessageTime.bmt_Short,true);
                            }
                            
                            


                        }
                        catch (Exception ex)
                        {
                            SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                        break;
                }
            }
            return true;
        }

        public static void CarregaFormulario()
        {
            SAPbouiCOM.Form Form = SBOApp.Application.Forms.ActiveForm;



            //((EditText)Form.Items.Item("tx_dtIni").Specific).Value = DtInicial.ToString("dd/MM/yyyy");
            //((EditText)Form.Items.Item("tx_dtFim").Specific).Value = DtFinal.ToString("dd/MM/yyyy");

            Form.DataSources.UserDataSources.Add("U_DtIni", SAPbouiCOM.BoDataType.dt_DATE, 10);
            SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_dtIni").Specific;
            oEdit.DataBind.SetBound(true, "", "U_DtIni");


            Form.DataSources.UserDataSources.Add("U_DtFim", SAPbouiCOM.BoDataType.dt_DATE, 10);
            oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_dtFim").Specific;
            oEdit.DataBind.SetBound(true, "", "U_DtFim");




        }

        public void GerarRelatorio()
        {

            SAPbouiCOM.Form oForm = SBOApp.Application.Forms.ActiveForm;

            DateTime DtInicial = DateTime.Parse("1900/01/01");
            DateTime DtFinal = DateTime.Parse("1900/01/01");
            string Despacho = string.Empty;
            string Transportadora = string.Empty;
            string listaSerial = string.Empty;
            string schema = string.Empty;

            DtInicial = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtIni").Value);
            DtFinal = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtFim").Value);

            ComboBox cb = (ComboBox)oForm.Items.Item("cb_desp").Specific;
            Despacho = cb.Selected.Description;

            cb = (ComboBox)oForm.Items.Item("cb_Trasnp").Specific;
            Transportadora = cb.Selected.Value;

            _grid = oForm.Items.Item("grid_Item").Specific;


            for (int i = 0; i < _grid.Rows.Count; i++)
            {
                if (Despacho == "SEDEX" || Despacho == "PAC")
                {
                    if (_grid.DataTable.GetValue("Sel", i).ToString() == "Y")
                    {
                        if (listaSerial == "")
                        {
                            listaSerial = _grid.DataTable.GetValue(3, i).ToString();
                        }
                        else
                        {
                            listaSerial += ",";

                            listaSerial += _grid.DataTable.GetValue(3, i).ToString();
                        }

                    }
                }
                else
                {
                    if (_grid.DataTable.GetValue("Sel", i).ToString() == "Y")
                    {
                        if (listaSerial == "")
                        {
                            listaSerial = _grid.DataTable.GetValue(3, i).ToString();
                        }
                        else
                        {
                            listaSerial += ",";

                            listaSerial += _grid.DataTable.GetValue(3, i).ToString();
                        }
                    }
                }
            }

            SAPbobsCOM.Company company = SBOApp.Company;
            schema = company.CompanyDB;

            Hashtable reportParams = new Hashtable();
            reportParams.Add("Transp", Transportadora);
            reportParams.Add("Serial", listaSerial);

            reportParams.Add("DataIncial", DtInicial);
            reportParams.Add("DataFinal", DtFinal);

            reportParams.Add("Schema@", schema);


            CrystalReport crRelatorio = new CrystalReport();
            crRelatorio.ExecuteCrystalReport(@"C:\CVA Consultoria\Relatórios\Lista Postagem.rpt", reportParams);

            if (Despacho == "SEDEX" || Despacho == "PAC")
            {
                oForm.Items.Item("btn_IntCo").Enabled = true;
            }else
            {
                
                    SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    oRecordSet.DoQuery(String.Format(Query.UpdateOINVTransportadora,  DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), listaSerial));
                    oRecordSet.DoQuery(String.Format(Query.UpdateODLNTransportadora,  DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), listaSerial));
            }

        }

        private void CarregaGrid()
        {
            SAPbouiCOM.Form oForm = SBOApp.Application.Forms.ActiveForm;


            string DtInicial = string.Empty;
            string DtFinal = string.Empty;
            string Despacho = string.Empty;
            string Transportadora = string.Empty;

            DtInicial = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtIni").Value).ToString("yyyy/MM/dd");
            DtFinal = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtFim").Value).ToString("yyyy/MM/dd");

            ComboBox cb = (ComboBox)oForm.Items.Item("cb_desp").Specific;
            Despacho = cb.Selected.Description;
            Despacho += "%";

            ComboBox _cb = (ComboBox)oForm.Items.Item("cb_Trasnp").Specific;
            Transportadora = _cb.Selected.Value;
           

            _grid = (Grid)oForm.Items.Item("grid_Item").Specific;
            DataTable dt;

            try
            {
                dt = oForm.DataSources.DataTables.Item("dt");
            }
            catch
            {
                dt = oForm.DataSources.DataTables.Add("dt");
            }

            string sql = (string.Format(Query.CarregaListaPostagem, Despacho, Transportadora, DtInicial, DtFinal));
            dt.ExecuteQuery(sql);
            _grid.DataTable = dt;
            _grid.Columns.Item("Sel").Type = BoGridColumnType.gct_CheckBox;

            if (Despacho.StartsWith("SEDEX") || Despacho.StartsWith("PAC"))
            {
                //for (int i = 0; i <= _grid.Rows.Count - 1; i++)
                //{
                //    _grid.DataTable.Columns.Item("Sel").Cells.Item(i).Value = "Y";
                //}
                for (int i = 0; i <= _grid.Columns.Count - 1; i++)
                {
                   
                    _grid.Columns.Item(i).Editable = false;



                }
                _grid.Columns.Item("Sel").Editable = true;
            }
            else
            {
                for (int i = 0; i <= _grid.Columns.Count - 1; i++)
                {
                    _grid.Columns.Item(i).Editable = false;
                }
                _grid.Columns.Item("Sel").Editable = true;
            }
            oForm.Freeze(false);
        }

        private void IntegraçãoCorreio()
        {
            try
            {
                SAPbouiCOM.Form oForm = SBOApp.Application.Forms.ActiveForm;

                DateTime DtInicial = DateTime.Parse("1900/01/01");
                DateTime DtFinal = DateTime.Parse("1900/01/01");
                string Despacho = string.Empty;
                string Transportadora = string.Empty;
                string listaSerial = string.Empty;

                int IdServico = 0;
                string URL = string.Empty;
                string User = string.Empty;
                string PassWord = string.Empty;

                long retorno = 0;
                bool ret = false;

                DtInicial = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtIni").Value);
                DtFinal = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtFim").Value);

                ComboBox cb = (ComboBox)oForm.Items.Item("cb_desp").Specific;
                Despacho = cb.Selected.Description;
                

                cb = (ComboBox)oForm.Items.Item("cb_Trasnp").Specific;
                Transportadora = cb.Selected.Value;

                _grid = oForm.Items.Item("grid_Item").Specific;

                string local = GetPathXML();

                if(local.Length == 0)
                {
                    SBOApp.Application.SetStatusBarMessage("Informe nos parâmetros gerais um caminho para salvar o arquivo XML.", BoMessageTime.bmt_Medium, true);
                }else
                {
                    for (int i = 0; i < _grid.Rows.Count; i++)
                    {

                        if (_grid.DataTable.GetValue("Sel", i).ToString() == "Y")
                        {
                            if (listaSerial == "")
                            {
                                listaSerial = _grid.DataTable.GetValue(3, i).ToString();
                            }
                            else
                            {
                                listaSerial += ",";

                                listaSerial += _grid.DataTable.GetValue(3, i).ToString();
                            }

                        }
                    }

                    int count = 0;

                    for (int j = 0; j < _grid.Rows.Count; j++)
                    {
                        if (_grid.DataTable.GetValue("Sel", j).ToString() == "Y")
                        {
                            count++;
                        }

                    }

                    var lista_Remetente = _CorreioModelRemetente(Transportadora, listaSerial, DtInicial.ToString("yyyyMMdd"), DtFinal.ToString("yyyyMMdd"));
                    var lista_Objeto = _CorreioModelObjeto(Transportadora, listaSerial, DtInicial.ToString("yyyyMMdd"), DtFinal.ToString("yyyyMMdd"));
                    List<string> ListaEtiquetas = new List<string>();
                    List<string> ListaEtiquetasNew = new List<string>();

                    var _plp = new Model.correioslog();

                    _plp.tipo_arquivo = tipo_arquivo.Postagem;
                    _plp.versao_arquivo = 2.3M;

                    foreach (var item in lista_Remetente)
                    {
                        // TAG PLP
                        _plp.plp = new plp();
                        _plp.plp.id_plp = "";
                        _plp.plp.valor_global = "";
                        _plp.plp.mcu_unidade_postagem = "";
                        _plp.plp.nome_unidade_postagem = "";
                        _plp.plp.cartao_postagem = item.cartao_postagem.PadLeft(10, '0');

                        //TAG Remetente
                        _plp.remetente = new remetente();
                        _plp.remetente.numero_contrato = item.numero_contrato.PadLeft(10, '0');
                        _plp.remetente.numero_diretoria = Convert.ToSByte(item.numero_diretoria);
                        _plp.remetente.codigo_administrativo = item.codigo_administrativo.PadLeft(8, '0');
                        _plp.remetente.nome_remetente = item.nome_remetente;
                        _plp.remetente.logradouro_remetente = item.logradouro_remetente;
                        _plp.remetente.numero_remetente = item.numero_remetente;
                        _plp.remetente.complemento_remetente = item.complemento_remetente;
                        _plp.remetente.bairro_remetente = item.bairro_remetente;
                        if (item.cep_remetente.Length > 8)
                        {
                            _plp.remetente.cep_remetente = item.cep_remetente.Remove(8);
                        }
                        else
                        {
                            _plp.remetente.cep_remetente = item.cep_remetente;
                        }

                        _plp.remetente.cidade_remetente = item.cidade_remetente;
                        uf_remetente uf;
                        Enum.TryParse(item.uf_remetente, out uf);
                        _plp.remetente.uf_remetente = uf;
                        _plp.remetente.telefone_remetente = "";
                        _plp.remetente.fax_remetente = item.fax_remetente.ToString();
                        _plp.remetente.email_remetente = item.email_remetente;
                        _plp.remetente.celular_remetente = "";
                        _plp.remetente.cpf_cnpj_remetente = "";
                        _plp.remetente.ciencia_conteudo_proibido = "S";

                        IdServico = item.id_servico;
                        URL = item.URL;
                        User = item.User;
                        PassWord = item.PassWord;

                    }

                    _plp.objeto_postal = new objeto_postal[count];

                    int k = 0;
                    foreach (var item in lista_Objeto)
                    {
                        var obj = new objeto_postal();

                        //TAG ObjetoPostal
                        obj.numero_etiqueta = item.Cod_Rastreio;
                        obj.codigo_objeto_cliente = "";
                        obj.codigo_servico_postagem = item.Cod_Servico;
                        obj.cubagem = "0,0000";
                        string peso = string.Empty;
                        if (double.Parse(item.Peso_Tarifado) > 1)
                        {
                            peso = (double.Parse(item.Peso_Tarifado) * 1000).ToString();

                            //if (peso.ToString().Length <= 4)
                            //{
                            //    peso = peso.PadLeft(5, '0');
                            //}

                        }
                        else
                        {
                            peso = (double.Parse(item.Peso_Tarifado)*1000).ToString();
                            //if (peso.ToString().Length <= 4)
                            //{
                            //    peso =  peso.PadLeft(5,'0');
                            //}
                        }

                        obj.peso = peso.ToString();
                        obj.rt1 = "";
                        obj.rt2 = "";
                        obj.restricao_anac = "S";
                        // TAG Destinatário
                        obj.destinatario = new destinatario();
                        obj.destinatario.nome_destinatario = item.Destinatario;
                        if(item.Telefone.Length > 2 && item.Telefone.Substring(2, 1) != "9")
                        {
                            obj.destinatario.telefone_destinatario = item.Telefone.ToString();
                        }else
                        {
                            obj.destinatario.telefone_destinatario = "";
                        }
                        if(item.Celular.Length > 2 && item.Celular.Substring(2, 1) == "9")
                        {
                            obj.destinatario.celular_destinatario = item.Celular.ToString();
                        }else
                        {
                            obj.destinatario.celular_destinatario = "";
                        }
                        
                        obj.destinatario.email_destinatario = item.E_mail;
                        obj.destinatario.logradouro_destinatario = item.Logradouro;
                        obj.destinatario.complemento_destinatario = item.Complemento;
                        obj.destinatario.numero_end_destinatario = item.NumeroDestinatario.ToString();
                        obj.destinatario.cpf_cnpj_destinatario = "";
                        

                        //TAG Nacional
                        obj.nacional = new nacional();
                        obj.nacional.bairro_destinatario = item.Bairro;
                        obj.nacional.cidade_destinatario = item.Cidade;
                        uf_destinatario _uf;
                        Enum.TryParse(item.UF, out _uf);
                        obj.nacional.uf_destinatario = _uf;
                        obj.nacional.cep_destinatario = item.CEP;
                        obj.nacional.codigo_usuario_postal = "";
                        obj.nacional.centro_custo_cliente = "";
                        obj.nacional.numero_nota_fiscal = item.NF.ToString();
                        obj.nacional.serie_nota_fiscal = item.SerieNF;
                        obj.nacional.valor_nota_fiscal = item.VlrNF.ToString();
                        obj.nacional.natureza_nota_fiscal = "";
                        obj.nacional.descricao_objeto = "";
                        obj.nacional.valor_a_cobrar = "0,00";



                        // TAG Serviço Adicional
                        obj.servico_adicional = new servico_adicional();
                        obj.servico_adicional.codigo_servico_adicional = new string[2];
                        obj.servico_adicional.codigo_servico_adicional[0] = "025";
                        //obj.servico_adicional.codigo_servico_adicional[1] = "001";
                        if (item.Cod_Servico == "04669")
                        {
                            obj.servico_adicional.codigo_servico_adicional[1] = "064";
                        }else
                        {
                            obj.servico_adicional.codigo_servico_adicional[1] = "019";
                        }
                        
                        obj.servico_adicional.valor_declarado = item.VlrNF.ToString();

                        // TAG Dimensão Objeto
                        obj.dimensao_objeto = new dimensao_objeto();
                        obj.dimensao_objeto.tipo_objeto = "002";
                        obj.dimensao_objeto.dimensao_altura = item.Altura.ToString();
                        obj.dimensao_objeto.dimensao_largura = item.Largura.ToString();
                        obj.dimensao_objeto.dimensao_comprimento =item.Comprimento.ToString();
                        obj.dimensao_objeto.dimensao_diametro = "0";

                        //obj.data_captacao = "";
                        obj.data_postagem_sara = "";
                        obj.status_processamento = 0;
                        obj.numero_comprovante_postagem = "";
                        obj.valor_cobrado = "";

                        _plp.objeto_postal[k] = obj;
                        ListaEtiquetas.Add(item.Cod_Rastreio);
                        k++;
                    }

                    foreach (var item in ListaEtiquetas)
                    {
                        string prefixo = string.Empty;
                        string meio = string.Empty;
                        string sufixo = string.Empty;
                        string NovoCodRastreio = string.Empty;

                        if(item == "")
                        {
                            SBOApp.Application.SetStatusBarMessage("Existem itens sem código de rastreio!" + retorno, BoMessageTime.bmt_Medium, true);
                            return;
                        }

                        prefixo = item.Substring(0, 2);
                        meio = item.Substring(2, 9);
                        sufixo = item.Substring(11, 2);

                        meio = meio.Remove(meio.Length - 1);

                        NovoCodRastreio = prefixo + meio + sufixo;

                        ListaEtiquetasNew.Add(NovoCodRastreio);
                    }

                    var t = new XMLAtributos();
                    t.IsValid(_plp);
                    var xml = CreateXML(_plp);
                    
                    string substitui = " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";
                    xml = xml.Replace(substitui, "");
                    xml = xml.Replace("iso", "ISO");
                    
                    if (local.Substring(local.Length - 1) != "\\")
                    {
                        local = local.Trim() + "\\";
                    }


                    string fileName = local + DateTime.Now.ToString("yyyyMMdd_HHmm_PL") + "Enviado.xml";

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml); // Salve o documento em um arquivo. 
                    doc.Save(fileName);

                    CorreiosServices.AtendeClienteService _AtendeClienteService = new CorreiosServices.AtendeClienteService(URL);


                    _AtendeClienteService.fechaPlpVariosServicos(xml, IdServico, true, _plp.plp.cartao_postagem, ListaEtiquetasNew.ToArray(), User, PassWord, out retorno, out ret);


                    SBOApp.Application.SetStatusBarMessage("Integração Realizada com Sucesso, N° PLP: " + retorno, BoMessageTime.bmt_Short, false);

                    foreach (var item in lista_Objeto)
                    {

                        SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordSet.DoQuery(String.Format(Query.UpdateOINV, retorno, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Convert.ToInt32(item.NF)));
                        oRecordSet.DoQuery(String.Format(Query.UpdateODLN, retorno, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Convert.ToInt32(item.NF)));
                    }

                    
                            if (local.Substring(local.Length - 1) != "\\")
                            {
                                local = local.Trim() + "\\";
                            }

                            fileName = local + DateTime.Now.ToString("yyyyMMdd_HHmm_PL") + retorno + ".xml";

                            doc = new XmlDocument();
                            doc.LoadXml(xml); // Salve o documento em um arquivo. 
                            doc.Save(fileName);

                }
            }
            catch (Exception ex)
            {
                string local = @"C:\XML_PLP\Log_Envio_PLP\";
                System.IO.File.WriteAllText(local + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt", ex.Message);

                SBOApp.Application.SetStatusBarMessage("Servidor Indisponivel. Erro -> " + ex.Message, BoMessageTime.bmt_Short, true);
            }

        }

        private List<CorreioModelRemetente> _CorreioModelRemetente(string Transportadora, string listaSerial, string DtInicial, string DtFinal)
        {
            var ListaCorreioModelRemetente = new List<CorreioModelRemetente>();

            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.Carrega_Remetente, Transportadora, listaSerial, DtInicial, DtFinal));

            if (!oRecordSet.EoF)
            {
                var model = new CorreioModelRemetente();

                model.numero_contrato = oRecordSet.Fields.Item(0).Value;
                model.numero_diretoria = oRecordSet.Fields.Item(1).Value;
                model.codigo_administrativo = oRecordSet.Fields.Item(2).Value;
                model.nome_remetente = oRecordSet.Fields.Item(3).Value;
                model.logradouro_remetente = oRecordSet.Fields.Item(4).Value;
                model.numero_remetente = oRecordSet.Fields.Item(5).Value;
                model.complemento_remetente = oRecordSet.Fields.Item(6).Value;
                model.bairro_remetente = oRecordSet.Fields.Item(7).Value;
                model.cep_remetente = oRecordSet.Fields.Item(8).Value;
                model.cidade_remetente = oRecordSet.Fields.Item(9).Value;
                model.uf_remetente = oRecordSet.Fields.Item(10).Value;
                model.cartao_postagem = oRecordSet.Fields.Item(11).Value;
                model.fax_remetente = oRecordSet.Fields.Item(12).Value;
                model.email_remetente = oRecordSet.Fields.Item(13).Value;
                model.id_servico = Convert.ToInt32(oRecordSet.Fields.Item(14).Value);
                model.URL = oRecordSet.Fields.Item(15).Value;
                model.User = oRecordSet.Fields.Item(16).Value;
                model.PassWord = oRecordSet.Fields.Item(17).Value;

                ListaCorreioModelRemetente.Add(model);
                oRecordSet.MoveNext();

            }

            return ListaCorreioModelRemetente;

        }

        private List<CorreioModelObjeto> _CorreioModelObjeto(string Transportadora, string listaSerial, string DtInicial, string DtFinal)
        {
            var ListaCorreioModelObjeto = new List<CorreioModelObjeto>();

            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.Carrega_ObjPosta, Transportadora, listaSerial, DtInicial, DtFinal));

            if (!oRecordSet.EoF)
            {
                for (int i = 0; i < oRecordSet.RecordCount; i++)
                {
                    var model = new CorreioModelObjeto();

                    model.Cod_Rastreio = oRecordSet.Fields.Item(0).Value;
                    model.Cod_Servico = oRecordSet.Fields.Item(1).Value.ToString();
                    model.Peso_Tarifado = oRecordSet.Fields.Item(2).Value.ToString();
                    model.Destinatario = oRecordSet.Fields.Item(3).Value;

                    //if (oRecordSet.Fields.Item(4).Value == "")
                    //{
                    //    SBOApp.Application.SetStatusBarMessage("Telefone do Destinatário Obrigatório!");
                    //    break;
                    //}
                    //else
                    //{
                    model.Telefone = oRecordSet.Fields.Item(4).Value.ToString();
                    //}
                    
                    
                    

                    //if (oRecordSet.Fields.Item(5).Value == "")
                    //{
                    //    SBOApp.Application.SetStatusBarMessage("Celular do Destinatário Obrigatório!");
                    //    break;
                    //}
                    //else
                    //{
                    model.Celular = oRecordSet.Fields.Item(5).Value.ToString();
                    //}                    

                    model.Logradouro = oRecordSet.Fields.Item(6).Value;
                    model.Complemento = oRecordSet.Fields.Item(7).Value;
                    model.NumeroDestinatario = oRecordSet.Fields.Item(8).Value;
                    model.Bairro = oRecordSet.Fields.Item(9).Value;
                    model.Cidade = oRecordSet.Fields.Item(10).Value;
                    model.UF = oRecordSet.Fields.Item(11).Value;
                    model.CEP = oRecordSet.Fields.Item(12).Value;
                    model.NF = Convert.ToInt32(oRecordSet.Fields.Item(13).Value);
                    model.SerieNF = oRecordSet.Fields.Item(14).Value;
                    model.VlrNF = Convert.ToDecimal(oRecordSet.Fields.Item(15).Value);
                    model.TipoObjeto = oRecordSet.Fields.Item(16).Value;
                    model.Altura = oRecordSet.Fields.Item(17).Value;
                    model.Largura = oRecordSet.Fields.Item(18).Value;
                    model.Comprimento = oRecordSet.Fields.Item(19).Value;
                    model.E_mail = oRecordSet.Fields.Item(20).Value;


                    ListaCorreioModelObjeto.Add(model);
                    oRecordSet.MoveNext();
                }
            }

            return ListaCorreioModelObjeto;

        }

        public static string CreateXML(Object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            using (StringWriterWithEncoding textWriter = new StringWriterWithEncoding(Encoding.GetEncoding("ISO-8859-1")))
            {
                using (XmlCDataWriter xmlWriter = new XmlCDataWriter(textWriter))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    serializer.Serialize(xmlWriter, obj);
                    ns.Add("", "");
                }
                return textWriter.ToString();
            }
        }

        private string GetPathXML()
        {
            var PathXML = string.Empty;

            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.GetPathXML));

            if (!oRecordSet.EoF)
            {
                PathXML = oRecordSet.Fields.Item(0).Value;
            }

            return PathXML;
        }
    }

    public class XMLAtributos : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            IList<PropertyInfo> propriedades = new List<PropertyInfo>(value.GetType().GetProperties());

            foreach (PropertyInfo propriedade in propriedades)
            {
                object valor = propriedade.GetValue(value, null);
                if (valor == null)
                {
                    propriedade.SetValue(value, "", null);
                }
            }
            return true;
        }
    }

    public sealed class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;
        private XmlDocument xmlDoc = new XmlDocument();

        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }

    public class XmlCDataWriter : XmlTextWriter
    {
        public override void WriteString(string text)
        {
            if (WriteState == WriteState.Element)
            {
                if (!VerificaCData(text))
                {
                    base.WriteString(text);
                }
                else
                {
                    WriteCData(text);
                }

            }
            else
            {
                base.WriteString(text);
            }
        }

        /// <summary>
        /// Creates an instance of the XmlTextWriter class using the specified <see cref="T:System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="w">The TextWriter to write to. It is assumed that the TextWriter is already set to the correct encoding. </param>
        public XmlCDataWriter(TextWriter w) : base(w)
        {
        }

        //verifica se é necessário inserir CDATA
        //valores numéricos, fixos e etiqueta não devem ter CDATA
        private bool VerificaCData(string text)
        {
            decimal number = 0;
            uf_destinatario _uf;
            if (decimal.TryParse(text, out number) || Enum.TryParse(text, out _uf) || text == "S" || text == "Postagem")
            {
                return false;
            }

            if (text.Length == 13)
            {
                string numero = text.Substring(2, 9);
                if (decimal.TryParse(numero, out number))
                {
                    return false;
                }

            }

            return true;

        }
    }
}
