# SpawnDev.BlazorJS.SimplePeer

[![NuGet version](https://badge.fury.io/nu/SpawnDev.BlazorJS.SimplePeer.svg?label=SpawnDev.BlazorJS.SimplePeer)](https://www.nuget.org/packages/SpawnDev.BlazorJS.SimplePeer)

**SpawnDev.BlazorJS.SimplePeer** brings the amazing [simple-peer](https://github.com/feross/simple-peer) library to Blazor WebAssembly.

**SpawnDev.BlazorJS.SimplePeer** uses [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) for Javascript interop allowing strongly typed, full usage of the [simple-peer](https://github.com/feross/simple-peer) Javascript library. Voice, video and data channels are all fully supported in Blazor WebAssembly. The **SpawnDev.BlazorJS.SimplePeer** API is a strongly typed version of the API found at the [simple-peer](https://github.com/feross/simple-peer?tab=readme-ov-file#api) repo. 

### Demo
[Simple Demo](https://lostbeard.github.io/SpawnDev.BlazorJS.SimplePeer/)

### Getting started

Add the Nuget package `SpawnDev.BlazorJS.SimplePeer` to your project using your package manager of choice.

Modify the Blazor WASM `Program.cs` to initialize SpawnDev.BlazorJS for Javascript interop.  
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
// Load the SimplePeer Javascript library. Can be called in a component instead if desired, or loaded using a <script> tag in the index.html
await SimplePeer.Init();
// Run app using BlazorJSRunAsync extension method
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

<PageTitle>SimplePeer Test</PageTitle>

<h1>SimplePeer Test</h1>

<p>
    An "offer" will be generated by the initiator. Paste this into the receiver's form and
hit submit. The receiver generates an "answer". Paste this into the initiator's form and
hit submit. <a href="https://github.com/feross/simple-peer?tab=readme-ov-file#usage">Original Example</a>
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

An "offer" will be generated by the initiator. Paste this into the receiver's form and
hit submit. The receiver generates an "answer". Paste this into the initiator's form and
hit submit.

Now you have a direct P2P connection between two browsers!

## API

### peer = new SimplePeer([SimplePeerOptions opts])


Create a new WebRTC peer connection.

A "data channel" for text/binary communication is always established, because it's cheap and often useful. For video/voice communication, pass the `stream` option.

If `opts` is specified, then the default options (shown below) will be overridden.

```
{
  Initiator: false,
  ChannelConfig: {},
  ChannelName: '<random string>',
  Config: { iceServers: [{ urls: 'stun:stun.l.google.com:19302' }, { urls: 'stun:global.stun.twilio.com:3478?transport=udp' }] },
  OfferOptions: {},
  AnswerOptions: {},
  Stream: false,
  Streams: [],
  Trickle: true,
  AllowHalfTrickle: false,
  ObjectMode: false
}
```

The SimplePeerOptions properties do the following:

- `Initiator` - `bool` set to `true` if this is the initiating peer
- `ChannelConfig` - `RTCDataChannelOptions` custom webrtc data channel configuration (used by [`createDataChannel`](https://developer.mozilla.org/en-US/docs/Web/API/RTCPeerConnection/createDataChannel))
- `ChannelName` - `string` custom webrtc data channel name
- `Config` - `RTCConfiguration` custom webrtc configuration (used by [`RTCPeerConnection`](https://developer.mozilla.org/en-US/docs/Web/API/RTCPeerConnection) constructor)
- `OfferOptions` - `RTCOfferOptions` custom offer options (used by [`createOffer`](https://developer.mozilla.org/en-US/docs/Web/API/RTCPeerConnection/createOffer) method)
- `AnswerOptions` - `RTCAnswerOptions` custom answer options (used by [`createAnswer`](https://developer.mozilla.org/en-US/docs/Web/API/RTCPeerConnection/createAnswer) method)
- `Stream` - `MediaStream` if video/voice is desired, pass stream returned from [`getUserMedia`](https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia)
- `Streams` - `MediaStream[]` an array of MediaStreams returned from [`getUserMedia`](https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia)
- `Trickle` - `bool` set to `false` to disable [trickle ICE](http://webrtchacks.com/trickle-ice/) and get a single 'signal' event (slower)
- `ObjectMode` - `bool` set to `true` to create the stream in [Object Mode](https://nodejs.org/api/stream.html#stream_object_mode). In this mode, incoming string data is not automatically converted to `NodeBuffer` objects.

## Methods

### `peer.Signal(object data)`

Call this method whenever the remote peer emits a `peer.OnSignal` event.

The `data` will encapsulate a webrtc offer, answer, or ice candidate. These messages help
the peers to eventually establish a direct connection to each other. The contents of these
messages are an implementation detail that can be ignored by the user of this module;
simply pass the data from 'signal' events to the remote peer and call `peer.signal(data)`
to get connected.

### `peer.Send(string/TypedArray/ArrayBuffer/Blob/byte[] data)`

Send text/binary data to the remote peer. `data` can be any of several types: `string`,
`Buffer` (see [buffer](https://github.com/feross/buffer)), `TypedArray` (`Uint8Array`,
etc.), `ArrayBuffer`, or `Blob` (in browsers that support it).

Note: If this method is called before the `peer.OnConnect` event has fired, then an exception will be thrown. Use `peer.Write(data)` (which is inherited from the node.js [duplex stream](http://nodejs.org/api/stream.html) interface) if you want this data to be buffered instead.

### `peer.AddStream(MediaStream stream)`

Add a `MediaStream` to the connection.

### `peer.RemoveStream(MediaStream stream)`

Remove a `MediaStream` from the connection.

### `peer.AddTrack(MediaStreamTrack track, MediaStream stream)`

Add a `MediaStreamTrack` to the connection. Must also pass the `MediaStream` you want to attach it to.

### `peer.RemoveTrack(MediaStreamTrack track, MediaStream stream)`

Remove a `MediaStreamTrack` from the connection. Must also pass the `MediaStream` that it was attached to.

### `peer.ReplaceTrack(MediaStreamTrack oldTrack, MediaStreamTrack newTrack, MediaStream stream)`

Replace a `MediaStreamTrack` with another track. Must also pass the `MediaStream` that the old track was attached to.

### `peer.AddTransceiver(string kind, RTCRtpTransceiverOptions init)`

Add a `RTCRtpTransceiver` to the connection. Can be used to add transceivers before adding tracks. Automatically called as necessary by `AddTrack`.

### `peer.Destroy([NodeError err])`

Destroy and cleanup this peer connection.

If the optional `err` parameter is passed, then it will be emitted as an `'error'`
event on the stream.

### `SimplePeer.WEBRTC_SUPPORT`

Detect native WebRTC support in the javascript environment.

```cs
if (SimplePeer.WEBRTC_SUPPORT) {
  // webrtc support!
} else {
  // fallback
}
```

## Events

**Note:** Registered event handlers need to be unregistered (`-=` or `RemoveListener`) when they are no longer needed to prevent memory leaks. Lambda event handlers are used here to keep the examples simple.

`SimplePeer` inherits from `EventEmitter`. Event handlers can be added using `JSEventCallback` and `+=/-=` operators or using `EventEmitter.On` and `EventEmitter.RemoveListener` methods.

### `peer.OnSignal += (JSObject data) => {}`

Fired when the peer wants to send signaling data to the remote peer.

**It is the responsibility of the application developer (that's you!) to get this data to
the other peer.** This usually entails using a websocket signaling server. This data is an
`Object`, so  remember to call `JSON.Stringify(data)` to serialize it first. Then, simply
call `peer.Signal(data)` on the remote peer.

(Be sure to listen to this event immediately to avoid missing it. For `Initiator = true`
peers, it fires right away. For `Initiator = false` peers, it fires when the remote
offer is received.)

### `peer.OnConnect += () => {}`

Fired when the peer connection and data channel are ready to use.

### `peer.OnData += (NodeBuffer data) => {}`

Received a message from the remote peer (via the data channel). 

For `ObjectMode = false` peers (default) `data` is a `NodeBuffer`. For `ObjectMode = true`
peers, `data` can be a `string` or a `NodeBuffer`.

### `peer.OnStream += (MediaStream stream) => {}`

Received a remote video stream, which can be displayed in a video tag:

```cs
peer.OnStream += stream => {
    using var document = JS.Get<Document>("document");
    using var video = document.QuerySelector<HTMLVideoElement>("video");
    video.SrcObject = stream;
    video.Play();
};
```

### `peer.OnTrack += (MediaStreamTrack track, MediaStream stream) => {}`

Received a remote audio/video track. Streams may contain multiple tracks.

### `peer.OnClose += () => {}`

Called when the peer connection has closed.

### `peer.OnError += (NodeError err) => {}`

Fired when a fatal error occurs. Usually, this means bad signaling data was received from the remote peer.

`err` is a `NodeError` object.

## error codes

Errors returned by the `error` event have an `err.Code` property that will indicate the origin of the failure. Constants for these errors can be found in the class `SimplePeer.ErrorCodes`.

Possible error codes:
- `ERR_WEBRTC_SUPPORT`
- `ERR_CREATE_OFFER`
- `ERR_CREATE_ANSWER`
- `ERR_SET_LOCAL_DESCRIPTION`
- `ERR_SET_REMOTE_DESCRIPTION`
- `ERR_ADD_ICE_CANDIDATE`
- `ERR_ICE_CONNECTION_FAILURE`
- `ERR_SIGNALING`
- `ERR_DATA_CHANNEL`
- `ERR_CONNECTION_FAILURE`







































