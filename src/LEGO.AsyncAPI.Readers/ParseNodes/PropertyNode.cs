using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Exceptions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.Exceptions;
using SharpYaml.Serialization;

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    internal class PropertyNode : ParseNode
    {
        public PropertyNode(ParsingContext context, string name, YamlNode node) : base(
            context)
        {
            Name = name;
            Value = Create(context, node);
        }

        public string Name { get; set; }

        public ParseNode Value { get; set; }

        public void ParseField<T>(
            T parentInstance,
            IDictionary<string, Action<T, ParseNode>> fixedFields,
            IDictionary<Func<string, bool>, Action<T, string, ParseNode>> patternFields)
        {
            Action<T, ParseNode> fixedFieldMap;
            var found = fixedFields.TryGetValue(Name, out fixedFieldMap);

            if (fixedFieldMap != null)
            {
                try
                {
                    Context.StartObject(Name);
                    fixedFieldMap(parentInstance, Value);
                }
                catch (AsyncApiReaderException ex)
                {
                    Context.Diagnostic.Errors.Add(new Async
                }
                catch (AsyncApiException ex)
                {
                    ex.Pointer = Context.GetLocation();
                    Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
                }
                finally
                {
                    Context.EndObject();
                }
            }
            else
            {
                var map = patternFields.Where(p => p.Key(Name)).Select(p => p.Value).FirstOrDefault();
                if (map != null)
                {
                    try
                    {
                        Context.StartObject(Name);
                        map(parentInstance, Name, Value);
                    }
                    catch (AsyncApiReaderException ex)
                    {
                        Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
                    }
                    catch (AsyncApiException ex)
                    {
                        ex.Pointer = Context.GetLocation();
                        Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
                    }
                    finally
                    {
                        Context.EndObject();
                    }
                }
                else
                {
                    Context.Diagnostic.Errors.Add(
                        new AsyncApiError("", $"{Name} is not a valid property at {Context.GetLocation()}"));
                }
            }
        }

        public override IAsyncApiAny CreateAny()
        {
            throw new NotImplementedException();
        }
    }
}
