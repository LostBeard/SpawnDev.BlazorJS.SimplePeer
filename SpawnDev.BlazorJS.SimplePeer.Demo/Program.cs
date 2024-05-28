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
