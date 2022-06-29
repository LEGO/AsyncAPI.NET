// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System.IO;

    public class AsyncApiJsonWriter : AsyncApiWriterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiJsonWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        public AsyncApiJsonWriter(TextWriter textWriter)
            : base(textWriter, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiJsonWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="settings">Settings for controlling how the AsyncApi document will be written out.</param>
        public AsyncApiJsonWriter(TextWriter textWriter, AsyncJsonWriterSettings settings)
            : base(textWriter, settings)
        {
            this.produceTerseOutput = settings.Terse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiJsonWriter"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="settings">Settings for controlling how the AsyncApi document will be written out.</param>
        /// <param name="terseOutput"> Setting for allowing the JSON emitted to be in terse format.</param>
        public AsyncApiJsonWriter(TextWriter textWriter, AsyncApiWriterSettings settings, bool terseOutput = false)
            : base(textWriter, settings)
        {
            this.produceTerseOutput = terseOutput;
        }

        /// <summary>
        /// Indicates whether or not the produced document will be written in a compact or pretty fashion.
        /// </summary>
        private readonly bool produceTerseOutput = false;

        /// <summary>
        /// Base Indentation Level.
        /// This denotes how many indentations are needed for the property in the base object.
        /// </summary>
        protected override int BaseIndentation => 1;

        /// <summary>
        /// Write JSON start object.
        /// </summary>
        public override void WriteStartObject()
        {
            var previousScope = this.CurrentScope();

            var currentScope = this.StartScope(ScopeType.Object);

            if (previousScope != null && previousScope.Type == ScopeType.Array)
            {
                currentScope.IsInArray = true;

                if (previousScope.ObjectCount != 1)
                {
                    this.Writer.Write(WriterConstants.ArrayElementSeparator);
                }

                this.WriteLine();
                this.WriteIndentation();
            }

            this.Writer.Write(WriterConstants.StartObjectScope);

            this.IncreaseIndentation();
        }

        /// <summary>
        /// Write JSON end object.
        /// </summary>
        public override void WriteEndObject()
        {
            var currentScope = this.EndScope(ScopeType.Object);
            if (currentScope.ObjectCount != 0)
            {
                this.WriteLine();
                this.DecreaseIndentation();
                this.WriteIndentation();
            }
            else
            {
                if (!this.produceTerseOutput)
                {
                    this.Writer.Write(WriterConstants.WhiteSpaceForEmptyObject);
                }

                this.DecreaseIndentation();
            }

            this.Writer.Write(WriterConstants.EndObjectScope);
        }

        /// <summary>
        /// Write JSON start array.
        /// </summary>
        public override void WriteStartArray()
        {
            var previousScope = this.CurrentScope();

            var currentScope = this.StartScope(ScopeType.Array);

            if (previousScope != null && previousScope.Type == ScopeType.Array)
            {
                currentScope.IsInArray = true;

                if (previousScope.ObjectCount != 1)
                {
                    this.Writer.Write(WriterConstants.ArrayElementSeparator);
                }

                this.WriteLine();
                this.WriteIndentation();
            }

            this.Writer.Write(WriterConstants.StartArrayScope);
            this.IncreaseIndentation();
        }

        /// <summary>
        /// Write JSON end array.
        /// </summary>
        public override void WriteEndArray()
        {
            var current = this.EndScope(ScopeType.Array);
            if (current.ObjectCount != 0)
            {
                this.WriteLine();
                this.DecreaseIndentation();
                this.WriteIndentation();
            }
            else
            {
                this.Writer.Write(WriterConstants.WhiteSpaceForEmptyArray);
                this.DecreaseIndentation();
            }

            this.Writer.Write(WriterConstants.EndArrayScope);
        }

        /// <summary>
        /// Write property name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// public override void WritePropertyName(string name)
        public override void WritePropertyName(string name)
        {
            this.VerifyCanWritePropertyName(name);

            var currentScope = this.CurrentScope();
            if (currentScope.ObjectCount != 0)
            {
                this.Writer.Write(WriterConstants.ObjectMemberSeparator);
            }

            this.WriteLine();

            currentScope.ObjectCount++;

            this.WriteIndentation();

            name = name.GetJsonCompatibleString();

            this.Writer.Write(name);

            this.Writer.Write(WriterConstants.NameValueSeparator);

            if (!this.produceTerseOutput)
            {
                this.Writer.Write(WriterConstants.NameValueSeparatorWhiteSpaceSuffix);
            }
        }

        /// <summary>
        /// Write string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public override void WriteValue(string value)
        {
            this.WriteValueSeparator();

            value = value.GetJsonCompatibleString();

            this.Writer.Write(value);
        }

        /// <summary>
        /// Write null value.
        /// </summary>
        public override void WriteNull()
        {
            this.WriteValueSeparator();

            this.Writer.Write("null");
        }

        /// <summary>
        /// Writes a separator of a value if it's needed for the next value to be written.
        /// </summary>
        protected override void WriteValueSeparator()
        {
            if (this.Scopes.Count == 0)
            {
                return;
            }

            var currentScope = this.Scopes.Peek();

            if (currentScope.Type == ScopeType.Array)
            {
                if (currentScope.ObjectCount != 0)
                {
                    this.Writer.Write(WriterConstants.ArrayElementSeparator);
                }

                this.WriteLine();
                this.WriteIndentation();
                currentScope.ObjectCount++;
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
        /// Write the indentation.
        /// </summary>
        public override void WriteIndentation()
        {
            if (this.produceTerseOutput)
            {
                return;
            }

            base.WriteIndentation();
        }

        /// <summary>
        /// Writes a line terminator to the text string or stream.
        /// </summary>
        private void WriteLine()
        {
            if (this.produceTerseOutput)
            {
                return;
            }

            this.Writer.WriteLine();
        }
    }
}
