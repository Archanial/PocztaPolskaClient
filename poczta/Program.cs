using poczta;
using poczta.Sledzenie;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSoapCore();
builder.Services.AddSingleton<SledzeniePortTypeClient>();
builder.Services.AddTransient<IPostClient, PostClient>();
builder.Services.AddTransient<IPostApi, PostApi>();
builder.Services.AddSoapWsSecurityFilter("sledzeniepp", "PPSA");

var app = builder.Build();

// Add credentials to the client
var client = app.Services.GetRequiredService<SledzeniePortTypeClient>();
client.ClientCredentials.UserName.UserName = "sledzeniepp";
client.ClientCredentials.UserName.Password = "PPSA";

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSoapEndpoint<IPostApi>(path: "/PostApi.svc", new SoapEncoderOptions());

app.MapGet("/welcome", () => app.Services.GetRequiredService<IPostApi>().GetWelcomeMessage("test"))
    .WithName("welcome")
    .WithOpenApi();
app.MapGet("/version", () => app.Services.GetRequiredService<IPostApi>().GetVersion())
    .WithName("version")
    .WithOpenApi();
app.MapPost("/checkLocalShipments",
        (string[] numbers) => app.Services.GetRequiredService<IPostApi>().CheckLocalShipments(numbers))
    .WithName("checkLocalShipments")
    .WithOpenApi();
app.MapPost("/checkShipments", 
        (string[] numbers) => app.Services.GetRequiredService<IPostApi>().CheckShipments(numbers))
    .WithName("checkShipments")
    .WithOpenApi();
app.MapGet("/checkSingleShipment",
        (string number) => app.Services.GetRequiredService<IPostApi>().CheckSingleShipment(number))
    .WithName("checkSingleShipment")
    .WithOpenApi();
app.MapPost("/checkSingleLocalShipment",
        (string number) => app.Services.GetRequiredService<IPostApi>().CheckSingleLocalShipment(number))
    .WithName("checkSingleLocalShipment")
    .WithOpenApi();
app.MapPost("/checkShipmentsByDate",
        (string[] numbers, DateTime startDate, DateTime endDate) => 
            app.Services.GetRequiredService<IPostApi>().CheckShipmentsByDate(numbers, startDate, endDate))
    .WithName("checkShipmentsByDate")
    .WithOpenApi();
app.MapPost("/checkLocalShipmentsByDate",
        (string[] numbers, DateTime startDate, DateTime endDate) => 
            app.Services.GetRequiredService<IPostApi>().CheckLocalShipmentsByDate(numbers, startDate, endDate))
    .WithName("checkLocalShipmentsByDate")
    .WithOpenApi();
app.MapGet("/getMaxShipments", () => app.Services.GetRequiredService<IPostApi>().GetMaxShipments())
    .WithName("getMaxShipments")
    .WithOpenApi();

app.Run();