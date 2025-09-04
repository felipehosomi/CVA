using BLL.Classes;

using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class OportunittyContract
    {
        private OpportunityBLL _opportunityBLL { get; set; }

        public OportunittyContract()
        {
            this._opportunityBLL = new OpportunityBLL();
        }


        public string Generate_NewCode(int id)
        {
            return _opportunityBLL.Generate_NewCode(id);
        }

        public MessageModel Save(OpportunityModel oportunitty)
        {
            return _opportunityBLL.Save(oportunitty);
        }

        public MessageModel ConvertToProject(OpportunityModel oportunitty)
        {
            return _opportunityBLL.CopyToProject(oportunitty);
        }

        //public List<OpportunityModel> Get_All()
        //{
        //    return _opportunityBLL.Get_All();
        //}

        public OpportunityModel Get(int id)
        {
            return _opportunityBLL.Get(id);
        }

        public List<OpportunityModel> Search(string code, int clientId)
        {
            return _opportunityBLL.Search(code, clientId);
        }

    }
}