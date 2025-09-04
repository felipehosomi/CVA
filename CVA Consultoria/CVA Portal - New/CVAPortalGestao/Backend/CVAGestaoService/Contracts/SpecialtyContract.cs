using BLL.Classes;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class SpecialtyContract
    {
        private SpecialtyBLL _specialtyBLL { get; set; }
        private CollaboratorBLL _collaboratorBLL { get; set; }

        public SpecialtyContract()
        {
            this._specialtyBLL = new SpecialtyBLL();
            this._collaboratorBLL = new CollaboratorBLL();
        }

        public MessageModel Save(SpecialtyModel specialty)
        {
            try
            {
                return _specialtyBLL.Save(specialty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> Get()
        {
            return _specialtyBLL.Get();
        }

        public List<SpecialtyModel> Get_Specialty(int projectId, int StepId, int user)
        {
            return _specialtyBLL.Get_Specialty(projectId, StepId, user);
        }


        public List<StatusModel> GetSpecificStatus()
        {
            return _specialtyBLL.GetSpecificStatus();
        }

        public List<SpecialtyModel> GetByCollaborator(CollaboratorModel collaborator)
        {
            return _collaboratorBLL.GetSpecialtyByCollaborator(collaborator);
        }

        public SpecialtyModel GetId(int specialtyID)
        {
            return _specialtyBLL.Get_ByID(specialtyID);
        }

        public List<SpecialtyModel> Get_All()
        {
            return _specialtyBLL.Get_All();
        }

        public List<SpecialtyTypeModel> Get_TiposEspecialidade()
        {
            return _specialtyBLL.Get_TiposEspecialidade();
        }
    }
}