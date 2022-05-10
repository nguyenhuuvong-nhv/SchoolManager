using AutoMapper;
using Entities.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.AutoMapper;
using Services.Implementations;
using Services.Implementations.Internal;
using Services.Interfaces;
using Services.Interfaces.Internal;

namespace StudentManager.App_Start
{
    public class ApplicationServicesInstaller
    {
        public static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IConfigTextManager, ConfigTextManager>();

            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IStudentService, StudentService>();

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

        }
    }
}
