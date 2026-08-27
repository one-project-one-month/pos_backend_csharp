using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Pos.App.Services;

public sealed class FlashMessageService(
    ITempDataDictionaryFactory factory,
    IHttpContextAccessor contextAccessor)
{
    public void Set(string message, string level = "success")
    {
        var data = GetDictionary();
        data["FlashMessage"] = message;
        data["FlashLevel"] = level;
        data.Save();
    }

    public (string? Message, string? Level) Consume()
    {
        var data = GetDictionary();
        var result = (data["FlashMessage"] as string, data["FlashLevel"] as string);
        data.Save();
        return result;
    }

    private ITempDataDictionary GetDictionary()
    {
        var context = contextAccessor.HttpContext
            ?? throw new InvalidOperationException("An active HTTP context is required.");
        return factory.GetTempData(context);
    }
}
