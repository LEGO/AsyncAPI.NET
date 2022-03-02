// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Serializers
{
    using System.Text;

    internal static class StringExtensions
    {
        /// <summary>
        /// Replace all non-ascii symbols with empty string.
        /// </summary>
        /// <param name="input">String to remove non-ascii symbols from.</param>
        /// <returns>String without non-ascii symbols.</returns>
        public static string RemoveNonAsciiSymbols(this string input)
        {
            return Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()),
                    Encoding.UTF8.GetBytes(input)));
        }
    }
}
