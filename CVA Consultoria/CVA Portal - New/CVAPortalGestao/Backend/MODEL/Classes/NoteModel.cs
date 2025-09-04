using System;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class NoteModel : IModel
    {
        public string Description { get; set; }
        public string IndicatedHours { get; set; }
        public string ProvidedHours { get; set; }
        public string Requester { get; set; }
        public string StatusPDA { get; set; }
        public string TotalLine { get; set; }
        public int USR { get; set; }

        public DateTime Date { get; set; }
        public DateTime? InitHour { get; set; }
        public DateTime? IntervalHour { get; set; }
        public DateTime? FinishHour { get; set; }

        public TicketModel Ticket { get; set; }
        public ProjectModel Project { get; set; }
        public SpecialtyModel Specialty { get; set; }

        public string Value { get; set; }
        public StepModel Step { get; set; }
        public string TotalHours { get; set; }
    }
}