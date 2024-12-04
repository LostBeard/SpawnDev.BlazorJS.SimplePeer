using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.MessagePack;
using SpawnDev.BlazorJS.SimplePeer;
using SpawnDev.BlazorJS.SimplePeer.Demo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// Add SpawnDev.BlazorJS interop
builder.Services.AddBlazorJSRuntime();
// Load the SimplePeer Javascript library. Can be called in a component instead if desired, or loaded using a <script> tag in the index.html
await SimplePeer.Init();
// MessagePack is used for the WebPeer demo
await MessagePackSerializer.Init();
// Run app using BlazorJSRunAsync extension method
await builder.Build().BlazorJSRunAsync();
