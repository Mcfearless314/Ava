using System.Security.Claims;
using System.Text;
using Ava.API.Authorization;
using Ava.API.Configuration;
using Infrastructure.Configuration;
using Infrastructure.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Configuration;
using Service.Services.Security;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);


    builder.Configuration.AddEnvironmentVariables(prefix: "AVA_");
    builder.Services.AddControllers();
    builder.Services.AddAuthorization();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
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


    builder.Services.AddAuthorization(options =>
    {
      options.AddPolicy("MustBeProjectManager", policy =>
      {
        policy.Requirements.Add(new MustBeProjectManagerRequirement());
      });
    });

    builder.Services.AddScoped<IAuthorizationHandler, MustBeProjectManagerHandler>();

    var app = builder.Build();


    Infrastructure.SeedData.DbSeeder.Seed(app.Services);

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
  }
}
