using System;
using System.Collections.Generic;

namespace LEGO.AsyncApi.Readers.ParseNodes
{
    internal class AnyMapFieldMapParameter<T, U>
    {

        public AnyMapFieldMapParameter(
            Func<T, IDictionary<string, U>> propertyMapGetter,
            Func<U, IAsyncApiAny> propertyGetter,
            Action<U, IAsyncApiAny> propertySetter,
            Func<T, AsyncApiSchema> schemaGetter)
        {
            this.PropertyMapGetter = propertyMapGetter;
            this.PropertyGetter = propertyGetter;
            this.PropertySetter = propertySetter;
            this.SchemaGetter = schemaGetter;
        }

        public Func<T, IDictionary<string, U>> PropertyMapGetter { get; }

        public Func<U, IAsyncApiAny> PropertyGetter { get; }
        
        public Action<U, IAsyncApiAny> PropertySetter { get; }
        
        public Func<T, AsyncApiSchema> SchemaGetter { get; }
    }
}