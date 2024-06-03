// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using NUnit.Framework;

    /// <summary>
    /// Base class for unit tests across the project. Can contain
    /// helper methods for working with unit tests.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase"/> class.
        /// </summary>
        protected TestBase()
        {
            this.TestContext = TestContext.CurrentContext;
        }

        /// <summary>
        /// Gets the current context of the running text.
        /// </summary>
        protected TestContext TestContext { get; }

        /// <summary>
        /// Writes information to the console which will only be
        /// printed when running in debug mode.
        /// </summary>
        /// <param name="message">The message to print.</param>
        [Conditional("DEBUG")]
        public void Log(string message)
        {
            TestContext.WriteLine(message);
        }

        /// <summary>
        /// Attempts to find the first file that matches the name of the active unit test
        /// and returns it as an expected type.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="resourceName">The name of the resource file with an optional extension.</param>
        /// <returns>The result.</returns>
        protected T GetTestData<T>([CallerMemberName] string resourceName = "")
        {
            string searchPattern = string.IsNullOrWhiteSpace(Path.GetExtension(resourceName))
                ? $"{resourceName}.*"
                : resourceName;

            string testDataDirectory = Path.Combine(Environment.CurrentDirectory, "TestData");

            string? testDataPath = Directory.GetFiles(testDataDirectory, searchPattern)
                .FirstOrDefault();

            Assume.That(File.Exists(testDataPath), $"No test data file named '{resourceName}' exists in directory '{testDataDirectory}'");

            object? result = null;
            Type resultType = typeof(T);

            if (typeof(string) == resultType)
            {
                result = File.ReadAllText(testDataPath);
            }
            else if (typeof(string[]) == resultType)
            {
                result = File.ReadAllLines(testDataPath);
            }
            else
            {
                throw new NotImplementedException($"No case has been defined to convering a resource into '{resultType.FullName}'. You can add a new one.");
            }

            return (T)result!;
        }
    }
}
