// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System.Globalization;

    /// <summary>
    /// Contains configuration settings for AsyncAPI used across the board.
    /// </summary>
    public static class Configuration
    {
        static Configuration()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";
            CultureInfo = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Gets the format used for reading and writing date time structures.
        /// </summary>
        public static string DateTimeFormat { get; }

        /// <summary>
        /// Gets the culture info used for strings.
        /// </summary>
        public static CultureInfo CultureInfo { get; }
    }
}
