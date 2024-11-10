
using CadastroLivros.Data.Persistence;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Service;
using CadastroLivros.Services.Interfaces;

namespace CadastroLivros
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            SetupServices(builder);

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
        }

        public static void SetupServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAppSettingsService, AppSettingsService>();

            builder.Services.AddScoped<IBasicPersistence, BasicPersistence>();
            builder.Services.AddScoped<ILivroPersistence, LivroPersistence>();
            builder.Services.AddScoped<IAutorPersistence, AutorPersistence>();
            builder.Services.AddScoped<IAssuntoPersistence, AssuntoPersistence>();

            builder.Services.AddScoped<ILivroService, LivroService>();
            builder.Services.AddScoped<IAutorService, AutorService>();
            builder.Services.AddScoped<IAssuntoService, AssuntoService>();
        }
    }
}
