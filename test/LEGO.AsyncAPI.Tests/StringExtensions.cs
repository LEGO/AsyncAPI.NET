// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.IO;

    public static class StringExtensions
    {
        public static Stream ToStream(this string input)
        {
            Stream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream, leaveOpen: true))
            {
                writer.Write(input);
            }

            stream.Position = 0;
            return stream;
        }

        public static string MakeLineBreaksEnvironmentNeutral(this string input)
        {
            return input.Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", Environment.NewLine);
        }
    }
}
