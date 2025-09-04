using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CVA.IntegracaoMagento.Integrator.Models.SAP
{
    public partial class ItemMasterData
    {
        [JsonProperty("odata.metadata")]
        public Uri OdataMetadata { get; set; }

        [JsonProperty("ItemCode")]
        public string ItemCode { get; set; }

        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        [JsonProperty("ForeignName")]
        public string ForeignName { get; set; }

        [JsonProperty("ItemsGroupCode")]
        public long? ItemsGroupCode { get; set; }

        [JsonProperty("CustomsGroupCode")]
        public long? CustomsGroupCode { get; set; }

        [JsonProperty("SalesVATGroup")]
        public object SalesVatGroup { get; set; }

        [JsonProperty("BarCode")]
        public string BarCode { get; set; }

        [JsonProperty("VatLiable")]
        public string VatLiable { get; set; }

        [JsonProperty("PurchaseItem")]
        public string PurchaseItem { get; set; }

        [JsonProperty("SalesItem")]
        public string SalesItem { get; set; }

        [JsonProperty("InventoryItem")]
        public string InventoryItem { get; set; }

        [JsonProperty("IncomeAccount")]
        public object IncomeAccount { get; set; }

        [JsonProperty("ExemptIncomeAccount")]
        public object ExemptIncomeAccount { get; set; }

        [JsonProperty("ExpanseAccount")]
        public object ExpanseAccount { get; set; }

        [JsonProperty("Mainsupplier")]
        public string Mainsupplier { get; set; }

        [JsonProperty("SupplierCatalogNo")]
        public object SupplierCatalogNo { get; set; }

        [JsonProperty("DesiredInventory")]
        public long? DesiredInventory { get; set; }

        [JsonProperty("MinInventory")]
        public long? MinInventory { get; set; }

        [JsonProperty("Picture")]
        public object Picture { get; set; }

        [JsonProperty("User_Text")]
        public string UserText { get; set; }

        [JsonProperty("SerialNum")]
        public object SerialNum { get; set; }

        [JsonProperty("CommissionPercent")]
        public long? CommissionPercent { get; set; }

        [JsonProperty("CommissionSum")]
        public long? CommissionSum { get; set; }

        [JsonProperty("CommissionGroup")]
        public long? CommissionGroup { get; set; }

        [JsonProperty("TreeType")]
        public string TreeType { get; set; }

        [JsonProperty("AssetItem")]
        public AssetItem AssetItem { get; set; }

        [JsonProperty("DataExportCode")]
        public object DataExportCode { get; set; }

        [JsonProperty("Manufacturer")]
        public long? Manufacturer { get; set; }

        [JsonProperty("QuantityOnStock")]
        public long? QuantityOnStock { get; set; }

        [JsonProperty("QuantityOrderedFromVendors")]
        public long? QuantityOrderedFromVendors { get; set; }

        [JsonProperty("QuantityOrderedByCustomers")]
        public long? QuantityOrderedByCustomers { get; set; }

        [JsonProperty("ManageSerialNumbers")]
        public AssetItem ManageSerialNumbers { get; set; }

        [JsonProperty("ManageBatchNumbers")]
        public string ManageBatchNumbers { get; set; }

        [JsonProperty("Valid")]
        public string Valid { get; set; }

        [JsonProperty("ValidFrom")]
        public object ValidFrom { get; set; }

        [JsonProperty("ValidTo")]
        public object ValidTo { get; set; }

        [JsonProperty("ValidRemarks")]
        public object ValidRemarks { get; set; }

        [JsonProperty("Frozen")]
        public AssetItem Frozen { get; set; }

        [JsonProperty("FrozenFrom")]
        public object FrozenFrom { get; set; }

        [JsonProperty("FrozenTo")]
        public object FrozenTo { get; set; }

        [JsonProperty("FrozenRemarks")]
        public object FrozenRemarks { get; set; }

        [JsonProperty("SalesUnit")]
        public string SalesUnit { get; set; }

        [JsonProperty("SalesItemsPerUnit")]
        public long? SalesItemsPerUnit { get; set; }

        [JsonProperty("SalesPackagingUnit")]
        public object SalesPackagingUnit { get; set; }

        [JsonProperty("SalesQtyPerPackUnit")]
        public long? SalesQtyPerPackUnit { get; set; }

        [JsonProperty("SalesUnitLength")]
        public float? SalesUnitLength { get; set; }

        [JsonProperty("SalesLengthUnit")]
        public long? SalesLengthUnit { get; set; }

        [JsonProperty("SalesUnitWidth")]
        public float? SalesUnitWidth { get; set; }

        [JsonProperty("SalesWidthUnit")]
        public long? SalesWidthUnit { get; set; }

        [JsonProperty("SalesUnitHeight")]
        public float? SalesUnitHeight { get; set; }

        [JsonProperty("SalesHeightUnit")]
        public long? SalesHeightUnit { get; set; }

        [JsonProperty("SalesUnitVolume")]
        public double SalesUnitVolume { get; set; }

        [JsonProperty("SalesVolumeUnit")]
        public long? SalesVolumeUnit { get; set; }

        [JsonProperty("SalesUnitWeight", NullValueHandling = NullValueHandling.Ignore)]
        public float? SalesUnitWeight { get; set; }

        [JsonProperty("SalesWeightUnit", NullValueHandling = NullValueHandling.Ignore)]
        public long? SalesWeightUnit { get; set; }

        [JsonProperty("PurchaseUnit")]
        public string PurchaseUnit { get; set; }

        [JsonProperty("PurchaseItemsPerUnit", NullValueHandling = NullValueHandling.Ignore)]
        public long? PurchaseItemsPerUnit { get; set; }

        [JsonProperty("PurchasePackagingUnit")]
        public object PurchasePackagingUnit { get; set; }

        [JsonProperty("PurchaseQtyPerPackUnit", NullValueHandling = NullValueHandling.Ignore)]
        public long? PurchaseQtyPerPackUnit { get; set; }

        [JsonProperty("PurchaseUnitLength")]
        public long? PurchaseUnitLength { get; set; }

        [JsonProperty("PurchaseLengthUnit")]
        public object PurchaseLengthUnit { get; set; }

        [JsonProperty("PurchaseUnitWidth")]
        public long? PurchaseUnitWidth { get; set; }

        [JsonProperty("PurchaseWidthUnit")]
        public object PurchaseWidthUnit { get; set; }

        [JsonProperty("PurchaseUnitHeight")]
        public long? PurchaseUnitHeight { get; set; }

        [JsonProperty("PurchaseHeightUnit")]
        public object PurchaseHeightUnit { get; set; }

        [JsonProperty("PurchaseUnitVolume")]
        public long? PurchaseUnitVolume { get; set; }

        [JsonProperty("PurchaseVolumeUnit")]
        public long? PurchaseVolumeUnit { get; set; }

        [JsonProperty("PurchaseUnitWeight")]
        public long? PurchaseUnitWeight { get; set; }

        [JsonProperty("PurchaseWeightUnit")]
        public object PurchaseWeightUnit { get; set; }

        [JsonProperty("PurchaseVATGroup")]
        public object PurchaseVatGroup { get; set; }

        [JsonProperty("SalesFactor1")]
        public long? SalesFactor1 { get; set; }

        [JsonProperty("SalesFactor2")]
        public long? SalesFactor2 { get; set; }

        [JsonProperty("SalesFactor3")]
        public long? SalesFactor3 { get; set; }

        [JsonProperty("SalesFactor4")]
        public long? SalesFactor4 { get; set; }

        [JsonProperty("PurchaseFactor1")]
        public long? PurchaseFactor1 { get; set; }

        [JsonProperty("PurchaseFactor2")]
        public long? PurchaseFactor2 { get; set; }

        [JsonProperty("PurchaseFactor3")]
        public long? PurchaseFactor3 { get; set; }

        [JsonProperty("PurchaseFactor4")]
        public long? PurchaseFactor4 { get; set; }

        [JsonProperty("MovingAveragePrice")]
        public long? MovingAveragePrice { get; set; }

        [JsonProperty("ForeignRevenuesAccount")]
        public object ForeignRevenuesAccount { get; set; }

        [JsonProperty("ECRevenuesAccount")]
        public object EcRevenuesAccount { get; set; }

        [JsonProperty("ForeignExpensesAccount")]
        public object ForeignExpensesAccount { get; set; }

        [JsonProperty("ECExpensesAccount")]
        public object EcExpensesAccount { get; set; }

        [JsonProperty("AvgStdPrice")]
        public long? AvgStdPrice { get; set; }

        [JsonProperty("DefaultWarehouse")]
        public object DefaultWarehouse { get; set; }

        [JsonProperty("ShipType")]
        public long? ShipType { get; set; }

        [JsonProperty("GLMethod")]
        public string GlMethod { get; set; }

        [JsonProperty("TaxType")]
        public string TaxType { get; set; }

        [JsonProperty("MaxInventory")]
        public long? MaxInventory { get; set; }

        [JsonProperty("ManageStockByWarehouse")]
        public string ManageStockByWarehouse { get; set; }

        [JsonProperty("PurchaseHeightUnit1")]
        public object PurchaseHeightUnit1 { get; set; }

        [JsonProperty("PurchaseUnitHeight1")]
        public long? PurchaseUnitHeight1 { get; set; }

        [JsonProperty("PurchaseLengthUnit1")]
        public object PurchaseLengthUnit1 { get; set; }

        [JsonProperty("PurchaseUnitLength1")]
        public long? PurchaseUnitLength1 { get; set; }

        [JsonProperty("PurchaseWeightUnit1")]
        public object PurchaseWeightUnit1 { get; set; }

        [JsonProperty("PurchaseUnitWeight1")]
        public long? PurchaseUnitWeight1 { get; set; }

        [JsonProperty("PurchaseWidthUnit1")]
        public object PurchaseWidthUnit1 { get; set; }

        [JsonProperty("PurchaseUnitWidth1")]
        public long? PurchaseUnitWidth1 { get; set; }

        [JsonProperty("SalesHeightUnit1")]
        public long? SalesHeightUnit1 { get; set; }

        [JsonProperty("SalesUnitHeight1")]
        public long? SalesUnitHeight1 { get; set; }

        [JsonProperty("SalesLengthUnit1")]
        public object SalesLengthUnit1 { get; set; }

        [JsonProperty("SalesUnitLength1")]
        public long? SalesUnitLength1 { get; set; }

        [JsonProperty("SalesWeightUnit1")]
        public object SalesWeightUnit1 { get; set; }

        [JsonProperty("SalesUnitWeight1")]
        public long? SalesUnitWeight1 { get; set; }

        [JsonProperty("SalesWidthUnit1")]
        public long? SalesWidthUnit1 { get; set; }

        [JsonProperty("SalesUnitWidth1")]
        public long? SalesUnitWidth1 { get; set; }

        [JsonProperty("ForceSelectionOfSerialNumber")]
        public string ForceSelectionOfSerialNumber { get; set; }

        [JsonProperty("ManageSerialNumbersOnReleaseOnly")]
        public AssetItem ManageSerialNumbersOnReleaseOnly { get; set; }

        [JsonProperty("WTLiable")]
        public AssetItem WtLiable { get; set; }

        [JsonProperty("CostAccountingMethod")]
        public string CostAccountingMethod { get; set; }

        [JsonProperty("SWW")]
        public object Sww { get; set; }

        [JsonProperty("WarrantyTemplate")]
        public object WarrantyTemplate { get; set; }

        [JsonProperty("IndirectTax")]
        public AssetItem IndirectTax { get; set; }

        [JsonProperty("ArTaxCode")]
        public object ArTaxCode { get; set; }

        [JsonProperty("ApTaxCode")]
        public object ApTaxCode { get; set; }

        [JsonProperty("BaseUnitName")]
        public object BaseUnitName { get; set; }

        [JsonProperty("ItemCountryOrg")]
        public object ItemCountryOrg { get; set; }

        [JsonProperty("IssueMethod")]
        public string IssueMethod { get; set; }

        [JsonProperty("SRIAndBatchManageMethod")]
        public string SriAndBatchManageMethod { get; set; }

        [JsonProperty("IsPhantom")]
        public AssetItem IsPhantom { get; set; }

        [JsonProperty("InventoryUOM")]
        public string InventoryUom { get; set; }

        [JsonProperty("PlanningSystem")]
        public string PlanningSystem { get; set; }

        [JsonProperty("ProcurementMethod")]
        public string ProcurementMethod { get; set; }

        [JsonProperty("ComponentWarehouse")]
        public string ComponentWarehouse { get; set; }

        [JsonProperty("OrderIntervals")]
        public object OrderIntervals { get; set; }

        [JsonProperty("OrderMultiple")]
        public long? OrderMultiple { get; set; }

        [JsonProperty("LeadTime")]
        public long? LeadTime { get; set; }

        [JsonProperty("MinOrderQuantity")]
        public long? MinOrderQuantity { get; set; }

        [JsonProperty("ItemType")]
        public string ItemType { get; set; }

        [JsonProperty("ItemClass")]
        public string ItemClass { get; set; }

        [JsonProperty("OutgoingServiceCode")]
        public long? OutgoingServiceCode { get; set; }

        [JsonProperty("IncomingServiceCode")]
        public long? IncomingServiceCode { get; set; }

        [JsonProperty("ServiceGroup")]
        public long? ServiceGroup { get; set; }

        [JsonProperty("NCMCode")]
        public long? NcmCode { get; set; }

        [JsonProperty("MaterialType")]
        public string MaterialType { get; set; }

        [JsonProperty("MaterialGroup")]
        public long? MaterialGroup { get; set; }

        [JsonProperty("ProductSource")]
        public string ProductSource { get; set; }

        [JsonProperty("Properties1")]
        public AssetItem Properties1 { get; set; }

        [JsonProperty("Properties2")]
        public AssetItem Properties2 { get; set; }

        [JsonProperty("Properties3")]
        public AssetItem Properties3 { get; set; }

        [JsonProperty("Properties4")]
        public AssetItem Properties4 { get; set; }

        [JsonProperty("Properties5")]
        public AssetItem Properties5 { get; set; }

        [JsonProperty("Properties6")]
        public AssetItem Properties6 { get; set; }

        [JsonProperty("Properties7")]
        public AssetItem Properties7 { get; set; }

        [JsonProperty("Properties8")]
        public AssetItem Properties8 { get; set; }

        [JsonProperty("Properties9")]
        public AssetItem Properties9 { get; set; }

        [JsonProperty("Properties10")]
        public AssetItem Properties10 { get; set; }

        [JsonProperty("Properties11")]
        public AssetItem Properties11 { get; set; }

        [JsonProperty("Properties12")]
        public AssetItem Properties12 { get; set; }

        [JsonProperty("Properties13")]
        public AssetItem Properties13 { get; set; }

        [JsonProperty("Properties14")]
        public AssetItem Properties14 { get; set; }

        [JsonProperty("Properties15")]
        public AssetItem Properties15 { get; set; }

        [JsonProperty("Properties16")]
        public AssetItem Properties16 { get; set; }

        [JsonProperty("Properties17")]
        public AssetItem Properties17 { get; set; }

        [JsonProperty("Properties18")]
        public AssetItem Properties18 { get; set; }

        [JsonProperty("Properties19")]
        public AssetItem Properties19 { get; set; }

        [JsonProperty("Properties20")]
        public AssetItem Properties20 { get; set; }

        [JsonProperty("Properties21")]
        public AssetItem Properties21 { get; set; }

        [JsonProperty("Properties22")]
        public AssetItem Properties22 { get; set; }

        [JsonProperty("Properties23")]
        public AssetItem Properties23 { get; set; }

        [JsonProperty("Properties24")]
        public AssetItem Properties24 { get; set; }

        [JsonProperty("Properties25")]
        public AssetItem Properties25 { get; set; }

        [JsonProperty("Properties26")]
        public AssetItem Properties26 { get; set; }

        [JsonProperty("Properties27")]
        public AssetItem Properties27 { get; set; }

        [JsonProperty("Properties28")]
        public AssetItem Properties28 { get; set; }

        [JsonProperty("Properties29")]
        public AssetItem Properties29 { get; set; }

        [JsonProperty("Properties30")]
        public AssetItem Properties30 { get; set; }

        [JsonProperty("Properties31")]
        public AssetItem Properties31 { get; set; }

        [JsonProperty("Properties32")]
        public AssetItem Properties32 { get; set; }

        [JsonProperty("Properties33")]
        public AssetItem Properties33 { get; set; }

        [JsonProperty("Properties34")]
        public AssetItem Properties34 { get; set; }

        [JsonProperty("Properties35")]
        public AssetItem Properties35 { get; set; }

        [JsonProperty("Properties36")]
        public AssetItem Properties36 { get; set; }

        [JsonProperty("Properties37")]
        public AssetItem Properties37 { get; set; }

        [JsonProperty("Properties38")]
        public AssetItem Properties38 { get; set; }

        [JsonProperty("Properties39")]
        public AssetItem Properties39 { get; set; }

        [JsonProperty("Properties40")]
        public AssetItem Properties40 { get; set; }

        [JsonProperty("Properties41")]
        public AssetItem Properties41 { get; set; }

        [JsonProperty("Properties42")]
        public AssetItem Properties42 { get; set; }

        [JsonProperty("Properties43")]
        public AssetItem Properties43 { get; set; }

        [JsonProperty("Properties44")]
        public AssetItem Properties44 { get; set; }

        [JsonProperty("Properties45")]
        public AssetItem Properties45 { get; set; }

        [JsonProperty("Properties46")]
        public AssetItem Properties46 { get; set; }

        [JsonProperty("Properties47")]
        public AssetItem Properties47 { get; set; }

        [JsonProperty("Properties48")]
        public AssetItem Properties48 { get; set; }

        [JsonProperty("Properties49")]
        public AssetItem Properties49 { get; set; }

        [JsonProperty("Properties50")]
        public AssetItem Properties50 { get; set; }

        [JsonProperty("Properties51")]
        public AssetItem Properties51 { get; set; }

        [JsonProperty("Properties52")]
        public AssetItem Properties52 { get; set; }

        [JsonProperty("Properties53")]
        public AssetItem Properties53 { get; set; }

        [JsonProperty("Properties54")]
        public AssetItem Properties54 { get; set; }

        [JsonProperty("Properties55")]
        public AssetItem Properties55 { get; set; }

        [JsonProperty("Properties56")]
        public AssetItem Properties56 { get; set; }

        [JsonProperty("Properties57")]
        public AssetItem Properties57 { get; set; }

        [JsonProperty("Properties58")]
        public AssetItem Properties58 { get; set; }

        [JsonProperty("Properties59")]
        public AssetItem Properties59 { get; set; }

        [JsonProperty("Properties60")]
        public AssetItem Properties60 { get; set; }

        [JsonProperty("Properties61")]
        public AssetItem Properties61 { get; set; }

        [JsonProperty("Properties62")]
        public AssetItem Properties62 { get; set; }

        [JsonProperty("Properties63")]
        public AssetItem Properties63 { get; set; }

        [JsonProperty("Properties64")]
        public AssetItem Properties64 { get; set; }

        [JsonProperty("AutoCreateSerialNumbersOnRelease")]
        public AssetItem AutoCreateSerialNumbersOnRelease { get; set; }

        [JsonProperty("DNFEntry")]
        public long? DnfEntry { get; set; }

        [JsonProperty("GTSItemSpec")]
        public object GtsItemSpec { get; set; }

        [JsonProperty("GTSItemTaxCategory")]
        public object GtsItemTaxCategory { get; set; }

        [JsonProperty("FuelID")]
        public long? FuelId { get; set; }

        [JsonProperty("BeverageTableCode")]
        public object BeverageTableCode { get; set; }

        [JsonProperty("BeverageGroupCode")]
        public object BeverageGroupCode { get; set; }

        [JsonProperty("BeverageCommercialBrandCode")]
        public long? BeverageCommercialBrandCode { get; set; }

        [JsonProperty("Series")]
        public long? Series { get; set; }

        [JsonProperty("ToleranceDays")]
        public object ToleranceDays { get; set; }

        [JsonProperty("TypeOfAdvancedRules")]
        public string TypeOfAdvancedRules { get; set; }

        [JsonProperty("IssuePrimarilyBy")]
        public string IssuePrimarilyBy { get; set; }

        [JsonProperty("NoDiscounts")]
        public AssetItem NoDiscounts { get; set; }

        [JsonProperty("AssetClass")]
        public string AssetClass { get; set; }

        [JsonProperty("AssetGroup")]
        public string AssetGroup { get; set; }

        [JsonProperty("InventoryNumber")]
        public string InventoryNumber { get; set; }

        [JsonProperty("Technician")]
        public object Technician { get; set; }

        [JsonProperty("Employee")]
        public object Employee { get; set; }

        [JsonProperty("Location")]
        public object Location { get; set; }

        [JsonProperty("AssetStatus")]
        public string AssetStatus { get; set; }

        [JsonProperty("CapitalizationDate")]
        public object CapitalizationDate { get; set; }

        [JsonProperty("StatisticalAsset")]
        public AssetItem StatisticalAsset { get; set; }

        [JsonProperty("Cession")]
        public AssetItem Cession { get; set; }

        [JsonProperty("DeactivateAfterUsefulLife")]
        public AssetItem DeactivateAfterUsefulLife { get; set; }

        [JsonProperty("ManageByQuantity")]
        public AssetItem ManageByQuantity { get; set; }

        [JsonProperty("UoMGroupEntry")]
        public long? UoMGroupEntry { get; set; }

        [JsonProperty("InventoryUoMEntry")]
        public long? InventoryUoMEntry { get; set; }

        [JsonProperty("DefaultSalesUoMEntry")]
        public long? DefaultSalesUoMEntry { get; set; }

        [JsonProperty("DefaultPurchasingUoMEntry")]
        public long? DefaultPurchasingUoMEntry { get; set; }

        [JsonProperty("DepreciationGroup")]
        public object DepreciationGroup { get; set; }

        [JsonProperty("AssetSerialNumber")]
        public string AssetSerialNumber { get; set; }

        [JsonProperty("InventoryWeight")]
        public long? InventoryWeight { get; set; }

        [JsonProperty("InventoryWeightUnit")]
        public object InventoryWeightUnit { get; set; }

        [JsonProperty("InventoryWeight1")]
        public long? InventoryWeight1 { get; set; }

        [JsonProperty("InventoryWeightUnit1")]
        public object InventoryWeightUnit1 { get; set; }

        [JsonProperty("DefaultCountingUnit")]
        public object DefaultCountingUnit { get; set; }

        [JsonProperty("CountingItemsPerUnit")]
        public long? CountingItemsPerUnit { get; set; }

        [JsonProperty("DefaultCountingUoMEntry")]
        public object DefaultCountingUoMEntry { get; set; }

        [JsonProperty("Excisable")]
        public AssetItem Excisable { get; set; }

        [JsonProperty("ChapterID")]
        public long? ChapterId { get; set; }

        [JsonProperty("ScsCode")]
        public object ScsCode { get; set; }

        [JsonProperty("SpProdType")]
        public object SpProdType { get; set; }

        [JsonProperty("ProdStdCost")]
        public long? ProdStdCost { get; set; }

        [JsonProperty("InCostRollup")]
        public string InCostRollup { get; set; }

        [JsonProperty("VirtualAssetItem")]
        public AssetItem VirtualAssetItem { get; set; }

        [JsonProperty("EnforceAssetSerialNumbers")]
        public AssetItem EnforceAssetSerialNumbers { get; set; }

        [JsonProperty("AttachmentEntry")]
        public object AttachmentEntry { get; set; }

        [JsonProperty("LinkedResource")]
        public object LinkedResource { get; set; }

        [JsonProperty("UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        [JsonProperty("UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        [JsonProperty("GSTRelevnt")]
        public AssetItem GstRelevnt { get; set; }

        [JsonProperty("SACEntry")]
        public long? SacEntry { get; set; }

        [JsonProperty("GSTTaxCategory")]
        public object GstTaxCategory { get; set; }

        [JsonProperty("ServiceCategoryEntry")]
        public long? ServiceCategoryEntry { get; set; }

        [JsonProperty("CapitalGoodsOnHoldPercent")]
        public object CapitalGoodsOnHoldPercent { get; set; }

        [JsonProperty("CapitalGoodsOnHoldLimit")]
        public object CapitalGoodsOnHoldLimit { get; set; }

        [JsonProperty("AssessableValue")]
        public long? AssessableValue { get; set; }

        [JsonProperty("AssVal4WTR")]
        public long? AssVal4Wtr { get; set; }

        [JsonProperty("SOIExcisable")]
        public string SoiExcisable { get; set; }

        [JsonProperty("TNVED")]
        public object Tnved { get; set; }

        [JsonProperty("ImportedItem")]
        public AssetItem ImportedItem { get; set; }

        [JsonProperty("PricingUnit")]
        public long? PricingUnit { get; set; }

        [JsonProperty("CreateDate")]
        public DateTime? CreateDate { get; set; }

        [JsonProperty("CreateTime")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("ItemPrices")]
        public List<ItemPrice> ItemPrices { get; set; }

        [JsonProperty("ItemWarehouseInfoCollection")]
        public ItemWarehouseInfoCollection[] ItemWarehouseInfoCollection { get; set; }

        [JsonProperty("ItemPreferredVendors")]
        public ItemPreferredVendor[] ItemPreferredVendors { get; set; }

        [JsonProperty("ItemLocalizationInfos")]
        public object[] ItemLocalizationInfos { get; set; }

        [JsonProperty("ItemProjects")]
        public object[] ItemProjects { get; set; }

        [JsonProperty("ItemDistributionRules")]
        public object[] ItemDistributionRules { get; set; }

        [JsonProperty("ItemAttributeGroups")]
        public object[] ItemAttributeGroups { get; set; }

        [JsonProperty("ItemDepreciationParameters")]
        public object[] ItemDepreciationParameters { get; set; }

        [JsonProperty("ItemPeriodControls")]
        public object[] ItemPeriodControls { get; set; }

        [JsonProperty("ItemUnitOfMeasurementCollection")]
        public ItemUnitOfMeasurementCollection[] ItemUnitOfMeasurementCollection { get; set; }

        [JsonProperty("ItemBarCodeCollection")]
        public ItemBarCodeCollection[] ItemBarCodeCollection { get; set; }

        [JsonProperty("ItemIntrastatExtension")]
        public ItemIntrastatExtension ItemIntrastatExtension { get; set; }

        [JsonProperty("U_CVA_FlexyImagem")]
        public string U_CVA_FlexyImagem { get; set; }
    }

    public partial class ItemBarCodeCollection
    {
        [JsonProperty("AbsEntry")]
        public long? AbsEntry { get; set; }

        [JsonProperty("UoMEntry")]
        public long? UoMEntry { get; set; }

        [JsonProperty("Barcode")]
        public string Barcode { get; set; }

        [JsonProperty("FreeText")]
        public object FreeText { get; set; }
    }

    public partial class ItemIntrastatExtension
    {
    }

    public partial class ItemPreferredVendor
    {
        [JsonProperty("BPCode")]
        public string BpCode { get; set; }
    }

    public partial class ItemPrice
    {
        [JsonProperty("PriceList")]
        public long? PriceList { get; set; }

        [JsonProperty("PriceListName")]
        public string PriceListName { get; set; }

        [JsonProperty("Price")]
        public double Price { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("AdditionalPrice1")]
        public long? AdditionalPrice1 { get; set; }

        [JsonProperty("AdditionalCurrency1")]
        public string AdditionalCurrency1 { get; set; }

        [JsonProperty("AdditionalPrice2")]
        public long? AdditionalPrice2 { get; set; }

        [JsonProperty("AdditionalCurrency2")]
        public string AdditionalCurrency2 { get; set; }

        [JsonProperty("BasePriceList")]
        public long? BasePriceList { get; set; }

        [JsonProperty("Factor")]
        public long? Factor { get; set; }

        [JsonProperty("UoMPrices")]
        public object[] UoMPrices { get; set; }
    }

    public partial class ItemUnitOfMeasurementCollection
    {
        [JsonProperty("UoMType")]
        public UoMType UoMType { get; set; }

        [JsonProperty("UoMEntry")]
        public long? UoMEntry { get; set; }

        [JsonProperty("DefaultBarcode")]
        public long? DefaultBarcode { get; set; }

        [JsonProperty("DefaultPackage")]
        public object DefaultPackage { get; set; }

        [JsonProperty("Length1")]
        public long? Length1 { get; set; }

        [JsonProperty("Length1Unit")]
        public long? Length1Unit { get; set; }

        [JsonProperty("Length2")]
        public long? Length2 { get; set; }

        [JsonProperty("Length2Unit")]
        public object Length2Unit { get; set; }

        [JsonProperty("Width1")]
        public long? Width1 { get; set; }

        [JsonProperty("Width1Unit")]
        public long? Width1Unit { get; set; }

        [JsonProperty("Width2")]
        public long? Width2 { get; set; }

        [JsonProperty("Width2Unit")]
        public object Width2Unit { get; set; }

        [JsonProperty("Height1")]
        public long? Height1 { get; set; }

        [JsonProperty("Height1Unit")]
        public long? Height1Unit { get; set; }

        [JsonProperty("Height2")]
        public long? Height2 { get; set; }

        [JsonProperty("Height2Unit")]
        public object Height2Unit { get; set; }

        [JsonProperty("Volume")]
        public long? Volume { get; set; }

        [JsonProperty("VolumeUnit")]
        public long? VolumeUnit { get; set; }

        [JsonProperty("Weight1")]
        public double? Weight1 { get; set; }

        [JsonProperty("Weight1Unit")]
        public long? Weight1Unit { get; set; }

        [JsonProperty("Weight2")]
        public long? Weight2 { get; set; }

        [JsonProperty("Weight2Unit")]
        public object Weight2Unit { get; set; }

        [JsonProperty("ItemUoMPackageCollection")]
        public object[] ItemUoMPackageCollection { get; set; }
    }

    public partial class ItemWarehouseInfoCollection
    {
        [JsonProperty("MinimalStock")]
        public long? MinimalStock { get; set; }

        [JsonProperty("MaximalStock")]
        public long? MaximalStock { get; set; }

        [JsonProperty("MinimalOrder")]
        public long? MinimalOrder { get; set; }

        [JsonProperty("StandardAveragePrice")]
        public double StandardAveragePrice { get; set; }

        [JsonProperty("Locked")]
        public AssetItem Locked { get; set; }

        [JsonProperty("InventoryAccount")]
        public object InventoryAccount { get; set; }

        [JsonProperty("CostAccount")]
        public object CostAccount { get; set; }

        [JsonProperty("TransferAccount")]
        public object TransferAccount { get; set; }

        [JsonProperty("RevenuesAccount")]
        public object RevenuesAccount { get; set; }

        [JsonProperty("VarienceAccount")]
        public object VarienceAccount { get; set; }

        [JsonProperty("DecreasingAccount")]
        public object DecreasingAccount { get; set; }

        [JsonProperty("IncreasingAccount")]
        public object IncreasingAccount { get; set; }

        [JsonProperty("ReturningAccount")]
        public object ReturningAccount { get; set; }

        [JsonProperty("ExpensesAccount")]
        public object ExpensesAccount { get; set; }

        [JsonProperty("EURevenuesAccount")]
        public object EuRevenuesAccount { get; set; }

        [JsonProperty("EUExpensesAccount")]
        public object EuExpensesAccount { get; set; }

        [JsonProperty("ForeignRevenueAcc")]
        public object ForeignRevenueAcc { get; set; }

        [JsonProperty("ForeignExpensAcc")]
        public object ForeignExpensAcc { get; set; }

        [JsonProperty("ExemptIncomeAcc")]
        public object ExemptIncomeAcc { get; set; }

        [JsonProperty("PriceDifferenceAcc")]
        public object PriceDifferenceAcc { get; set; }

        [JsonProperty("WarehouseCode")]
        public string WarehouseCode { get; set; }

        [JsonProperty("InStock")]
        public long? InStock { get; set; }

        [JsonProperty("Committed")]
        public long? Committed { get; set; }

        [JsonProperty("Ordered")]
        public long? Ordered { get; set; }

        [JsonProperty("CountedQuantity")]
        public long? CountedQuantity { get; set; }

        [JsonProperty("WasCounted")]
        public AssetItem WasCounted { get; set; }

        [JsonProperty("UserSignature")]
        public long? UserSignature { get; set; }

        [JsonProperty("Counted")]
        public long? Counted { get; set; }

        [JsonProperty("ExpenseClearingAct")]
        public object ExpenseClearingAct { get; set; }

        [JsonProperty("PurchaseCreditAcc")]
        public object PurchaseCreditAcc { get; set; }

        [JsonProperty("EUPurchaseCreditAcc")]
        public object EuPurchaseCreditAcc { get; set; }

        [JsonProperty("ForeignPurchaseCreditAcc")]
        public object ForeignPurchaseCreditAcc { get; set; }

        [JsonProperty("SalesCreditAcc")]
        public object SalesCreditAcc { get; set; }

        [JsonProperty("SalesCreditEUAcc")]
        public object SalesCreditEuAcc { get; set; }

        [JsonProperty("ExemptedCredits")]
        public object ExemptedCredits { get; set; }

        [JsonProperty("SalesCreditForeignAcc")]
        public object SalesCreditForeignAcc { get; set; }

        [JsonProperty("ExpenseOffsettingAccount")]
        public object ExpenseOffsettingAccount { get; set; }

        [JsonProperty("WipAccount")]
        public object WipAccount { get; set; }

        [JsonProperty("ExchangeRateDifferencesAcct")]
        public object ExchangeRateDifferencesAcct { get; set; }

        [JsonProperty("GoodsClearingAcct")]
        public object GoodsClearingAcct { get; set; }

        [JsonProperty("NegativeInventoryAdjustmentAccount")]
        public object NegativeInventoryAdjustmentAccount { get; set; }

        [JsonProperty("CostInflationOffsetAccount")]
        public object CostInflationOffsetAccount { get; set; }

        [JsonProperty("GLDecreaseAcct")]
        public object GlDecreaseAcct { get; set; }

        [JsonProperty("GLIncreaseAcct")]
        public object GlIncreaseAcct { get; set; }

        [JsonProperty("PAReturnAcct")]
        public object PaReturnAcct { get; set; }

        [JsonProperty("PurchaseAcct")]
        public object PurchaseAcct { get; set; }

        [JsonProperty("PurchaseOffsetAcct")]
        public object PurchaseOffsetAcct { get; set; }

        [JsonProperty("ShippedGoodsAccount")]
        public object ShippedGoodsAccount { get; set; }

        [JsonProperty("StockInflationOffsetAccount")]
        public object StockInflationOffsetAccount { get; set; }

        [JsonProperty("StockInflationAdjustAccount")]
        public object StockInflationAdjustAccount { get; set; }

        [JsonProperty("VATInRevenueAccount")]
        public object VatInRevenueAccount { get; set; }

        [JsonProperty("WipVarianceAccount")]
        public object WipVarianceAccount { get; set; }

        [JsonProperty("CostInflationAccount")]
        public object CostInflationAccount { get; set; }

        [JsonProperty("WHIncomingCenvatAccount")]
        public object WhIncomingCenvatAccount { get; set; }

        [JsonProperty("WHOutgoingCenvatAccount")]
        public object WhOutgoingCenvatAccount { get; set; }

        [JsonProperty("StockInTransitAccount")]
        public object StockInTransitAccount { get; set; }

        [JsonProperty("WipOffsetProfitAndLossAccount")]
        public object WipOffsetProfitAndLossAccount { get; set; }

        [JsonProperty("InventoryOffsetProfitAndLossAccount")]
        public object InventoryOffsetProfitAndLossAccount { get; set; }

        [JsonProperty("DefaultBin")]
        public object DefaultBin { get; set; }

        [JsonProperty("DefaultBinEnforced")]
        public AssetItem DefaultBinEnforced { get; set; }

        [JsonProperty("PurchaseBalanceAccount")]
        public object PurchaseBalanceAccount { get; set; }

        [JsonProperty("ItemCode")]
        public string ItemCode { get; set; }

        [JsonProperty("ItemCycleCounts")]
        public object[] ItemCycleCounts { get; set; }
    }

    public enum AssetItem { TYes, TNo };

    public enum UoMType { IutInventory, IutPurchasing, IutSales };
}
