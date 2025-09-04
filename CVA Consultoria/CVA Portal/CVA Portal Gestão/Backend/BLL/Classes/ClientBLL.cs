using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;
using System.Linq;

namespace BLL.Classes
{
    public class ClientBLL
    {
        #region Atributos
        private ClientDAO _clientDAO { get; set; }
        private StatusBLL _statusBLL { get; set; }
        private ContactBLL _contactBLL { get; set; }
        private AddressBLL _addressBLL { get; set; }
        private PoliticExpenseBLL _politicExpenseBLL { get; set; }
        #endregion

        #region Construtor
        public ClientBLL()
        {
            this._clientDAO = new ClientDAO();
            this._statusBLL = new StatusBLL();
            this._contactBLL = new ContactBLL();
            this._addressBLL = new AddressBLL();
            this._politicExpenseBLL = new PoliticExpenseBLL();
        }
        #endregion

        public ClientModel Get(int id)
        {
            try
            {
                var model = _clientDAO.Get(id);
                if (model != null)
                {
                    model.Contact = GetContacts(model.Id);
                    model.Addresses = GetAddress(model);
                    model.PoliticExpense = GetPoliticExpense(model);

                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ClientModel> LoadCombo()
        {
            var list = new List<ClientModel>();
            var result = _clientDAO.LoadCombo();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ClientModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"]),
                    Name = result.Rows[i]["Name"].ToString(),
                    Tag = result.Rows[i]["Tag"].ToString(),
                };
                list.Add(model);
            }
            return list;
        }

        public List<ClientModel> Search(string name)
        {
            var result = _clientDAO.Search(name);
            return LoadSimplifiedModel(result);
        }

        public MessageModel Save(ClientModel model)
        {
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(ClientModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                model.Id = _clientDAO.Insert(model);
                if (model.Id == 0)
                    return MessageBLL.Generate("Erro ao cadastrar cliente", 99, true);

                foreach (var item in model.Contact)
                {
                    item.User = new UserModel() { Id = model.User.Id };
                    item.Status = new StatusModel() { Id = model.Status.Id };

                    var contactResult = _contactBLL.Insert(item);
                    if (contactResult.Error == null)
                    {
                        var IdItem = _clientDAO.Add_Contact(model, contactResult.Success.Code);
                        if (IdItem == 0)
                            return MessageBLL.Generate("Erro adicionar contato ao cliente", contactResult.Error.Code, true);
                    }
                }

                foreach (var item in model.Addresses)
                {
                    item.User = new UserModel() { Id = model.User.Id };
                    item.Status = new StatusModel() { Id = model.Status.Id };

                    var addressResult = _addressBLL.Save(item);
                    if (addressResult.Error == null)
                    {
                        var IdItem = _clientDAO.Add_Address(model, addressResult.Success.Code);
                        if (IdItem == 0)
                            return MessageBLL.Generate("Erro adicionar endereços do cliente", addressResult.Error.Code, true);
                    }
                }

                if (model.LocalPoliticExpense == 0)
                {
                    var politicExpenseResult = AddPoliticExpense(ref model);
                    if (politicExpenseResult.Error != null) return politicExpenseResult;
                }

                return MessageBLL.Generate("Cliente cadastrado com sucesso", 99);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MessageModel Update(ClientModel model)
        {
            try
            {
                var update = _clientDAO.Update(model);
                if (update == 0) return MessageBLL.Generate("Erro ao atualizar cliente!", 99, true);

                _clientDAO.Delete_Contact(model);
                foreach (var item in model.Contact)
                {
                    item.User = new UserModel() { Id = model.User.Id };
                    item.Status = new StatusModel() { Id = model.Status.Id };

                    var contactResult = _contactBLL.Insert(item);
                    if (contactResult.Error == null)
                    {
                        var IdItem = _clientDAO.Add_Contact(model, contactResult.Success.Code);
                        if (IdItem == 0)
                            return MessageBLL.Generate("Erro adicionar contato ao cliente", contactResult.Error.Code, true);
                    }
                }

                _clientDAO.Delete_Address(model);
                foreach (var item in model.Addresses)
                {
                    item.User = new UserModel() { Id = model.User.Id };
                    item.Status = new StatusModel() { Id = model.Status.Id };

                    var addressResult = _addressBLL.Save(item);
                    if (addressResult.Error == null)
                    {
                        var IdItem = _clientDAO.Add_Address(model, addressResult.Success.Code);
                        if (IdItem == 0)
                            return MessageBLL.Generate("Erro adicionar endereços do cliente", addressResult.Error.Code, true);
                    }
                }

                _clientDAO.Delete_PoliticExpense(model);
                var politicExpenseResult = AddPoliticExpense(ref model);
                if (politicExpenseResult.Error != null)
                    return politicExpenseResult;



                return MessageBLL.Generate("Cliente atualizado com sucesso!", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }





        public List<ClientModel> LoadSimplifiedModel(DataTable result)
        {
            var modelList = new List<ClientModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ClientModel()
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Name = result.Rows[i]["Name"].ToString(),
                    CNPJ = result.Rows[i]["CNPJ"].ToString(),
                    Status = new StatusModel() { Descricao = result.Rows[i]["Status.Descricao"].ToString() }
                };
                modelList.Add(model);
            }
            return modelList;
        }







        public List<StatusModel> GetSpecificStatus()
        {
            return _statusBLL.Get(ClientModel.oObjectType);
        }




        public List<ContactModel> GetContacts(int id)
        {
            try
            {
                if (id != 0)
                    return _clientDAO.Get_Contacts(id);
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<AddressModel> GetAddress(ClientModel client)
        {
            try
            {
                List<AddressModel> addressList = _clientDAO.Get_Address(client);
                for (int i = 0; i < addressList.Count; i++)
                {
                    addressList[i].Uf = new UfModel();
                    addressList[i].Uf.Id = addressList[i].Uf_Id;
                }

                return addressList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PoliticExpenseModel> GetPoliticExpense(ClientModel client)
        {
            try
            {
                var data = _clientDAO.Get_PoliticExpenses(client);
                if (data.Rows.Count > 0)
                {
                    var list = new List<PoliticExpenseModel>();
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                       list.Add(new PoliticExpenseModel()
                        {
                            Value = data.Rows[i]["Value"].ToString(),
                            Expense = new ExpenseTypeModel()
                            {
                                Name = data.Rows[i]["Name"].ToString(),
                                Id = Convert.ToInt32(data.Rows[i]["Id"].ToString())
                            },
                            Status = new StatusModel{Id = Convert.ToInt32(data.Rows[i]["Status"])}
                        });
                    }
                    return list;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public MessageModel AddPoliticExpense(ref ClientModel client)
        {
            try
            {
                if (client.PoliticExpense != null)
                {
                    foreach (var item in client.PoliticExpense)
                    {
                        item.User = client.User;
                        item.Status = new StatusModel()
                        {
                            Id = client.Status.Id
                        };

                        var result = _politicExpenseBLL.Save(item);
                        if (result.Error != null) return result;

                        if (_clientDAO.Add_PoliticExpense(client, item) == 0)
                            return MessageBLL.Generate("Erro ao adicionar política de despesa ao cliente", 99, true);
                    }
                }
                return MessageBLL.Generate("Política(s) de despesa adicionada(s) ao cliente", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public MessageModel ValidateModel(ClientModel client)
        {
            if (client.Addresses == null)
                return MessageBLL.Generate("Obrigatório o envio de pelo menos um endereço", 99, true);
            if (string.IsNullOrEmpty(client.CNPJ))
                return MessageBLL.Generate("Obrigatório o envio do CNPJ", 99, true);
            if (string.IsNullOrEmpty(client.FantasyName))
                return MessageBLL.Generate("Obrigatório informar o nome fantasia", 99, true);
            if (string.IsNullOrEmpty(client.IE))
                client.IE = "I";
            if (string.IsNullOrEmpty(client.Name))
                return MessageBLL.Generate("Obrigatório informar a razão social", 99, true);
            if (string.IsNullOrEmpty(client.Description))
                client.Description = "";

            var statusId = client.Status.Id;
            var statusSelected = (from stu in _statusBLL.Get(ClientModel.oObjectType)
                                  where stu.Id == statusId
                                  select stu).FirstOrDefault();
            if (statusSelected == null)
                return MessageBLL.Generate("Obrigatório a seleção de um status válido", 99, true);

            return null;
        }
    }
}