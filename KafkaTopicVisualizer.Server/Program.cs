using KafkaTopicVisualizer.Server.Streams;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSingleton<IConsumeTopics, VerificationDocumentConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseWebAssemblyDebugging();
}
app.UseStaticFiles();

app.UseBlazorFrameworkFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseGrpcWeb();

app.UseEndpoints(endpoints =>
{
	endpoints.MapGrpcService<InsuranceVerification>().EnableGrpcWeb();
	endpoints.MapFallbackToFile("index.html");
});

app.Run();
