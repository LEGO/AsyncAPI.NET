namespace LEGO.AsyncAPI.Models.Bindings.Sns;

public class OrderingConfiguration
{
    /// <summary>
    /// What type of SNS Topic is this?
    /// </summary>
    public Ordering Type { get; set; }
    
    /// <summary>
    /// True to turn on de-duplication of messages for a channel.
    /// </summary>
    public bool ContentBasedDeduplication { get; set; }
}

public enum Ordering
{
    Standard,
    Fifo
}