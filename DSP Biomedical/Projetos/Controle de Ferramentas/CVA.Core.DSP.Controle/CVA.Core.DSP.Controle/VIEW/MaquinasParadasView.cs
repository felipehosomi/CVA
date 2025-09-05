using CVA.Core.DSP.Controle.BLL;
using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.MODEL;
using CVA.Core.DSP.Controle.Resources.Select;
using Dover.Framework.Attribute;
using Dover.Framework.DAO;
using Dover.Framework.Form;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.VIEW
{


    [FormAttribute("CVAMaquinasParadas", "CVA.Core.DSP.Controle.Resources.Forms.ApontamentodeMaquinasParadas.srf")]
    [MenuEvent(UniqueUID = "CVAMaquinasParadas")]

    public class MaquinasParadasView : DoverUserFormBase
    {
        #region Atributos

        public IForm _form { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public ItemEvent ItemEventInfo { get; set; }        
        public SAPbobsCOM.Company _company { get; set; }
      

        public MaquinasParadasBLL _maquinasParadasBLL { get; set; }
        public MaquinasParadasDAO _maquinasParadasDAO { get; set; }
        public CampoMaquinaModel CampoModel { get; set; }


        public SAPbouiCOM.Grid grid_item { get; set; }

        public SAPbouiCOM.ComboBox cb_maq { get; set; }
        public SAPbouiCOM.ComboBox cb_op { get; set; }
        public SAPbouiCOM.ComboBox cb_prod { get; set; }
        public SAPbouiCOM.ComboBox cb_mot { get; set; }
        public SAPbouiCOM.ComboBox cb_order { get; set; }

        public SAPbouiCOM.EditText et_lote { get; set; }
        public SAPbouiCOM.EditText et_hrIni { get; set; }
        public SAPbouiCOM.EditText et_hrFim { get; set; }
        public SAPbouiCOM.EditText et_dtIni { get; set; }
        public SAPbouiCOM.EditText et_dtFim { get; set; }
        public SAPbouiCOM.EditText et_dura { get; set; }
        public SAPbouiCOM.EditText et_dtFim1 { get; set; }
        public SAPbouiCOM.EditText et_hrFim1 { get; set; }

        public SAPbouiCOM.Button btn_add { get; set; }
        public SAPbouiCOM.Button btn_cancel { get; set; }
        public SAPbouiCOM.Button btn_altera { get; set; }
        public SAPbouiCOM.Button btn_remove { get; set; }
        public SAPbouiCOM.Button btn_cres { get; set; }
        public SAPbouiCOM.Button btn_decre { get; set; }


        public int x { get; set; }
        #endregion

        #region Construtor

        public MaquinasParadasView()
        {
            
        }
        #endregion

        public override void OnInitializeComponent()
        {
            if (_form!=null)
            {
                return;
            }
            _form = ((DoverUserFormBase)(this)).UIAPIRawForm;
            this.UIAPIRawForm.Freeze(true);


            

            grid_item = this.GetItem("grid_item").Specific as SAPbouiCOM.Grid;

            cb_maq   = this.GetItem("cb_maq").Specific as SAPbouiCOM.ComboBox;
            cb_op    = this.GetItem("cb_op").Specific as SAPbouiCOM.ComboBox;
            cb_prod  = this.GetItem("cb_prod").Specific as SAPbouiCOM.ComboBox;
            cb_mot   = this.GetItem("cb_mot").Specific as SAPbouiCOM.ComboBox;
            cb_order = this.GetItem("cb_order").Specific as SAPbouiCOM.ComboBox;

            et_dtIni  = this.GetItem("et_dtIni").Specific as SAPbouiCOM.EditText;
            et_dtFim  = this.GetItem("et_dtFim").Specific as SAPbouiCOM.EditText;
            et_hrIni  = this.GetItem("et_hrIni").Specific as SAPbouiCOM.EditText;
            et_hrFim  = this.GetItem("et_hrFim").Specific as SAPbouiCOM.EditText;
            et_dura   = this.GetItem("et_dura").Specific as SAPbouiCOM.EditText;
            et_lote   = this.GetItem("et_lote").Specific as SAPbouiCOM.EditText;
            et_dtFim1 = this.GetItem("et_dtFim1").Specific as SAPbouiCOM.EditText;
            et_hrFim1 = this.GetItem("et_hrFim1").Specific as SAPbouiCOM.EditText;


            btn_add    = this.GetItem("btn_add").Specific as SAPbouiCOM.Button;
            btn_cancel = this.GetItem("btn_cancel").Specific as SAPbouiCOM.Button;
            btn_altera = this.GetItem("btn_altera").Specific as SAPbouiCOM.Button;
            btn_remove = this.GetItem("btn_remove").Specific as SAPbouiCOM.Button;
            btn_decre  = this.GetItem("btn_decre").Specific as SAPbouiCOM.Button;
            btn_cres   = this.GetItem("btn_cres").Specific as SAPbouiCOM.Button;

            et_dtFim1.Item.Visible = false;
            et_hrFim1.Item.Visible = false;

            _maquinasParadasBLL = new MaquinasParadasBLL();
            _maquinasParadasDAO = new MaquinasParadasDAO();

            IniciaGrid();
            OnCustomInitializeEvents();
            CarregaGrid();
            this.UIAPIRawForm.Freeze(false);           
        }

        public override void OnInitializeFormEvents()
        {

        }

        private void OnCustomInitializeEvents()
        {
            btn_add.ClickAfter += Btn_add_ClickAfter;
            btn_cancel.ClickAfter += Btn_cancel_ClickAfter;
            btn_altera.ClickAfter += Btn_altera_ClickAfter;
            btn_remove.ClickAfter += Btn_remove_ClickAfter;
            btn_cres.ClickAfter += Btn_cres_ClickAfter;
            btn_decre.ClickAfter += Btn_decre_ClickAfter;


            et_dtFim.LostFocusAfter += Et_dtFim_LostFocusAfter;
            et_dtIni.LostFocusAfter += Et_dtIni_LostFocusAfter;
            et_hrIni.LostFocusAfter += Et_hrIni_LostFocusAfter;
            et_hrFim.LostFocusAfter += Et_hrFim_LostFocusAfter;

            CarregaCombos();

        }
      
        
        // Botões
        protected internal virtual void Btn_add_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (ValidaCampos())
            {
                var model = new CampoMaquinaModel();

                model.dataInicial = Convert.ToDateTime(et_dtIni.String);

                if (String.IsNullOrEmpty(et_dtFim.Value) && String.IsNullOrEmpty(et_hrFim.Value))
                {
                    model.dataFinal = et_dtFim.Value = null;
                    model.horaFinal = et_hrFim.Value = "";

                }
                else
                {
                    model.dataFinal = et_dtFim.String;
                    model.horaFinal = et_hrFim.String;
                }

                model.horaInicial = et_hrIni.String;                
                model.duracao = et_dura.Value;
                model.maquina = cb_maq.Selected.Description;
                model.motivo = cb_mot.Selected.Description;
                model.operador = cb_op.Selected.Description;
                model.produto = cb_prod.Selected.Description;
                model.lote = et_lote.Value;

                if (x != 1)
                {
                    _maquinasParadasBLL.insert(model);
                }
                else
                {
                    var linha = grid_item.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
                    model.Code = Convert.ToInt32(grid_item.DataTable.GetValue("Código", linha).ToString().Trim());
                    _maquinasParadasDAO.Update(model);
                    x = 0;
                }
                if (!_maquinasParadasDAO.Update(model))
                {
                    return;
                }
                else
                {
                    ApagaDadosTela();
                    CarregaGrid();
                }
            }
            
        }

        protected internal virtual void Btn_cancel_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            var lote = et_lote.Value;

            if (!string.IsNullOrEmpty(lote))
            {
                var resposta = _application.MessageBox(@"Todos os dados serão perdidos." +
                                                                "Deseja continuar?", 1, "Sim", "Não");
                if (resposta == 1)
                {
                    _form.Close();
                }
            }
            else
            {
                _form.Close();
            }





        }

        protected internal virtual void Btn_altera_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (grid_item.Rows.SelectedRows.Count == 0)
            {
                _application.MessageBox("Por favor, selecione a linha que deseja alterar.");
                return;
            }
            else
            {
                AtualizaCampos();
            }
            
            
        }

        protected internal virtual void Btn_remove_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (grid_item.Rows.SelectedRows.Count == 0)
            {
                _application.MessageBox("Por favor, selecione a linha que deseja remover.");
                return;
            }
            else
            {
                RemoveApontamento();
            }
            
        }
       
        protected internal virtual void Btn_cres_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (cb_order.Selected.Description == "")
            {
                _application.MessageBox("Por favor, selecione a coluna que deseja ordenar.");
                return;

            }
            else
            {
                grid_item = ((SAPbouiCOM.Grid)(UIAPIRawForm.Items.Item("grid_item").Specific));
                try
                {
                    grid_item.Columns.Item(cb_order.Selected.Description).TitleObject.Sortable = true;
                    grid_item.Columns.Item(cb_order.Selected.Description).TitleObject.Sort(BoGridSortType.gst_Ascending);

                }
                catch (Exception ex)
                {

                }

            }
          
        }

        protected internal virtual void Btn_decre_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (cb_order.Selected.Description == "")
            {
                _application.MessageBox("Por favor, selecione a coluna que deseja ordenar.");
                return;

            }
            else
            {
                grid_item = ((SAPbouiCOM.Grid)(UIAPIRawForm.Items.Item("grid_item").Specific));
                try
                {
                    grid_item.Columns.Item(cb_order.Selected.Description).TitleObject.Sortable = true;
                    grid_item.Columns.Item(cb_order.Selected.Description).TitleObject.Sort(BoGridSortType.gst_Descending);

                }
                catch (Exception ex)
                {

                }
            }
        }


        // LostFocus
        protected virtual void Et_hrFim_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            CalculaTempo();
        }

        protected virtual void Et_hrIni_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            CalculaTempo();
        }

        protected virtual void Et_dtIni_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            CalculaTempo();
        }

        protected virtual void Et_dtFim_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {

            CalculaTempo();
        }

        private void CarregaCombos()
        {
            cb_mot.ValidValues.Add("", "");
            cb_op.ValidValues.Add("", "");
            cb_prod.ValidValues.Add("", "");
           
            var listaMaquinas = _maquinasParadasBLL.GetMaquinas();
            foreach (var item in listaMaquinas)
            {
                cb_maq.ValidValues.Add(item.code, item.name);
            }

            var listaMotivos = _maquinasParadasBLL.GetMotivos();
            foreach (var item in listaMotivos)
            {
                cb_mot.ValidValues.Add(item.code, item.name);
            }

            var listaOperador = _maquinasParadasBLL.GetOperador();
            foreach (var item in listaOperador)
            {
                cb_op.ValidValues.Add(item.ID, item.Nome);
            }

            var listaProduto = _maquinasParadasBLL.GetProduto();
            foreach (var item in listaProduto)
            {
                cb_prod.ValidValues.Add(item.code, item.descricao);
            }
            
        }

        private bool ValidaCampos()
        {
            CalculaTempo();
            //valida combobox 
            if (cb_maq.Selected == null)
            {
                _application.MessageBox("O campo maquina não está preenchido!");
                return false;
            }

            if (cb_op.Selected == null)
            {
                _application.MessageBox("O campo operador não está preenchido!");
                return false;
            }

            if (cb_prod.Selected == null)
            {
                _application.MessageBox("O campo produto não está preenchido!");
                return false;
            }

            if (cb_mot.Selected == null)
            {
                _application.MessageBox("O campo motivo não está preenchido!");
                return false;
            }

            //valida edittext 

            if (String.IsNullOrEmpty(et_lote.Value))
            {
                _application.MessageBox("O campo lote não está preenchido!");
                return false;
            }

            //if (String.IsNullOrEmpty(et_dtIni.Value))
            //{
            //    _application.MessageBox("O campo data inicial não está preenchido!");
            //    return false;
            //}

            //if (String.IsNullOrEmpty(et_dtFim.Value))
            //{
            //    _application.MessageBox("O campo data final não está preenchido!");
            //    return false;
            //}

            //if (String.IsNullOrEmpty(et_hrIni.Value))
            //{
            //    _application.MessageBox("O campo hora inicio não está preenchido!");
            //    return false;
            //}

            //if (String.IsNullOrEmpty(et_hrFim.Value))
            //{
            //    _application.MessageBox("O campo hora fim não está preenchido!");
            //    return false;
            //}

            return true;



        }

        private void ApagaDadosTela()
        {
           
            cb_maq.Select("", BoSearchKey.psk_Index);
            cb_mot.Select("", BoSearchKey.psk_Index);
            cb_op.Select("", BoSearchKey.psk_Index);
            cb_prod.Select("", BoSearchKey.psk_Index);
            cb_order.Select("", BoSearchKey.psk_Index);

            et_dtIni.Value = "";
            et_dtFim.Value = "";
            et_hrIni.Value = "";
            et_hrFim.Value = "";
            et_dura.Value = "";
            et_lote.Value = "";
        }

        private void CalculaTempo()
        {
            var horaInicial = et_hrIni.String;
            var horaFinal = et_hrFim.String;
            var dataInicial = et_dtIni.String;
            var dataFinal = et_dtFim.String;

            if (!String.IsNullOrEmpty(horaInicial) && !String.IsNullOrEmpty(horaFinal) && !String.IsNullOrEmpty(dataInicial) && !String.IsNullOrEmpty(dataFinal))
            {
                int totalDias = (DateTime.Parse(dataFinal).Subtract(DateTime.Parse(dataInicial))).Days;

                //verifica se está parado a mais de um dia
                if (totalDias > 0)
                {
                    var HrFinal = Convert.ToDateTime(horaFinal);
                    var HrInicial = Convert.ToDateTime(horaInicial);
                    double MinFinal = Convert.ToDouble(HrFinal.Minute / 60.0);
                    double MinInicial = Convert.ToDouble(HrInicial.Minute / 60.0);

                    //calcula as horas paradas
                    var totalHoras = (totalDias * 24) + HrFinal.Hour + MinFinal - HrInicial.Hour - MinInicial;
                    if (totalHoras > 24)
                    {
                        var totalHoras2 = totalHoras - (totalDias * 24);
                        et_dura.Value = totalDias.ToString("0" + "dia(s)") + totalHoras2.ToString("0.00 " + "hora(s)");
                    }
                    else
                    {
                        et_dura.Value = totalHoras.ToString("0 . 00") + "hora(s)";
                    }
                }
                else
                {
                    var HrFinal = Convert.ToDateTime(horaFinal);
                    var HrInicial = Convert.ToDateTime(horaInicial);
                    double MinFinal = Convert.ToDouble(HrFinal.Minute / 60.0);
                    double MinInicial = Convert.ToDouble(HrInicial.Minute / 60.0);

                    //calcula as horas paradas
                    var totalHoras = HrFinal.Hour + MinFinal - HrInicial.Hour - MinInicial;

                    et_dura.Value = totalHoras.ToString("0.00") + " hora(s)";
                }
            }
        }     

        private void IniciaGrid()
        {
            DataTable dt = _form.DataSources.DataTables.Add("CVA_MAQ_PARADAS");
            dt.ExecuteQuery(@"select convert(varchar(72), '') as 'Código'
	                               , convert(varchar(72), '') as 'Maquina'
	                               , convert(varchar(72), '') as 'Operador'
	                               , convert(varchar(72), '') as 'Motivo'
	                               , convert(varchar(72), '') as 'Lote'
	                               , convert(varchar(72), '') as 'Hora Inicio'
	                               , convert(varchar(72), '') as 'Hora Fim'
	                               , convert(varchar(72), '') as 'Data Inicio'
	                               , convert(varchar(72), '') as 'Data Fim'
	                               , convert(varchar(72),'')  as 'Duração'"
                           );

            grid_item.DataTable = dt;


            FormatarGrid();

        }

        private void FormatarGrid()
        {
            grid_item.Columns.Item("Código").Width = 100;
            grid_item.Columns.Item("Maquina").Width = 100;
            grid_item.Columns.Item("Operador").Width = 100;
            grid_item.Columns.Item("Motivo").Width = 100;
            grid_item.Columns.Item("Lote").Width = 100;
            grid_item.Columns.Item("Hora Inicio").Width = 100;
            grid_item.Columns.Item("Hora Fim").Width = 100;
            grid_item.Columns.Item("Data Inicio").Width = 100;
            grid_item.Columns.Item("Data Fim").Width = 100;
            grid_item.Columns.Item("Duração").Width = 100;

            //CarregaGrid();

        }

        private void CarregaGrid()
        {
            _form.Freeze(true);

            grid_item.DataTable.ExecuteQuery(@"select isnull(convert(int,Code), 0) as 'Código'
	                                                , isnull(U_Maquina, '') as 'Maquina'
	                                                , isnull(U_operador, '') as 'Operador'
	                                                , isnull(U_produto, '') as 'Produto'
	                                                , isnull(U_motivo, '') as 'Motivo'
	                                                , isnull(U_lote, '') as 'Lote'
	                                                , isnull(U_dtInicio, '') as 'Data Inicio'
	                                                , isnull(RIGHT('0' + CONVERT(varchar(10), U_horaIni / 100), 2) + ':' + RIGHT('0' + CONVERT(varchar(10), U_horaIni % 100), 2),'') as 'Hora Inicio'
	                                                , case when isnull(U_datafinal1,'') = '01/01/0001 00:00:00' then '' 
	                                                       when isnull(U_datafinal1,'') <> '01/01/0001 00:00:00' then  isnull(U_datafinal1,'')
	                                                       end as 'Data Fim'
	                                                , case when ISNULL(U_horaFinal1,'') = '00:00' then ''
	                                                      when ISNULL(U_horaFinal1,'') <> '00:00' then ISNULL(U_horaFinal1,'') 
	                                               	    end as 'Hora Fim'	
	                                                , isnull(U_duracao,'')  as 'Duração'
	                                                from [@CVA_MAQ_PARADAS]
                                                    where U_status = 'A'
                                                    order by convert(int,Code), U_dtInicio");
      
            _form.Freeze(false);
        }

        private void AtualizaCampos()
        {
            var linha = grid_item.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
            

            
            var model = new CampoMaquinaModel();

            model.Code     = Convert.ToInt32(grid_item.DataTable.GetValue("Código", linha).ToString().Trim());
            model.maquina  = grid_item.DataTable.GetValue("Maquina", linha).ToString().Trim();
            model.motivo   = grid_item.DataTable.GetValue("Motivo", linha).ToString().Trim();
            model.operador = grid_item.DataTable.GetValue("Operador", linha).ToString().Trim();
            model.produto  = grid_item.DataTable.GetValue("Produto", linha).ToString().Trim();
            model.dataInicial = Convert.ToDateTime(grid_item.DataTable.GetValue("Data Inicio", linha));            
            model.horaInicial = grid_item.DataTable.GetValue("Hora Inicio", linha).ToString().Trim();            
            model.duracao     = grid_item.DataTable.GetValue("Duração", linha).ToString().Trim();
            model.lote        = grid_item.DataTable.GetValue("Lote", linha).ToString().Trim();

            if (String.IsNullOrEmpty(model.dataFinal) && String.IsNullOrEmpty(model.horaFinal))
            {
               et_dtFim1.Value  = grid_item.DataTable.GetValue("Data Fim", linha).ToString();
               et_hrFim1.Value = grid_item.DataTable.GetValue("Hora Fim", linha).ToString().Trim();

                model.dataFinal = et_dtFim1.Value;
                model.horaFinal = et_hrFim1.Value;

                et_dtFim.Item.Visible = true;
                et_hrFim.Item.Visible = true;

                et_dtFim1.Item.Visible = false;
                et_hrFim1.Item.Visible = false;
            }
            


            cb_maq.Select(model.maquina, BoSearchKey.psk_ByDescription);
            cb_mot.Select(model.motivo, BoSearchKey.psk_ByDescription);
            cb_op.Select(model.operador, BoSearchKey.psk_ByDescription);
            cb_prod.Select(model.produto, BoSearchKey.psk_ByDescription);

            et_dtIni.String =  model.dataInicial.ToString().Substring(0,10);
            et_dtFim.String = model.dataFinal.ToString();
            et_hrIni.String = model.horaInicial;
            et_hrFim.String = model.horaFinal;
            et_dura.Value   = model.duracao;
            et_lote.Value   = model.lote;
            x = 1;

        }

        private void RemoveApontamento()
        {
            var linha = grid_item.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);
            int cod = Convert.ToInt32(grid_item.DataTable.GetValue("Código", linha).ToString().Trim());

            if (!btn_remove.Item.Enabled)
                return;

            for (int i = 0; i < grid_item.Rows.Count; i++)
            {
                if (grid_item.Rows.IsSelected(i))
                {
                    var resposta = _application.MessageBox(@"Tem certeza que deseja excluir o apontamento selecionado?" + "", 1, "Sim", "Não");
                    if (resposta == 1)
                    {
                        var model = new CampoMaquinaModel();

                        model.Code = Convert.ToInt32(grid_item.DataTable.GetValue("Código", linha).ToString().Trim());
                        model.maquina = grid_item.DataTable.GetValue("Maquina", linha).ToString().Trim();
                        model.motivo = grid_item.DataTable.GetValue("Motivo", linha).ToString().Trim();
                        model.operador = grid_item.DataTable.GetValue("Operador", linha).ToString().Trim();
                        model.produto = grid_item.DataTable.GetValue("Produto", linha).ToString().Trim();
                        model.dataInicial = Convert.ToDateTime(grid_item.DataTable.GetValue("Data Inicio", linha));
                        model.dataFinal = grid_item.DataTable.GetValue("Data Fim", linha).ToString().Trim();
                        model.horaInicial = grid_item.DataTable.GetValue("Hora Inicio", linha).ToString().Trim();
                        model.horaFinal = grid_item.DataTable.GetValue("Hora Fim", linha).ToString().Trim();
                        model.duracao = grid_item.DataTable.GetValue("Duração", linha).ToString().Trim();
                        model.lote = grid_item.DataTable.GetValue("Lote", linha).ToString().Trim();

                        _maquinasParadasBLL.Delete(model);
                        grid_item.DataTable.Rows.Remove(i);
                        CarregaGrid();
                    }


                }


            }
        }

    }
}
