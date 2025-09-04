using CVA.Core.Cianet.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Cianet.BLL
{
    public class RegraFreteBLL
    {
        #region Atributos
        public RegraFreteDAO _dao;
        #endregion

        #region Construtor
        public RegraFreteBLL()
        {
            this._dao = new RegraFreteDAO();
        }
        #endregion

        public double CalculaFrete(string cliente, string produto, string quantidade, double totalProduto, out string transpCode, out double percent)
        {
            var porcentagem = _dao.CalculaFrete(cliente, produto, quantidade, out transpCode);

            percent = porcentagem;

            return ((porcentagem / 100) * totalProduto);
        }

        public int Get_IdDespesa()
        {
            return _dao.Get_IdDespesa();
        }

        public bool Check_DespesaFrete(string tipo, int idDespesa, int numDoc)
        {
            return _dao.Check_DespesaFrete(tipo, idDespesa, numDoc);
        }

        public void Update_ValorFrete(string tipo, double totalFrete, int idDespesa, int numDoc)
        {
            _dao.Update_ValorFrete(tipo, totalFrete, idDespesa, numDoc);
        }

        public void Insert_DespesaFrete(string tipo, double totalFrete, int idDespesa, int numDoc)
        {
            _dao.Insert_DespesaFrete(tipo, totalFrete, idDespesa, numDoc);
        }

        public string Check_TipoEnvio(string trnspCode)
        {
           return _dao.Check_TipoEnvio(trnspCode);
        }

        public int Check_UserPermission(string user)
        {
            return _dao.Check_UserPermission(user);
        }
    } 
}
