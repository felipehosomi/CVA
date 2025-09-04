using System;

namespace CVA.Core.ObrigacoesFiscais.MODEL
{
    public class FileFilterModel
    {
        public string Layout { get; set; }
        public string LayoutDesc { get; set; }
        public int BranchId { get; set; }
        public int Period { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool ExcelLayout { get; set; }
        public string Directory { get; set; }
    }
}
