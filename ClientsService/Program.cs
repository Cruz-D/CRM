using ClientsService.Application;
using ClientsService.Domain;
using ClientsService.Infrastructure;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ClientsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton(sp =>
            {
                string endpoint = builder.Configuration["CosmosDb:EndpointUri"]!;

                string primaryKey = builder.Configuration["CosmosDb:PrimaryKey"]!;

                return new CosmosClient(endpoint, primaryKey, new CosmosClientOptions
                {
                    ConnectionMode = ConnectionMode.Gateway,
                });

            });


            // Redis: obtiene la cadena de conexión desde User Secrets, variables de entorno, etc.
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = builder.Configuration["Redis:ConnectionString"];
                if (string.IsNullOrWhiteSpace(configuration))
                    throw new InvalidOperationException("Redis connection string is not configured.");
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddSingleton<RedisCacheService>();

            builder.Services.AddScoped<IClientRepository>(sp =>
            {
                var cosmos = sp.GetRequiredService<CosmosClient>();
                var databaseName = builder.Configuration["CosmosDb:DatabaseName"]!;
                var containerName = builder.Configuration["CosmosDb:ContainerName"]!;
                var redisCacheService = sp.GetRequiredService<RedisCacheService>();

                var container = cosmos.GetContainer(
                    databaseName ?? throw new InvalidOperationException("Database name not configured."),
                    containerName ?? throw new InvalidOperationException("Container name not configured.")
                );

                return new ClientsRepository(cosmos, container, redisCacheService);
            });


            builder.Services.AddScoped<CreateClientUseCase>();
            builder.Services.AddScoped<GetAllClientsUseCase>();
            builder.Services.AddScoped<GetClientUseCase>();
            builder.Services.AddScoped<UpdateClientUseCase>();
            builder.Services.AddScoped<DeleteClientUseCase>();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger");
                    return;
                }
                await next();
            });

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
        }
    }
}
