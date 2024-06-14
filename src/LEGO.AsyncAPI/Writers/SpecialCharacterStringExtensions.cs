// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    public static class SpecialCharacterStringExtensions
    {
        /// <summary>
        /// Handles control characters and backslashes and adds double quotes
        /// to get JSON-compatible string.
        /// </summary>
        internal static string GetJsonCompatibleString(this string value)
        {
            if (value == null)
            {
                return "null";
            }

            // Show the control characters as strings
            // http://json.org/

            // Replace the backslash first, so that the new backslashes created by other Replaces are not duplicated.
            value = value.Replace("\\", "\\\\");

            value = value.Replace("\b", "\\b");
            value = value.Replace("\f", "\\f");
            value = value.Replace("\n", "\\n");
            value = value.Replace("\r", "\\r");
            value = value.Replace("\t", "\\t");
            value = value.Replace("\"", "\\\"");

            return $"\"{value}\"";
        }
    }
}
