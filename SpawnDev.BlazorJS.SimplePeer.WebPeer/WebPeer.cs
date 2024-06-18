using Microsoft.Extensions.DependencyInjection;
using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.JSObjects.WebRTC;
using SpawnDev.BlazorJS.Reflection;
using System.Reflection;
using Array = SpawnDev.BlazorJS.JSObjects.Array;

namespace SpawnDev.BlazorJS.SimplePeer.WebPeer
{
    /// <summary>
    /// Client and server implementation for remotely calling .Net methods using SimplePeer
    /// </summary>
    public class WebPeer : RemoteDispatcher, IDisposable
    {
        public event Action<WebPeer> OnClose;
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
                Connection.OnSignal -= DataConnection_OnSignal;
                Connection.OnConnect -= DataConnection_OnOpen;
                Connection.OnClose -= DataConnection_OnClose;
                Connection.OnError -= DataConnection_OnError;
                Connection.RemoveListener<Uint8Array>("data", DataConnection_OnData);
                Connection.Dispose();
            }
            base.Dispose();
        }
        private void DataConnection_OnSignal(JSObject data)
        {
            var signalJson = JSON.Stringify(data);
            OnSignal?.Invoke(this, signalJson);
        }

        private void InitDataConnection(SimplePeer dataConnection)
        {
            Connection = dataConnection;
            Connection.OnSignal += DataConnection_OnSignal;
            Connection.OnConnect += DataConnection_OnOpen;
            Connection.OnClose += DataConnection_OnClose;
            Connection.OnError += DataConnection_OnError;
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
        private void DataConnection_OnOpen()
        {
            SendReadyFlag();
        }
        private void DataConnection_OnClose()
        {
            ResetWhenReady();
            OnClose?.Invoke(this);
        }
        private void DataConnection_OnError(NodeError error)
        {
            //Log($"DataConnection_OnError: {error.Type}");
        }
        protected override Task<string?> CanCallCheck(MethodInfo methodInfo, RemoteCallableAttribute? remoteCallableAttr, ServiceDescriptor? info, object? instance)
        {
            return base.CanCallCheck(methodInfo, remoteCallableAttr, info, instance);
        }
        protected override Task<string?> PreCallCheck(MethodInfo methodInfo, object?[]? args = null)
        {
            return base.PreCallCheck(methodInfo, args);
        }
        protected override Task<object?> ResolveLocalInstance(Type parameterType)
        {
            return base.ResolveLocalInstance(parameterType);
        }
        protected override void SendCall(object?[] args) => Send(args);
    }
}
