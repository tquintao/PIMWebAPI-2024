using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Adicionar suporte para JSON Patch
// ================================
builder.Services.AddControllers().AddNewtonsoftJson();

// ================================
// Configuração do DbContext com PostgreSQL
// ================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================================
// Configuração do Swagger para documentação da API
// ================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================================
// Configuração de CORS para permitir acessos de qualquer origem
// ================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// ================================
// Configuração do pipeline de requisições HTTP
// ================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ativando o uso da política de CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
