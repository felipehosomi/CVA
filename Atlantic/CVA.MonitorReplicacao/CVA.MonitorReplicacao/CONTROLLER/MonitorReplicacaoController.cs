using CVA.MonitorReplicacao.DAO;
using CVA.MonitorReplicacao.FORM;
using CVA.MonitorReplicacao.MODEL;
using Dover.Framework.Attribute;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Globalization;

namespace CVA.MonitorReplicacao.CONTROLLER
{
    [FormAttribute(B1Forms.Monitor, "CVA.MonitorReplicacao.FORM.View.MonitorReplicacao.srf")]
    [MenuEvent(UniqueUID = B1Menus.Monitor)]


    public class MonitorReplicacaoController : DoverUserFormBase
    {
        private Button bt_Search { get; set; }
        private Grid gr_Reg { get; set; }
        private EditText et_IdFrom { get; set; }
        private EditText et_IdTo { get; set; }
        private EditText et_DtFrom { get; set; }
        private EditText et_DtTo { get; set; }
        private EditText et_Code { get; set; }
        private EditText et_Status { get; set; }
        private EditText et_Source { get; set; }
        private EditText et_Obj { get; set; }
        private EditText et_Func { get; set; }
        private EditText et_Base { get; set; }
        private DataTable dt_Reg { get; set; }

        private MonitorDAO _dao { get; set; }
        private SAPbouiCOM.Application _application { get; set; }

        private FilterModel _FilterModel { get; set; }

        //Construtor
        public MonitorReplicacaoController(MonitorDAO dao, SAPbouiCOM.Application application)
        {
            _dao = dao;
            _application = application;
            _FilterModel = new FilterModel();

            bt_Search = this.GetItem("bt_Search").Specific as Button;
            gr_Reg = this.GetItem("gr_Reg").Specific as Grid;

            et_IdFrom = this.GetItem("et_IdFrom").Specific as EditText;
            et_IdTo = this.GetItem("et_IdTo").Specific as EditText;
            et_DtFrom = this.GetItem("et_DtFrom").Specific as EditText;
            et_DtTo = this.GetItem("et_DtTo").Specific as EditText;
            et_Code = this.GetItem("et_Code").Specific as EditText;
            et_Status = this.GetItem("et_Status").Specific as EditText;
            et_Source = this.GetItem("et_Source").Specific as EditText;
            et_Obj = this.GetItem("et_Obj").Specific as EditText;
            et_Func = this.GetItem("et_Func").Specific as EditText;
            et_Base = this.GetItem("et_Base").Specific as EditText;

            dt_Reg = this.UIAPIRawForm.DataSources.DataTables.Item("dt_Reg");

            bt_Search.ClickAfter += Bt_Search_ClickAfter;

            Search();
        }

        protected virtual void Bt_Search_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            this.Search();
        }

        //Atualiza a grid com os dados do banco
        public void Search()
        {
            _FilterModel.IdFrom = et_IdFrom.Value;
            _FilterModel.IdTo = et_IdTo.Value;
            _FilterModel.DateFrom = et_DtFrom.Value;
            _FilterModel.DateTo = et_DtTo.Value;
            _FilterModel.Code = et_Code.Value;
            _FilterModel.Status = et_Status.Value;
            _FilterModel.Source = et_Source.Value;
            _FilterModel.Object = et_Obj.Value;
            _FilterModel.Function = et_Func.Value;
            _FilterModel.ErrorBase = et_Base.Value;

            dt_Reg.ExecuteQuery(_dao.GetRegQuery(_FilterModel));
        }
    }
}

