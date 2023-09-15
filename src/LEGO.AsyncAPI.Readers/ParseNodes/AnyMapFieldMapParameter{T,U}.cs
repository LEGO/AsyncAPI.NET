// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;

    internal class AnyMapFieldMapParameter<T, U>
    {
        public AnyMapFieldMapParameter(
            Func<T, IDictionary<string, U>> propertyMapGetter,
            Func<U, AsyncApiAny> propertyGetter,
            Action<U, AsyncApiAny> propertySetter,
            Func<T, AsyncApiSchema> schemaGetter)
        {
            this.PropertyMapGetter = propertyMapGetter;
            this.PropertyGetter = propertyGetter;
            this.PropertySetter = propertySetter;
            this.SchemaGetter = schemaGetter;
        }

        public Func<T, IDictionary<string, U>> PropertyMapGetter { get; }

        public Func<U, AsyncApiAny> PropertyGetter { get; }

        public Action<U, AsyncApiAny> PropertySetter { get; }

        public Func<T, AsyncApiSchema> SchemaGetter { get; }
    }
}