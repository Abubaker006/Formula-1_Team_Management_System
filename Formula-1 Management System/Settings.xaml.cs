using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public static class GlobalContext
        {
            public static string userId { get; set; }
        }
        //this checks for user-id that is it passed as a arguement then its globally contexted
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Dictionary<string, string> parameters)
            {
                if (parameters.ContainsKey("userId"))
                {
                    GlobalContext.userId = parameters["userId"];
                }
            }

        }
        public Settings()
        {
            this.InitializeComponent();
        }
        private async void LogoutBtn_Click(Object sender, RoutedEventArgs e)
        {
            // Delay for a few seconds before closing the application
            ErrorInSettings.Text = $"Loging out..";
            await Task.Delay(3000); // Adjust the delay time as needed
                                    // Close the application
            Application.Current.Exit();

        }
        private async void DeleteAccount_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                DeleteAccountBtn.Content = "Deleting...";

                //first deleting the images stored.

                string imagePathToDeleteImageOne = $@"c:\f1_images\driver_images\{GlobalContext.userId}driver_1.png";
                string imagePathToDeleteImageTwo= $@"c:\f1_images\driver_images\{GlobalContext.userId}driver_2.png";
                string ownerImagePathToDelete = $@"c:\f1_images\{GlobalContext.userId}.png";
                if (File.Exists(imagePathToDeleteImageOne))
                {
                    File.Delete(imagePathToDeleteImageOne); 
                }
                if (File.Exists(imagePathToDeleteImageTwo))
                {
                    File.Delete(imagePathToDeleteImageTwo);
                }
                if (File.Exists(ownerImagePathToDelete))
                {
                    File.Delete(ownerImagePathToDelete);
                }

                string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
                var dataBaseName= App.Configuration.GetSection("AppSettings:DataBaseName").Value;
                var userCollectionName= App.Configuration.GetSection("AppSettings:UserCollectionName").Value;
                var driverCollectionName = App.Configuration.GetSection("AppSettings:DriverCollectionName").Value;

                var client = new MongoClient(mongoDbConnectionString);
                var database = client.GetDatabase(dataBaseName);

                // Delete data from the "driversInformation" collection
                var driverCollection = database.GetCollection<DriverInformationDataModel>(driverCollectionName);
                var driverFilter = Builders<DriverInformationDataModel>.Filter.Eq(u => u.userID, GlobalContext.userId);
                var driverResult = await driverCollection.DeleteManyAsync(driverFilter);

                // Check the result if needed
                if (driverResult.DeletedCount > 0)
                {
                    Console.WriteLine($"Deleted {driverResult.DeletedCount} document(s) from driversInformation with userID: {GlobalContext.userId}");

                    // Delete data from the "RegisteredUsersData" collection after successfully deleting from "driversInformation"
                    var userCollection = database.GetCollection<RegisteredUsersData>(userCollectionName);
                    var userFilter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                    var userResult = await userCollection.DeleteManyAsync(userFilter);

                    // Check the result if needed
                    if (userResult.DeletedCount > 0)
                    {
                        Console.WriteLine($"Deleted {userResult.DeletedCount} document(s) from RegisteredUsersData with userID: {GlobalContext.userId}");
                        DeleteAccountBtn.Content = "Deleted";

                        // Delay for a few seconds before closing the application
                        ErrorInSettings.Text = $"Account has been deleted";
                        await Task.Delay(3000); // Adjust the delay time as needed
                                                // Close the application
                        Application.Current.Exit();
                    }
                    else
                    {
                        ErrorInSettings.Text = $"No documents found in driversInformation with userID";
                        DeleteAccountBtn.Content = "Delete Failed";
                    }
                }
                else
                {
                    ErrorInSettings.Text=$"No documents found in driversInformation with userID";
                    DeleteAccountBtn.Content = "Delete Failed";
                }
            }
            catch (Exception ex)
            {
                ErrorInSettings.Text = $"Exception was faced in Deleting: {ex.Message}";
                DeleteAccountBtn.Content = "Delete Failed";
            }
        }

    }
}
