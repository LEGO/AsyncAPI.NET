namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiServerRules
    {
        public static ValidationRule<AsyncApiServer> ServerRequiredFields =>
            new ValidationRule<AsyncApiServer>(
                (context, server) =>
                {
                    context.Enter("url");
                    if (server.Url == null)
                    {
                        context.CreateError(
                            nameof(ServerRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "url", "server"));
                    }

                    context.Exit();

                    context.Enter("protocol");
                    if (server.Protocol == null)
                    {
                        context.CreateError(
                            nameof(ServerRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "protocol", "server"));
                    }

                    context.Exit();
                });
    }
}