using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class UserViewBLL
    {
        #region Atributos
        private UserViewDAO _userViewDAO { get; set; }
        #endregion

        #region Construtor
        public UserViewBLL()
        {
            this._userViewDAO = new UserViewDAO();
        }
        #endregion

        public List<UserViewModel> Get()
        {
            try
            {
                return _userViewDAO.Get();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserViewModel> Get_ByProfileID(int profileID)
        {
            try
            {
                return _userViewDAO.Get_ByProfileID(profileID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
