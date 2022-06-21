using System;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncApi.Readers.ParseNodes
{
    internal class AnyFieldMapParameter<T>
    {
        public AnyFieldMapParameter(
            Func<T, IAsyncApiAny> propertyGetter,
            Action<T, IAsyncApiAny> propertySetter,
            Func<T, AsyncApiSchema> schemaGetter)
        {
            this.PropertyGetter = propertyGetter;
            this.PropertySetter = propertySetter;
            this.SchemaGetter = schemaGetter;
        }

        public Func<T, IAsyncApiAny> PropertyGetter { get; }

        public Action<T, IAsyncApiAny> PropertySetter { get; }

        public Func<T, AsyncApiSchema> SchemaGetter { get; }
    }
}