using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar o suporte para JSON Patch usando Newtonsoft.Json
builder.Services.AddControllers().AddNewtonsoftJson();

// Configuração do DbContext com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração para Swagger e OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
