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
        /// Render all local references as inline objects
        /// </summary>
        InlineLocalReferences,

        /// <summary>
        /// Render all references as inline objects.
        /// </summary>
        InlineAllReferences
    }

}
