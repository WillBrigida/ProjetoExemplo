using Core;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RegisterServices();
builder.Services.RegisterViewModels();

await builder.Build().RunAsync();
