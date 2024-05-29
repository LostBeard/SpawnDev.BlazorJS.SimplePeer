# SpawnDev.BlazorJS.SimplePeer

[![NuGet version](https://badge.fury.io/nu/SpawnDev.BlazorJS.SimplePeer.svg?label=SpawnDev.BlazorJS.SimplePeer)](https://www.nuget.org/packages/SpawnDev.BlazorJS.SimplePeer)

**SpawnDev.BlazorJS.SimplePeer** brings the amazing [SimplePeer](https://github.com/feross/simple-peer) library to Blazor WebAssembly.

**SpawnDev.BlazorJS.SimplePeer** uses [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) for Javascript interop allowing strongly typed, full usage of the [SimplePeer](https://github.com/feross/simple-peer) Javascript library. Voice, video and data channels are all fully supported in Blazor WebAssembly. The **SpawnDev.BlazorJS.SimplePeer** API is a strongly typed version of the API found on the [SimplePeer](https://github.com/feross/simple-peer?tab=readme-ov-file#api) repo. 

### Demo
[Simple Demo](https://lostbeard.github.io/SpawnDev.BlazorJS.SimplePeer/)

### Getting started

Add the Nuget package ```SpawnDev.BlazorJS.SimplePeer``` to your project using your package manager of choice.

Modify the Blazor WASM ```Program.cs``` to initialize SpawnDev.BlazorJS for Javascript interop.  
Example Program.cs 
```cs
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.SimplePeer;
using SpawnDev.BlazorJS.SimplePeer.Demo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// Add SpawnDev.BlazorJS interop
builder.Services.AddBlazorJSRuntime();
// Load the SimplePeer Javascript library. Can be called in a component instead if desired.
await SimplePeer.Init();
// Run app using BlazorJSRunAsync extension
await builder.Build().BlazorJSRunAsync();
```

ManualConnectExample.razor  
Modified version of this [SimplePer usage example](https://github.com/feross/simple-peer?tab=readme-ov-file#usage)
```cs
@page "/ManualConnectExample"
@using SpawnDev.BlazorJS.JSObjects;
@using System.Text;
@using System.Text.Json;
@implements IDisposable

<PageTitle>Counter</PageTitle>

<h1>SimplePeer Test</h1>

<p>
    This test lets you manually connect two peers by copying and pasting the signal messages. This example is meant to mirror the original. <a href="https://github.com/feross/simple-peer?tab=readme-ov-file#usage">Original Example</a>
</p>

<div>
    Role: @PeerRole<br />
    <button disabled="@(peer != null)" class="btn btn-primary" @onclick="@(()=>Init(true))">Create Initiator</button>
    <button disabled="@(peer != null)" class="btn btn-primary" @onclick="@(()=>Init(false))">Create Receiver</button>
</div>

<div>
    <textarea style="width: 600px; word-wrap: break-word; white-space: normal;" @bind=@incoming></textarea>
    <button disabled="@(peer == null)" @onclick=@Submit>submit</button>
</div>
<pre style="width: 600px; word-wrap: break-word; white-space: normal;">@outgoing</pre>

@code {
    [Inject] BlazorJSRuntime JS { get; set; }

    string PeerRole => peer == null ? "(select)" : (peer.Initiator ? "initiator" : "receiver");
    SimplePeer? peer = null;
    Document? document = null;
    string outgoing = "";
    string incoming = "";

    void Init(bool initiator)
    {
        peer = new SimplePeer(new SimplePeerOptions
            {
                Initiator = initiator,
                Trickle = false,
            }
        );

        peer.OnError += SimplePeer_OnError;
        peer.OnSignal += SimplePeer_OnSignal;
        peer.OnConnect += SimplePeer_OnConnect;
        peer.OnClose += SimplePeer_OnClose;
        peer.OnData += SimplePeer_OnData;
    }

    void Submit()
    {
        peer!.Signal(JSON.Parse(incoming)!);
    }

    void SimplePeer_OnConnect()
    {
        JS.Log("CONNECT");
        peer!.Send("Hello " + Guid.NewGuid().ToString());
    }

    void SimplePeer_OnClose()
    {
        JS.Log("CLOSE");
    }

    void SimplePeer_OnSignal(JSObject data)
    {
        JS.Log("SIGNAL", JSON.Stringify(data));
        outgoing = JSON.Stringify(data);
        StateHasChanged();
    }

    void SimplePeer_OnError(NodeError error)
    {
        outgoing = error.Code!;
        StateHasChanged();
    }

    void SimplePeer_OnData(NodeBuffer data)
    {
        outgoing = Encoding.UTF8.GetString((byte[])data!);
        StateHasChanged();
    }

    public void Dispose()
    {
        if (peer != null)
        {
            peer.OnError -= SimplePeer_OnError;
            peer.OnSignal -= SimplePeer_OnSignal;
            peer.OnConnect -= SimplePeer_OnConnect;
            peer.OnClose -= SimplePeer_OnClose;
            peer.OnData -= SimplePeer_OnData;
            peer.Destroy();
            peer.Dispose();
            peer = null;
        }
        if (document != null)
        {
            document.Dispose(); 
            document = null;
        }
    }
}
```
