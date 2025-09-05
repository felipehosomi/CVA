using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.MODEL
{

    public class CampoMaquinaModel
    {
        public DateTime dataInicial { get; set; }
        public String dataFinal   { get; set; }



        public String horaInicial   { get; set; }
        public String horaFinal     { get; set; }

        public String lote          { get; set; }
        public String maquina       { get; set; }
        public String produto       { get; set; }
        public String motivo        { get; set; }
        public String operador      { get; set; }
        public String duracao       { get; set; }
        public String status       { get; set; }

        public int Code             { get; set; }
        public int Name             { get; set; }
    }

    public class MaquinaModel
    {
        public String code { get; set; }
        public String name { get; set; }
    }

    public class MotivoModel
    {
        public String code      { get; set; }
        public String name { get; set; }
    }

    public class OperadorModel
    {
        public String ID { get; set; }
        public String Nome { get; set; }
    }

    public class ProdutoModel
    {
        public String code { get; set; }
        public String descricao { get; set; }
    }

    public class MaxValue
    {
        public int Code { get; set; }
        public int Name { get; set; }
    }

}
