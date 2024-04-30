namespace DotNet8.PosFrontendBlazor.Services;

public class InjectService
{
    private readonly ISnackbar _snackbar;

    public InjectService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void ShowMessage(string message, EnumResponseType responseType)
    {
        switch (responseType)
        {
            case EnumResponseType.Success:
                _snackbar.Add(message, Severity.Success);
                break;
            case EnumResponseType.Information:
                _snackbar.Add(message, Severity.Info);
                break;
            case EnumResponseType.Warning:
                _snackbar.Add(message, Severity.Warning);
                break;
            case EnumResponseType.Error:
                _snackbar.Add(message, Severity.Error);
                break;
            default:
                break;
        }
    }
}

public enum EnumResponseType
{
    Success,
    Information,
    Warning,
    Error
}
