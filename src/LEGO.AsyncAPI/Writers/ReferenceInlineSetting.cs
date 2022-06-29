// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    public enum ReferenceInlineSetting
    {
        /// <summary>
        /// Render all references as $ref.
        /// </summary>
        DoNotInlineReferences,

        /// <summary>
        /// Render references as inline objects.
        /// </summary>
        InlineReferences,
    }
}
