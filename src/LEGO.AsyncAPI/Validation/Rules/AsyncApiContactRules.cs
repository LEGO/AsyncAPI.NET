// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiContactRules
    {
        public static ValidationRule<AsyncApiContact> EmailMustBeEmailFormat =>
            new ValidationRule<AsyncApiContact>(
                (context, contact) =>
                {
                    context.Enter("email");
                    if (contact != null && contact.Email != null)
                    {
                        if (!contact.Email.IsEmailAddress())
                        {
                            context.CreateError(
                                nameof(EmailMustBeEmailFormat),
                                string.Format(Resource.Validation_EmailMustBeEmailFormat, contact.Email));
                        }
                    }

                    context.Exit();
                });

        public static ValidationRule<AsyncApiContact> ContactUrlMustBeAbsolute =>
            new ValidationRule<AsyncApiContact>(
                (context, contact) =>
                {
                    context.Enter("url");
                    if (contact != null && contact.Url != null && !contact.Url.IsAbsoluteUri)
                    {
                            context.CreateError(
                                nameof(ContactUrlMustBeAbsolute),
                                string.Format(Resource.Validation_MustBeAbsoluteUrl, "url", "contact"));
                    }

                    context.Exit();
                });
    }
}
