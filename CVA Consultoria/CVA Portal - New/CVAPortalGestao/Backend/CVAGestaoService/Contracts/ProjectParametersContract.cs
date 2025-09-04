using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ProjectParametersContract
    {
        #region Atributos
        public ProjectParametersBLL _projectParametersBLL { get; set; }
        #endregion

        #region Construtor
        public ProjectParametersContract()
        {
            this._projectParametersBLL = new ProjectParametersBLL();
        }
        #endregion

        public List<ProjectParametersModel> Get_All()
        {
            return _projectParametersBLL.Get_All();
        }

        public MessageModel Save(ProjectParametersModel model)
        {
            return _projectParametersBLL.Save(model);
        }
    }
}
