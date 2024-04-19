namespace DotNet8.PosBackendApi.Models;

public class ResponseModel
{
    public object ReturnCommand(ReturnCommandModel model)
    {
        JObject jsonObject = new JObject(
            new JProperty("message", model.message),
            new JProperty("isSuccess", model.isSuccess),
            new JProperty("data", model.item is null ? model.item :
                new JObject(
                    new JProperty(model.enumPos.ToString().ToLower(), JToken.FromObject(model.item))
                )
            )
        );
        return jsonObject;
    }

    public object ReturnById(ReturnByIdModel model)
    {
        JObject jsonObject = new JObject(
            new JProperty("message", model.message),
            new JProperty("isSuccess", model.isSuccess),
            new JProperty("data", new JObject(
                    new JProperty(model.enumPos.ToString().ToLower(), JToken.FromObject(model.item))
                )
            )
        );
        return jsonObject;
    }

    public object ReturnGet(ReturnGetModel model)
    {
        JObject jsonObject = new JObject(
            new JProperty("message", model.message),
            new JProperty("isSuccess", model.isSuccess),
            new JProperty("result", model.count),
            new JProperty("data", new JObject(
                    new JProperty(model.enumPos.ToString().ToLower(), JToken.FromObject(model.item))
                )
            )
        );
        return jsonObject;
    }

    public class ReturnGetModel
    {
        public string message { get; set; }
        public int count { get; set; }
        public EnumPos enumPos { get; set; }
        public bool isSuccess { get; set; }
        public object item { get; set; }
    }

    public class ReturnByIdModel
    {
        public string message { get; set; }
        public EnumPos enumPos { get; set; }
        public bool isSuccess { get; set; }
        public object item { get; set; }
    }

    public class ReturnCommandModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public EnumPos enumPos { get; set; }
        public object? item { get; set; }
    }
}