// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Resolvers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
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

        /// <summary>
        /// Do not serialize an enumerable property if it is empty.
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // only relevant to classes inheriting from IEnumerable excluding strings
            if (property.PropertyType != typeof(string) && property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
            {
                Predicate<object> oldShouldSerialize = property.ShouldSerialize;

                Predicate<object> newShouldSerialize = instance =>
                {
                    var collection = property.ValueProvider.GetValue(instance) as ICollection;
                    var stringCollection = property.ValueProvider.GetValue(instance) as ICollection<string>;

                    return (collection == null || collection.Count > 0) &&
                           (stringCollection == null || stringCollection.Any());
                };

                property.ShouldSerialize = oldShouldSerialize != null
                    ? o => oldShouldSerialize(o) && newShouldSerialize(o)
                    : newShouldSerialize;
            }

            return property;
        }
    }
}