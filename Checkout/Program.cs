using Steeltoe.Discovery.Client;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDiscoveryClient(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/tester", () => { return "Hello from tester in checkout";});

app.Run();
//https://localhost:7252;