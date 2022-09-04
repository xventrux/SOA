using API.DataAccess;
using API.Infrastructure.Repository;
using API.Mapper.Mapping;
using API.Infrastructure.FileSystem;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;
using System.IO;
using System.Reflection;
using API.AppServices.Services.BookServices;

namespace SOA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Регистрация контекста базы данных
            services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<DbContext>(s => s.GetRequiredService<BaseDbContext>());

            //Регистрация репозитория
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //Регистрация авто маппера
            services.AddAutoMapper(typeof(ApplicationMapperProfile));

            //Регистрация сервиса для работы с файлами
            services.AddScoped<IStorage>(sp =>
            {
                IWebHostEnvironment env = sp.GetRequiredService<IWebHostEnvironment>();
                Storage fileService = new Storage(env.WebRootPath);
                return fileService;
            });

            #region Бизнес сервисы
            //Регистрация сервиса для работы с книгами
            services.AddScoped(typeof(IBookService), typeof(BookService));
            #endregion

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SOA", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SOA v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
