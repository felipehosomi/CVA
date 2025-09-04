using BLL.Classes;
using MODEL.Classes;
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

        public MessageModel Remove(NoteModel model)
        {
            return _noteBLL.Remove(model);
        }
        public List<NoteModel> Get_UserNotes(int id, int mes, int ano)
        {
            return _noteBLL.Get_UserNotes(id, mes, ano);
        }

        public List<NoteModel> Search(NoteFilterModel model)
        {
            return _noteBLL.Search(model);
        }
    }
}