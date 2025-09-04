using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class PricingItemBLL
    {
        #region Atributos
        private PricingItemDAO _pricingItemDAO { get; set; }
        #endregion

        #region Construtor
        public PricingItemBLL()
        {
            this._pricingItemDAO = new PricingItemDAO();
        }
        #endregion


        public PricingItemModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<PricingItemModel> Get_All()
        {
            throw new NotImplementedException();
        }

        public MessageModel Insert(PricingItemModel model)
        {
            throw new NotImplementedException();
        }

        public List<PricingItemModel> Get_PricingItens(int id)
        {
            var result = _pricingItemDAO.Get_PricingItens(id);
            return LoadModel(result);
        }
        public List<PricingItemModel> Get_PricingItens_oprt(int id)
        {
            var result = _pricingItemDAO.Get_PricingItens_oprt(id);
            return LoadModel(result);
        }

        public MessageModel Insert_PricingItem(PricingModel model, PricingItemModel item)
        {
            var isValid = ValidateModel(item);
            if (isValid != null)
                return isValid;

            if (_pricingItemDAO.Insert_PricingItem(model, item) > 0)
                return MessageBLL.Generate("Item inserido com sucesso", 0);

            else
                return MessageBLL.Generate("Erro ao inserir item", 99, true);
        }

        public List<PricingItemModel> LoadModel(DataTable result)
        {
            var modelList = new List<PricingItemModel>();

            #region Projeto
            if (result.Rows[0]["Tipo"].ToString() == "P")
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    var model = new PricingItemModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                        Especialidade = new SpecialtyModel()
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Especialidade.Id"].ToString()),
                            Name = result.Rows[i]["Especialidade.Nome"].ToString()
                        },
                        Colaborador = result.Rows[i]["Colaborador"].ToString(),

                        EspecialidadeHoras = Convert.ToDouble(result.Rows[i]["EspecialidadeHoras"].ToString()),
                        EspecialidadeValor = Convert.ToDouble(result.Rows[i]["EspecialidadeValor"].ToString()),
                        EspecialidadeCusto = Convert.ToDouble(result.Rows[i]["EspecialidadeCusto"].ToString())
                    };
                    modelList.Add(model);
                }
            }
            #endregion

            #region Oportunidade
            if (result.Rows[0]["Tipo"].ToString() == "O")
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    var model = new PricingItemModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                        Especialidade = new SpecialtyModel()
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Especialidade.Id"].ToString()),
                            Name = result.Rows[i]["Especialidade.Nome"].ToString()
                        },
                        Colaborador = result.Rows[i]["Colaborador"].ToString(),

                        EspecialidadeHoras = Convert.ToDouble(result.Rows[i]["EspecialidadeHoras"].ToString()),
                        EspecialidadeValor = Convert.ToDouble(result.Rows[i]["EspecialidadeValor"].ToString()),
                        EspecialidadeCusto = Convert.ToDouble(result.Rows[i]["EspecialidadeCusto"].ToString()),
                        ValorBackoffice = Convert.ToDouble(result.Rows[i]["ValorBackoffice"].ToString()),
                        ValorRisco = Convert.ToDouble(result.Rows[i]["ValorRisco"].ToString()),
                        ValorMargem = Convert.ToDouble(result.Rows[i]["ValorMargem"].ToString()),
                        ValorComissao = Convert.ToDouble(result.Rows[i]["ValorComissao"].ToString()),
                        ValorImposto = Convert.ToDouble(result.Rows[i]["ValorImposto"].ToString()),
                        HotelDiarias = Convert.ToDouble(result.Rows[i]["HotelDiarias"].ToString()),
                        HotelValor = Convert.ToDouble(result.Rows[i]["HotelValor"].ToString()),
                        HotelTotal = Convert.ToDouble(result.Rows[i]["HotelTotal"].ToString()),
                        KmTrechos = Convert.ToDouble(result.Rows[i]["KmTrechos"].ToString()),
                        KmDistancia = Convert.ToDouble(result.Rows[i]["KmDistancia"].ToString()),
                        KmValor = Convert.ToDouble(result.Rows[i]["KmValor"].ToString()),
                        KmTotal = Convert.ToDouble(result.Rows[i]["KmTotal"].ToString()),
                        AlimentacaoDias = Convert.ToDouble(result.Rows[i]["AlimentacaoDias"].ToString()),
                        AlimentacaoValor = Convert.ToDouble(result.Rows[i]["AlimentacaoValor"].ToString()),
                        AlimentacaoTotal = Convert.ToDouble(result.Rows[i]["AlimentacaoTotal"].ToString()),
                        DeslocamentoHoras = Convert.ToDouble(result.Rows[i]["DeslocamentoHoras"].ToString()),
                        DeslocamentoValor = Convert.ToDouble(result.Rows[i]["DeslocamentoValor"].ToString()),
                        DeslocamentoTotal = Convert.ToDouble(result.Rows[i]["DeslocamentoTotal"].ToString()),
                        AereoTrechos = Convert.ToDouble(result.Rows[i]["AereoTrechos"].ToString()),
                        AereoValor = Convert.ToDouble(result.Rows[i]["AereoValor"].ToString()),
                        AereoTotal = Convert.ToDouble(result.Rows[i]["AereoTotal"].ToString()),
                        RecursoSubTotal = Convert.ToDouble(result.Rows[i]["RecursoSubTotal"].ToString()),
                        RecursoDespesas = Convert.ToDouble(result.Rows[i]["RecursoDespesas"].ToString()),
                        RecursoValorComDespesas = Convert.ToDouble(result.Rows[i]["RecursoValorComDespesas"].ToString()),
                        RecursoValorComImpostos = Convert.ToDouble(result.Rows[i]["RecursoValorComImpostos"].ToString()),
                        RecursoValorHorasSemDespesa = Convert.ToDouble(result.Rows[i]["RecursoValorHorasSemDespesa"].ToString()),
                        RecursoValorHorasComDespesa = Convert.ToDouble(result.Rows[i]["RecursoValorHorasComDespesa"].ToString())
                    };
                    modelList.Add(model);
                }
            }
            #endregion
            
            return modelList;
        }

        public MessageModel Remove(int id)
        {
            try
            {
                if (_pricingItemDAO.Remove(id) > 0)
                    return MessageBLL.Generate("Item removido com sucesso", 0);
                else
                    return MessageBLL.Generate("Erro ao remover item", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Update(PricingItemModel model)
        {
            throw new NotImplementedException();
        }

        public MessageModel Opportunitty_Insert_PricingItem(PricingModel model, PricingItemModel item)
        {
            var isValid = ValidateModel(item);
            if (isValid != null)
                return isValid;

            if (_pricingItemDAO.Opportunitty_Insert_PricingItem(model, item) > 0)
                return MessageBLL.Generate("Item inserido com sucesso", 0);

            else
                return MessageBLL.Generate("Erro ao inserir item", 99, true);
        }

        public MessageModel ValidateModel(PricingItemModel model)
        {
            //if (model.Especialidade == null)
            //    return MessageBLL.Generate("Obrigatório informar a especialidade", 99, true);
            //if (model.Valor == null)
            //    return MessageBLL.Generate("Obrigatório informar o valor", 99, true);
            //if (model.Horas == null)
            //    return MessageBLL.Generate("Obrigatório informar as horas", 99, true);

            //else
            return null;
        }

        public void Remove_Oprt(int id)
        {
            _pricingItemDAO.Remove_Oprt(id);
        }
    }
}
