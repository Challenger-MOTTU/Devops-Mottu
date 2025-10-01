using System.Reflection;
using Challenger.Application.UseCase;
using Challenger.Application.UseCase.CreateMoto;
using Challenger.Application.UseCase.CreateUser;
using Challenger.Application.UseCase.UpdateUser;
using Challenger.Infrastructure;

namespace WebApplication2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Motix API",
                Version = "v1",
                Description = "API com Setores, Motos e Movimentos. CRUD + paginação + HATEOAS."
            });
 
            // LÊ o arquivo XML gerado pelo csproj (XML comments)
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
        });
        
        builder.Services.AddDBContext(builder.Configuration);
        builder.Services.AddRepositories();
        
        builder.Services.AddScoped<ICreatePatioUseCase, CreatePatioUseCase>();
        builder.Services.AddScoped<IUpdatePatioUseCase, UpdatePatioUseCase>();
        builder.Services.AddScoped<ICreateMotoUseCase, CreateMotoUseCase>();
        builder.Services.AddScoped<IUpdateMotoUseCase, UpdateMotoUseCase>();
        builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        builder.Services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || true)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}