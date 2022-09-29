using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using KafkaTopicVisualizer.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(services =>
{
	var config = services.GetRequiredService<IConfiguration>();
	var backendUrl = config["BackendUrl"];

	if (string.IsNullOrWhiteSpace(backendUrl))
	{
		var navManager = services.GetRequiredService<NavigationManager>();
		backendUrl = navManager.BaseUri;
	}

	var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

	return GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

await builder.Build().RunAsync();
