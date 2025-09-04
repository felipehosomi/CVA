using System.Collections.Generic;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class FichaInspecaoAmostraModel
    {
        public int IdAmostra { get; set; }
        public List<FichaInspecaoItemModel> ItemList { get; set; }
    }
}
