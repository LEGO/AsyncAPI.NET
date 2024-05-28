// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using FluentAssertions;
    using FluentAssertions.Primitives;
    using NUnit.Framework;

    /// <summary>
    /// Contains extension methods for working with fluent assertions.
    /// </summary>
    internal static class FluentAssertionExtensions
    {
        private static readonly char[] SeperatorChars;

        static FluentAssertionExtensions()
        {
            SeperatorChars = new[]
            {
                '\r',
                '\n',
            };
        }

        /// <summary>
        /// Checks if the string is equal to other be ingores platform spesefic features
        /// line new line breaks. This also checks to validate strings that are multiple lines
        /// are the same number of lines.
        /// </summary>
        /// <param name="assertions">The assertion object.</param>
        /// <param name="input">The actaul value.</param>
        public static void BePlatformAgnosticEquivalentTo(
            this StringAssertions assertions,
            string input)
        {
            TestContext context = TestContext.CurrentContext;
            StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries;
            string[] expected = assertions.Subject.Split(SeperatorChars, splitOptions);
            string[] actual = input.Split(SeperatorChars, splitOptions);

            // So we don't go out of range
            int minLength = Math.Min(expected.Length, actual.Length);
            const int previewSize = 3;

            for (int i = 0; i < minLength; i++)
            {
                string actaulLine = actual[i];
                string expectedLine = expected[i];

                if (!string.Equals(actaulLine, expectedLine))
                {
                    TestContext.WriteLine($"The line {i} does not match");
                    TestContext.WriteLine("-----------------");

                    // Show lines above
                    for (int x = previewSize - 1; x >= 1; x--)
                    {
                        int index = i - x;
                        if (index >= 0)
                        {
                            TestContext.WriteLine($"  {index:00}|{actual[index]}");
                        }
                    }

                    TestContext.WriteLine($"- {i:00}|{expectedLine}");
                    TestContext.WriteLine($"+ {i:00}|{actaulLine}");

                    for (int x = 1; x < previewSize + 1; x++)
                    {
                        int index = i + x;
                        if (index < actual.Length)
                        {
                            TestContext.WriteLine($"  {index:00}|{actual[index]}");
                        }
                        else
                        {
                            TestContext.WriteLine("\\ end of file \\");
                            break;
                        }
                    }

                    Assert.Fail();
                }
            }

            actual.Length.Should().Be(expected.Length);
        }
    }
}
