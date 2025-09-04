using CVA.Core.ControlCommon.SERVICE.OACT;

namespace CVA.Core.ControlCommon.BLL
{
    public class PlanoContasBLL
    {
        private PlanoContasDAO _planoContasDAO { get; set; }

        public PlanoContasBLL(PlanoContasDAO planoContasDAO)
        {
            this._planoContasDAO = planoContasDAO;
        }

        public bool Exists(string code)
        {
            return this._planoContasDAO.Exists(code) > 0;
        }
    }
}
