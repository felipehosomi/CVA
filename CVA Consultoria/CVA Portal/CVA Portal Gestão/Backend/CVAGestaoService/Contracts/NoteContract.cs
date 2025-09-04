using BLL.Classes;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class NoteContract
    {
        #region Atributos
        private NoteBLL _noteBLL { get; set; }
        #endregion

        #region Construtor
        public NoteContract()
        {
            this._noteBLL = new NoteBLL();
        }
        #endregion

       
       
        public MessageModel Save(NoteModel note)
        {
            return _noteBLL.Save(note);
        }

    


        public MessageModel Note_Remove(int noteID)
        {
            try
            {
                return _noteBLL.Remove(noteID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<NoteModel> Get_UserNotes(int id)
        {
            return _noteBLL.Get_UserNotes(id);
        }
      

        public List<NoteModel> Note_FiltrarInterno(NoteFilterModel model)
        {
            return _noteBLL.FiltrarInterno(model);
        }


        public List<NoteModel> Search(NoteFilterModel model)
        {
            return _noteBLL.Search(model);
        }


        public List<AuthorizedHoursModel> GetAuthorizedHours()
        {
            return null;// _noteBLL.GetAuthorizedHours();
        }
        public List<AuthorizedHoursModel> GetAuthorizedHoursByCollaborator(int collaboratorId)
        {
            return null;//_noteBLL.GetAuthorizedHoursByCollaborator(collaboratorId);
        }
    }
}