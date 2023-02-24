// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiInfoRules
    {
        public static ValidationRule<AsyncApiInfo> InfoRequiredFields =>
            new ValidationRule<AsyncApiInfo>(
                (context, info) =>
                {
                    context.Enter("title");
                    if (info.Title == null)
                    {
                        context.CreateError(
                            nameof(InfoRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "title", "info"));
                    }

                    context.Exit();

                    context.Enter("version");
                    if (info.Version == null)
                    {
                        context.CreateError(
                            nameof(InfoRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "version", "info"));
                    }

                    context.Exit();
                });

        public static ValidationRule<AsyncApiInfo> TermsOfServiceUrlMustBeAbsolute =>
            new ValidationRule<AsyncApiInfo>(
               (context, info) =>
               {
                   context.Enter("termsOfService");
                   if (info.TermsOfService != null && !info.TermsOfService.IsAbsoluteUri)
                   {
                       context.CreateError(
                            nameof(TermsOfServiceUrlMustBeAbsolute),
                            string.Format(Resource.Validation_MustBeAbsoluteUrl, "termsOfService", "info"));

                   }

                   context.Exit();
               });
    }
}
