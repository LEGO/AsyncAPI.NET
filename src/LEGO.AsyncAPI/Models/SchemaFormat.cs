// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    public enum SchemaFormat
    {
        [Display("application/vnd.aai.asyncapi;version=2.5.0")]
        AsyncApi,

        [Display("application/vnd.aai.asyncapi+json;version=2.5.0")]
        AsyncApiJson,

        [Display("application/vnd.aai.asyncapi+yaml;version=2.5.0")]
        AsyncApiYaml,

        [Display("application/schema+json;version=draft-07")]
        JsonSchemaJson,

        [Display("application/schema+yaml;version=draft-07")]
        JsonSchemaYaml,

        Unsupported = 99999999,
    }
}
