using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // добавить сервисов к контейнеру DI 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            // настройка объекта строго типизированных параметров
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // настройки DI для микросервисов 
            services.AddScoped<IUserService, UserService>();
        }

        // настройка конвейера HTTP-запросов
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // Глобальная политика CORS  - технология современных браузеров, которая позволяет предоставить веб-странице доступ к ресурсам другого домена.
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // реализация jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
