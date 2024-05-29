using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.JSObjects.WebRTC;

namespace SpawnDev.BlazorJS.SimplePeer
{
    /// <summary>
    /// Duplex streams are streams that implement both the Readable and Writable interfaces.<br/>
    /// https://nodejs.org/api/stream.html#class-streamduplex<br/>
    /// https://nodejs.org/api/stream.html#class-streamreadable<br/>
    /// https://nodejs.org/api/stream.html#class-streamwritable
    /// </summary>
    public class Duplex : EventEmitter
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Duplex(IJSInProcessObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// If false then the stream will automatically end the writable side when the readable side ends. Set initially by the allowHalfOpen constructor option, which defaults to true.
        /// </summary>
        public bool AllowHalfOpen => JSRef!.Get<bool>("allowHalfOpen");

        #region Readable
        /// <summary>
        /// The 'close' event is emitted when the stream and any of its underlying resources (a file descriptor, for example) have been closed. The event indicates that no more events will be emitted, and no further computation will occur.
        /// </summary>
        public JSEventCallback OnClose { get => new JSEventCallback("close", On, RemoveListener); set { } }
        /// <summary>
        /// chunk &lt;Buffer> | &lt;string> | &lt;any> The chunk of data. For streams that are not operating in object mode, the chunk will be either a string or Buffer. For streams that are in object mode, the chunk can be any JavaScript value other than null.<br/>
        /// 
        /// </summary>
        public JSEventCallback<JSObject> OnData { get => new JSEventCallback<JSObject>("data", On, RemoveListener); set { } }
        #endregion
    }
    /// <summary>
    /// Simple WebRTC video, voice, and data channels<br/>
    /// https://github.com/feross/simple-peer?tab=readme-ov-file#api
    /// </summary>
    public class SimplePeer : EventEmitter
    {
        /// <summary>
        /// Errors returned by the error event have an err.code property that will indicate the origin of the failure.
        /// </summary>
        public static class ErrCodes
        {
            public const string ERR_WEBRTC_SUPPORT = "ERR_WEBRTC_SUPPORT";
            public const string ERR_CREATE_OFFER = "ERR_CREATE_OFFER";
            public const string ERR_CREATE_ANSWER = "ERR_CREATE_ANSWER";
            public const string ERR_SET_LOCAL_DESCRIPTION = "ERR_SET_LOCAL_DESCRIPTION";
            public const string ERR_SET_REMOTE_DESCRIPTION = "ERR_SET_REMOTE_DESCRIPTION";
            public const string ERR_ADD_ICE_CANDIDATE = "ERR_ADD_ICE_CANDIDATE";
            public const string ERR_ICE_CONNECTION_FAILURE = "ERR_ICE_CONNECTION_FAILURE";
            public const string ERR_SIGNALING = "ERR_SIGNALING";
            public const string ERR_DATA_CHANNEL = "ERR_DATA_CHANNEL";
            public const string ERR_CONNECTION_FAILURE = "ERR_CONNECTION_FAILURE";
        }
        /// <summary>
        /// Detect native WebRTC support in the javascript environment.
        /// </summary>
        public static bool WEBRTC_SUPPORT => JS.Get<bool>("SimplePeer.WEBRTC_SUPPORT");
        /// <summary>
        /// SimplePeer Default RTCConfiguration
        /// </summary>
        public static RTCConfiguration? DefaultConfig => JS.Get<RTCConfiguration?>("SimplePeer.config");
        static Task? _Init = null;
        /// <summary>
        /// Load the SimplePeer Javascript library
        /// </summary>
        /// <returns>Returns when the library has been loaded</returns>
        public static Task Init()
        {
            _Init ??= JS.LoadScript("_content/SpawnDev.BlazorJS.SimplePeer/simplepeer.min.js");
            return _Init;
        }
        #region Constructors
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public SimplePeer(IJSInProcessObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Create a new WebRTC peer connection.
        /// </summary>
        /// <param name="config"></param>
        public SimplePeer(SimplePeerOptions config) : base(JS.New(nameof(SimplePeer), config)) { }
        /// <summary>
        /// Create a new WebRTC peer connection.
        /// </summary>
        public SimplePeer() : base(JS.New(nameof(SimplePeer))) { }
        #endregion
        #region Properties
        /// <summary>
        /// Set to true if this is the initiating peer
        /// </summary>
        public bool Initiator => JSRef!.Get<bool>("initiator");
        /// <summary>
        /// Set to false to disable trickle ICE and get a single 'signal' event (slower)
        /// </summary>
        public bool Trickle => JSRef!.Get<bool>("trickle");
        /// <summary>
        /// Custom webrtc data channel configuration (used by createDataChannel)
        /// </summary>
        public string ChannelName => JSRef!.Get<string>("channelName");
        /// <summary>
        /// Custom webrtc configuration (used by RTCPeerConnection constructor)
        /// </summary>
        public RTCConfiguration? Config => JSRef!.Get<RTCConfiguration?>("config");
        /// <summary>
        /// Custom webrtc data channel configuration (used by createDataChannel)
        /// </summary>
        public RTCDataChannelOptions? ChannelConfig => JSRef!.Get<RTCDataChannelOptions?>("channelConfig");
        /// <summary>
        /// Custom offer options (used by createOffer method)
        /// </summary>
        public RTCOfferOptions? OfferOptions => JSRef!.Get<RTCOfferOptions?>("offerOptions");
        /// <summary>
        /// Custom answer options (used by createAnswer method)
        /// </summary>
        public RTCAnswerOptions? AnswerOptions => JSRef!.Get<RTCAnswerOptions?>("answerOptions");
        /// <summary>
        /// An array of MediaStreams returned from getUserMedia
        /// </summary>
        public Array<MediaStream> Streams => JSRef!.Get<Array<MediaStream>>("streams");
        /// <summary>
        /// Is true if it is safe to call writable.write(), which means the stream has not been destroyed, errored, or ended.
        /// </summary>
        public bool Writable => JSRef!.Get<bool>("writable");
        /// <summary>
        /// 
        /// </summary>
        public bool Readable => JSRef!.Get<bool>("readable");
        /// <summary>
        /// If false then the stream will automatically end the writable side when the readable side ends. Set initially by the allowHalfOpen constructor option, which defaults to true.
        /// </summary>
        public bool AllowHalfOpen => JSRef!.Get<bool>("allowHalfOpen");
        /// <summary>
        /// 
        /// </summary>
        public bool Destroying => JSRef!.Get<bool>("destroying");
        /// <summary>
        /// The instance has been destroyed
        /// </summary>
        public bool Destroyed => JSRef!.Get<bool>("destroyed");
        /// <summary>
        /// 
        /// </summary>
        public bool AllowHalfTrickle => JSRef!.Get<bool>("allowHalfTrickle");
        /// <summary>
        /// 
        /// </summary>
        public int IceCompleteTimeout => JSRef!.Get<int>("iceCompleteTimeout");
        #endregion
        #region Methods
        /// <summary>
        /// Destroy and cleanup this peer connection.<br/>
        /// </summary>
        public void Destroy() => JSRef!.CallVoid("destroy");
        /// <summary>
        /// Call this method whenever the remote peer emits a peer.on('signal') event.<br/>
        /// The data will encapsulate a webrtc offer, answer, or ice candidate. These messages help the peers to eventually establish a direct connection to each other. The contents of these strings are an implementation detail that can be ignored by the user of this module; simply pass the data from 'signal' events to the remote peer and call peer.signal(data) to get connected.
        /// </summary>
        /// <param name="data"></param>
        public void Signal(object data) => JSRef!.CallVoid("signal", data);
        /// <summary>
        /// Send text/binary data to the remote peer. data can be any of several types: String, Buffer (see buffer), ArrayBufferView (Uint8Array, etc.), ArrayBuffer, or Blob (in browsers that support it).<br/>
        /// Note: If this method is called before the peer.on('connect') event has fired, then an exception will be thrown. Use peer.write(data) (which is inherited from the node.js duplex stream interface) if you want this data to be buffered instead.
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data) => JSRef!.CallVoid("send", data);
        /// <summary>
        /// Send text/binary data to the remote peer. data can be any of several types: String, Buffer (see buffer), ArrayBufferView (Uint8Array, etc.), ArrayBuffer, or Blob (in browsers that support it).<br/>
        /// Note: If this method is called before the peer.on('connect') event has fired, then an exception will be thrown. Use peer.write(data) (which is inherited from the node.js duplex stream interface) if you want this data to be buffered instead.
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data) => JSRef!.CallVoid("send", data);
        /// <summary>
        /// Send text/binary data to the remote peer. data can be any of several types: String, Buffer (see buffer), ArrayBufferView (Uint8Array, etc.), ArrayBuffer, or Blob (in browsers that support it).<br/>
        /// Note: If this method is called before the peer.on('connect') event has fired, then an exception will be thrown. Use peer.write(data) (which is inherited from the node.js duplex stream interface) if you want this data to be buffered instead.
        /// </summary>
        /// <param name="data"></param>
        public void Send(ArrayBuffer data) => JSRef!.CallVoid("send", data);
        /// <summary>
        /// Send text/binary data to the remote peer. data can be any of several types: String, Buffer (see buffer), ArrayBufferView (Uint8Array, etc.), ArrayBuffer, or Blob (in browsers that support it).<br/>
        /// Note: If this method is called before the peer.on('connect') event has fired, then an exception will be thrown. Use peer.write(data) (which is inherited from the node.js duplex stream interface) if you want this data to be buffered instead.
        /// </summary>
        /// <param name="data"></param>
        public void Send(TypedArray data) => JSRef!.CallVoid("send", data);
        /// <summary>
        /// Send text/binary data to the remote peer. data can be any of several types: String, Buffer (see buffer), ArrayBufferView (Uint8Array, etc.), ArrayBuffer, or Blob (in browsers that support it).<br/>
        /// Note: If this method is called before the peer.on('connect') event has fired, then an exception will be thrown. Use peer.write(data) (which is inherited from the node.js duplex stream interface) if you want this data to be buffered instead.
        /// </summary>
        /// <param name="data"></param>
        public void Send(Blob data) => JSRef!.CallVoid("send", data);
        /// <summary>
        /// Add a MediaStream to the connection.
        /// </summary>
        /// <param name="stream"></param>
        public void AddStream(MediaStream stream) => JSRef!.CallVoid("addStream", stream);
        /// <summary>
        /// Remove a MediaStream from the connection.
        /// </summary>
        /// <param name="stream"></param>
        public void RemoveStream(MediaStream stream) => JSRef!.CallVoid("removeStream", stream);
        /// <summary>
        /// Add a MediaStreamTrack to the connection. Must also pass the MediaStream you want to attach it to.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="stream"></param>
        public void AddTrack(MediaStreamTrack track, MediaStream stream) => JSRef!.CallVoid("addTrack", track, stream);
        /// <summary>
        /// Remove a MediaStreamTrack from the connection. Must also pass the MediaStream that it was attached to.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="stream"></param>
        public void RemoveTrack(MediaStreamTrack track, MediaStream stream) => JSRef!.CallVoid("removeTrack", track, stream);
        /// <summary>
        /// Replace a MediaStreamTrack with another track. Must also pass the MediaStream that the old track was attached to.
        /// </summary>
        /// <param name="oldTrack"></param>
        /// <param name="newTrack"></param>
        /// <param name="stream"></param>
        public void ReplaceTrack(MediaStreamTrack oldTrack, MediaStreamTrack newTrack, MediaStream stream) => JSRef!.CallVoid("replaceTrack", oldTrack, newTrack, stream);
        /// <summary>
        /// Add a RTCRtpTransceiver to the connection. Can be used to add transceivers before adding tracks. Automatically called as neccesary by addTrack.
        /// </summary>
        /// <param name="kind">
        /// The possible values are a string with one of the following values:<br/>
        /// "audio": the track is an audio track.<br/>
        /// "video": the track is a video track.
        /// </param>
        /// <param name="init">An object for specifying any options when creating the new transceiver</param>
        public void AddTransceiver(string kind, TransceiverRequestInit init) => JSRef!.CallVoid("addTransceiver", kind, init);
        #endregion
        #region Events
        /// <summary>
        /// Fired when the peer wants to send signaling data to the remote peer.<br/>
        /// It is the responsibility of the application developer (that's you!) to get this data to the other peer. This usually entails using a websocket signaling server. This data is an Object, so remember to call JSON.stringify(data) to serialize it first. Then, simply call peer.signal(data) on the remote peer.<br/>
        /// (Be sure to listen to this event immediately to avoid missing it. For initiator: true peers, it fires right away. For initatior: false peers, it fires when the remote offer is received.)
        /// </summary>
        public JSEventCallback<JSObject> OnSignal { get => new JSEventCallback<JSObject>("signal", On, RemoveListener); set { } }
        /// <summary>
        /// Fired when the peer connection and data channel are ready to use.
        /// </summary>
        public JSEventCallback OnConnect { get => new JSEventCallback("connect", On, RemoveListener); set { } }
        /// <summary>
        /// Received a message from the remote peer (via the data channel).<br/>
        /// data will be either a String or a NodeBuffer.<br/>
        /// SimplePeerOptions.ObjectMode set to true to create the stream in Object Mode. In this mode, incoming string data is not automatically converted to NodeBuffer objects.
        /// </summary>
        public JSEventCallback<NodeBuffer> OnData { get => new JSEventCallback<NodeBuffer>("data", On, RemoveListener); set { } }
        /// <summary>
        /// Received a remote video stream, which can be displayed in a video tag.
        /// </summary>
        public JSEventCallback<MediaStream> OnStream { get => new JSEventCallback<MediaStream>("stream", On, RemoveListener); set { } }
        /// <summary>
        /// Received a remote audio/video track. Streams may contain multiple tracks.
        /// </summary>
        public JSEventCallback<MediaStreamTrack, MediaStream> OnTrack{ get => new JSEventCallback<MediaStreamTrack, MediaStream>("track", On, RemoveListener); set { } }
        /// <summary>
        /// Called when the peer connection has closed.
        /// </summary>
        public JSEventCallback OnClose { get => new JSEventCallback("close", On, RemoveListener); set { } }
        /// <summary>
        /// Fired when a fatal error occurs. Usually, this means bad signaling data was received from the remote peer.<br/>
        /// Errors returned by the error event have an err.code property that will indicate the origin of the failure.<br/>
        /// </summary>
        public JSEventCallback<NodeError> OnError { get => new JSEventCallback<NodeError>("error", On, RemoveListener); set { } }
        #endregion
    }
}
