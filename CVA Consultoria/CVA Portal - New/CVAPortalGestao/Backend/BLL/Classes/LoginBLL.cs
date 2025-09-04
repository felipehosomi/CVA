using System;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class LoginBLL
    {
        #region Atributos
        private LoginDAO _loginDAO { get; set; }
        private ProfileBLL _profileBLL { get; set; }
        private CollaboratorBLL _collaboratorBLL { get; set; }
        #endregion

        #region Construtor
        public LoginBLL()
        {
            this._loginDAO = new LoginDAO();
            this._profileBLL = new ProfileBLL();
            this._collaboratorBLL = new CollaboratorBLL();
        }
        #endregion

        public bool VerifyValues(ref UserModel user, string password)
        {
            try
            {
                var decrypt = EncryptBLL.Decrypt(user.Password, user.Email);
                return decrypt.Equals(password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MessageModel LogOff(int userId)
        {
            try
            {
                this._loginDAO = new LoginDAO();
                if (_loginDAO.LogOff(userId) > 0)
                    return MessageBLL.Generate("Logoff efetuado com sucesso!", 0);
                return MessageBLL.Generate("Erro ao efetuar logoff", 100, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 100, true);
            }
        }

        public UserModel LogIn(string email, string password)
        {
            try
            {
                var encrypt = EncryptBLL.Encrypt(password, email);
                var user = _loginDAO.LogIn(email);

                if (user.Id == 0)
                    return null;

                user.Email = email;
                if (VerifyValues(ref user, password))
                {
                    user.Password = string.Empty;
                    user.Profile = _profileBLL.GetUser(user.Id);
                    user.Collaborator = _collaboratorBLL.GetByUserID(user.Id);

                    return user;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UserModel FirstAccess(UserModel user)
        {
            try
            {
                if (String.IsNullOrEmpty(user.NewPassword))
                    return null;
                if (String.IsNullOrEmpty(user.RepeatPassword))
                    return null;
                if (String.IsNullOrEmpty(user.OldPassword))
                    return null;

                if (user.NewPassword == user.RepeatPassword)
                {
                    if (LogIn(user.Email, user.OldPassword) != null)
                    {
                        _loginDAO.AlterPassword(user.Email, EncryptBLL.Encrypt(user.NewPassword, user.Email));
                        user.Profile = _profileBLL.GetUser(user.Id);
                        user.Collaborator = _collaboratorBLL.GetByUserID(user.Id);
                    }

                    else
                        return null;

                    //nao retorna as senhas para a view
                    user.Password = String.Empty;
                    user.NewPassword = String.Empty;
                    user.OldPassword = String.Empty;

                    return user;
                }

                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
