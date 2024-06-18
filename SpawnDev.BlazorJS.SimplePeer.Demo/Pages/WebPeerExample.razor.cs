using Microsoft.AspNetCore.Components;
using SpawnDev.BlazorJS.JSObjects;
using SpawnDev.BlazorJS.Reflection;
using SpawnDev.BlazorJS.SimplePeer.WebPeer;

namespace SpawnDev.BlazorJS.SimplePeer.Demo.Pages
{
    public partial class WebPeerExample
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] BlazorJSRuntime JS { get; set; }
        [Inject] IServiceProvider ServiceProvider { get; set; }

        string peerRole => webPeer == null ? "(select)" : (webPeer.Connection!.Initiator ? "initiator" : "receiver");
        WebPeer.WebPeer? webPeer = null;
        string outgoing = "";
        string incoming = "";
        string log = "";

        void Init(bool initiator)
        {
            webPeer = new WebPeer.WebPeer(ServiceProvider, initiator);
            webPeer.OnSignal += SimplePeer_OnSignal;
            webPeer.Connection!.OnError += SimplePeer_OnError;
            webPeer.Connection.OnConnect += SimplePeer_OnConnect;
            webPeer.Connection.OnClose += SimplePeer_OnClose;

            // this lets the static method call the instance method
            InstanceConsoleLog = Log;
        }

        void Submit()
        {
            webPeer!.Signal(incoming);
        }

        async Task CallMethodTest()
        {
            if (webPeer == null) return;
            try
            {
                var logIgRet = await webPeer!.Run(() => ConsoleLog("remote log", null));
                Log($"logIgRet: {logIgRet}");
            }
            catch (Exception ex)
            {
                JS.Log($"CallMethodTest error: {ex.Message}");
            }
        }

        void Log(string msg)
        {
            log += $"{msg}<br/>";
            StateHasChanged();
        }

        static long logId = 0;

        [RemoteCallable]
        static long ConsoleLog(string msg, [FromLocal] WebPeer.WebPeer? peer = null)
        {
            InstanceConsoleLog?.Invoke($"ConsoleLog: {logId} {msg}");
            return logId++;
        }

        static Action<string>? InstanceConsoleLog = null;

        void SimplePeer_OnConnect()
        {
            JS.Log("CONNECT");
            outgoing = "Connected<br/>";
            StateHasChanged();
        }

        void SimplePeer_OnClose()
        {
            JS.Log("CLOSE");
            outgoing += "Closed<br/>";
            StateHasChanged();
        }

        void SimplePeer_OnSignal(WebPeer.WebPeer webPeer,string signalJson)
        {
            outgoing = signalJson;
            StateHasChanged();
        }

        void SimplePeer_OnError(NodeError error)
        {
            outgoing = error.Code! + "<br/>";
            StateHasChanged();
        }

        public void Dispose()
        {
            if (webPeer != null)
            {
                webPeer.Connection.OnError -= SimplePeer_OnError;
                webPeer.OnSignal -= SimplePeer_OnSignal;
                webPeer.Connection.OnConnect -= SimplePeer_OnConnect;
                webPeer.Connection.OnClose -= SimplePeer_OnClose;
                webPeer.Dispose();
                webPeer = null;
            }
            InstanceConsoleLog = null;
        }
    }
}
