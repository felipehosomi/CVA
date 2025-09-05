using System;

namespace CVA.View.Comissionamento.Views
{
    public class RegraComissaoForm
    {
        public const string Type = "CVAREGR";
        public const string ObjType = "UDOREGR";

        public const string EditCode = "Item_18";
        public const string EditName = "Item_19";
        public const string ComboMeta = "cb_Meta";
        public const string ComboEquipe = "cb_Equipe";
        public const string ComboFilial = "cb_Filial";
        public const string ComboTipoComissao = "Item_20";
        public const string EditComissionado = "Item_26";
        public const string EditComissionadoDesc = "Item_27";
        public const string ComboMomentoComissao = "Item_21";
        public const string EditPercentualComissao = "Item_23";
        public const string CheckAtivo = "Item_44";

        public const string ButtonOk = "1";
        public const string ButtonCancelar = "2";
        public const string ButtonDuplicar = "Item_45";

        public const string DbDataSource = "@CVA_REGR_COMISSAO";
        public const string ChooseFromListOSLP1 = "OSLP1";
        public const string ChooseFromListOSLP2 = "OSLP2";
        public const string ChooseFromListOCRD = "OCRD";
        public const string ChooseFromListOITM = "OITM";
        public const string ChooseFromListOITB = "OITB";
        public const string ChooseFromListOPRC = "OPRC";
        public const string UserDataSourceSlpName1 = "SlpName1";
        public const string UserDataSourceSlpName2 = "SlpName2";
        public const string UserDataSourceCardName = "CardName";
        public const string UserDataSourceItemName = "ItemName";
        public const string UserDataSourceItmsGrpNam = "ItmsGrpNam";
        public const string UserDataSourcePrcName = "PrcName";
        public const string UserDataSourceCounty = "County";
        public const string UserDataSourceGroupName = "GroupName";

        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Forms\\cva_regras_comissao.srf";

    }
}
