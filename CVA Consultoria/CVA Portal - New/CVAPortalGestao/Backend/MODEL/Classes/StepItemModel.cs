using MODEL.Interface;

namespace MODEL.Classes
{
    public class StepItemModel : IModel
    {
        public StepModel Fase { get; set; }
        public SpecialtyModel Especialidade { get; set; }
        public CollaboratorModel Colaborador { get; set; }
        public string CustoOrcado { get; set; }
        public string HorasOrcadas { get; set; }
    }
}
