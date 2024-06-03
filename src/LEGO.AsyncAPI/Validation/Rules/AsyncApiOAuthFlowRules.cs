// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using System.Linq;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiOAuthFlowRules
    {
        public static ValidationRule<AsyncApiOAuthFlow> OAuthFlowRequiredFields =>
            new ValidationRule<AsyncApiOAuthFlow>(
                (context, oauthFlow) =>
                {
                    context.Enter("authorizationUrl");
                    if (oauthFlow.AuthorizationUrl == null)
                    {
                        context.CreateError(
                            nameof(OAuthFlowRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "authorizationUrl", "flow"));
                    }

                    context.Exit();

                    context.Enter("tokenUrl");
                    if (oauthFlow.TokenUrl == null)
                    {
                        context.CreateError(
                            nameof(OAuthFlowRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "tokenUrl", "flow"));
                    }

                    context.Exit();

                    context.Enter("scopes");
                    if (oauthFlow.Scopes == null || !oauthFlow.Scopes.Keys.Any())
                    {
                        context.CreateError(
                            nameof(OAuthFlowRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "scopes", "flow"));
                    }

                    context.Exit();
                });

        public static ValidationRule<AsyncApiOAuthFlow> OAuthFlowUrlMustBeAbsolute =>
           new ValidationRule<AsyncApiOAuthFlow>(
               (context, oauthFlow) =>
               {
                   context.Enter("authorizationUrl");
                   if (oauthFlow.AuthorizationUrl != null && !oauthFlow.AuthorizationUrl.IsAbsoluteUri)
                   {
                       context.CreateError(
                            nameof(OAuthFlowUrlMustBeAbsolute),
                            string.Format(Resource.Validation_MustBeAbsoluteUrl, "authorizationUrl", "flow"));
                   }

                   context.Exit();

                   context.Enter("tokenUrl");
                   if (oauthFlow.TokenUrl != null && !oauthFlow.TokenUrl.IsAbsoluteUri)
                   {
                       context.CreateError(
                            nameof(OAuthFlowUrlMustBeAbsolute),
                            string.Format(Resource.Validation_MustBeAbsoluteUrl, "tokenUrl", "flow"));
                   }

                   context.Exit();
               });
    }
}