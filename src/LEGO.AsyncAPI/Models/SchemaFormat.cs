// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    public enum SchemaFormat
    {
        Unsupported,

        [Display("application/vnd.aai.asyncapi;version=2.5.0")]
        AsyncApi,

        [Display("application/vnd.aai.asyncapi+json;version=2.5.0")]
        AsyncApiJson,

        [Display("application/vnd.aai.asyncapi+yaml;version=2.5.0")]
        AsyncApiYaml,

        [Display("application/vnd.aai.asyncapi+json;version=2.5.0")]
        JsonSchemaJson,

        [Display("application/vnd.aai.asyncapi+yaml;version=2.5.0")]
        JsonSchemaYaml,
    }
}
