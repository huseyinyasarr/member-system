using System.Text;
using MemberSystem.Business.Interfaces;
using MemberSystem.Business.Services;
using MemberSystem.Domain.Entities;
using MemberSystem.Domain.Interfaces;
using MemberSystem.Infrastructure.Data;
using MemberSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// DbContext i�in MsSQL ba�lant�s�n� yap�land�r�n.
builder.Services.AddDbContext<UserSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository ve Business servislerini enjekte edin.
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUserService, UserService>();

// Controller�lar� ekleyin.
builder.Services.AddControllers();

// Swagger/OpenAPI deste�i ekleyin.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("User")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new ()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
        };
    });

var app = builder.Build();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:3000", "https://localhost:5001")
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
