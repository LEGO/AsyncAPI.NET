// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Readers
{
    using System;
    using System.Reflection;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;
    using YamlDotNet.RepresentationModel;

    internal class YamlConverterTests
    {
        private static readonly MethodInfo GenericGetValueMethodInfo;

        static YamlConverterTests()
        {
            GenericGetValueMethodInfo = typeof(JsonValue)
                .GetMethod("GetValue", BindingFlags.Public | BindingFlags.Instance)!;
        }

        [Test]
        public void ToJsonValue_PlainString_CanGetStringValue()
            => ComposeJsonValue(
                    input: "hello world",
                    assertValueType: () => typeof(string));

        [Test]
        [TestCase("true")]
        [TestCase("false")]
        public void ToJsonValue_PlainBoolean_CanGetBoolValue(string input)
            => ComposeJsonValue(
                input: input,
                assertValueType: () => typeof(bool));

        [Test]
        [TestCase("2022-12-31")] // Canonical
        [TestCase("2022-12-31T18:59:59-05:00")] // ISO 8601 
        [TestCase("2001-12-14 21:59:43.10 -5")] // Spaced
        public void ToJsonValue_PlainDateTime_CanGetDateTimeValue(string input)
            => ComposeJsonValue(
                input: input,
                assertValueType: () => typeof(DateTime));

        [Test]
        public void ToJsonValue_PlainInt_CanGetIntValue()
            => ComposeJsonValue(
                input: "2022",
                assertValueType: () => typeof(int));

        [Test]
        public void ToJsonValue_PlainDouble_CanGetDoubleValue()
            => ComposeJsonValue(
                input: "2022.20",
                assertValueType: () => typeof(double));

        [Test]
        public void GetScalarValue_PlainString_MatchesExpectedScalerValue()
            => ComposeJsonValue(
                    input: "hello world",
                    assertScalerValue: () => "hello world");

        [Test]
        [TestCase("true")]
        [TestCase("false")]
        public void GetScalarValue_PlainBoolean_MatchesExpectedScalerValue(string input)
            => ComposeJsonValue(
                input: input,
                assertScalerValue: () => input);

        [Test]
        public void GetScalarValue_PlainDateTime_MatchesExpectedScalerValue()
            => ComposeJsonValue(
                input: "2022-12-31T18:59:59-05:00",
                assertScalerValue: () => "2022-12-31T18:59:59-05:00");

        [Test]
        public void GetScalarValue_PlainInt_MatchesExpectedScalerValue()
            => ComposeJsonValue(
                input: "2022",
                assertScalerValue: () => "2022");

        [Test]
        public void GetScalarValue_PlainDouble_MatchesExpectedScalerValue()
            => ComposeJsonValue(
                input: "2022.20",
                assertScalerValue: () => "2022.2"); // extra zero dropped

        private void ComposeJsonValue(
            string input,
            Func<Type>? assertValueType = null,
            Func<string>? assertScalerValue = null)
        {
            YamlScalarNode node = new YamlScalarNode(input)
            {
                Style = YamlDotNet.Core.ScalarStyle.Plain,
            };

            JsonValue jValue = node.ToJsonValue();

            jValue.GetScalarValue();

            if (assertValueType != null)
            {
                Type valueType = assertValueType();
                MethodInfo genericGetValueMethod = GenericGetValueMethodInfo.MakeGenericMethod(valueType);
                object? result = genericGetValueMethod.Invoke(jValue, null);
                Assert.IsNotNull(result);
                Assert.IsInstanceOf(valueType, result);
            }

            if (assertScalerValue != null)
            {
                string expectedValue = assertScalerValue();
                string actualValue = jValue.GetScalarValue();
                Assert.AreEqual(expectedValue, actualValue);
            }
        }
    }
}
