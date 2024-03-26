// Copyright (c) The LEGO Group. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LEGO.AsyncAPI.Writers
{
    /// <summary>
    /// Used to conver an AsyncApi schema into a yaml document.
    /// </summary>
    public class AsyncApiYamlWriter : AsyncApiWriterBase
    {
        private static readonly Regex YamlNumberRegex;
        private static readonly char[] YamlIndicators;
        private static readonly string[] YamlPlainStringForbiddenCobinations;
        private static readonly string[] YamlPlainStringForbiddenTerminals;
        private static readonly char[] YamlControlCharacters;

        static AsyncApiYamlWriter()
        {
            YamlNumberRegex = new Regex("^[+-]?[0-9]*\\.?[0-9]*$", RegexOptions.Compiled);
            YamlIndicators = new char[] { '-', '?', ':', ',', '{', '}', '[', ']', '&', '*', '#', '?', '|', '-', '>', '!', '%', '@', '`', '\'', '"', };
            YamlPlainStringForbiddenCobinations = new string[] { ": ", " #", "[", "]", "{", "}", ",", };
            YamlPlainStringForbiddenTerminals = new string[] { ":" };
            YamlControlCharacters = new char[] { '\0', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x0e', '\x0f', '\x10', '\x11', '\x12', '\x13', '\x14', '\x15', '\x16', '\x17', '\x18', '\x19', '\x1a', '\x1b', '\x1c', '\x1d', '\x1e', '\x1f', };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiYamlWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        [Obsolete($"Please use overridden constructor that takes in a {nameof(AsyncApiWriterSettings)} instance.")]
        public AsyncApiYamlWriter(TextWriter textWriter)
            : this(textWriter, new AsyncApiWriterSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiYamlWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="settings">The settings used to read and write yaml</param>
        public AsyncApiYamlWriter(TextWriter textWriter, AsyncApiWriterSettings settings)
            : base(textWriter, settings)
        {
        }

        /// <summary>
        /// Allow rendering of multi-line strings using YAML | syntax.
        /// </summary>
        public bool UseLiteralStyle { get; set; }

        /// <summary>
        /// Base Indentation Level.
        /// This denotes how many indentations are needed for the property in the base object.
        /// </summary>
        protected override int BaseIndentation => 0;

        /// <summary>
        /// Write YAML start object.
        /// </summary>
        public override void WriteStartObject()
        {
            var previousScope = this.CurrentScope();

            var currentScope = this.StartScope(ScopeType.Object);

            if (previousScope != null && previousScope.Type == ScopeType.Array)
            {
                currentScope.IsInArray = true;

                this.Writer.WriteLine();

                this.WriteIndentation();

                this.Writer.Write(WriterConstants.PrefixOfArrayItem);
            }

            this.IncreaseIndentation();
        }

        /// <summary>
        /// Write YAML end object.
        /// </summary>
        public override void WriteEndObject()
        {
            var previousScope = this.EndScope(ScopeType.Object);
            this.DecreaseIndentation();

            var currentScope = this.CurrentScope();

            // If the object is empty, indicate it by writing { }
            if (previousScope.ObjectCount == 0)
            {
                // If we are in an object, write a white space preceding the braces.
                if (currentScope != null && currentScope.Type == ScopeType.Object)
                {
                    this.Writer.Write(" ");
                }

                this.Writer.Write(WriterConstants.EmptyObject);
            }
        }

        /// <summary>
        /// Write YAML start array.
        /// </summary>
        public override void WriteStartArray()
        {
            var previousScope = this.CurrentScope();

            var currentScope = this.StartScope(ScopeType.Array);

            if (previousScope != null && previousScope.Type == ScopeType.Array)
            {
                currentScope.IsInArray = true;

                this.Writer.WriteLine();

                this.WriteIndentation();

                this.Writer.Write(WriterConstants.PrefixOfArrayItem);
            }

            this.IncreaseIndentation();
        }

        /// <summary>
        /// Write YAML end array.
        /// </summary>
        public override void WriteEndArray()
        {
            var previousScope = this.EndScope(ScopeType.Array);
            this.DecreaseIndentation();

            var currentScope = this.CurrentScope();

            // If the array is empty, indicate it by writing [ ]
            if (previousScope.ObjectCount == 0)
            {
                // If we are in an object, write a white space preceding the braces.
                if (currentScope != null && currentScope.Type == ScopeType.Object)
                {
                    this.Writer.Write(" ");
                }

                this.Writer.Write(WriterConstants.EmptyArray);
            }
        }

        /// <summary>
        /// Write the property name and the delimiter.
        /// </summary>
        public override void WritePropertyName(string name)
        {
            this.VerifyCanWritePropertyName(name);

            var currentScope = this.CurrentScope();

            // If this is NOT the first property in the object, always start a new line and add indentation.
            if (currentScope.ObjectCount != 0)
            {
                this.Writer.WriteLine();
                this.WriteIndentation();
            }

            // Only add newline and indentation when this object is not in the top level scope and not in an array.
            // The top level scope should have no indentation and it is already in its own line.
            // The first property of an object inside array can go after the array prefix (-) directly.
            else if (!this.IsTopLevelScope() && !currentScope.IsInArray)
            {
                this.Writer.WriteLine();
                this.WriteIndentation();
            }

            name = this.GetYamlCompatibleString(name);

            this.Writer.Write(name);
            this.Writer.Write(":");

            currentScope.ObjectCount++;
        }

        /// <summary>
        /// Write string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public override void WriteValue(string value)
        {
            if (!this.UseLiteralStyle || value.IndexOfAny(new[] { '\n', '\r' }) == -1)
            {
                this.WriteValueSeparator();

                value = this.GetYamlCompatibleString(value);

                this.Writer.Write(value);
            }
            else
            {
                if (this.CurrentScope() != null)
                {
                    this.WriteValueSeparator();
                }

                this.Writer.Write("|");

                this.WriteChompingIndicator(value);

                // Write indentation indicator when it starts with spaces
                if (value.StartsWith(" "))
                {
                    this.Writer.Write(IndentationString.Length);
                }

                this.Writer.WriteLine();

                this.IncreaseIndentation();

                using (StringReader reader = new(value))
                {
                    bool firstLine = true;
                    while (reader.ReadLine() is var line && line != null)
                    {
                        if (firstLine)
                        {
                            firstLine = false;
                        }
                        else
                        {
                            this.Writer.WriteLine();
                        }

                        // Indentations for empty lines aren't needed.
                        if (line.Length > 0)
                        {
                            this.WriteIndentation();
                        }

                        this.Writer.Write(line);
                    }
                }

                this.DecreaseIndentation();
            }
        }

        private void WriteChompingIndicator(string value)
        {
            var trailingNewlines = 0;
            var end = value.Length - 1;

            // We only need to know whether there are 0, 1, or more trailing newlines
            while (end >= 0 && trailingNewlines < 2)
            {
                var found = value.LastIndexOfAny(new[] { '\n', '\r' }, end, 2);
                if (found == -1 || found != end)
                {
                    // does not ends with newline
                    break;
                }

                if (value[end] == '\r')
                {
                    // ends with \r
                    end--;
                }
                else if (end > 0 && value[end - 1] == '\r')
                {
                    // ends with \r\n
                    end -= 2;
                }
                else
                {
                    // ends with \n
                    end -= 1;
                }

                trailingNewlines++;
            }

            switch (trailingNewlines)
            {
                case 0:
                    // "strip" chomping indicator
                    this.Writer.Write("-");
                    break;
                case 1:
                    // "clip"
                    break;
                default:
                    // "keep" chomping indicator
                    this.Writer.Write("+");
                    break;
            }
        }

        /// <summary>
        /// Write null value.
        /// </summary>
        public override void WriteNull()
        {
            // YAML allows null value to be represented by either nothing or the word null.
            // We will write nothing here.
            this.WriteValueSeparator();
        }

        /// <summary>
        /// Write value separator.
        /// </summary>
        protected override void WriteValueSeparator()
        {
            if (this.IsArrayScope())
            {
                // If array is the outermost scope and this is the first item, there is no need to insert a newline.
                if (!this.IsTopLevelScope() || this.CurrentScope().ObjectCount != 0)
                {
                    this.Writer.WriteLine();
                }

                this.WriteIndentation();
                this.Writer.Write(WriterConstants.PrefixOfArrayItem);

                this.CurrentScope().ObjectCount++;
            }
            else
            {
                this.Writer.Write(" ");
            }
        }

        /// <summary>
        /// Writes the content raw value.
        /// </summary>
        public override void WriteRaw(string value)
        {
            this.WriteValueSeparator();
            this.Writer.Write(value);
        }

        /// <summary>
        /// Escapes all special characters and put the string in quotes if necessary to
        /// get a YAML-compatible string.
        /// </summary>
        /// <param name="input">The string to turn into yaml.</param>
        /// <returns>The string as yaml.</returns>
        internal string GetYamlCompatibleString(string input)
        {
            if (input == null)
            {
                return "null";
            }

            switch (input.ToLower())
            {
                case "":
                    return "''";

                case "~":
                // Example 2.20. Floating Point
                case "-.inf":
                case ".inf":
                case ".nan":
                // Example 2.21. Miscellaneous
                case "null":

                // Booleans
                case "true":
                case "false":
                    return $"'{input}'";
            }

            // If string includes a control character, wrapping in double quote is required.
            if (input.Any(c => YamlControlCharacters.Contains(c)))
            {
                // Replace the backslash first, so that the new backslashes created by other Replaces are not duplicated.
                input = input.Replace("\\", "\\\\");

                // Escape the double quotes.
                input = input.Replace("\"", "\\\"");

                // Escape all the control characters.
                input = input.Replace("\0", "\\0");
                input = input.Replace("\x01", "\\x01");
                input = input.Replace("\x02", "\\x02");
                input = input.Replace("\x03", "\\x03");
                input = input.Replace("\x04", "\\x04");
                input = input.Replace("\x05", "\\x05");
                input = input.Replace("\x06", "\\x06");
                input = input.Replace("\a", "\\a");
                input = input.Replace("\b", "\\b");
                input = input.Replace("\t", "\\t");
                input = input.Replace("\n", "\\n");
                input = input.Replace("\v", "\\v");
                input = input.Replace("\f", "\\f");
                input = input.Replace("\r", "\\r");
                input = input.Replace("\x0e", "\\x0e");
                input = input.Replace("\x0f", "\\x0f");
                input = input.Replace("\x10", "\\x10");
                input = input.Replace("\x11", "\\x11");
                input = input.Replace("\x12", "\\x12");
                input = input.Replace("\x13", "\\x13");
                input = input.Replace("\x14", "\\x14");
                input = input.Replace("\x15", "\\x15");
                input = input.Replace("\x16", "\\x16");
                input = input.Replace("\x17", "\\x17");
                input = input.Replace("\x18", "\\x18");
                input = input.Replace("\x19", "\\x19");
                input = input.Replace("\x1a", "\\x1a");
                input = input.Replace("\x1b", "\\x1b");
                input = input.Replace("\x1c", "\\x1c");
                input = input.Replace("\x1d", "\\x1d");
                input = input.Replace("\x1e", "\\x1e");
                input = input.Replace("\x1f", "\\x1f");

                return $"\"{input}\"";
            }

            // If string
            // 1) includes a character forbidden in plain string,
            // 2) starts with an indicator, OR
            // 3) has trailing/leading white spaces,
            // wrap the string in single quote.
            // http://www.yaml.org/spec/1.2/spec.html#style/flow/plain
            if (YamlPlainStringForbiddenCobinations.Any(fc => input.Contains(fc)) ||
                YamlIndicators.Any(i => input.StartsWith(i.ToString())) ||
                YamlPlainStringForbiddenTerminals.Any(i => input.EndsWith(i.ToString())) ||
                input.Trim() != input)
            {
                // Escape single quotes with two single quotes.
                input = input.Replace("'", "''");

                return $"'{input}'";
            }

            // If string can be mistaken as a number, a boolean, or a timestamp,
            // wrap it in quot number, a boolean, or a timestamp
            if (decimal.TryParse(input, NumberStyles.Float, this.Settings.CultureInfo, out decimal _) ||
                bool.TryParse(input, out bool _) ||
                DateTime.TryParseExact(input, this.Settings.DateTimeFormat, this.Settings.CultureInfo, DateTimeStyles.RoundtripKind, out DateTime _))
            {
                return $"'{input}'";
            }

            // Handle numbers
            return YamlNumberRegex.IsMatch(input) ? $"'{input}'" : input;
        }
    }
}
