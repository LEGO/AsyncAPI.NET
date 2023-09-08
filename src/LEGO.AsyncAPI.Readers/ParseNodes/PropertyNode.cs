// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Exceptions;

    public class PropertyNode : ParseNode
    {
        public PropertyNode(ParsingContext context, string name, JsonNode node)
            : base(
            context)
        {
            this.Name = name;
            this.Value = Create(context, node);
        }

        public string Name { get; set; }

        public ParseNode Value { get; set; }

        public void ParseField<T>(
            T parentInstance,
            IDictionary<string, Action<T, ParseNode>> fixedFields,
            IDictionary<Func<string, bool>, Action<T, string, ParseNode>> patternFields)
        {
            var found = fixedFields.TryGetValue(this.Name, out var fixedFieldMap);

            if (fixedFieldMap != null)
            {
                try
                {
                    this.Context.StartObject(this.Name);
                    fixedFieldMap(parentInstance, this.Value);
                }
                catch (AsyncApiReaderException ex)
                {
                    this.Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
                }
                catch (AsyncApiException ex)
                {
                    ex.Pointer = this.Context.GetLocation();
                    this.Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
                }
                finally
                {
                    this.Context.EndObject();
                }
            }
            else
            {
                var map = patternFields.Where(p => p.Key(this.Name)).Select(p => p.Value).FirstOrDefault();
                if (map != null)
                {
                    try
                    {
                        this.Context.StartObject(this.Name);
                        map(parentInstance, this.Name, this.Value);
                    }
                    catch (AsyncApiReaderException ex)
                    {
                        this.Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
                    }
                    catch (AsyncApiException ex)
                    {
                        ex.Pointer = this.Context.GetLocation();
                        this.Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
                    }
                    finally
                    {
                        this.Context.EndObject();
                    }
                }
                else
                {
                    this.Context.Diagnostic.Errors.Add(
                        new AsyncApiError("", $"{this.Name} is not a valid property at {this.Context.GetLocation()}"));
                }
            }
        }
    }
}
