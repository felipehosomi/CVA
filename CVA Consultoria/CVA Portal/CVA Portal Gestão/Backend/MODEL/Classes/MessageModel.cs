namespace MODEL.Classes
{
    public class MessageModel
    {
        public ErrorMessage Error { get; set; }
        public SuccessMessage Success { get; set; }
    }
    public class ErrorMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
    public class SuccessMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}