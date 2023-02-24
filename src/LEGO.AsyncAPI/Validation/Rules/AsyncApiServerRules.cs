namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    internal static class AsyncApiServerRules
    {
        public static ValidationRule<AsyncApiServer> AsyncApiServerRequiredFields =>
            new ValidationRule<AsyncApiServer>(
                (context, server) =>
                {
                    // info
                    context.Enter("url");
                    if (server.Url == null)
                    {
                        context.CreateError(
                            nameof(AsyncApiServerRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "url", "server"));
                    }

                    context.Exit();

                    // channels
                    context.Enter("protocol");
                    if (server.Protocol == null)
                    {
                        context.CreateError(
                            nameof(AsyncApiServerRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "protocol", "server"));
                    }

                    context.Exit();
                });
    }
}