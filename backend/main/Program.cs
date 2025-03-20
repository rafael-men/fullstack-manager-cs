using System.Text;
using main.Data;
using main.Repository;
using main.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyMethod()  
              .AllowAnyHeader();
    });
});

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

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Insira o token JWT com o prefixo 'Bearer '"
    });

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




builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString); 

});



Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

var key = Encoding.ASCII.GetBytes("8d99dX1fZPqfD56Tkcj3pZdTdzdfsdfsdfdffwefsdcsrggwsfdsa");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "seu_emissor",
        ValidateAudience = true,
        ValidAudience = "seu_publico",
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Procurador", policy =>
        policy.RequireRole("Procurador"));

    options.AddPolicy("Cliente", policy =>
        policy.RequireRole("Cliente"));
});




builder.Services.AddScoped<ProcessoService>();
builder.Services.AddScoped<ProcuradorService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<DocumentoService>();
builder.Services.AddScoped<PrazoService>();
builder.Services.AddScoped<ProcessoRepository>();
builder.Services.AddScoped<UserService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();


app.Run();
