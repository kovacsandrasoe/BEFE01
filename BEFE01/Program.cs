
using BEFE01.Data;
using BEFE01.Tools;
using Microsoft.EntityFrameworkCore;

namespace BEFE01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<BookDbContext>(opt =>
            {
                opt
                .UseSqlServer(builder.Configuration["db:conn"])
                .UseLazyLoadingProxies();
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                
            }, typeof(DtoProfile));

            builder.Services.AddControllers(cfg =>
            {
                cfg.Filters.Add<ExceptionFilter>();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
        }
    }
}
