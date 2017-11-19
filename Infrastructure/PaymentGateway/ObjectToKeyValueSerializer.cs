using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace PaymentGateway
{
    public static class ObjectToKeyValueSerializer
    {
        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            while (true)
            {
                if (metaToken == null)
                {
                    return null;
                }

                if (!(metaToken is JToken token))
                {
                    metaToken = JObject.FromObject(metaToken);
                    continue;
                }

                if (token.HasValues)
                {
                    var contentData = new Dictionary<string, string>();

                    return token.Children()
                        .ToList()
                        .Select(child => child.ToKeyValue())
                        .Where(childContent => childContent != null)
                        .Aggregate(contentData, (current, childContent) => current.Concat(childContent)
                            .ToDictionary(k => k.Key, v => v.Value));
                }

                var jValue = token as JValue;
                if (jValue?.Value == null)
                {
                    return null;
                }

                var value = jValue?.Type == JTokenType.Date ?
                    jValue?.ToString("o", CultureInfo.InvariantCulture) : 
                    jValue?.ToString(CultureInfo.InvariantCulture);

                return new Dictionary<string, string> {{token.Path, value}};
                break;
            }
        }
    }
}