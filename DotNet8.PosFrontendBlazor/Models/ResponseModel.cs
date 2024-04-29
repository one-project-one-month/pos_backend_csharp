namespace DotNet8.PosFrontendBlazor.Models
{
    public class ResponseModel
    {
        public string Token { get; set; }
        public int Count { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
