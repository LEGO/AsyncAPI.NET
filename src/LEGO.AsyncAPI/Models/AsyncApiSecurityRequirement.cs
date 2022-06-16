// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiSecurityRequirement : Dictionary<AsyncApiSecurityScheme, IList<string>>,
       IAsyncApiSerializable
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiSecurityRequirement"/> class.
        /// This constructor ensures that only Reference.Id is considered when two dictionary keys
        /// of type <see cref="AsyncApiSecurityScheme"/> are compared.
        /// </summary>
        public AsyncApiSecurityRequirement()
            : base(new AsyncApiSecuritySchemeReferenceEqualityComparer())
        {
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiSecurityRequirement"/> to Open Api v3.0
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            foreach (var securitySchemeAndScopesValuePair in this)
            {
                var securityScheme = securitySchemeAndScopesValuePair.Key;
                var scopes = securitySchemeAndScopesValuePair.Value;

                if (securityScheme.Reference == null)
                {
                    // Reaching this point means the reference to a specific AsyncApiSecurityScheme fails.
                    // We are not able to serialize this SecurityScheme/Scopes key value pair since we do not know what
                    // string to output.
                    continue;
                }

                securityScheme.SerializeV2(writer);

                writer.WriteStartArray();

                foreach (var scope in scopes)
                {
                    writer.WriteValue(scope);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiSecurityRequirement"/> to Open Api v2.0
        /// </summary>
        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            foreach (var securitySchemeAndScopesValuePair in this)
            {
                var securityScheme = securitySchemeAndScopesValuePair.Key;
                var scopes = securitySchemeAndScopesValuePair.Value;

                if (securityScheme.Reference == null)
                {
                    // Reaching this point means the reference to a specific AsyncApiSecurityScheme fails.
                    // We are not able to serialize this SecurityScheme/Scopes key value pair since we do not know what
                    // string to output.
                    continue;
                }

                securityScheme.SerializeV2(writer);

                writer.WriteStartArray();

                foreach (var scope in scopes)
                {
                    writer.WriteValue(scope);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Comparer for AsyncApiSecurityScheme that only considers the Id in the Reference
        /// (i.e. the string that will actually be displayed in the written document)
        /// </summary>
        private class AsyncApiSecuritySchemeReferenceEqualityComparer : IEqualityComparer<AsyncApiSecurityScheme>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            public bool Equals(AsyncApiSecurityScheme x, AsyncApiSecurityScheme y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                if (x.Reference == null || y.Reference == null)
                {
                    return false;
                }

                return x.Reference.Id == y.Reference.Id;
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            public int GetHashCode(AsyncApiSecurityScheme obj)
            {
                return obj?.Reference?.Id == null ? 0 : obj.Reference.Id.GetHashCode();
            }
        }
    }
}