using MODEL.Classes;

using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface ICollaborator
    {
        [OperationContract]
        CollaboratorModel Collaborator_Get(int id);

        [OperationContract]
        MessageModel Collaborator_Insert(CollaboratorModel model);

        [OperationContract]
        MessageModel Collaborator_Update(CollaboratorModel model);

        [OperationContract]
        MessageModel Collaborator_Remove_Specialty(SpecialtyModel model, int idUser);

        [OperationContract]
        CollaboratorModel ImportarDadosColaborador();

        [OperationContract]
        List<StatusModel> CollaboratorStatus();

        [OperationContract]
        List<CollaboratorModel> GetCollaborator();

        [OperationContract]
        List<CollaboratorModel> LoadCombo_Collaborator();

        [OperationContract]
        List<CollaboratorModel> Collaborator_Get_NotUser();

        [OperationContract]
        List<SpecialtyModel> Collaborator_Get_Specialties(int id);



        [OperationContract]
        List<CollaboratorModel> GetPMs();

        [OperationContract]
        List<CollaboratorModel> Collaborator_Get_Active();



        [OperationContract]
        List<CollaboratorTypeModel> GetCollaboratorTypes();

        [OperationContract]
        List<CollaboratorModel> CollaboratorFromSpecialty(SpecialtyModel specialty);

        [OperationContract]
        List<CollaboratorModel> Get_CollaboratorBySpecialty(int id);
        
        [OperationContract]
        CollaboratorModel GetCollaboratorById(int collaboratorId);

        [OperationContract]
        List<CollaboratorModel> GetCollaboratorNotUser();

        [OperationContract]
        List<SpecialtyModel> GetSpecialtiesForCollaborator(int id);

        [OperationContract]
        List<CollaboratorModel> GetCollaboratorByFilters(string name, string cpf, string cnpj, int sector, int specialty, int status);
    }
}