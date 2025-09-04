using System;
using System.Collections.Generic;
using System.Linq;
using MODEL.Interface;

namespace MODEL.Classes
{
    public class ProjectModel : IModel
    {
        /* -- CVA_PRJ -- */
        public string Codigo { get; set; }
        public string Tag { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataPrevista { get; set; }
        public ClientModel Cliente { get; set; }
        public string ResponsavelDespesa { get; set; }
        public ProjectTypeModel TipoProjeto { get; set; }
        public CollaboratorModel Gerente { get; set; }
        public string ControleHoras { get; set; }

        /* -- CVA_PROJETOS_FASES -- */
        public List<StepModel> Fases { get; set; }

        /* -- CVA_PROJETOS_RECURSOS -- */
        public List<ResourceModel> Recursos { get; set; }

        /* -- CVA_PROJETOS_MEMBROS -- */
        public List<MemberModel> Membros { get; set; }

        /* -- CVA_PROJETOS_PRICING -- */
        public List<SoldHoursModel> HorasVendidas { get; set; }

        /* -- CVA_PROJETOS_ALTERACOES -- */
        public List<AlterationModel> Alteracoes { get; set; }

        public int HorasOrcadas { get { return HorasVendidas != null ? HorasVendidas.Sum(x => x.Horas) : 0; } }
        public string HorasOrcadasFormat
        {
            get
            {
                var ts = TimeSpan.FromHours(HorasOrcadas);
                ts = TimeSpan.FromMinutes(1 * Math.Ceiling(ts.TotalMinutes / 1));
                return $"{((int)ts.TotalHours).ToString().PadLeft(2, '0')}:{ts:mm}";
            }
        }
        public double HorasConsumidas { get { return HorasVendidas != null ? HorasVendidas.Sum(x => x.HorasConsumidas) : 0; } }
        public string HorasConsumidasFormat
        {
            get
            {
                var ts = TimeSpan.FromHours(HorasConsumidas);
                ts = TimeSpan.FromMinutes(1 * Math.Ceiling(ts.TotalMinutes / 1));
                return $"{((int)ts.TotalHours).ToString().PadLeft(2, '0')}:{ts:mm}";
            }
        }
    }
}