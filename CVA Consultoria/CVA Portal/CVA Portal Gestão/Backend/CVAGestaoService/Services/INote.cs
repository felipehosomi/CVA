using MODEL.Classes;

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface INote
    {
        [OperationContract]
        MessageModel Save(NoteModel note);

        [OperationContract]
        MessageModel Note_Remove(int id);

        [OperationContract]
        List<NoteModel> Get_UserNotes(int id);


        [OperationContract]
        List<NoteModel> Note_Search(NoteFilterModel model);

        [OperationContract]
        List<AuthorizedHoursModel> GetAuthorizedHours();

        [OperationContract]
        List<AuthorizedHoursModel> GetAuthorizedHoursByCollaborator(int collaboratorId);
    }
}

