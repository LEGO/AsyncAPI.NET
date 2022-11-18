namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Exceptions;

    public class AsyncApiError
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class using the message and pointer from the given exception.
        /// </summary>
        public AsyncApiError(AsyncApiException exception)
            : this(exception.Pointer, exception.Message)
        {
        }

        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class.
        /// </summary>
        public AsyncApiError(string pointer, string message)
        {
            this.Pointer = pointer;
            this.Message = message;
        }

        /// <summary>
        /// Message explaining the error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Pointer to the location of the error.
        /// </summary>
        public string Pointer { get; set; }

        /// <summary>
        /// Gets the string representation of <see cref="AsyncApiError"/>.
        /// </summary>
        public override string ToString()
        {
            return this.Message + (!string.IsNullOrEmpty(this.Pointer) ? " [" + this.Pointer + "]" : "");
        }
    }
}