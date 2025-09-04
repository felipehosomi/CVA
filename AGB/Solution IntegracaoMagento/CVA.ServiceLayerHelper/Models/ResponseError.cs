namespace ServiceLayerHelper
{
    internal class ResponseError
    {
        public Error error { get; set; }
    }

    internal class Error
    {
        public int code { get; set; }
        public Message message { get; set; }
    }

    internal class Message
    {
        public string lang { get; set; }
        public string value { get; set; }
    }
}
