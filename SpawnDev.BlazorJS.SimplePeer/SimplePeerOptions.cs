using System.Text.Json.Serialization;

namespace SpawnDev.BlazorJS.SimplePeer
{
    /// <summary>
    /// SimplePeer options
    /// https://github.com/feross/simple-peer?tab=readme-ov-file#peer--new-peeropts
    /// </summary>
    public class SimplePeerOptions
    {
        /// <summary>
        /// set to true if this is the initiating peer
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Initiator { get; set; }
        /// <summary>
        /// set to false to disable trickle ICE and get a single 'signal' event (slower)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Trickle { get; set; }
    }
}
