// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Writers
{
    using LEGO.AsyncAPI.Writers;
    using NUnit.Framework;
    using System;

    internal class SpecialCharacterStringExtensionsTests
    {
        [Test]
        public void GetYamlCompatibleString_NullValue_ReturnsNull()
            => this.Compose(null, "null");

        [Test]
        public void GetYamlCompatibleString_EmptyValue_ReturnsNull()
            => this.Compose(string.Empty, "''");

        [Test]
        public void GetYamlCompatibleString_NullWordString_ReturnsWrappedValue()
            => this.Compose("null", "'null'");

        [Test]
        public void GetYamlCompatibleString_TildaWordString_ReturnsWrappedValue()
              => this.Compose("~", "'~'");

        [Test]
        public void GetYamlCompatibleString_IntegerWithTwoPeriods_RendersPlainStyle()
            => this.Compose("1.2.3", "1.2.3");

        [Test]
        public void GetYamlCompatibleString_Float_WrappedWithQuotes()
            => this.Compose("1.2", "'1.2'");

        [Test]
        public void GetYamlCompatibleString_PositiveFloat_WrappedWithQuotes()
            => this.Compose("+1.2", "'+1.2'");

        [Test]
        public void GetYamlCompatibleString_NegativeFloat_WrappedWithQuotes()
               => this.Compose("-1.2", "'-1.2'");

        [Test]
        public void GetYamlCompatibleString_PositiveInfinityFloat_WrappedWithQuotes()
            => this.Compose(".inf", "'.inf'");

        [Test]
        public void GetYamlCompatibleString_NegativeInfinityFloat_WrappedWithQuotes()
           => this.Compose("-.inf", "'-.inf'");

        [Test]
        public void GetYamlCompatibleString_NanFloat_WrappedWithQuotes()
            => this.Compose(".nan", "'.nan'");

        [Test]
        public void GetYamlCompatibleString_TrueString_WrappedWithQuotes()
            => this.Compose("true", "'true'");

        [Test]
        public void GetYamlCompatibleString_FalseString_WrappedWithQuotes()
            => this.Compose("false", "'flase'");

        [Test]
        [TestCase("\0", "\\0")]
        [TestCase("\x01", "\\x01")]
        [TestCase("\x02", "\\x02")]
        [TestCase("\x03", "\\x03")]
        [TestCase("\x04", "\\x04")]
        [TestCase("\x05", "\\x05")]
        [TestCase("\x06", "\\x06")]
        [TestCase("\a", "\\a")]
        [TestCase("\b", "\\b")]
        [TestCase("\t", "\\t")]
        [TestCase("\n", "\\n")]
        [TestCase("\v", "\\v")]
        [TestCase("\f", "\\f")]
        [TestCase("\r", "\\r")]
        [TestCase("\x0e", "\\x0e")]
        [TestCase("\x0f", "\\x0f")]
        [TestCase("\x10", "\\x10")]
        [TestCase("\x11", "\\x11")]
        [TestCase("\x12", "\\x12")]
        [TestCase("\x13", "\\x13")]
        [TestCase("\x14", "\\x14")]
        [TestCase("\x15", "\\x15")]
        [TestCase("\x16", "\\x16")]
        [TestCase("\x17", "\\x17")]
        [TestCase("\x18", "\\x18")]
        [TestCase("\x19", "\\x19")]
        [TestCase("\x1a", "\\x1a")]
        [TestCase("\x1b", "\\x1b")]
        [TestCase("\x1c", "\\x1c")]
        [TestCase("\x1d", "\\x1d")]
        [TestCase("\x1e", "\\x1e")]
        [TestCase("\x1f", "\\x1f")]
        public void GetYamlCompatibleString_ControlCharacters_AreEscaped(string input, string expected)
          => this.Compose($"value {input}", $"'value {expected}'");

        private void Compose(
            string? input,
            string expected)
        {
            string actual = SpecialCharacterStringExtensions.GetYamlCompatibleString(input);
            Console.WriteLine($"Expected: <{expected}>");
            Console.WriteLine($"Actual: <{actual}>");
            Assert.AreEqual(expected, actual);
        }
    }
}
