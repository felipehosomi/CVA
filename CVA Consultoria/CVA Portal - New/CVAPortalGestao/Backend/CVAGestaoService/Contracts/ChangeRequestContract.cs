using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ChangeRequestContract
    {
        #region Atributos
        private ChangeRequestBLL _changeRequestBLL { get; set; }
        #endregion

        #region Construtor
        public ChangeRequestContract()
        {
            this._changeRequestBLL = new ChangeRequestBLL();
        }
        #endregion


        public ChangeRequestModel ChangeRequest_Get(int id)
        {
            return _changeRequestBLL.Get(id);
        }

        public List<ChangeRequestModel> ChangeRequest_Get_for_Project(int id)
        {
            return _changeRequestBLL.Get_for_Project(id);
        }

        public MessageModel ChangeRequest_Save(ChangeRequestModel model)
        {
            return _changeRequestBLL.Save(model);
        }
    }
}