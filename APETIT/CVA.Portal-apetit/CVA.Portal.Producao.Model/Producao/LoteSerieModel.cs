namespace CVA.Portal.Producao.Model.Producao
{
    public class LoteSerieModel
    {
        public string Item { get; set; }

        public string Lote { get; set; }

        public string Serie { get; set; }

        public int SystemSerie { get; set; }

        public double QtdeDisponivel { get; set; }

        public double Quantidade { get; set; }

        public bool Selecionado { get; set; }
    }
}
