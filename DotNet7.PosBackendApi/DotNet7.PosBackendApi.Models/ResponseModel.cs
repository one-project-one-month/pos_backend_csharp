using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNet7.PosBackendApi.Models
{
    public class ResponseModel
    {
        public object ReturnGet(int count, object lst)
        {
            var model = new
            {
                message = "Success",
                result = count,
                data = new
                {
                    product = lst
                }
            };
            return model;
        }
        public object ReturnById(object item)
        {
            var model = new
            {
                message = "Success",
                data = new
                {
                    product = item
                }
            };
            return model;
        }
        public object ReturnCommand(bool isSuccess, string message,
            object? item = null, JObject jObject = null)
        {
            JProperty parentProp = (JProperty)jObject.First;
            string name = parentProp.Name;
            var model = new
            {
                message = message,
                isSuccess = isSuccess,
                data = item is null ? item : new
                {
                    product = item
                }
            };
            return model;
        }

        public object ReturnCommandV1(bool isSuccess, string message,
            string productCategory,object? item = null)
        {
            JObject jsonObject = new JObject(
                new JProperty("message", message),
                new JProperty("isSuccess", isSuccess),
                new JProperty("data", item is null ? item :
                    new JObject(
                        new JProperty(productCategory, JToken.FromObject(item))
                    )
                )
            );
            return jsonObject;
        }
    }
}
