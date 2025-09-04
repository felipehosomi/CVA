using BLL.Classes;
using MODEL.Classes;

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class AuthorizationContract
    {
        #region Atributos
        private AuthorizationBLL _authorizationBLL { get; set; }
        #endregion

        #region Construtor
        public AuthorizationContract()
        {
            this._authorizationBLL = new AuthorizationBLL();
        }
        #endregion

        public List<AuthorizedDayModel> Get_DiasAutorizados(int idCol)
        {
            return _authorizationBLL.Get_DiasAutorizados(idCol);
        }

        public MessageModel AddDiaAutorizado(AuthorizedDayModel model)
        {
            return _authorizationBLL.AddDiaAutorizado(model);
        }

        public MessageModel RemoveDiaAutorizado(int id)
        {
            return _authorizationBLL.RemoveDiaAutorizado(id);
        }

        public List<AuthorizedHoursModel> Get_HorasAutorizadas(int idCol)
        {
            return _authorizationBLL.Get_HorasAutorizadas(idCol);
        }

        public MessageModel AddHorasAutorizadas(AuthorizedHoursModel model)
        {
            return _authorizationBLL.AddHorasAutorizadas(model);
        }

        public MessageModel RemoveHorasAutorizadas(int id)
        {
            return _authorizationBLL.RemoveHorasAutorizadas(id);
        }


        public List<HoursLimitModel> Get_LimiteHoras(int idCol)
        {
            return _authorizationBLL.Get_LimiteHoras(idCol);
        }

        public MessageModel AddLimiteHoras(HoursLimitModel model)
        {
            return _authorizationBLL.AddLimiteHoras(model);
        }

        public MessageModel RemoveLimiteHoras(int id)
        {
            return _authorizationBLL.RemoveLimiteHoras(id);
        }
    }
}