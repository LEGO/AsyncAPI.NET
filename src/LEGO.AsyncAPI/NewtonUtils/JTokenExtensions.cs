namespace LEGO.AsyncAPI.NewtonUtils
{
    using System.Globalization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Based on https://stackoverflow.com/a/60158588/322644.
    /// </summary>
    public static class JTokenExtensions
    {
        private const string RefPropertyName = "$ref";

        /// <summary>
        /// Replace all occurrences of references in format "#/foo/../bar" with referenced objects.
        /// </summary>
        /// <param name="root">JToken object with references to be replaced.</param>
        public static void ResolveReferences(this JToken root)
        {
            if (root is not JContainer container)
            {
                return;
            }

            do
            {
                var refs = container.Descendants().OfType<JObject>().Where(IsRefObject).ToList();
                foreach (var refObj in refs)
                {
                    var path = GetRefObjectValue(refObj);
                    var original = ResolveRef(root, path);
                    if (original != null)
                    {
                        refObj.Replace(original);
                    }
                }
            }
            while (container.Descendants().OfType<JObject>().Where(IsRefObject).Any());
        }

        private static bool IsRefObject(JObject obj)
        {
            return GetRefObjectValue(obj) != null;
        }

        private static string GetRefObjectValue(JObject obj)
        {
            if (obj.Count == 1)
            {
                var refValue = obj[RefPropertyName];
                if (refValue is { Type: JTokenType.String })
                {
                    return (string)refValue;
                }
            }

            return null;
        }

        private static JToken ResolveRef(JToken token, string path)
        {
            if (!path.StartsWith("#/"))
            {
                return null;
            }

            var components = path.Replace("#/", string.Empty).Split('/');

            int count = 0;


            foreach (var component in components)
            {
                count++;

                if (token is JObject obj)
                {
                    token = obj[component];
                    if (token["$id"] != null && count == components.Length)
                    {
                        token = obj[component].DeepClone();
                        token["$id"].Parent.Remove();
                    }
                }
                else if (token is JArray)
                {
                    token = token[int.Parse(component, NumberFormatInfo.InvariantInfo)];
                }
                else
                {
                    return null;
                }
            }

            return token;
        }
    }
}
