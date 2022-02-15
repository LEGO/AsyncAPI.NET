namespace LEGO.AsyncAPI.Surface.Stubs
{
    using System.Text.Json;
    using Json.Schema;
    using Newtonsoft.Json.Linq;

    public interface IAsyncApiSchemaValidator
    {
        Task<ValidationResults> ValidateAsync(JObject jsonDocument, CancellationToken cancellationToken);
    }
}