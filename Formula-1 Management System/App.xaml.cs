using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.Configuration;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; }

        public static AppConfiguration AppConfig { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory) // Set the base path to the output directory
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


            Configuration = configurationBuilder.Build();

            AppConfig = new AppConfiguration
            {
                MongoDbConnectionString = Configuration.GetSection("AppSettings:MongoDbConnectionString").Value,
                DriverCollectionName=Configuration.GetSection("AppSettings:DriverCollectionName").Value,
                UserCollectionName = Configuration.GetSection("AppSettings:UserCollectionName").Value,
                DataBaseName=Configuration.GetSection("AppSettings:DataBaseName").Value,
                GoogleAppsPassword = Configuration.GetSection("AppSettings:GoogleAppsPassword").Value,
                GmailAccountName = Configuration.GetSection("AppSettings:GmailAccountName").Value
            };
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }
}
