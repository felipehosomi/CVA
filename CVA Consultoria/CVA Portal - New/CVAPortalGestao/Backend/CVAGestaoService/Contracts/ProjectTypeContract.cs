using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ProjectTypeContract
    {
        #region Atributos
        private ProjectTypeBLL _projectTypeBLL { get; set; }
        #endregion

        #region Construtor
        public ProjectTypeContract()
        {
            this._projectTypeBLL = new ProjectTypeBLL();
        }
        #endregion

        public ProjectTypeModel Get(int id)
        {
            return _projectTypeBLL.Get(id);
        }

        public List<ProjectTypeModel> Get_All()
        {
            return _projectTypeBLL.Get_All();
        }

        public MessageModel Insert(ProjectTypeModel model)
        {
            return _projectTypeBLL.Insert(model);
        }

        public MessageModel Update(ProjectTypeModel model)
        {
            return _projectTypeBLL.Update(model);
        }

        public MessageModel Remove(int id)
        {
            return _projectTypeBLL.Remove(id);
        }
    }
}
