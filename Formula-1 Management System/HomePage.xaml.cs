using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.Storage;
using Windows.Storage.Streams;





// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public static class GlobalContext
        {
            public static string userId { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Dictionary<string, string> parameters)
            {
                if (parameters.ContainsKey("userId"))
                {
                    GlobalContext.userId = parameters["userId"];
                    // Now you have the userId, you can use it in your Dashboard component
                }
            }
        }

        public HomePage()
        {
            this.InitializeComponent();
            Loaded += HomePage_Loaded;
        }

        public void EditDetailsBtnHandle(object sender, RoutedEventArgs e)
        {
          
              var parameters = new Dictionary<string, string>();
               parameters.Add("userId", GlobalContext.userId);

              Frame.Navigate(typeof(EditingPanelnfo), parameters);
          
        }

    
        public async void HomePage_Loaded(Object sender, RoutedEventArgs e)
        {
            string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
            var dataBaseName= App.Configuration.GetSection("AppSettings:DataBaseName").Value;
            var usersCollectionName= App.Configuration.GetSection("AppSettings:UserCollectionName").Value;

            var client = new MongoClient(mongoDbConnectionString);
            var database = client.GetDatabase(dataBaseName);
            var collection = database.GetCollection<RegisteredUsersData>(usersCollectionName);

            try
            {
                var user = await collection.Find(u => u.Id == GlobalContext.userId).FirstOrDefaultAsync();

                if (user != null)
                {
                    //other information
                    TextBlockTeamName.Text = user.TeamName.ToString();
                    TextBlockCOuntryName.Text = user.countryName.ToString();
                    TextBlockEngine.Text = user.EngineManufacturer.ToString();
                    TextBlockCategory.Text = user.RacingCategory.ToString();
                    TextBlockPodium.Text = user.podiums.ToString();
                    TextBlockGP.Text = user.numberofGp.ToString();
                    TextBlockPosition.Text = user.highestPostionInGrid.ToString();
                    TextBlockChampoinships.Text = user.champoinships.ToString();
                    TextBlockManufactureAwards.Text = user.manufacturerAwards.ToString();
                    //owner-name
                    TextBlockOwnerName.Text = user.OwnerName.ToString();

                    //ownerImage
                    string imagePath = $@"c:\f1_images\{GlobalContext.userId}.png";

                    // Check if the file exists before setting the ImageSource
                    if (File.Exists(imagePath))
                    {
                        var bitmapImage = new BitmapImage(new Uri(imagePath));
                        OwnerImageFromDatabase.ImageSource = bitmapImage;
                    }


                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
