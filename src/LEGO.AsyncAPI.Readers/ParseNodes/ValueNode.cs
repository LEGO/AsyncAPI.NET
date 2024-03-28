// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using System;
    using System.Text.Json.Nodes;

    public class ValueNode : ParseNode
    {
        private readonly JsonNode node;
        private string cachedScalarValue;
        public ValueNode(ParsingContext context, JsonNode node)
            : base(
            context)
        {
            if (!(node is JsonValue scalarNode))
            {
                throw new AsyncApiReaderException("Expected a value.");
            }

            this.node = scalarNode;
        }

        public override string GetScalarValue()
        {
            if (this.cachedScalarValue == null)
            {
                // TODO: Update this property to use the .ToString() or JsonReader. 
                var scalarNode = this.node is JsonValue value ? value : throw new AsyncApiException($"Expected scalar value");
                this.cachedScalarValue = Convert.ToString(scalarNode.GetValue<object>(), this.Context.Settings.CultureInfo);
            }

            return this.cachedScalarValue;
        }

        public override string GetScalarValueOrDefault(string defaultValue = null)
        {
            var value = this.GetScalarValue();
            if (value is not null)
            {
                return value;
            }

            return defaultValue;
        }

        public override int GetIntegerValue()
        {
            if (int.TryParse(this.GetScalarValue(), out int value))
            {
                return value;
            }

            throw new AsyncApiReaderException("Value could not parse to integer.");
        }

        public override int? GetIntegerValueOrDefault(int? defaultValue = null)
        {
            if (int.TryParse(this.GetScalarValue(), out int value))
            {
                return value;
            }

            return defaultValue;
        }

        public override long GetLongValue()
        {
            if (long.TryParse(this.GetScalarValue(), out long value))
            {
                return value;
            }

            throw new AsyncApiReaderException("Value could not parse to long.");
        }

        public override long? GetLongValueOrDefault(long? defaultValue = null)
        {
            if (long.TryParse(this.GetScalarValue(), out long value))
            {
                return value;
            }

            return defaultValue;
        }

        public override bool GetBooleanValue()
        {
            if (bool.TryParse(this.GetScalarValue(), out bool value))
            {
                return value;
            }

            throw new AsyncApiReaderException("Value could not parse to bool.");
        }

        public override bool? GetBooleanValueOrDefault(bool? defaultValue = null)
        {
            if (bool.TryParse(this.GetScalarValue(), out bool value))
            {
                return value;
            }

            return defaultValue;
        }

        public override AsyncApiAny CreateAny()
        {
            var value = this.GetScalarValue();
            return new AsyncApiAny(this.node);
        }
    }
}