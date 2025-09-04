using CVA.Core.ControlCommon.MODEL.CVACommon;
using CVA.Core.ControlCommon.SERVICE.BasesReceptoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.BLL.CVACommon
{
    public class ParceiroNegocioBLL
    {
        ParceiroNegocioDAO _parceiroNegocioBLL { get; set; }

        public ParceiroNegocioBLL(ParceiroNegocioDAO parceiroNegocioDAO)
        {
            _parceiroNegocioBLL = parceiroNegocioDAO;
        }

        /// <summary>
        /// Valida se o PN está sendo utilizado em algum documento em todas as bases
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string ValidaExclusao(string cardCode)
        {
            //CVACommonEDM edm = new CVACommonEDM();
            //List<Base> baseList = edm.CVA_BAS.Where(b => b.ID != 1).ToList();

            //foreach (var baseModel in baseList)
            //{
            //    object value = _parceiroNegocioBLL.ValidaParceiroNegocio(baseModel, cardCode);
            //    if (value != null)
            //    {
            //        return String.Format("Impossível remover, parceiro de negócio sendo utilizado. Base: {0}. Documento: {1}", baseModel.COMP, value);
            //    }
            //}

            return String.Empty;
        }
    }
}
