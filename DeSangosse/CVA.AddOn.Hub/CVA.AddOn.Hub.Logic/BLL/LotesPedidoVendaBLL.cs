using CVA.AddOn.Common;
using CVA.AddOn.Hub.Logic.DAO.Lote;
using CVA.AddOn.Hub.Logic.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class LotesPedidoVendaBLL
    {
        public int Aux = 0;

        public void InsertLote(List<LotesModel> ListaItemLote)
        {
            var _lote_PV = new Lote_PV();
            int cod = 0;
            for (int i = 0; i < ListaItemLote.Count; i++)
            {
                cod++;
                _lote_PV.InsertLote(cod, ListaItemLote[i]);
            }
        }
        
        public void ApagaDadosTabelaLote()
        {
            var _lote_PV = new Lote_PV();
            _lote_PV.ApagaDadosTabelaLote();
        }

        public List<LotesModel> PegaLote()
        {
            var _lote_PV = new Lote_PV();
            var result = _lote_PV.pegaLote();

            var modelLista = new List<LotesModel>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new LotesModel()
                {
                    Item = result.Rows[i]["U_ItemCode"].ToString(),
                    Lote = result.Rows[i]["U_Lote"].ToString()
                };

                modelLista.Add(model);


            }
            return modelLista;
        }

       

        public bool ValidaLotes(List<LotesModel> List)
        {
            var result = true;

            if (Aux == 0)
            {
                var lotebll = new LotesPedidoVendaBLL();
                var lista = lotebll.PegaLote();

                int x = 0;
                var erro = "";
                foreach (var l in List)
                {
                    foreach (var item in lista)
                    {
                        x = 0;
                        if (l.Item == item.Item)
                        {
                            x = 1;
                            break;
                        }

                    }

                    if (x == 0)
                    {
                        erro = erro + ($"Item {l.Item}: Informar número(s) de lote \n");
                    }
                }

                if (!string.IsNullOrEmpty(erro))
                {
                    SBOApp.Application.MessageBox(erro);

                    lotebll.ApagaDadosTabelaLote();
                    result = false;
                }
                else
                {
                    result = true;
                }
                Aux = 1;
            }
            return result;
        }
    }
}
