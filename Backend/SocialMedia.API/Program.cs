using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.API.Mapper;
using SocialMedia.API.Middlewares;
using SocialMedia.Application.Abstractions;
using SocialMedia.Application.Hubs;
using SocialMedia.Application.Implementations;
using SocialMedia.Core.Context;
using SocialMedia.Core.Hubs;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(PostMapper));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter Bearer Token "
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddScoped<INotificationsService , NotificationsService>();


var connectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<AppdbContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddInfrastructureService();
builder.Services.AddApplicationService();

//builder.Services.AddIdentity<User, Role>()//so important it overrdide addAuthetnications and support cookies so jwt doesnt work correctly
//    .AddEntityFrameworkStores<AppdbContext>()
//    .AddDefaultTokenProviders();
builder.Services.AddIdentityCore<User>(options =>
{
  
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<Role>()
.AddEntityFrameworkStores<AppdbContext>()
.AddDefaultTokenProviders();
builder.Services.AddHostedService<StoryCleanupService>();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ITokenBlocklistService, TokenBlocklistService>();
Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

var jwtSettings = builder.Configuration.GetSection("JWT");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken =context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken)&& path.StartsWithSegments("/chatHub"))
                {
                    context.Token = accessToken;
                    Console.WriteLine($"Token extracted...");
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Auth failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token valid for: {context.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddScoped<IChatService, ChatService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(policy =>
{
    policy.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .SetIsOriginAllowed(_ => true);
});


app.Use(async (context, next) =>
{
    context.Response.Headers["Cross-Origin-Opener-Policy"] = "unsafe-none";
    context.Response.Headers["Cross-Origin-Embedder-Policy"] = "unsafe-none";
    await next();
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<TokenBlocklistMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");//important to be after authentication to read the jwt token
app.MapHub<NotificationsHub>("/notificationHub");

app.Run();
