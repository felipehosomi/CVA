using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class UserContract
    {
        private UserBLL _userBLL { get; set; }

        public UserContract()
        {
            this._userBLL = new UserBLL();
        }

        public UserModel GetUser(int id)
        {
            return _userBLL.Get(id);
        }

        public MessageModel Save(UserModel user)
        {
            return _userBLL.Save(user);
        }

        public List<StatusModel> GetSpecificStatus()
        {
            return _userBLL.GetSpecificStatus();
        }

        public List<UserModel> GetUsers()
        {
            return _userBLL.GetUsers();
        }

        public MessageModel Update_ByUser(UserModel user)
        {
            return _userBLL.UpdateByUser(user);
        }
    }
}