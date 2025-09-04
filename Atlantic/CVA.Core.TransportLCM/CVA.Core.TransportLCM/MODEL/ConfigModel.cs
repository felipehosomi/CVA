namespace CVA.Core.TransportLCM.MODEL
{
    public class ConfigModel
    {
        /// <summary>
        /// 1 - Replicadora / 2 - Consolidadora / 3 - Portal
        /// </summary>
        public int Tipo { get; set; }
        public string Servidor { get; set; }
        public string Banco { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }
}
