namespace LEGO.AsyncAPI.Surface.Stubs
{
    using Json.Schema;
    using Newtonsoft.Json.Linq;

    public interface IAsyncApiSchemaValidator
    {
        Task<ValidatorResult?> ValidateAsync(JObject jObject, CancellationToken cancellationToken);
    }
}