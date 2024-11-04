using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Adicionar suporte para JSON Patch
// ================================
builder.Services.AddControllers().AddNewtonsoftJson();

// ================================
// Configura��o do DbContext com PostgreSQL
// ================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================================
// Configura��o do Swagger para documenta��o da API
// ================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================================
// Configura��o de CORS para permitir acessos de qualquer origem
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
// Configura��o do pipeline de requisi��es HTTP
// ================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ativando o uso da pol�tica de CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
