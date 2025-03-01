using Microsoft.OpenApi.Models;
using UserManagement.Authentication;
using UserManagement.Services;
using UserManagement.SetupOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(o => o.LowercaseUrls = true);

builder.Services.AddAuthentication().AddJwtBearer();
builder
    .Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "UserManagement", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid JWT token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer",
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

var app = builder.Build();
app.Use(
    async (context, next) =>
    {
        var authHeader = context.Request.Headers.Authorization;
        Console.WriteLine($"Authorization header: {authHeader}");
        await next();
    }
);
app.Use(
    async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception)
        {
            Console.WriteLine("An error occurred.");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("An error occurred.");
        }
    }
);

app.Use(
    async (context, next) =>
    {
        var path = context.Request.Path;
        var method = context.Request.Method;
        Console.WriteLine($"Request: {method}: {path}");
        await next();
        var statusCode = context.Response.StatusCode;
        Console.WriteLine($"Response: {statusCode}");
    }
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
