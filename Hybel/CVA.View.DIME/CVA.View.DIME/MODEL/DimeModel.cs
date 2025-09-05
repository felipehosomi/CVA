using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.DIME.MODEL
{
    public class DimeModel
    {
        public DimeContadorModel ContadorModel { get; set; }
        public DimeEmpresaModel EmpresaModel { get; set; }
        public List<DimeDocumentModel> Quadro01 { get; set; }
        public List<DimeDocumentModel> Quadro02 { get; set; }
        public List<DimeSTModel> DimeSTList { get; set; }
        public List<DimeSomatorioModel> Quadro03 { get; set; }
        public List<DimeSomatorioModel> Quadro04 { get; set; }
        public List<DimeSomatorioModel> Quadro05 { get; set; }
        public List<DimeSomatorioModel> Quadro09 { get; set; }
        public List<DimeSomatorioModel> Quadro11 { get; set; }
        public List<DimeQuadro12Model> Quadro12 { get; set; }
        public List<DimeQuadro46Model> Quadro46 { get; set; }
        public List<DimeQuadro49Model> Quadro49 { get; set; }
        public List<DimeQuadro50Model> Quadro50 { get; set; }
        public List<DimeQuadro80Model> Quadro80 { get; set; } = new List<DimeQuadro80Model>();
        public List<DimeQuadro81Model> Quadro81 { get; set; } = new List<DimeQuadro81Model>();
        public List<DimeQuadro82Model> Quadro82 { get; set; } = new List<DimeQuadro82Model>();
        public List<DimeQuadro83Model> Quadro83 { get; set; } = new List<DimeQuadro83Model>();
        public List<DimeQuadro84Model> Quadro84 { get; set; } = new List<DimeQuadro84Model>();

        public DimeQuadro98Model Dime98 { get; set; }
        public DimeQuadro99Model Dime99 { get; set; }
    }
}
