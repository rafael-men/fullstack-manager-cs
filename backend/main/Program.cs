using System.Reflection;
using main.Data;
using main.Repository;
using main.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API com .NET para Gerenciamento de Processos Jurídicos",
        Version = "v1",
        Description = "Documentação da API de PJ",
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString); 

});

builder.Services.AddScoped<ProcessoService>();
builder.Services.AddScoped<ProcuradorService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<DocumentoService>();
builder.Services.AddScoped<PrazoService>();
builder.Services.AddScoped<ProcessoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
app.UseRouting();

app.Run();
