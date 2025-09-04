using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface INote
    {
        [OperationContract]
        MessageModel Note_Save(NoteModel note);

        [OperationContract]
        MessageModel Note_Remove(NoteModel model);

        [OperationContract]
        List<NoteModel> Get_UserNotes(int id, int mes, int ano);

        [OperationContract]
        List<NoteModel> Note_Search(NoteFilterModel model);       
    }
}