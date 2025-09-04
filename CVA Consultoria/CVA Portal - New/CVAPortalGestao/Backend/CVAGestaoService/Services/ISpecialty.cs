using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface ISpecialty
    {
        [OperationContract]
        MessageModel SaveSpecialty(SpecialtyModel specialty);

        [OperationContract]
        List<StatusModel> SpecialtyStatus();

        [OperationContract]
        List<SpecialtyModel> GetSpecialtys();

        [OperationContract]
        List<SpecialtyModel> GetSpecialtyByColaborator(CollaboratorModel collaborator);

        [OperationContract]
        SpecialtyModel GetSpecialty_ByID(int specialtyID);

        [OperationContract]
        List<SpecialtyModel> Specialty_Get_All();

        [OperationContract]
        List<SpecialtyTypeModel> Specialty_Get_TiposEspecialidade();

        [OperationContract]
        List<SpecialtyModel> Get_Specialty(int projectId, int StepId, int user);
       
    }
}
