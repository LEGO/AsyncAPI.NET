// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using LEGO.AsyncAPI.Models;
    using System;

    public class AsyncApiWriterSettings
    {
        private ReferenceInlineSetting referenceInline = ReferenceInlineSetting.DoNotInlineReferences;

        internal LoopDetector LoopDetector { get; } = new LoopDetector();

        /// <summary>
        /// Gets or sets indicates how references in the source document should be handled.
        /// </summary>
        [Obsolete]
        public ReferenceInlineSetting ReferenceInline
        {
            get
            {
                return this.referenceInline;
            }

            set
            {
                this.referenceInline = value;
                switch (this.referenceInline)
                {
                    case ReferenceInlineSetting.DoNotInlineReferences:
                        this.InlineReferences = false;
                        break;
                    case ReferenceInlineSetting.InlineReferences:
                        this.InlineReferences = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether indicates if local references should be rendered as an inline object.
        /// </summary>
        public bool InlineReferences { get; set; } = false;

        internal bool ShouldInlineReference(AsyncApiReference reference)
        {
            return this.InlineReferences;
        }
    }
}
