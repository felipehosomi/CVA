using System;

namespace SkaSapWs.BLL.MODEL
{
    public class SspExportInt
    {
        public string BELNR_ID { get; set; }
        public int BELPOS_ID { get; set; }
        public int POS_ID { get; set; }
        public string TYP { get; set; }
        public int RESOURCENPOS_ID { get; set; }
        public string PERS_ID { get; set; }
        public string ANFZEIT { get; set; }
        public string ENDZEIT { get; set; }
        public double ZEIT { get; set; }
        public int MENGE_GUT_RM { get; set; }
        public int MENGE_SCHLECHT_RM { get; set; }
        public string ABGKZ { get; set; }
        public string manualbooking { get; set; }
        public string APLATZ_ID { get; set; }
        public string KSTST_ID { get; set; }
        public string GRUND { get; set; }
        public DateTime DocDate { get; set; }
        public string Project { get; set; }
        public string TIMETYPE_ID { get; set; }
        public string EXTERNAL_COST { get; set; }
        public string BatchNum { get; set; }
        public int UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string WKZ_ID { get; set; }
    }
}
