namespace ServiceLayerHelper
{
    internal class LoginResponse
    {
        public string SessionId { get; set; }
        public string Version { get; set; }
        public int SessionTimeout { get; set; }
    }
}
