using System;

namespace MODEL.Classes
{
    public class NoteFilterModel
    {
        public int UserId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int ProjectId { get; set; }
        public int StatusId { get; set; }
        public int ClientId { get; set; }
        public int Chamado { get; set; }
        public bool interno { get; set; }
    }
}