namespace ServiceLayerHelper
{
    public class AttachmentResponse
    {
        public string odatametadata { get; set; }
        public int AbsoluteEntry { get; set; }
        public Attachments2_Lines[] Attachments2_Lines { get; set; }
    }

    public class Attachments2_Lines
    {
        public string SourcePath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentDate { get; set; }
        public int UserID { get; set; }
        public string Override { get; set; }
    }
}
