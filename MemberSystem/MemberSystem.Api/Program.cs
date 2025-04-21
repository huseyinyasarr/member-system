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

// JWT Ayarlarýný Yapýlandýrma (appsettings.json'dan okuma)
builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtTokenSettings"));

// JWT Kimlik Doðrulamasý Yapýlandýrmasý
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
            ClockSkew = TimeSpan.Zero // Ýsteðe baðlý: Token süresinin hemen dolmasý için
        };
    });

// DbContext için MsSQL baðlantýsýný yapýlandýrýn.
builder.Services.AddDbContext<UserSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository ve Business servislerini enjekte edin.
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Özel UserRepository'i kaydettik
builder.Services.AddScoped<IUserService, UserService>();

// Controller’larý ekleyin.
builder.Services.AddControllers();

// Swagger/OpenAPI desteði ekleyin.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    // JWT Bearer kimlik doðrulamasý için güvenlik þemasý tanýmlama
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Güvenlik gereksinimlerini belirtme (hangi endpointlerin yetkilendirme gerektirdiðini tanýmlar)
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
    options.WithOrigins("http://localhost:3000", "https://localhost:7137", "https://localhost:5094")
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// Geliþtirme ortamýnda Swagger’ý kullanýn.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Kimlik doðrulama middleware'ini ekleyin

app.UseAuthorization();  // Yetkilendirme middleware'ini ekleyin

app.MapControllers();

app.Run();