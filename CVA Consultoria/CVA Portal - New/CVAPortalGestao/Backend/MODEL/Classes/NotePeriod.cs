using System;

namespace MODEL.Classes
{
    public class NotePeriod
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Description
        {
            get
            {
                return $"{GetMonthName()}/{Year}";
            }
        }

        private string GetMonthName()
        {
            switch (Month)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
                default:
                    throw new Exception($"Mês {Month} inválido");
            }
        }
    }
}
