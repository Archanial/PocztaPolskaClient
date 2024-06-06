using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using poczta;
using poczta.Sledzenie;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Poczta API", Version = "v1" });
    option.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Input your username and password to access this API"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddSoapCore();
builder.Services.AddSingleton<SledzeniePortTypeClient>();
builder.Services.AddTransient<IPostClient, PostClient>();
builder.Services.AddTransient<IPostApi, PostApi>();
//builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
builder.Services.AddSoapWsSecurityFilter(builder.Configuration.GetSection("ApiUsername").Value,
    builder.Configuration.GetSection("ApiPassword").Value);
builder.Services.Configure<ApiCredentials>(builder.Configuration.GetSection("ApiCredentials"));
var allowedAddresses = builder.Configuration.GetSection("AllowedAddresses").Get<AllowedAddresses>();
builder.Services.AddSoapMessageProcessor(new WhitelistHandler(allowedAddresses ?? new AllowedAddresses
{
    IsEnabled = false,
    Addresses = []
}));

builder.Services.AddControllers();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<BasicAuthMiddleware>();

// Add credentials to the client
var credentials = app.Services.GetRequiredService<IOptions<ApiCredentials>>().Value;
var client = app.Services.GetRequiredService<SledzeniePortTypeClient>();
client.ClientCredentials.UserName.UserName = credentials.Username;
client.ClientCredentials.UserName.Password = credentials.Password;

app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseSoapEndpoint<IPostApi>(path: "/PostApi.svc", new SoapEncoderOptions());

app.Run();