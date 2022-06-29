// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System.IO;

    public class AsyncApiYamlWriter : AsyncApiWriterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiYamlWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        public AsyncApiYamlWriter(TextWriter textWriter)
            : this(textWriter, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiYamlWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="settings"></param>
        public AsyncApiYamlWriter(TextWriter textWriter, AsyncApiWriterSettings settings)
            : base(textWriter, settings)
        {
        }

        /// <summary>
        /// Allow rendering of multi-line strings using YAML | syntax
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

            name = name.GetYamlCompatibleString();

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

                value = value.GetYamlCompatibleString();

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

                using (var reader = new StringReader(value))
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
    }
}
