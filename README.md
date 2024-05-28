# SpawnDev.BlazorJS.SimplePeer

[![NuGet version](https://badge.fury.io/nu/SpawnDev.BlazorJS.SimplePeer.svg?label=SpawnDev.BlazorJS.SimplePeer)](https://www.nuget.org/packages/SpawnDev.BlazorJS.SimplePeer)

**SpawnDev.BlazorJS.SimplePeer** brings the amazing [SimplePeer](https://github.com/feross/simple-peer) library to Blazor WebAssembly.

**SpawnDev.BlazorJS.SimplePeer** uses [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) for Javascript interop allowing strongly typed, full usage of the [SimplePeer](https://github.com/feross/simple-peer) Javascript library. Voice, video and data channels are all fully supported in Blazor WebAssembly.

### Demo
[Live Demo](https://lostbeard.github.io/SpawnDev.BlazorJS.SimplePeer/)

### Getting started

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
```html
@page "/ManualConnectExample"
@using SpawnDev.BlazorJS.JSObjects;
@using System.Text;
@using System.Text.Json;

<PageTitle>Counter</PageTitle>

<h1>SimplePeer Test</h1>

<p>
    This test lets you manually connect two peers. This example is meant to mirror the original. Some standard Blazor usage styles or ignored to keep the examples closer. <a href="https://github.com/feross/simple-peer?tab=readme-ov-file#usage">Original Example</a>
</p>

<div>
    Role: @PeerRole<br />
    <button disabled="@(peer != null)" class="btn btn-primary" @onclick="@(()=>Init(true))">Create Initiator</button>
    <button disabled="@(peer != null)" class="btn btn-primary" @onclick="@(()=>Init(false))">Create Receiver</button>
</div>

<style>
    #outgoing {
        width: 600px;
        word-wrap: break-word;
        white-space: normal;
    }
</style>
<form>
    <textarea id="incoming"></textarea>
    <button disabled="@(peer == null)" type="submit">submit</button>
</form>
<pre id="outgoing"></pre>

@code {
    [Inject] BlazorJSRuntime JS { get; set; }

    string PeerRole = "(select)";
    SimplePeer? peer = null;

    async Task Init(bool initiator)
    {
        PeerRole = initiator ? "initiator" : "receiver";
        var document = JS.Get<Document>("document");
        peer = new SimplePeer(new SimplePeerOptions
            {
                Initiator = initiator,
                Trickle = false,
            }
        );

        peer.On<JSObject>("error", err => JS.Log("error", err));

        peer.On<JSObject>("signal", data =>
        {
            JS.Log("SIGNAL", JSON.Stringify(data));
            document.QuerySelector("#outgoing")!.TextContent = JSON.Stringify(data);
        });

        document.QuerySelector("form")!.AddEventListener<Event>("submit", ev =>
        {
            ev.PreventDefault();
            var json = document.QuerySelector<HTMLInputElement>("#incoming")!.Value;
            peer.Signal(JSON.Parse(json)!);
        });

        peer.On("connect", () =>
        {
            JS.Log("CONNECT");
            peer.Send("Hello " + Guid.NewGuid().ToString());
        });

        peer.On<NodeBuffer>("data", data =>
        {
            JS.Log("data:", data);
            document.QuerySelector("#outgoing")!.TextContent = Encoding.UTF8.GetString((byte[])data!);
        });
    }
}
```
