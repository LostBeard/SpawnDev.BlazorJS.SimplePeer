using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.JSObjects.WebRTC;
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
        /// Set to true if this is the initiating peer
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Initiator { get; set; }
        /// <summary>
        /// Set to false to disable trickle ICE and get a single 'signal' event (slower)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Trickle { get; set; }
        /// <summary>
        /// Custom webrtc data channel configuration (used by createDataChannel)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ChannelName { get; set; }
        /// <summary>
        /// Custom webrtc configuration (used by RTCPeerConnection constructor)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RTCConfiguration? Config { get; set; }
        /// <summary>
        /// Custom webrtc data channel configuration (used by createDataChannel)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RTCDataChannelOptions? ChannelConfig { get; set; }
        /// <summary>
        /// Custom offer options (used by createOffer method)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RTCOfferOptions? OfferOptions { get; set; }
        /// <summary>
        /// Custom answer options (used by createAnswer method)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RTCAnswerOptions? AnswerOptions { get; set; }
        /// <summary>
        /// If video/voice is desired, pass stream returned from getUserMedia
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MediaStream? Stream{ get; set; }
        /// <summary>
        /// An array of MediaStreams returned from getUserMedia
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MediaStream[]? Streams { get; set; }
        /// <summary>
        /// Set to true to create the stream in Object Mode. In this mode, incoming string data is not automatically converted to Buffer objects.<br/>
        /// false - incoming string data is automatically converted to NodeBuffer objects. Default if omitted.<br/>
        /// true - incoming string data is not automatically converted to NodeBuffer objects. Incoming data will be received as string or NodeBuffer depending on how it was sent.<br/>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ObjectMode { get; set; }
    }
}
