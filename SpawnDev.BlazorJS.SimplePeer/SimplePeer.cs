using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpawnDev.BlazorJS.SimplePeer
{
    public class SimplePeerConfig
    {
        public bool Initiator { get; set; }
        public bool Trickle { get; set; }
    }
    public class SimplePeer : EventEmitter
    {
        static Task? _Init = null;
        public static Task Init()
        {
            _Init ??= JS.LoadScript("_content/SpawnDev.BlazorJS.SimplePeer/simplepeer.min.js");
            return _Init;
        }
        static Task? _Import = null;
        public static Task Import()
        {
            _Import ??= JS.Import("_content/SpawnDev.BlazorJS.SimplePeer/simplepeer.min.js");
            return _Import;
        }
        public SimplePeer(IJSInProcessObjectReference _ref) : base(_ref) { }
        public SimplePeer(SimplePeerConfig config):base(JS.New(nameof(SimplePeer), config)) { }
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
    }
}
