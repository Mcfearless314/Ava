using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Ava.API.Authorization;
using Ava.API.Authorization.Policies;
using Ava.API.Configuration;
using Ava.API.Middleware;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Configuration;
using Service.Services.Security;

namespace Ava.API;

public static class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables(prefix: "AVA_");
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAuthorization();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DBConnection"));
    builder.Services.AddService();

    var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>()!;

    builder.Services.AddScoped<JwtTokenService>(sp =>
      new JwtTokenService(jwtSettings.Secret, jwtSettings.Issuer, jwtSettings.Audience));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          NameClaimType = ClaimTypes.NameIdentifier,
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
      });

    builder.Services.AddSwaggerGen(c =>
    {
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme, Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
          },
          new List<string>()
        }
      });
    });

    builder.Services.AddAuthorization(options =>
    {
      options.AddPolicy("MustBeProjectManager",
        policy => { policy.Requirements.Add(new MustBeProjectManagerRequirement()); });

      options.AddPolicy("MustBeAdmin",
        policy => { policy.Requirements.Add(new MustBeAdminRequirement()); });

      options.AddPolicy("MustBeProjectUser",
        policy => { policy.Requirements.Add(new MustBeProjectUserRequirement()); });

      options.AddPolicy("MustBeAdminOrProjectManager",
        policy => { policy.Requirements.Add(new MustBeAdminOrProjectManagerRequirement()); });

      options.AddPolicy("MustBeAdminOrProjectUser",
        policy => { policy.Requirements.Add(new MustBeAdminOrProjectUserRequirement()); });

      options.AddPolicy("MustBePartOfOrganisation",
        policy => { policy.Requirements.Add(new MustBePartOfOrganisationRequirement()); });
    });

    builder.Services.AddScoped<IAuthorizationHandler, MustBePartOfOrganisationHandler>();

    builder.Services.AddScoped<IAuthorizationHandler, MustBeProjectManagerHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, MustBeAdminHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, MustBeProjectUserHandler>();


    builder.Services.AddScoped<MustBeProjectUserHandler>();
    builder.Services.AddScoped<MustBeAdminHandler>();
    builder.Services.AddScoped<MustBeProjectManagerHandler>();

    builder.Services.AddScoped<IAuthorizationHandler, MustBeAdminOrProjectManagerHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, MustBeAdminOrProjectUserHandler>();

    builder.Services.AddRateLimiter(options =>
    {
      options.AddPolicy("LoginLimiter", context =>
      {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
          PermitLimit = 5,
          Window = TimeSpan.FromMinutes(1),
          QueueLimit = 0
        });
      });
      options.OnRejected = async (context, token) =>
      {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
          "{\"error\": \"Too many requests. Please wait and try again.\"}",
          token);
      };

      options.AddPolicy("RegisterLimiter", context =>
      {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
          PermitLimit = 5,
          Window = TimeSpan.FromMinutes(30),
          QueueLimit = 0
        });
      });
      options.OnRejected = async (context, token) =>
      {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
          "{\"error\": \"Too many requests. Please wait and try again.\"}",
          token);
      };

      options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
      {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
          PermitLimit = 25,
          Window = TimeSpan.FromMinutes(1),
          QueueLimit = 0
        });
      });

      options.OnRejected = async (context, token) =>
      {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
          "{\"error\": \"Too many requests. Please wait and try again later.\"}",
          token);
      };
    });

    var app = builder.Build();


    Infrastructure.SeedData.DbSeeder.Seed(app.Services);

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseMiddleware<GlobalExceptionHandler>();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();

    app.MapControllers();

    app.Run();
  }
}
