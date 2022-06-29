// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Writers;

    internal static class AsyncApiWriterExtensions
    {
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