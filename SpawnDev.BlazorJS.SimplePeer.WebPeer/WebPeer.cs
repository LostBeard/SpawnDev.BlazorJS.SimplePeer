using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.JSObjects.WebRTC;
using SpawnDev.BlazorJS.WebWorkers;
using Array = SpawnDev.BlazorJS.JSObjects.Array;

namespace SpawnDev.BlazorJS.SimplePeer.WebPeer
{
    /// <summary>
    /// Client and server implementation for remotely calling .Net methods using SimplePeer
    /// </summary>
    public class WebPeer : RemoteDispatcher, IDisposable
    {
        /// <summary>
        /// Invoked when SimplePeer.OnClose event fires
        /// </summary>
        public event Action<WebPeer> OnClose;
        /// <summary>
        /// Invoked when the SimplePeer has a signal to send to the remote SimplePeer via a signaler
        /// </summary>
        public event Action<WebPeer, string> OnSignal;
        /// <summary>
        /// Passes the signal message from the signaler interface to SimplePeer
        /// </summary>
        /// <param name="signalJson"></param>
        public void Signal(string signalJson)
        {
            Connection!.Signal(JSON.Parse(signalJson)!);
        }
        /// <summary>
        /// The underlying SimplePeer
        /// </summary>
        public SimplePeer? Connection { get; protected set; } = null;
        public WebPeer(IServiceProvider serviceProvider, SimplePeer dataConnection) : base(serviceProvider)
        {
            InitDataConnection(dataConnection);
        }
        public WebPeer(IServiceProvider serviceProvider, SimplePeerOptions simplePeerOptions) : base(serviceProvider)
        {
            simplePeerOptions.ObjectMode = null;
            var simplePeer = new SimplePeer(simplePeerOptions);
            InitDataConnection(simplePeer);
        }
        public WebPeer(IServiceProvider serviceProvider, bool initiator, bool trickle = false, RTCConfiguration? rtcConfig = null) : base(serviceProvider)
        {
            var simplePeerOptions = new SimplePeerOptions
            {
                Initiator = initiator,
                Trickle = trickle,
                Config = rtcConfig,
            };
            var simplePeer = new SimplePeer(simplePeerOptions);
            InitDataConnection(simplePeer);
        }
        /// <summary>
        /// Disposes resources
        /// </summary>
        public override void Dispose()
        {
            if (IsDisposed) return;
            if (Connection != null)
            {
                Connection.OnSignal -= SimplePeer_OnSignal;
                Connection.OnConnect -= SimplePeer_OnConnect;
                Connection.OnClose -= SimplePeer_OnClose;
                Connection.RemoveListener<Uint8Array>("data", DataConnection_OnData);
                Connection.Dispose();
            }
            base.Dispose();
        }
        private void SimplePeer_OnSignal(JSObject data)
        {
            var signalJson = JSON.Stringify(data);
            OnSignal?.Invoke(this, signalJson);
        }

        private void InitDataConnection(SimplePeer dataConnection)
        {
            Connection = dataConnection;
            Connection.OnSignal += SimplePeer_OnSignal;
            Connection.OnConnect += SimplePeer_OnConnect;
            Connection.OnClose += SimplePeer_OnClose;
            Connection.On<Uint8Array>("data", DataConnection_OnData);
        }
        private void Send(object?[] args)
        {
            using var uint8Array = MessagePack.MessagePack.Encode(args);
            Connection!.Write(uint8Array);
        }
        private void DataConnection_OnData(Uint8Array data)
        {
            var msg = MessagePack.MessagePack.Decode<Array>(data);
            data.Dispose();
            _ = HandleCall(msg);
        }
        private void SimplePeer_OnConnect()
        {
            SendReadyFlag();
        }
        private void SimplePeer_OnClose()
        {
            ResetWhenReady();
            OnClose?.Invoke(this);
        }
        protected override void SendCall(object?[] args) => Send(args);
    }
}
