using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Adicionar suporte para JSON Patch
// ================================
// O Newtonsoft.Json é adicionado para manipulação de JSON, principalmente
// para dar suporte a operações como JSON Patch.
builder.Services.AddControllers().AddNewtonsoftJson();

// ================================
// Configuração do DbContext com PostgreSQL
// ================================
// Configuração para usar o Npgsql (PostgreSQL) como o provedor de banco de dados.
// O método `GetConnectionString("DefaultConnection")` busca a string de conexão
// no arquivo de configuração (appsettings.json, por exemplo).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================================
// Configuração do Swagger para documentação da API
// ================================
// O Swagger é configurado para gerar documentação e possibilitar a
// visualização e teste da API diretamente no browser.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ================================
// Configuração do pipeline de requisições HTTP
// ================================
// Verifica se o ambiente é de desenvolvimento para habilitar o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita a redireção de HTTP para HTTPS
app.UseHttpsRedirection();

// Middleware para autorização (controle de acesso)
app.UseAuthorization();

// Mapeamento de controladores, ou seja, as rotas das APIs são gerenciadas
app.MapControllers();

// Executa a aplicação
app.Run();
