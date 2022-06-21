// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncApi.Readers.ParseNodes
{
    internal class AnyListFieldMapParameter<T>
    {
        public AnyListFieldMapParameter(
            Func<T, IList<IAsyncApiAny>> propertyGetter,
            Action<T, IList<IAsyncApiAny>> propertySetter,
            Func<T, AsyncApiSchema> schemaGetter)
        {
            this.PropertyGetter = propertyGetter;
            this.PropertySetter = propertySetter;
            this.SchemaGetter = schemaGetter;
        }

        public Func<T, IList<IAsyncApiAny>> PropertyGetter { get; }

        public Action<T, IList<IAsyncApiAny>> PropertySetter { get; }
        
        public Func<T, AsyncApiSchema> SchemaGetter { get; }
    }
}