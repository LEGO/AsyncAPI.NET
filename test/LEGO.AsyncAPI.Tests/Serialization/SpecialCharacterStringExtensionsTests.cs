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
            => this.Compose("false", "'false'");

        [Test]
        public void GetYamlCompatibleString_DateTimeSlashString_WrappedWithQuotes()
            => this.Compose("12/31/2022 23:59:59", "12/31/2022 23:59:59");

        [Test]
        public void GetYamlCompatibleString_DateTimeDashString_WrappedWithQuotes()
            => this.Compose("2022-12-31 23:59:59", "2022-12-31 23:59:59");

        [Test]
        public void GetYamlCompatibleString_DateTimeISOString_NotWrappedWithQuotes()
            => this.Compose("2022-12-31T23:59:59Z", "2022-12-31T23:59:59Z");

        [Test]
        public void GetYamlCompatibleString_DateTimeCanonicalString_NotWrappedWithQuotes()
            => this.Compose("2001-12-15T02:59:43.1Z", "2001-12-15T02:59:43.1Z");

        [Test]
        public void GetYamlCompatibleString_DateTimeSpacedString_NotWrappedWithQuotes()
            => this.Compose("2001-12-14 21:59:43.10 -5", "2001-12-14 21:59:43.10 -5");

        [Test]
        public void GetYamlCompatibleString_DateString_NotWrappedWithQuotes()
            => this.Compose("2002-12-14", "2002-12-14");

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
          => this.Compose($"value {input}", $"\"value {expected}\"");

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
