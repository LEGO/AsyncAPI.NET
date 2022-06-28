using LEGO.AsyncAPI.Exceptions;
using LEGO.AsyncAPI.Models;

namespace LEGO.AsyncAPI.Services
{
    internal class AsyncApiReferenceError : AsyncApiError
    {
        public AsyncApiReferenceError(AsyncApiException exception) : base(exception)
        {
        }
    }
}