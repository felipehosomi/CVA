using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Helpers
{
    public static class JsonHelper
    {
        public static string SerializeToMinimalJson(object obj, bool sendDefaultValues)
        {
            JsonSerializer serializer;

            if (sendDefaultValues)
                serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };
            else
                serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };
            return JToken.FromObject(obj, serializer).RemoveEmptyChildren().ToString();
        }

        public static JToken RemoveEmptyChildren(this JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                JObject copy = new JObject();
                foreach (JProperty prop in token.Children<JProperty>())
                {
                    JToken child = prop.Value;
                    if (child.HasValues)
                    {
                        child = child.RemoveEmptyChildren();
                    }
                    if (!child.IsEmptyOrDefault())
                    {
                        copy.Add(prop.Name, child);
                    }
                }
                return copy;
            }
            else if (token.Type == JTokenType.Array)
            {
                JArray copy = new JArray();
                foreach (JToken item in token.Children())
                {
                    JToken child = item;
                    if (child.HasValues)
                    {
                        child = child.RemoveEmptyChildren();
                    }
                    if (!child.IsEmptyOrDefault())
                    {
                        copy.Add(child);
                    }
                }
                return copy;
            }
            return token;
        }

        public static bool IsEmptyOrDefault(this JToken token)
        {
            return (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues);
        }


        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static object Deserialize(string obj)
        {
            return JsonConvert.DeserializeObject(obj);
        }

    }
}
