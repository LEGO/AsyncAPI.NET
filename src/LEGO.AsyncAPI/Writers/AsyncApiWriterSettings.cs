// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using LEGO.AsyncAPI.Models;

    public class AsyncApiWriterSettings
    {
        private ReferenceInlineSetting referenceInline = ReferenceInlineSetting.DoNotInlineReferences;

        internal LoopDetector LoopDetector { get; } = new LoopDetector();

        /// <summary>
        /// Gets or sets indicates how references in the source document should be handled.
        /// </summary>
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
                        this.InlineLocalReferences = false;
                        this.InlineExternalReferences = false;
                        break;
                    case ReferenceInlineSetting.InlineLocalReferences:
                        this.InlineLocalReferences = true;
                        this.InlineExternalReferences = false;
                        break;
                    case ReferenceInlineSetting.InlineAllReferences:
                        this.InlineLocalReferences = true;
                        this.InlineExternalReferences = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether indicates if local references should be rendered as an inline object
        /// </summary>
        public bool InlineLocalReferences { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether indicates if external references should be rendered as an inline object
        /// </summary>
        public bool InlineExternalReferences { get; set; } = false;

        internal bool ShouldInlineReference(AsyncApiReference reference)
        {
            return (reference.IsLocal && this.InlineLocalReferences)
                             || (reference.IsExternal && this.InlineExternalReferences);
        }

    }

}
