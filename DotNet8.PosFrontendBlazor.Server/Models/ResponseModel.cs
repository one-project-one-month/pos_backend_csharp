namespace DotNet8.PosFrontendBlazor.Server.Models;

public class ResponseModel
{
    public string Token { get; set; }
    public int Result { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsError => !IsSuccess;
    public string Message { get; set; }
}