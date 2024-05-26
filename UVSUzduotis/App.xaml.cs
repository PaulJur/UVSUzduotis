using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using UVSUzduotis.Data;

namespace UVSUzduotis
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //NOTE: comments are for myself, because this is my first time working with services.

        private ServiceProvider serviceProvider;

        public App()
        {
            

            ServiceCollection services = new ServiceCollection();//Register application services
            ConfigureServices(services);//Configure DI
            serviceProvider = services.BuildServiceProvider();

        }

        //Adds DI to MainWindow for DBContext
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<UVSDBContext>();//Adds UVSDBContext as a service for EF.

            services.AddSingleton<MainWindow>();//Only one instance of MainWindow.
        }

        //Startup event
        private void OnStartup(object sender, EventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }

}
