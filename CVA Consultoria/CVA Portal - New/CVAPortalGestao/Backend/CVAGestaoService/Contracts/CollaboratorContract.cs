using BLL.Classes;
using MODEL.Classes;

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class CollaboratorContract
    {
        #region Atributos
        private CollaboratorBLL _collaboratorBLL { get; set; }
        #endregion

        #region Construtor
        public CollaboratorContract()
        {
            this._collaboratorBLL = new CollaboratorBLL();
        }
        #endregion

        public CollaboratorModel Get(int id)
        {
            return _collaboratorBLL.Get(id);
        }

        public MessageModel Insert(CollaboratorModel model)
        {
            return _collaboratorBLL.Insert(model);
        }

        public MessageModel Update(CollaboratorModel model)
        {
            return _collaboratorBLL.Update(model);
        }

        public MessageModel Remove_Specialty(SpecialtyModel model, int idUser)
        {
            return _collaboratorBLL.Remove_Specialty(model, idUser);
        }


        public CollaboratorModel ImportarDadosColaborador()
        {
            return _collaboratorBLL.ImportarDadosColaborador();
        }
        public List<StatusModel> GetSpecificStatus()
        {
            return _collaboratorBLL.GetSpecificStatus();
        }
        
        public List<CollaboratorModel> Get()
        {
            return _collaboratorBLL.Get();
        }

        public List<CollaboratorModel> LoadCombo_Collaborator()
        {
            return _collaboratorBLL.LoadCombo_Collaborator();
        }

        public List<CollaboratorModel> Collaborator_Get_NotUser()
        {
            return _collaboratorBLL.Get_NotUser();
        }

        public List<SpecialtyModel> Get_Specialties(int id)
        {
            return _collaboratorBLL.Get_Specialties(id);
        }

        public List<CollaboratorModel> Get_CollaboratorBySpecialty(int id)
        {
            return _collaboratorBLL.Get_CollaboratorBySpecialty(id);
        }
        
        public List<CollaboratorModel> GetPMs()
        {
            return _collaboratorBLL.GetPMs();
        }

        public List<CollaboratorModel> Get_Active()
        {
            return _collaboratorBLL.Get_Active();
        }




        public List<CollaboratorTypeModel> GetActiveTypes()
        {
            return _collaboratorBLL.GetActiveTypes();
        }

        public List<CollaboratorModel> GetFromSpecialty(SpecialtyModel specialty)
        {
            return _collaboratorBLL.CollaboratorsBySpecialty(specialty);
        }

        public CollaboratorModel GetByKey(int collaboratorId)
        {
            return _collaboratorBLL.GetById(collaboratorId);
        }

        public List<CollaboratorModel> Get_NotUser()
        {
            return _collaboratorBLL.Get_NotUser();
        }

        public List<CollaboratorModel> GetCollaboratorByFilters(string name, string cpf, string cnpj, int sector, int specialty, int status)
        {
            return _collaboratorBLL.GetCollaboratorByFilters(name, cpf, cnpj, sector, specialty, status);
        }

        public List<SpecialtyModel> GetSpecialtiesForCollaborator(int id)
        {
            return _collaboratorBLL.GetSpecialtiesForCollaborator(id);
        }        
    }
}