using BLL.Classes;
using MODEL.Classes;
using System;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class LoginContract
    {
        private LoginBLL _loginBLL { get; set; }

        public LoginContract()
        {
            this._loginBLL = new LoginBLL();
        }

        public UserModel LogIn(string email, string password)
        {
            try
            {
                return _loginBLL.LogIn(email, password);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MessageModel LogOff(int userId)
        {
            try
            {
                return _loginBLL.LogOff(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public UserModel FirstAccess(UserModel model)
        {
            return _loginBLL.FirstAccess(model);
        }
    }
}