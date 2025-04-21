using System.Text;
using MemberSystem.Business.Interfaces;
using MemberSystem.Business.Services;
using MemberSystem.Domain.Entities;
using MemberSystem.Domain.Interfaces;
using MemberSystem.Infrastructure.Data;
using MemberSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MemberSystem.Api.Modeller;
using Microsoft.OpenApi.Models; // JwtTokenSettings modelini kullanabilmek için bu using ifadesini ekledik

var builder = WebApplication.CreateBuilder(args);

// JWT Ayarlarını Yapılandırma (appsettings.json'dan okuma)
//builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtTokenSettings"));

builder.Services.Configure<MemberSystem.Api.Modeller.JwtTokenSettings>(builder.Configuration.GetSection("JwtTokenSettings"));

// JWT Kimlik Doğrulaması Yapılandırması
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtTokenSettings = builder.Configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtTokenSettings.Issuer,
            ValidAudience = jwtTokenSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero // İsteğe bağlı: Token süresinin hemen dolması için
        };
    });

// DbContext için MsSQL bağlantısını yapılandırın.
builder.Services.AddDbContext<UserSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository ve Business servislerini enjekte edin.
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Özel UserRepository'i kaydettik
builder.Services.AddScoped<IUserService, UserService>();

// Controller’ları ekleyin.
builder.Services.AddControllers();

// Swagger/OpenAPI desteği ekleyin.
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Güvenlik gereksinimlerini belirtme (hangi endpointlerin yetkilendirme gerektirdiğini tanımlar)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:3000", "https://localhost:5001")
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// Geliştirme ortamında Swagger’ı kullanın.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Kimlik doğrulama middleware'ini ekleyin

app.UseAuthorization();  // Yetkilendirme middleware'ini ekleyin

app.MapControllers();

app.Run();

