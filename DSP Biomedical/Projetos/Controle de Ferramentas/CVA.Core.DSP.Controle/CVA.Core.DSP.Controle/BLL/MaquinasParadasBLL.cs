using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.MODEL;
using CVA.Core.DSP.Controle.VIEW;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.BLL
{
    public class MaquinasParadasBLL
    {
        #region Atributos

        public BusinessOneDAO _businessOneDAO { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public SAPbobsCOM.Company _company { get; set; }

        public MaquinasParadasDAO _maquinasParadasDAO { get; set; }
        public MaquinasParadasView _maquinasParadasView { get; set; }
   
       

        #endregion

        #region Construtor

        public MaquinasParadasBLL()
        {
            //this._businessOneDAO = _businessOneDAO;
            _maquinasParadasDAO = new MaquinasParadasDAO();
        }

        #endregion

        public List<MaquinaModel> GetMaquinas()
        {
            var modelList = new List<MaquinaModel>();

            var result = _maquinasParadasDAO.GetMaquinas();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new MaquinaModel()
                {
                    code = result.Rows[i]["ResCode"].ToString(),
                    name = result.Rows[i]["ResName"].ToString(),
                };

                modelList.Add(model);
            }
            return modelList;
        }

        public List<MotivoModel> GetMotivos()
        {
            var motivoModelList = new List<MotivoModel>();

            var result = _maquinasParadasDAO.GetMotivos();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new MotivoModel()
                {
                    code = result.Rows[i]["code"].ToString(),
                    name = result.Rows[i]["name"].ToString(),
                };

                motivoModelList.Add(model);
            }
            return motivoModelList;
        }

        public List<OperadorModel> GetOperador()
        {
            var operadorModelList = new List<OperadorModel>();

            var result = _maquinasParadasDAO.GetOperador();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new OperadorModel()
                {
                    ID = result.Rows[i]["EMPID"].ToString(),
                    Nome = result.Rows[i]["NOME"].ToString()
                };

                operadorModelList.Add(model);
            }
            return operadorModelList;
        }

        public List<ProdutoModel> GetProduto()
        {
            var produtoModelList = new List<ProdutoModel>();

            var result = _maquinasParadasDAO.GetProduto();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ProdutoModel()
                {
                    code = result.Rows[i]["CODE"].ToString(),
                    descricao = result.Rows[i]["NAME"].ToString()
                };

                produtoModelList.Add(model);
            }
            return produtoModelList;
        }

        public MaxValue GetMaxValueMaqParadas()
        {

            var result = _maquinasParadasDAO.BuscaMaxValueMaqParadas();
            var x = result.Rows[0]["Code"];
            var y = result.Rows[0]["Name"];


            var model = new MaxValue()
            {
                Code = Convert.ToInt32(result.Rows[0]["CODE"].ToString()),
                Name = Convert.ToInt32(result.Rows[0]["NAME"].ToString())
            };
            
            return model;
        }

        //public MaxValue GetPesquisaMaquina(int code)
        //{
        //    var model = new MaxValue();

        //    var result = _maquinasParadasDAO.BuscaMaquinaParada(model.Code);

        //    return model;
        //}

        public void insert(CampoMaquinaModel model)
        {
            var maxvalue = GetMaxValueMaqParadas();
            model.Code = maxvalue.Code;
            model.Name = maxvalue.Name;

            _maquinasParadasDAO.Insert(model);
        }

        public void Update(CampoMaquinaModel model)
        {
            _maquinasParadasDAO.Update(model);
        }

        public void Delete(CampoMaquinaModel model)
        {
            _maquinasParadasDAO.Delete(model);
        }

    }
}
