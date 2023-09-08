﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;

    internal class AnyListFieldMapParameter<T>
    {
        public AnyListFieldMapParameter(
            Func<T, IList<AsyncApiAny>> propertyGetter,
            Action<T, IList<AsyncApiAny>> propertySetter,
            Func<T, AsyncApiSchema> schemaGetter)
        {
            this.PropertyGetter = propertyGetter;
            this.PropertySetter = propertySetter;
            this.SchemaGetter = schemaGetter;
        }

        public Func<T, IList<AsyncApiAny>> PropertyGetter { get; }

        public Action<T, IList<AsyncApiAny>> PropertySetter { get; }

        public Func<T, AsyncApiSchema> SchemaGetter { get; }
    }
}