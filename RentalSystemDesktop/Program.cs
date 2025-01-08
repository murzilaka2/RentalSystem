using Microsoft.Extensions.DependencyInjection;
using RentalSystem.Interfaces;
using RentalSystem.Repository;
using RentalSystemDesktop.Forms;
using DomainLayer;
using RentalSystemDesktop.ViewModels;
using RentalSystem.Services;
using Microsoft.Extensions.Configuration;
using DomainLayer.Interfaces;
using InfrastructureLayer.Repository;

namespace RentalSystemDesktop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            var services = new ServiceCollection();
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(serviceProvider.GetRequiredService<Login>());
        }

        private static void ConfigureServices(ServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRental, RentalRepository>();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<IRole, RoleRepository>();
            services.AddSingleton<IConnectionStringProvider, ConfigConnectionStringProvider>();
            services.AddSingleton<IUser, UserRepository>();
            services.AddSingleton<ICar, CarRepository>();
            services.AddSingleton<QueryBuilder>();

            services.AddTransient<Login>();
            services.AddTransient<Dashboard>();
        }
    }
}