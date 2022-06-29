// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Services
{
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;

    internal class AsyncApiReferenceError : AsyncApiError
    {
        public AsyncApiReferenceError(AsyncApiException exception)
            : base(exception)
        {
        }
    }
}