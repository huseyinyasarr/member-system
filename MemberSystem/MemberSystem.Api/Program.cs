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
using Microsoft.OpenApi.Models; // JwtTokenSettings modelini kullanabilmek i�in bu using ifadesini ekledik

var builder = WebApplication.CreateBuilder(args);

// JWT Ayarlar�n� Yap�land�rma (appsettings.json'dan okuma)
builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtTokenSettings"));

// JWT Kimlik Do�rulamas� Yap�land�rmas�
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
            ClockSkew = TimeSpan.Zero // �ste�e ba�l�: Token s�resinin hemen dolmas� i�in
        };
    });

// DbContext i�in MsSQL ba�lant�s�n� yap�land�r�n.
builder.Services.AddDbContext<UserSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository ve Business servislerini enjekte edin.
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUserRepository, UserRepository>(); // �zel UserRepository'i kaydettik
builder.Services.AddScoped<IUserService, UserService>();

// Controller�lar� ekleyin.
builder.Services.AddControllers();

// Swagger/OpenAPI deste�i ekleyin.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    // JWT Bearer kimlik do�rulamas� i�in g�venlik �emas� tan�mlama
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // G�venlik gereksinimlerini belirtme (hangi endpointlerin yetkilendirme gerektirdi�ini tan�mlar)
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

// Geli�tirme ortam�nda Swagger�� kullan�n.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Kimlik do�rulama middleware'ini ekleyin

app.UseAuthorization();  // Yetkilendirme middleware'ini ekleyin

app.MapControllers();

app.Run();