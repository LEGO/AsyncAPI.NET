// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;

    public static class StringExtensions
    {
        public static string MakeLineBreaksEnvironmentNeutral(this string input)
        {
            return input.Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", Environment.NewLine);
        }
    }
}
