using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.Cardapio.Model
{
    public class PlanejamentoData
    {
        public string Code { get; set; }
        public string U_CVA_ID_CLIENTE { get; set; }
        public string U_CVA_ID_CONTRATO { get; set; }
        public string U_CVA_ID_MODEL_CARD { get; set; }
        public string U_CVA_GRPSERVICO { get; set; }
        public DateTime U_CVA_VIGENCIA_CONTR { get; set; }
        public double U_CVA_CUSTO_PADRAO { get; set; }
        public DateTime U_CVA_DATA_REF { get; set; }
        public string U_CVA_DIA_SEMANA { get; set; }
        public int U_CVA_QTD_COMENSAIS { get; set; }
        public string U_CVA_DES_CLIENTE { get; set; }
        public string U_CVA_DES_MODELO_CARD { get; set; }
        public string U_CVA_DES_GRPSERVICO { get; set; }
        public double U_CVA_TOT_C_MEDIO { get; set; }
        public double U_CVA_TOT_C_PADRAO { get; set; }
        public double U_CVA_TOT_VARIACAO_V { get; set; }
        public double U_CVA_TOT_VARIACAO_P { get; set; }
        //public double U_CVA_TOT_C { get; set; }
        public List<PlanejamentoDataLinha> PlanejamentoLinhas { get; set; } = new List<PlanejamentoDataLinha>();
        public List<ModeloLinha> ModeloLinhas { get; set; } = new List<ModeloLinha>();

        public PlanejamentoData() { }

        public PlanejamentoData(SAPbobsCOM.Recordset rec)
        {
            PlanejamentoLinhas = new List<PlanejamentoDataLinha>();
            if (rec.RecordCount > 0)
            {
                Code = (dynamic)rec.Fields.Item("Code")?.Value;
                U_CVA_ID_CLIENTE = (dynamic)rec.Fields.Item("U_CVA_ID_CLIENTE")?.Value;
                U_CVA_ID_CONTRATO = (dynamic)rec.Fields.Item("U_CVA_ID_CONTRATO")?.Value;
                U_CVA_ID_MODEL_CARD = (dynamic)rec.Fields.Item("U_CVA_ID_MODEL_CARD")?.Value;
                U_CVA_GRPSERVICO = (dynamic)rec.Fields.Item("U_CVA_GRPSERVICO")?.Value;
                U_CVA_VIGENCIA_CONTR = (dynamic)rec.Fields.Item("U_CVA_VIGENCIA_CONTR")?.Value;
                U_CVA_CUSTO_PADRAO = (dynamic)rec.Fields.Item("U_CVA_CUSTO_PADRAO")?.Value;
                U_CVA_DATA_REF = (dynamic)rec.Fields.Item("U_CVA_DATA_REF")?.Value;
                U_CVA_DIA_SEMANA = (dynamic)rec.Fields.Item("U_CVA_DIA_SEMANA")?.Value;
                U_CVA_QTD_COMENSAIS = (dynamic)rec.Fields.Item("U_CVA_QTD_COMENSAIS")?.Value;
                U_CVA_DES_CLIENTE = (dynamic)rec.Fields.Item("U_CVA_DES_CLIENTE")?.Value;
                U_CVA_DES_MODELO_CARD = (dynamic)rec.Fields.Item("U_CVA_DES_MODELO_CARD")?.Value;
                U_CVA_DES_GRPSERVICO = (dynamic)rec.Fields.Item("U_CVA_DES_GRPSERVICO")?.Value;
                U_CVA_TOT_C_MEDIO = (dynamic)rec.Fields.Item("U_CVA_TOT_C_MEDIO")?.Value;
                U_CVA_TOT_C_PADRAO = (dynamic)rec.Fields.Item("U_CVA_TOT_C_PADRAO")?.Value;
                U_CVA_TOT_VARIACAO_V = (dynamic)rec.Fields.Item("U_CVA_TOT_VARIACAO_V")?.Value;
                U_CVA_TOT_VARIACAO_P = (dynamic)rec.Fields.Item("U_CVA_TOT_VARIACAO_P")?.Value;
                //U_CVA_TOT_C = rec.Fields.Item("U_CVA_TOT_C")?.Value;

                while (!rec.EoF && Code == rec.Fields.Item("Code")?.Value.ToString())
                {
                    PlanejamentoLinhas.Add(new PlanejamentoDataLinha(rec));
                    rec.MoveNext();
                }
            }
        }

        public PlanejamentoData Copy()
        {
            var copy = new PlanejamentoData();
            copy.Code = Code;
            copy.U_CVA_ID_CLIENTE = U_CVA_ID_CLIENTE;
            copy.U_CVA_ID_CONTRATO = U_CVA_ID_CONTRATO;
            copy.U_CVA_ID_MODEL_CARD = U_CVA_ID_MODEL_CARD;
            copy.U_CVA_GRPSERVICO = U_CVA_GRPSERVICO;
            copy.U_CVA_VIGENCIA_CONTR = U_CVA_VIGENCIA_CONTR;
            copy.U_CVA_CUSTO_PADRAO = U_CVA_CUSTO_PADRAO;
            copy.U_CVA_DATA_REF = U_CVA_DATA_REF;
            copy.U_CVA_DIA_SEMANA = U_CVA_DIA_SEMANA;
            copy.U_CVA_QTD_COMENSAIS = U_CVA_QTD_COMENSAIS;
            copy.U_CVA_DES_CLIENTE = U_CVA_DES_CLIENTE;
            copy.U_CVA_DES_MODELO_CARD = U_CVA_DES_MODELO_CARD;
            copy.U_CVA_DES_GRPSERVICO = U_CVA_DES_GRPSERVICO;
            copy.U_CVA_TOT_C_MEDIO = U_CVA_TOT_C_MEDIO;
            copy.U_CVA_TOT_C_PADRAO = U_CVA_TOT_C_PADRAO;
            copy.U_CVA_TOT_VARIACAO_V = U_CVA_TOT_VARIACAO_V;
            copy.U_CVA_TOT_VARIACAO_P = U_CVA_TOT_VARIACAO_P;
            //copy.U_CVA_TOT_C = U_CVA_TOT_C;
            copy.PlanejamentoLinhas = PlanejamentoLinhas.ToList();
            copy.ModeloLinhas = ModeloLinhas.ToList();
            return copy;
        }
    }

    public class PlanejamentoDataLinha
    {
        public string Code { get; set; }
        public int LineId { get; set; }
        public string U_CVA_TIPO_PRATO { get; set; }
        public string U_CVA_TIPO_PRATO_DES { get; set; }
        public string U_CVA_INSUMO { get; set; }
        public double U_CVA_PERCENT { get; set; }
        public int U_CVA_QTD { get; set; }
        public double U_CVA_CUSTO_MEDIO { get; set; }
        public double U_CVA_TOTAL { get; set; }
        public string U_CVA_INSUMO_DES { get; set; }
        public int U_CVA_QTD_ORI { get; set; }
        public string U_CVA_MODELO_LIN_ID { get; set; }
        public PlanejamentoDataLinha() { }

        public PlanejamentoDataLinha(SAPbobsCOM.Recordset rec)
        {
            Code = (dynamic)rec.Fields.Item("LINE_CODE")?.Value;
            LineId = (dynamic)rec.Fields.Item("LineId")?.Value;
            U_CVA_TIPO_PRATO = (dynamic)rec.Fields.Item("U_CVA_TIPO_PRATO")?.Value;
            U_CVA_TIPO_PRATO_DES = (dynamic)rec.Fields.Item("U_CVA_TIPO_PRATO_DES")?.Value;
            U_CVA_INSUMO = (dynamic)rec.Fields.Item("U_CVA_INSUMO")?.Value;
            U_CVA_PERCENT = (dynamic)rec.Fields.Item("U_CVA_PERCENT")?.Value;
            U_CVA_QTD = (dynamic)rec.Fields.Item("U_CVA_QTD")?.Value;
            U_CVA_CUSTO_MEDIO = (dynamic)rec.Fields.Item("U_CVA_CUSTO_MEDIO")?.Value;
            U_CVA_TOTAL = (dynamic)rec.Fields.Item("U_CVA_TOTAL")?.Value;
            U_CVA_INSUMO_DES = (dynamic)rec.Fields.Item("U_CVA_INSUMO_DES")?.Value;
            U_CVA_QTD_ORI = (dynamic)rec.Fields.Item("U_CVA_QTD_ORI")?.Value;
            U_CVA_MODELO_LIN_ID = (dynamic)rec.Fields.Item("U_CVA_MODELO_LIN_ID")?.Value;
        }
    }

    public class ModeloLinha
    {
        public string Code { get; set; }
        public int LineId { get; set; }
        public string U_CVA_DES_MODELO_CARD { get; set; }
        public string U_CVA_TIPO_PRATO { get; set; }
        public string U_CVA_TIPO_PRATO_DES { get; set; }
        public ModeloLinha() { }

        public ModeloLinha(SAPbobsCOM.Recordset rec)
        {
            Code = (dynamic)rec.Fields.Item("Code")?.Value;
            U_CVA_DES_MODELO_CARD = (dynamic)rec.Fields.Item("BpName")?.Value;
            LineId = (dynamic)rec.Fields.Item("LineId")?.Value;
            U_CVA_TIPO_PRATO = (dynamic)rec.Fields.Item("U_CVA_TIPO_PRATO")?.Value;
            U_CVA_TIPO_PRATO_DES = (dynamic)rec.Fields.Item("U_CVA_TIPO_PRATO_DES")?.Value;
        }
    }
}
