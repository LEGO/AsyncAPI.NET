using LEGO.AsyncAPI.Exceptions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Extensions
{
    /// <summary>
    /// Extension methods to verify validatity and add an extension to Extensions property.
    /// </summary>
    public static class AsyncApiExtensibleExtensions
    {
        /// <summary>
        /// Add extension into the Extensions
        /// </summary>
        /// <typeparam name="T"><see cref="IAsyncApiExtensible"/>.</typeparam>
        /// <param name="element">The extensible AsyncApi element. </param>
        /// <param name="name">The extension name.</param>
        /// <param name="any">The extension value.</param>
        public static void AddExtension<T>(this T element, string name, IAsyncApiExtension any)
            where T : IAsyncApiExtensible
        {
            if (element == null)
            {
                throw Error.ArgumentNull(nameof(element));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw Error.ArgumentNullOrWhiteSpace(nameof(name));
            }

            if (!name.StartsWith(AsyncApiConstants.ExtensionFieldNamePrefix))
            {
                throw new AsyncApiException(string.Format("The filed name '{0}' of extension doesn't begin with x-.", name));
            }

            element.Extensions[name] = any ?? throw Error.ArgumentNull(nameof(any));
        }
    }
}