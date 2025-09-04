using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class PricingBLL
    {
        #region Atributos
        private PricingItemBLL _pricingItemBLL { get; set; }
        private PricingDAO _pricingDAO { get; set; }
        #endregion

        #region Construtor
        public PricingBLL()
        {
            this._pricingItemBLL = new PricingItemBLL();
            this._pricingDAO = new PricingDAO();
        }
        #endregion


        public PricingModel Get(int id)
        {
            try
            {
                var result = _pricingDAO.Get(id);
                return LoadModel(result)[0];
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        public PricingModel Get_Info(int id)
        {
            try
            {
                var result = _pricingDAO.Get_info(id);
                return LoadModel_oprt(result)[0];
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public List<PricingModel> Get_All()
        {
            try
            {
                var result = _pricingDAO.Get_All();
                return LoadModel(result);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public PricingModel Get_By_Project(int id)
        {
            try
            {
                var result = _pricingDAO.Get_By_Project(id);
                if (result.Rows.Count > 0)
                    return LoadModel(result)[0];
                else
                    return new PricingModel();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public PricingModel Get_By_Opportunitty(int id)
        {
            try
            {
                var result = _pricingDAO.Get_By_Opportunitty(id);
                if (result.Rows.Count > 0)
                    return LoadModel_oprt(result)[0];
                else
                {
                    var model = new PricingModel();
                    model.PorcentagemBackoffice = 15.00;
                    model.PorcentagemRisco = 10.00;
                    model.PorcentagemMargem = 25.00;
                    model.PorcentagemComissao = 2.00;
                    model.PorcentagemImposto = 18.33;

                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public MessageModel Insert_ProjectPricing(PricingModel model, int id)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                model.Id = _pricingDAO.Insert(model, id);
                if (model.Id > 0)
                {
                    foreach (var item in model.Itens)
                        _pricingItemBLL.Insert_PricingItem(model, item);

                    return MessageBLL.Generate("Pricing inserido com sucesso", 0);
                }

                else
                    return MessageBLL.Generate("Erro ao inserir Pricing", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel InsertOprt(PricingModel model, int id)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                model.Id = _pricingDAO.Opportunitty_Insert(model, id);
                if (model.Id > 0)
                {
                    foreach (var item in model.Itens)
                        _pricingItemBLL.Opportunitty_Insert_PricingItem(model, item);

                    return MessageBLL.Generate("Pricing inserido com sucesso", 0);
                }

                else
                    return MessageBLL.Generate("Erro ao inserir Pricing", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Opportunitty_Insert(PricingModel model, int id)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                model.Id = _pricingDAO.Opportunitty_Insert(model, id);
                if (model.Id > 0)
                {
                    foreach (var item in model.Itens)
                        _pricingItemBLL.Opportunitty_Insert_PricingItem(model, item);

                    return MessageBLL.Generate("Pricing inserido com sucesso", 0);
                }

                else
                    return MessageBLL.Generate("Erro ao inserir Pricing", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Update_ProjectPricing(PricingModel model, int id)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_pricingDAO.Update(model, id) > 0)
                {
                    _pricingItemBLL.Remove(model.Id);
                    foreach (var item in model.Itens)
                        _pricingItemBLL.Insert_PricingItem(model, item);
                    return MessageBLL.Generate("Pricing atualizado com sucesso", 0);
                }
                else
                    return MessageBLL.Generate("Erro ao atualizar Pricing", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Update_OpportunityPricing(PricingModel model, int id)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_pricingDAO.Opportunitty_Update(model, id) > 0)
                {
                    _pricingItemBLL.Remove_Oprt(model.Id);
                    foreach (var item in model.Itens)
                        _pricingItemBLL.Opportunitty_Insert_PricingItem(model, item);
                    return MessageBLL.Generate("Pricing atualizado com sucesso", 0);
                }
                else
                    return MessageBLL.Generate("Erro ao atualizar Pricing", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Remove(int id)
        {
            try
            {
                if (_pricingDAO.Remove(id) > 0)
                    return MessageBLL.Generate("Pricing removido com sucesso", 0);
                else
                    return MessageBLL.Generate("Erro ao remover Pricing", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel ValidateModel(PricingModel model)
        {
            //if (model.Nome == null)
            //    return MessageBLL.Generate("Obrigatório informar o nome", 99, true);
            //if (model.Data == null)
            //    return MessageBLL.Generate("Obrigatório informar a data", 99, true);
            //if (model.Solicitante == null)
            //    return MessageBLL.Generate("Obrigatório informar o solicitante", 99, true);
            if (model.PorcentagemBackoffice == null)
                return MessageBLL.Generate("Obrigatório informar a porcentagem de backoffice", 99, true);
            if (model.PorcentagemRisco == null)
                return MessageBLL.Generate("Obrigatório informar a porcentagem de risco", 99, true);
            if (model.PorcentagemMargem == null)
                return MessageBLL.Generate("Obrigatório informar a porcentagem de margem", 99, true);
            if (model.PorcentagemComissao == null)
                return MessageBLL.Generate("Obrigatório informar a porcentagem de comissão", 99, true);
            if (model.PorcentagemImposto == null)
                return MessageBLL.Generate("Obrigatório informar a porcentagem de imposto", 99, true);
            if (model.Itens == null)
                return MessageBLL.Generate("Obrigatório informar ao menos um item para o Pricing", 99, true);

            else
                return null;
        }

        public List<PricingModel> LoadModel(DataTable result)
        {
            var modelList = new List<PricingModel>();

            if (result != null)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    var model = new PricingModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                        PorcentagemBackoffice = Convert.ToDouble(result.Rows[i]["PorcentagemBackoffice"].ToString()),
                        PorcentagemRisco = Convert.ToDouble(result.Rows[i]["PorcentagemRisco"].ToString()),
                        PorcentagemMargem = Convert.ToDouble(result.Rows[i]["PorcentagemMargem"].ToString()),
                        PorcentagemComissao = Convert.ToDouble(result.Rows[i]["PorcentagemComissao"].ToString()),
                        PorcentagemImposto = Convert.ToDouble(result.Rows[i]["PorcentagemImposto"].ToString())
                    };
                    model.Itens = _pricingItemBLL.Get_PricingItens(model.Id);

                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public List<PricingModel> LoadModel_oprt(DataTable result)
        {
            var modelList = new List<PricingModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new PricingModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    PorcentagemBackoffice = Convert.ToDouble(result.Rows[i]["PorcentagemBackoffice"].ToString()),
                    PorcentagemRisco = Convert.ToDouble(result.Rows[i]["PorcentagemRisco"].ToString()),
                    PorcentagemMargem = Convert.ToDouble(result.Rows[i]["PorcentagemMargem"].ToString()),
                    PorcentagemComissao = Convert.ToDouble(result.Rows[i]["PorcentagemComissao"].ToString()),
                    PorcentagemImposto = Convert.ToDouble(result.Rows[i]["PorcentagemImposto"].ToString())
                };
                model.Itens = _pricingItemBLL.Get_PricingItens_oprt(model.Id);

                modelList.Add(model);
            }
            return modelList;
        }
    }
}
