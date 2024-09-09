using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Adicionar suporte para JSON Patch
// ================================
// O Newtonsoft.Json � adicionado para manipula��o de JSON, principalmente
// para dar suporte a opera��es como JSON Patch.
builder.Services.AddControllers().AddNewtonsoftJson();

// ================================
// Configura��o do DbContext com PostgreSQL
// ================================
// Configura��o para usar o Npgsql (PostgreSQL) como o provedor de banco de dados.
// O m�todo `GetConnectionString("DefaultConnection")` busca a string de conex�o
// no arquivo de configura��o (appsettings.json, por exemplo).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================================
// Configura��o do Swagger para documenta��o da API
// ================================
// O Swagger � configurado para gerar documenta��o e possibilitar a
// visualiza��o e teste da API diretamente no browser.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ================================
// Configura��o do pipeline de requisi��es HTTP
// ================================
// Verifica se o ambiente � de desenvolvimento para habilitar o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita a redire��o de HTTP para HTTPS
app.UseHttpsRedirection();

// Middleware para autoriza��o (controle de acesso)
app.UseAuthorization();

// Mapeamento de controladores, ou seja, as rotas das APIs s�o gerenciadas
app.MapControllers();

// Executa a aplica��o
app.Run();
