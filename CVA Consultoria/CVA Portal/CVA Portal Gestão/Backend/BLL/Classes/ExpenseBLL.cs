using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ExpenseBLL
    {
        #region Atributos
        public ExpenseDAO _expenseDAO { get; set; }
        public ExpenseTypeBLL _expenseTypeBLL { get; set; }
        #endregion

        #region Construtor
        public ExpenseBLL()
        {
            _expenseDAO = new ExpenseDAO();
            _expenseTypeBLL = new ExpenseTypeBLL();

        }
        #endregion

        public ExpenseModel Get(int id)
        {
            var result = _expenseDAO.Get(id);
            return LoadModel(result)[0];
        }

        public List<ExpenseModel> Get_ByUser(int id)
        {
            var result = _expenseDAO.Get_ByUser(id);
            return LoadModel(result);
        }

        public MessageModel Save(ExpenseModel model)
        {
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(ExpenseModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_expenseDAO.Insert(model) > 0)
                    return MessageBLL.Generate("Despesa registrada com sucesso.", 0);
                else
                    return MessageBLL.Generate("Erro ao inserir despesa.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate("Atenção: " + ex.Message, 99, true);
            }
        }

        public MessageModel Update(ExpenseModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_expenseDAO.Update(model) > 0)
                    return MessageBLL.Generate("Despesa atualizada com sucesso.", 0);
                else
                    return MessageBLL.Generate("Erro ao atualizar a despesa.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate("Atenção: " + ex.Message, 99, true);
            }
        }

        public MessageModel Remove(int id)
        {
            try
            {
                if (_expenseDAO.Remove(id) > 0)
                    return MessageBLL.Generate("Despesa excluída com sucesso.", 0);
                else
                    return MessageBLL.Generate("Erro ao excluir despesa.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate("Atenção: " + ex.InnerException, 99, true);
            }
        }

        public List<ExpenseModel> Search(int col, int cli, int prj, DateTime? de, DateTime? ate)
        {
            try
            {
                var result = _expenseDAO.Search(col, cli, prj, de, ate);
                return LoadExtractedModel(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpenseModel> LoadExtractedModel(DataTable data)
        {
            var modelList = new List<ExpenseModel>();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                var model = new ExpenseModel()
                {
                    Data = Convert.ToDateTime(data.Rows[i]["Date"].ToString()),
                    User = new UserModel { Name = data.Rows[i]["Collaborator.Name"].ToString() },
                    Descricao = data.Rows[i]["Description"].ToString(),
                    NumNota = data.Rows[i]["DocumentNumber"].ToString(),
                    ValorNota = data.Rows[i]["DocumentNumberTotal"].ToString(),
                    ValorReembolso = data.Rows[i]["ExpenseTotal"].ToString(),

                    TipoDespesa = new ExpenseTypeModel() { Name = data.Rows[i]["ExpenseType.Name"].ToString() },

                    Projeto = new ProjectModel()
                    {
                        Cliente = new ClientModel {
                            Name = data.Rows[i]["Cliente"].ToString(),
                            PoliticExpense = new List<PoliticExpenseModel>()
                        },
                        Nome = data.Rows[i]["Project.Nome"].ToString(),
                        ResponsavelDespesa = data.Rows[i]["Responsavel"].ToString()
                    },
                };
                var politicaDespesa = new PoliticExpenseModel();
                politicaDespesa.Value = data.Rows[i]["Limite"].ToString();

                model.Projeto.Cliente.PoliticExpense.Add(politicaDespesa);





                modelList.Add(model);
            }
            return modelList;
        }

        public List<ExpenseModel> LoadModel(DataTable result)
        {
            var modelList = new List<ExpenseModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ExpenseModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Data = Convert.ToDateTime(result.Rows[i]["Date"].ToString()),
                    Descricao = result.Rows[i]["Description"].ToString(),
                    NumNota = result.Rows[i]["DocumentNumber"].ToString(),
                    ValorNota = result.Rows[i]["DocumentNumberTotal"].ToString(),
                    Quilometragem = result.Rows[i]["ExpenseKilometer"].ToString(),
                    ValorReembolso = result.Rows[i]["ExpenseTotal"].ToString(),
                    ValorDespesa = result.Rows[i]["ExpenseUserTotal"].ToString(),
                    TipoDespesa = new ExpenseTypeModel()
                    {
                        Id = Convert.ToInt32(result.Rows[i]["ExpenseType.Id"].ToString()),
                        Name = result.Rows[i]["ExpenseType.Name"].ToString(),
                    },
                    Projeto = new ProjectModel()
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Project.Id"].ToString()),
                        Nome = result.Rows[i]["Project.Nome"].ToString(),
                        Codigo = result.Rows[i]["Project.Codigo"].ToString(),
                        Tag = result.Rows[i]["Project.Tag"].ToString()
                    }
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public MessageModel ValidateModel(ExpenseModel model)
        {
            if (model.Projeto == null)
                return MessageBLL.Generate("Obrigatório informar o projeto.", 99, true);
            if (model.Data == null)
                return MessageBLL.Generate("Obrigatório informar a data.", 99, true);
            if (model.Data.Date > DateTime.Today)
                return MessageBLL.Generate("Não é possível lançar despesas com data futura.", 99, true);
            if (model.Data.Date.Month != DateTime.Today.Month)
                return MessageBLL.Generate("Período fechado para lançamento de despesas.", 99, true);
            if (model.TipoDespesa == null)
                return MessageBLL.Generate("Obrigatório informar o tipo de despesa.", 99, true);
            if (String.IsNullOrEmpty(model.Descricao))
                return MessageBLL.Generate("Obrigatório informar a descrição.", 99, true);
            if (String.IsNullOrEmpty(model.ValorDespesa) && String.IsNullOrEmpty(model.Quilometragem))
                return MessageBLL.Generate("Obrigatório informar o valor de reembolso ou quilometragem.", 99, true);
            if (Convert.ToDecimal(model.ValorDespesa) <= 0 && Convert.ToDecimal(model.Quilometragem) <= 0)
                return MessageBLL.Generate("O valor para reembolso ou quilometragem deve ser um maior do que zero.", 99, true);
            if (String.IsNullOrEmpty(model.NumNota))
                return MessageBLL.Generate("Obrigatório informar o número da NF.", 99, true);
            if (String.IsNullOrEmpty(model.ValorNota))
                return MessageBLL.Generate("Obrigatório informar o valor da NF.", 99, true);
            if (Convert.ToDecimal(model.ValorNota) <= 0)
                return MessageBLL.Generate("O valor da NF deve ser maior do que zero.", 99, true);
            if (String.IsNullOrEmpty(model.Anexo))
                model.Anexo = "";
            if (model.ValorNota.Contains("."))
                model.ValorNota = model.ValorNota.Replace(".", ",");
            if (model.ValorReembolso.Contains("."))
                model.ValorReembolso = model.ValorReembolso.Replace(".", ",");
            if (model.ValorDespesa == null)
                model.ValorDespesa = "0";
            if (model.ValorDespesa.Contains("."))
                model.ValorDespesa = model.ValorDespesa.Replace(".", ",");
            if (model.Quilometragem == null)
                model.Quilometragem = "0";
            if (model.Quilometragem.Contains(","))
                model.Quilometragem = model.Quilometragem.Replace(",", ".");

            return null;
        }
    }
}