// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class AsyncApiWriterBase : IAsyncApiWriter
    {
        /// <summary>
        /// The indentation string to prepand to each line for each indentation level.
        /// </summary>
        protected const string IndentationString = "  ";

        /// <summary>
        /// Gets or sets settings for controlling how the AsyncApi document will be written out.
        /// </summary>
        public AsyncApiWriterSettings Settings { get; set; }

        /// <summary>
        /// Scope of the AsyncApi element - object, array, property.
        /// </summary>
        protected readonly Stack<Scope> Scopes;

        /// <summary>
        /// Number which specifies the level of indentation.
        /// </summary>
        private int indentLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiWriterBase"/> class.
        /// </summary>
        /// <param name="textWriter">The text writer.</param>
        public AsyncApiWriterBase(TextWriter textWriter)
            : this(textWriter, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiWriterBase"/> class.
        /// </summary>
        /// <param name="textWriter"></param>
        /// <param name="settings"></param>
        public AsyncApiWriterBase(TextWriter textWriter, AsyncApiWriterSettings settings)
        {
            this.Writer = textWriter;
            this.Writer.NewLine = "\n";

            this.Scopes = new Stack<Scope>();
            if (settings == null)
            {
                settings = new AsyncApiWriterSettings();
            }

            this.Settings = settings;
        }

        /// <summary>
        /// Gets base Indentation Level.
        /// This denotes how many indentations are needed for the property in the base object.
        /// </summary>
        protected abstract int BaseIndentation { get; }

        /// <summary>
        /// Gets the text writer.
        /// </summary>
        protected TextWriter Writer { get; }

        /// <summary>
        /// Write start object.
        /// </summary>
        public abstract void WriteStartObject();

        /// <summary>
        /// Write end object.
        /// </summary>
        public abstract void WriteEndObject();

        /// <summary>
        /// Write start array.
        /// </summary>
        public abstract void WriteStartArray();

        /// <summary>
        /// Write end array.
        /// </summary>
        public abstract void WriteEndArray();

        /// <summary>
        /// Write the start property.
        /// </summary>
        public abstract void WritePropertyName(string name);

        /// <summary>
        /// Writes a separator of a value if it's needed for the next value to be written.
        /// </summary>
        protected abstract void WriteValueSeparator();

        /// <summary>
        /// Write null value.
        /// </summary>
        public abstract void WriteNull();

        /// <summary>
        /// Write content raw value.
        /// </summary>
        public abstract void WriteRaw(string value);

        /// <summary>
        /// Flush the writer.
        /// </summary>
        public void Flush()
        {
            this.Writer.Flush();
        }

        /// <summary>
        /// Write string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public abstract void WriteValue(string value);

        /// <summary>
        /// Write float value.
        /// </summary>
        /// <param name="value">The float value.</param>
        public virtual void WriteValue(float value)
        {
            this.WriteValueSeparator();
            this.Writer.Write(value);
        }

        /// <summary>
        /// Write double value.
        /// </summary>
        /// <param name="value">The double value.</param>
        public virtual void WriteValue(double value)
        {
            this.WriteValueSeparator();
            this.Writer.Write(value);
        }

        /// <summary>
        /// Write decimal value.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        public virtual void WriteValue(decimal value)
        {
            this.WriteValueSeparator();
            this.Writer.Write(value);
        }

        /// <summary>
        /// Write integer value.
        /// </summary>
        /// <param name="value">The integer value.</param>
        public virtual void WriteValue(int value)
        {
            this.WriteValueSeparator();
            this.Writer.Write(value);
        }

        /// <summary>
        /// Write long value.
        /// </summary>
        /// <param name="value">The long value.</param>
        public virtual void WriteValue(long value)
        {
            this.WriteValueSeparator();
            this.Writer.Write(value);
        }

        /// <summary>
        /// Write DateTime value.
        /// </summary>
        /// <param name="value">The DateTime value.</param>
        public virtual void WriteValue(DateTime value)
        {
            this.WriteValue(value.ToString("o"));
        }

        /// <summary>
        /// Write DateTimeOffset value.
        /// </summary>
        /// <param name="value">The DateTimeOffset value.</param>
        public virtual void WriteValue(DateTimeOffset value)
        {
            this.WriteValue(value.ToString("o"));
        }

        /// <summary>
        /// Write boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        public virtual void WriteValue(bool value)
        {
            this.WriteValueSeparator();
            this.Writer.Write(value.ToString().ToLower());
        }

        /// <summary>
        /// Write object value.
        /// </summary>
        /// <param name="value">The object value.</param>
        public virtual void WriteValue(object value)
        {
            if (value == null)
            {
                this.WriteNull();
                return;
            }

            var type = value.GetType();

            if (type == typeof(string))
            {
                this.WriteValue((string)(value));
            }
            else if (type == typeof(int) || type == typeof(int?))
            {
                this.WriteValue((int)value);
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
                this.WriteValue((long)value);
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                this.WriteValue((bool)value);
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                this.WriteValue((float)value);
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                this.WriteValue((double)value);
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                this.WriteValue((decimal)value);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                this.WriteValue((DateTime)value);
            }
            else if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
            {
                this.WriteValue((DateTimeOffset)value);
            }
            else
            {
                throw new AsyncApiWriterException(string.Format("The type '{0}' is not supported in AsyncApi document.", type.FullName));
            }
        }

        /// <summary>
        /// Increases the level of indentation applied to the output.
        /// </summary>
        public virtual void IncreaseIndentation()
        {
            this.indentLevel++;
        }

        /// <summary>
        /// Decreases the level of indentation applied to the output.
        /// </summary>
        public virtual void DecreaseIndentation()
        {
            if (this.indentLevel == 0)
            {
                throw new AsyncApiWriterException("Indentation level cannot be lower than 0.");
            }

            if (this.indentLevel < 1)
            {
                this.indentLevel = 0;
            }
            else
            {
                this.indentLevel--;
            }
        }

        /// <summary>
        /// Write the indentation.
        /// </summary>
        public virtual void WriteIndentation()
        {
            for (var i = 0; i < (this.BaseIndentation + this.indentLevel - 1); i++)
            {
                this.Writer.Write(IndentationString);
            }
        }

        /// <summary>
        /// Get current scope.
        /// </summary>
        /// <returns></returns>
        protected Scope CurrentScope()
        {
            return this.Scopes.Count == 0 ? null : this.Scopes.Peek();
        }

        /// <summary>
        /// Start the scope given the scope type.
        /// </summary>
        /// <param name="type">The scope type to start.</param>
        protected Scope StartScope(ScopeType type)
        {
            if (this.Scopes.Count != 0)
            {
                var currentScope = this.Scopes.Peek();

                currentScope.ObjectCount++;
            }

            var scope = new Scope(type);
            this.Scopes.Push(scope);
            return scope;
        }

        /// <summary>
        /// End the scope of the given scope type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected Scope EndScope(ScopeType type)
        {
            if (this.Scopes.Count == 0)
            {
                throw new AsyncApiWriterException("Scope must be present to end.");
            }

            if (this.Scopes.Peek().Type != type)
            {
                throw new AsyncApiWriterException(
                    string.Format(
                        "The scope to end is expected to be of type '{0}' but it is of type '{1}'.",
                        type,
                        this.Scopes.Peek().Type));
            }

            return this.Scopes.Pop();
        }

        /// <summary>
        /// Whether the current scope is the top level (outermost) scope.
        /// </summary>
        protected bool IsTopLevelScope()
        {
            return this.Scopes.Count == 1;
        }

        /// <summary>
        /// Whether the current scope is an object scope.
        /// </summary>
        protected bool IsObjectScope()
        {
            return this.IsScopeType(ScopeType.Object);
        }

        /// <summary>
        /// Whether the current scope is an array scope.
        /// </summary>
        /// <returns></returns>
        protected bool IsArrayScope()
        {
            return this.IsScopeType(ScopeType.Array);
        }

        private bool IsScopeType(ScopeType type)
        {
            if (this.Scopes.Count == 0)
            {
                return false;
            }

            return this.Scopes.Peek().Type == type;
        }

        /// <summary>
        /// Verifies whether a property name can be written based on whether
        /// the property name is a valid string and whether the current scope is an object scope.
        /// </summary>
        /// <param name="name">property name</param>
        protected void VerifyCanWritePropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            if (this.Scopes.Count == 0)
            {
                throw new AsyncApiWriterException(
                    string.Format("There must be an active scope for name '{0}' to be written.", name));
            }

            if (this.Scopes.Peek().Type != ScopeType.Object)
            {
                throw new AsyncApiWriterException(
                    string.Format("The active scope must be an object scope for property name '{0}' to be written.", name));
            }
        }
    }
}
