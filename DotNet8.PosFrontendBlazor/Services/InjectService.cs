using DotNet8.PosFrontendBlazor.Pages.Staff;

namespace DotNet8.PosFrontendBlazor.Services;

public class InjectService
{
    private readonly ISnackbar _snackbar;
    private readonly IDialogService _dialogService;

    public InjectService(ISnackbar snackbar, IDialogService dialogService)
    {
        _snackbar = snackbar;
        _dialogService = dialogService;
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

    public async Task<DialogResult> ShowModalBoxAsync<T>(string title) where T : IComponent
    {
        MudBlazor.DialogOptions options = new MudBlazor.DialogOptions()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            DisableBackdropClick = true,
            CloseOnEscapeKey = false
        };
        var dialog = await _dialogService.ShowAsync<T>("New Staff", options);
        var result = await dialog.Result;
        return result;
    }
}

public enum EnumResponseType
{
    Success,
    Information,
    Warning,
    Error
}
