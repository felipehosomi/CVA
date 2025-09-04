using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.View
{
    public class RegraComissaoForm
    {
        public const string Type = "CVAREGR";
        public const string ObjType = "UDOREGR";

        public const string EditCode = "Item_18";
        public const string EditName = "Item_19";
        public const string ComboTipoComissao = "Item_20";
        public const string EditComissionado = "Item_26";
        public const string EditComissionadoDesc = "Item_27";
        public const string ComboMomentoComissao = "Item_21";
        public const string EditVendedor = "Item_28";
        public const string EditVendedorDesc = "Item_29";
        public const string EditCodigoItem = "Item_30";
        public const string EditCodigoItemDesc = "Item_31";
        public const string EditGrupoItem = "Item_32";
        public const string EditGrupoItemDesc = "Item_33";
        public const string EditCentroCusto = "Item_34";
        public const string EditCentroCustoDesc = "Item_35";
        public const string ComboFabricante = "Item_25";
        public const string EditCliente = "Item_36";
        public const string EditClienteDesc = "Item_37";
        public const string EditCidade = "Item_38";
        public const string EditCidadeDesc = "Item_39";
        public const string ComboEstado = "Item_22";
        public const string ComboSetor = "Item_24";
        public const string EditPercentualComissao = "Item_23";
        public const string EditPrioridade = "Item_1";
        public const string EditComissaoReal = "Item_40";
        public const string EditGroupCode = "Item_42";
        public const string EditGroupName = "Item_43";
        public const string CheckAtivo = "Item_44";

        public const string ButtonOk = "1";
        public const string ButtonCancelar = "2";

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

        public static string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_regras_comissao.srf";
        
    }
}
