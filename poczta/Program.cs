using Microsoft.Extensions.Options;
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
builder.Services.AddSoapWsSecurityFilter(builder.Configuration.GetSection("Username").Value,
    builder.Configuration.GetSection("Password").Value);
builder.Services.Configure<ApiCredentials>(builder.Configuration.GetSection("ApiCredentials"));

var app = builder.Build();

// Add credentials to the client
var credentials = app.Services.GetRequiredService<IOptions<ApiCredentials>>().Value;
var client = app.Services.GetRequiredService<SledzeniePortTypeClient>();
client.ClientCredentials.UserName.UserName = credentials.Username;
client.ClientCredentials.UserName.Password = credentials.Password;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSoapEndpoint<IPostApi>(path: "/PostApi.svc", new SoapEncoderOptions());

app.MapGet("/welcome", (string name) => app.Services.GetRequiredService<IPostApi>().GetWelcomeMessage(name))
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

app.MapPost("/getSingleShipmentByBarCode", (byte[] imageDate) =>
        app.Services.GetRequiredService<IPostApi>().GetSingleShipmentByBarCode(imageDate))
    .WithName("getSingleShipmentByBarCode")
    .WithOpenApi();

app.Run();