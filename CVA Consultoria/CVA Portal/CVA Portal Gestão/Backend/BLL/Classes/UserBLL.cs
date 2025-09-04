using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class UserBLL
    {
        #region Atributos
        private UserDAO _userDAO { get; set; }
        private StatusBLL _statusBLL { get; set; }
        private ProfileBLL _profileBLL { get; set; }
        private EmailSenderBLL _emailSenderBLL { get; set; }
        #endregion

        #region Construtor
        public UserBLL()
        {
            this._userDAO = new UserDAO();
            this._profileBLL = new ProfileBLL();
            this._emailSenderBLL = new EmailSenderBLL();
            this._statusBLL = new StatusBLL();
        }
        #endregion


        public UserModel Get(int id)
        {
            try
            {
                var result = _userDAO.Get(id);

                var model = new UserModel()
                {
                    Id = Convert.ToInt32(result.Rows[0]["Id"]),
                    Status = new StatusModel { Id = Convert.ToInt32(result.Rows[0]["StatusId"]) },
                    Name = result.Rows[0]["Name"].ToString(),
                    Email = result.Rows[0]["Email"].ToString()
                };

                model.Profile = _profileBLL.GetUser(model.Id);
                model.Collaborator = GetCollaborator(model.Id);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> Get()
        {
            try
            {
                var users = _userDAO.Get_All();
                foreach (var user in users)
                    user.Profile = _profileBLL.GetUser(user.Id);

                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MessageModel Save(UserModel model)
        {
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(UserModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                model.Password = EncryptBLL.Encrypt("x12345", model.Email);

                if (_userDAO.Insert(model) > 0)
                {
                    NotifyUser(model.Email);
                    return MessageBLL.Generate("Usuario inserido com sucesso!", 0);
                }
                else
                    return MessageBLL.Generate("Erro ao inserir usuário", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Update(UserModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (!string.IsNullOrEmpty(model.Password))
                    model.Password = EncryptBLL.Encrypt(model.Password, model.Email);

                if (_userDAO.Update(model) > 0)
                    return MessageBLL.Generate("Usuário atualizado com sucesso!", 0);
                else
                    return MessageBLL.Generate("Erro ao atualizar usuário", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public CollaboratorModel GetCollaborator(int userID)
        {
            try
            {
                var result = _userDAO.User1_Get(userID);

                var model = new CollaboratorModel();

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    model.Id = Convert.ToInt32(result.Rows[i]["Id"].ToString());
                    model.Nome = result.Rows[i]["Nome"].ToString();
                    model.Email = result.Rows[i]["Email"].ToString();
                    model.Telefone = result.Rows[i]["Telefone"].ToString();
                    model.Celular = result.Rows[i]["Celular"].ToString();
                    model.CPF = result.Rows[i]["CPF"].ToString();
                    model.CNPJ = result.Rows[i]["CNPJ"].ToString();
                    model.RG = result.Rows[i]["RG"].ToString();
                    model.EmissaoRG = Convert.ToDateTime(result.Rows[i]["EmissaoRG"].ToString());
                    model.OrgaoEmissor = result.Rows[i]["OrgaoEmissor"].ToString();
                    model.Passaporte = result.Rows[i]["Passaporte"].ToString();
                    model.Nacionalidade = result.Rows[i]["Nacionalidade"].ToString();
                    if (!String.IsNullOrEmpty(result.Rows[i]["ValidadePassaporte"].ToString()))
                        model.ValidadePassaporte = Convert.ToDateTime(result.Rows[i]["ValidadePassaporte"].ToString());

                    model.Naturalidade = result.Rows[i]["Naturalidade"].ToString();
                    model.DataNascimento = Convert.ToDateTime(result.Rows[i]["DataNascimento"].ToString());
                    model.EstadoCivil = Convert.ToInt32(result.Rows[i]["EstadoCivil"].ToString());
                    model.Genero = Convert.ToInt32(result.Rows[i]["Genero"].ToString());
                    model.GerenciaProjetos = Convert.ToInt32(result.Rows[i]["GerenciaProjetos"].ToString());
                    model.LimiteHoras = Convert.ToDateTime(result.Rows[i]["LimiteHoras"].ToString());
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool VerifyExist(string email)
        {
            try
            {
                return _userDAO.IsExist(email) == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel ValidateModel(UserModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return MessageBLL.Generate("Informe o e-mail do usuário", 99, true);
            if (string.IsNullOrEmpty(model.Name))
                return MessageBLL.Generate("Informe o nome do usuário", 99, true);
            if (model.Profile == null)
                return MessageBLL.Generate("Informe o perfil do usuário", 99, true);
            if (model.Collaborator == null)
                return MessageBLL.Generate("Informe o colaborador", 99, true);
            if (model.Id == 0)
                if (VerifyExist(model.Email))
                    return MessageBLL.Generate("Usuário já cadastrado", 99, true);

            return null;
        }

        public List<StatusModel> GetSpecificStatus()
        {
            return _statusBLL.Get(UserModel.oObjectType);
        }

        public MessageModel UpdateByUser(UserModel user)
        {
            try
            {
                if (user.NewPassword == user.RepeatPassword)
                    user.Password = EncryptBLL.Encrypt(user.NewPassword, user.Email);
                else
                    return MessageBLL.Generate("Senhas não coincidem", 99, true);

                var result = _userDAO.Update_ByUSer(user);
                if (result > 0)
                    return MessageBLL.Generate("Cadastro atualizado com sucesso", 0);
                return MessageBLL.Generate("Erro ao atualizar cadastro", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> GetUsers()
        {
            return _userDAO.GetUsers();
        }

        public void NotifyUser(string email)
        {
            _emailSenderBLL.NotifyUser(email);
        }
    }
}