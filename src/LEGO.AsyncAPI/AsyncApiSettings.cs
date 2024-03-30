// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System.Globalization;

    /// <summary>
    /// Base class for setting common acorss the various projects in the solution.
    /// </summary>
    public abstract class AsyncApiSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiSettings"/> class.
        /// </summary>
        protected AsyncApiSettings()
        {
            this.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";
            this.CultureInfo = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets the format used for reading and writing date time structures.
        /// </summary>
        public string DateTimeFormat { get; }

        /// <summary>
        /// Gets the culture info used for strings.
        /// </summary>
        public CultureInfo CultureInfo { get; }
    }
}
