namespace LEGO.AsyncAPI.Writers
{
    public class AsyncJsonWriterSettings : AsyncApiWriterSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncJsonWriterSettings"/> class.
        /// </summary>
        public AsyncJsonWriterSettings()
        { }

        /// <summary>
        /// Indicates whether or not the produced document will be written in a compact or pretty fashion.
        /// </summary>
        public bool Terse { get; set; } = false;
    }
}
