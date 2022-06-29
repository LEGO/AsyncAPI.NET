// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiSecurityRequirement : Dictionary<AsyncApiSecurityScheme, IList<string>>, IAsyncApiSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiSecurityRequirement"/> class.
        /// This constructor ensures that only Reference.Id is considered when two dictionary keys
        /// of type <see cref="AsyncApiSecurityScheme"/> are compared.
        /// </summary>
        public AsyncApiSecurityRequirement()
            : base(new AsyncApiReferenceEqualityComparer())
        {
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiSecurityRequirement"/> to Async Api v2
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
    }
}