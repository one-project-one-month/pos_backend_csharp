namespace DotNet8.PosFrontendBlazor.Pages.Township
{
    public partial class P_TownshipDeleteDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string contentText { get; set; }
        [Parameter] public string buttonText { get; set; }
        [Parameter] public Color color { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void DeleteTownship()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}
