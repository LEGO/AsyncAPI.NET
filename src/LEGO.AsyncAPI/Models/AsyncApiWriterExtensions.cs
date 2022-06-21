// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    internal static class AsyncApiWriterExtensions
    {
        /// <summary>
        /// Temporary extension method until we add Settings property to IOpenApiWriter in next major version
        /// </summary>
        /// <param name="asyncApiWriter"></param>
        /// <returns></returns>
        internal static AsyncApiWriterSettings GetSettings(this IAsyncApiWriter asyncApiWriter)
        {
            if (asyncApiWriter is AsyncApiWriterBase)
            {
                return ((AsyncApiWriterBase)asyncApiWriter).Settings;
            }
            return new AsyncApiWriterSettings();
        }
    }
}