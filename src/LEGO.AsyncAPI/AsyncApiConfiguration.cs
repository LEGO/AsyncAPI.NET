// Copyright (c) The LEGO Group. All rights reserved.

using System;
using System.Globalization;

namespace LEGO.AsyncAPI
{
    /// <summary>
    /// Contains configuration settings for AsyncAPI used across the board.
    /// </summary>
    public static class AsyncApiConfiguration
    {
        static AsyncApiConfiguration()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";
        }

        /// <summary>
        /// Gets the format used for reading and writing date time structures.
        /// </summary>
        public static string DateTimeFormat { get; }

        /// <summary>
        /// Gets the culture info used for strings
        /// </summary>
        public static CultureInfo CultureInfo { get; }
    }
}
