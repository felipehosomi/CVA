using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class UserViewContract
    {
        private UserViewBLL _userViewBLL { get; set; }

        public UserViewContract()
        {
            this._userViewBLL = new UserViewBLL();
        }

        public List<UserViewModel> Get()
        {
            return _userViewBLL.Get();
        }
    }
}