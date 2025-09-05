using System;
using System.Collections.Generic;

namespace CVA.View.ControleQualidade.MODEL
{
    public class PlanoInspecao
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Aprovado { get; set; }
        public string Description { get; set; }
        public List<PlanoInspecaoLinha> Details { get; set; }
    }

    public class PlanoInspecaoLinha
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Analise { get; set; }
        public string EspecificacaoCode { get; set; }
        public string Equipamento { get; set; }

        public Atributo Especificacao { get; set; }
        public string ValorNominal { get; set; }
        public string ValorDe { get; set; }
        public string ValorAte { get; set; }
        public string Observacao { get; set; }
        public bool Added { get; set; }
    }

    public class DuplicarPlanoModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_Description { get; set; }
        public string U_Aprovado { get; set; }
        public string U_Aprovador { get; set; }
        public DateTime U_DtAprov { get; set; }
        public List<DuplicarPlanoModelLnha> Linha { get; set; }


    }

    public class DuplicarPlanoModelLnha
    {
        public string U_Analise { get; set; }
        public string U_Especificacao { get; set; }
        public string U_ValorNominal { get; set; }
        public string U_ValorDe { get; set; }
        public string U_ValorAte { get; set; }
        public string U_Inspecao { get; set; }
        public string U_Equipamento { get; set; }
        public string U_Observacao { get; set; }
        
    }

}
