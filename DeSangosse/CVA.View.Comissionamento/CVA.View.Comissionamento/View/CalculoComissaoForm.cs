using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.View
{
    public class CalculoComissaoForm
    {
        public const string Type = "CVACALC";
        public const string ObjType = "";

        public const string EditDataInicial = "dtInicial";
        public const string EditDataFinal = "dtFinal";
        public const string OptionTodas = "opTodas";
        public const string OptionPagas = "opPagas";
        public const string OptionNaoPagas = "opAberto";

        public const string MatrixItens = "mtxItens";

        public const string ColumnEditLinha = "#";
        public const string ColumnEditComissionado = "Col_0";
        public const string ColumnEditCodigoCliente = "Col_1";
        public const string ColumnEditRazaoSocial = "Col_2";
        public const string ColumnComboRegraComissao = "Col_3";
        public const string ColumnEditDataDocumento = "Col_4";
        public const string ColumnEditDataVencimento = "Col_5";
        public const string ColumnEditChaveNota = "Col_6";
        public const string ColumnEditCodigoItem = "Col_7";
        public const string ColumnEditDescricaoItem = "Col_8";
        public const string ColumnEditCentroCusto = "Col_9";
        public const string ColumnEditValor = "Col_10";
        public const string ColumnEditParcela = "Col_11";
        public const string ColumnEditImpostos = "Col_12";
        public const string ColumnEditPercentualComissao = "Col_13";
        public const string ColumnEditValorComissao = "Col_14";
        public const string ColumnCheckComissaoPaga = "Col_15";
        public const string ColumnEditDataPagamento = "Col_16";
        public const string ColumnEditPrioridade = "Col_17";
        public const string ColumnEditTipoObjeto = "Col_18";
        public const string ColumnEditLinhaItem = "Col_19";
        public const string ColumnEditMomento = "Col_20";


        public const string ButtonOk = "1";
        public const string ButtonCancelar = "2";
        public const string ButtonRecalcular = "btRecalc";
        public const string ButtonConfirmar = "btConfirma";

        public const string UserDatasourceUD_0 = "UD_0";
        public const string UserDatasourceUD_1 = "UD_1";
        public const string UserDatasourceUD_2 = "UD_2";
        public const string UserDatasourceUD_3 = "UD_3";
        public const string UserDatasourceUD_4 = "UD_4";
        public const string UserDatasourceUD_Col1 = "UD_Col1";
        public const string UserDatasourceUD_Col2 = "UD_Col2";
        public const string UserDatasourceUD_Col3 = "UD_Col3";
        public const string UserDatasourceUD_Col4 = "UD_Col4";
        public const string UserDatasourceUD_Col5 = "UD_Col5";
        public const string UserDatasourceUD_Col6 = "UD_Col6";
        public const string UserDatasourceUD_Col7 = "UD_Col7";
        public const string UserDatasourceUD_Col8 = "UD_Col8";
        public const string UserDatasourceUD_Col9 = "UD_Col9";
        public const string UserDatasourceUD_Col10 = "UD_Col10";
        public const string UserDatasourceUD_Col11 = "UD_Col11";
        public const string UserDatasourceUD_Col12 = "UD_Col12";
        public const string UserDatasourceUD_Col13 = "UD_Col13";
        public const string UserDatasourceUD_Col14 = "UD_Col14";
        public const string UserDatasourceUD_Col15 = "UD_Col15";
        public const string UserDatasourceUD_Col16 = "UD_Col16";
        public const string UserDatasourceUD_Col17 = "UD_Col17";
        public const string UserDatasourceUD_Col18 = "UD_Col18";
        public const string UserDatasourceUD_Col19 = "UD_Col19";
        public const string UserDatasourceUD_Col20 = "UD_Col20";
        public const string UserDatasourceUD_Col21 = "UD_Col21";
        public const string UserDatasourceUD_Col22 = "UD_Col22";
        public const string UserDatasourceUD_Col23 = "UD_Col23";
        public const string UserDatasourceUD_Col24 = "UD_Col24";
        
        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_calculo_comissao.srf";
    }
}
