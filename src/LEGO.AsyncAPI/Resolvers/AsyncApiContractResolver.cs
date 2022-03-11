// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    internal class AsyncApiContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly ExtensionDataSetter extensionDataSetter = new();
        private readonly ExtensionDataGetter extensionDataGetter = new();

        public override JsonContract ResolveContract(Type type)
        {
            var resolveContract = base.ResolveContract(type);
            if (resolveContract.GetType() != typeof(JsonObjectContract))
            {
                return resolveContract;
            }

            var jsonObjectContract = resolveContract as JsonObjectContract;
            jsonObjectContract.ExtensionDataSetter = this.extensionDataSetter.Setter;
            jsonObjectContract.ExtensionDataGetter = this.extensionDataGetter.Getter;

            return resolveContract;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var jsonProperties = base.CreateProperties(type, memberSerialization);
            try
            {
                var firstOrDefault = jsonProperties.First(property => property.PropertyName.Equals("extensions"));

                // The Extensions field is marked as ignored here so that the default mechanics leave it alone.
                // Without this, an Extensions property would be added to all exported JSON. This could be added as
                // [JsonIgnore] to each class however as the expected behaviour of [JsonIgnore] is to ignore the field
                // entirely, this seems like the lesser of two evils.
                firstOrDefault.Ignored = true;
            }
            catch (InvalidOperationException)
            {
                // no extensions found. This will only happen when the Model object does not extend the IExtensible interface
            }

            return jsonProperties;
        }
    }
}