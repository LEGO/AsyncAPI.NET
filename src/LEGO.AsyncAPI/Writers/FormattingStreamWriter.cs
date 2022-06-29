// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using System.IO;

    public class FormattingStreamWriter : StreamWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormattingStreamWriter"/> class.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="formatProvider"></param>
        public FormattingStreamWriter(Stream stream, IFormatProvider formatProvider)
            : base(stream)
        {
            this.FormatProvider = formatProvider;
        }

        /// <summary>
        /// The <see cref="IFormatProvider"/> associated with this <see cref="FormattingStreamWriter"/>.
        /// </summary>
        public override IFormatProvider FormatProvider { get; }
    }

}
