namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models.Bindings.Http;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<HttpMessageBinding> httpMessageBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "headers", (a, n) => { a.Headers = LoadSchema(n); } },
        };

        private static FixedFieldMap<HttpOperationBinding> httpOperationBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "type", (a, n) => { a.Type = n.GetScalarValue(); } },
            { "method", (a, n) => { a.Method = n.GetScalarValue(); } },
            { "query", (a, n) => { a.Query = LoadSchema(n); } },
        };

    }
}
