using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.Model
{
    public class AttachmentModel
    {
        public AttachmentModel()
        {
            Attachments2_Lines = new List<Attachments2_Lines>();
        }

        public string odatametadata { get; set; }
        public int AbsoluteEntry { get; set; }
        public List<Attachments2_Lines> Attachments2_Lines { get; set; }
    }

    public class Attachments2_Lines
    {
        public string SourcePath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentDate { get; set; }
        public string Override { get; set; }
        public string FreeText { get; set; }
    }

    public class AttachmentsAPI
    {
        public string type { get; set; }
        public string fileName { get; set; }
        public byte[] attachmentByte { get; set; }
        public string comments { get; set; }
    }

    public class AttachmentsPath
    {
        public string AttachPath { get; set; }
    }

}
