// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using System;
    using System.Text;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// AsyncApi primitive class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AsyncApiPrimitive<T> : IAsyncApiPrimitive
    {
        /// <summary>
        /// Initializes the <see cref="IAsyncApiPrimitive"/> class with the given value.
        /// </summary>
        /// <param name="value"></param>
        public AsyncApiPrimitive(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// The kind of <see cref="IAsyncApiAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Primitive;

        /// <summary>
        /// The primitive class this object represents.
        /// </summary>
        public abstract PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// Value of this <see cref="IAsyncApiPrimitive"/>
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Write out content of primitive element
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="specVersion"></param>
        public void Write(IAsyncApiWriter writer, AsyncApiVersion specVersion)
        {
            switch (this.PrimitiveType)
            {
                case PrimitiveType.Integer:
                    var intValue = (AsyncApiInteger)(IAsyncApiPrimitive)this;
                    writer.WriteValue(intValue.Value);
                    break;

                case PrimitiveType.Long:
                    var longValue = (AsyncApiLong)(IAsyncApiPrimitive)this;
                    writer.WriteValue(longValue.Value);
                    break;

                case PrimitiveType.Float:
                    var floatValue = (AsyncApiFloat)(IAsyncApiPrimitive)this;
                    writer.WriteValue(floatValue.Value);
                    break;

                case PrimitiveType.Double:
                    var doubleValue = (AsyncApiDouble)(IAsyncApiPrimitive)this;
                    writer.WriteValue(doubleValue.Value);
                    break;

                case PrimitiveType.String:
                    var stringValue = (AsyncApiString)(IAsyncApiPrimitive)this;
                    if (stringValue.IsRawString())
                    {
                        writer.WriteRaw(stringValue.Value);
                    }
                    else
                    {
                        writer.WriteValue(stringValue.Value);
                    }

                    break;

                case PrimitiveType.Byte:
                    var byteValue = (AsyncApiByte)(IAsyncApiPrimitive)this;
                    if (byteValue.Value == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.WriteValue(Convert.ToBase64String(byteValue.Value));
                    }

                    break;

                case PrimitiveType.Binary:
                    var binaryValue = (AsyncApiBinary)(IAsyncApiPrimitive)this;
                    if (binaryValue.Value == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.WriteValue(Encoding.UTF8.GetString(binaryValue.Value));
                    }

                    break;

                case PrimitiveType.Boolean:
                    var boolValue = (AsyncApiBoolean)(IAsyncApiPrimitive)this;
                    writer.WriteValue(boolValue.Value);
                    break;

                case PrimitiveType.Date:
                    var dateValue = (AsyncApiDate)(IAsyncApiPrimitive)this;
                    writer.WriteValue(dateValue.Value);
                    break;

                case PrimitiveType.DateTime:
                    var dateTimeValue = (AsyncApiDateTime)(IAsyncApiPrimitive)this;
                    writer.WriteValue(dateTimeValue.Value);
                    break;

                default:
                    throw new AsyncApiWriterException(
                        string.Format(
                            "The given primitive type '{0}' is not supported.",
                            this.PrimitiveType));
            }
        }
    }
}
