using MODEL.Interface;
using System;

namespace MODEL.Classes
{
    public class ExpenseModel : IModel
    {
        public ProjectModel Projeto { get; set; }
        public DateTime Data { get; set; }
        public ExpenseTypeModel TipoDespesa { get; set; }
        public string Descricao { get; set; }
        public string NumNota { get; set; }
        public string ValorNota { get; set; }
        public string ValorDespesa { get; set; }
        public string Quilometragem { get; set; }
        public string ValorReembolso { get; set; }        
        public string Anexo { get; set; }
        public int USR { get; set; }
    }
}
