using MemberSystem.Business.Interfaces;
using MemberSystem.Business.Services;
using MemberSystem.Domain.Entities;
using MemberSystem.Domain.Interfaces;
using MemberSystem.Infrastructure.Data;
using MemberSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext için MsSQL baðlantýsýný yapýlandýrýn.
builder.Services.AddDbContext<UserSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository ve Business servislerini enjekte edin.
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IUserService, UserService>();

// Controller’larý ekleyin.
builder.Services.AddControllers();

// Swagger/OpenAPI desteði ekleyin.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Geliþtirme ortamýnda Swagger’ý kullanýn.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
