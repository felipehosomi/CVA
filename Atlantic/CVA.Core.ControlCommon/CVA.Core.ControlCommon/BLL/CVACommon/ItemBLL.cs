using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL.CVACommon;
using CVA.Core.ControlCommon.SERVICE.BasesReceptoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.BLL.CVACommon
{
    public class ItemBLL
    {
        ItemDAO _itemDAO { get; set; }

        public ItemBLL(ItemDAO itemDAO)
        {
            _itemDAO = itemDAO;
        }

        /// <summary>
        /// Valida se o item está sendo utilizado em algum documento em todas as bases
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string ValidaExclusao(string itemCode)
        {
            //CVACommonEDM edm = new CVACommonEDM();
            //List<Base> baseList = edm.CVA_BAS.Where(b => b.ID != 1).ToList();

            //foreach (var baseModel in baseList)
            //{
            //    object value = _itemDAO.ValidaItem(baseModel, itemCode);
            //    if (value != null)
            //    {
            //        return String.Format("Impossível remover, item sendo utilizado. Base: {0}. Documento: {1}", baseModel.COMP, value);
            //    }
            //}
            
            return String.Empty;
        }
    }
}
