using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpawnDev.BlazorJS.SimplePeer
{
    /// <summary>
    /// 
    /// https://github.com/feross/simple-peer?tab=readme-ov-file#api
    /// </summary>
    public class SimplePeer : EventEmitter
    {
        static Task? _Init = null;
        public static Task Init()
        {
            _Init ??= JS.LoadScript("_content/SpawnDev.BlazorJS.SimplePeer/simplepeer.min.js");
            return _Init;
        }
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
        public void Send(object data) => JSRef!.CallVoid("send", data);
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
        /// data will be either a String or a Buffer/Uint8Array (see buffer).
        /// </summary>
        public JSEventCallback<JSObject> OnData { get => new JSEventCallback<JSObject>("data", On, RemoveListener); set { } }
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
        public JSEventCallback<JSObject> OnError { get => new JSEventCallback<JSObject>("error", On, RemoveListener); set { } }
        #endregion
    }
}
