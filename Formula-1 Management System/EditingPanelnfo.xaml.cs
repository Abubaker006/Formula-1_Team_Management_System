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
using Windows.System;
using MongoDB.Driver;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditingPanelnfo : Page
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

        public EditingPanelnfo()
        {
            this.InitializeComponent();
        }

        private async void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //this code opens the file dialog box to select image 
                var window = new Microsoft.UI.Xaml.Window();
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                var filePicker = new Windows.Storage.Pickers.FileOpenPicker();

                //verifies that image-type should be between these three
   
                filePicker.FileTypeFilter.Add(".png");
                WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

                var file = await filePicker.PickSingleFileAsync(); //check and syncs the image
                //check if the image is selected
                if (file != null)
                {
                    ImageUploaderBtn.Content = "Uploading!";

                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    var bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                    SelectedImage.Source = bitmapImage;

                    using (var dataReader = new DataReader(stream.GetInputStreamAt(0)))
                    {
                        if (stream != null && GlobalContext.userId != null)
                        {
                            await dataReader.LoadAsync((uint)stream.Size);
                            var imageBytes = new byte[stream.Size];
                            dataReader.ReadBytes(imageBytes);

                            // Save image to local file system
                            string baseDirectory = @"C:\f1_images\"; // Change this path to your desired base directory
                            //string userDirectory = Path.Combine(baseDirectory);

                            if (!Directory.Exists(baseDirectory))
                            {
                                Directory.CreateDirectory(baseDirectory);
                            }

                            // Generate a unique file name
                            string fileName = $"{GlobalContext.userId}.png"; // Change file extension as needed
                            string filePath = Path.Combine(baseDirectory, fileName);

                            // Write the image bytes to the file
                             File.WriteAllBytes(filePath, imageBytes);



                            if (File.Exists(filePath))
                            {
                                
                                // Update UI indicating successful upload and file saving
                                ImageUploaderBtn.Content = $"Saved at: {filePath}";
                            }
                            else
                            {
                                // File was not written successfully
                                ImageUploaderBtn.Content = "Not Saved!";
                            }
                        }
                        else
                        {
                            // Handle invalid data or missing userId
                            ImageUploaderBtn.Content = "Invalid data or user";
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("There is problem in uploading the picture");
                //wrongPassword.Text = ex.Message;
            }
        }

        private  void DeleteImageButton_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                string imagePathToDelete = $@"c:\f1_images\{GlobalContext.userId}.png";

                if (File.Exists(imagePathToDelete))
                {
                    File.Delete(imagePathToDelete);
                    // Optionally, update UI or perform further actions after deletion
                    SelectedImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/userImage.png"));
                }
                else
                {
                    // Handle the scenario where the file does not exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Exception was faced: {ex.Message}");
            }
        }
        private async void EditBtnClicked(Object sender, RoutedEventArgs e)
        {
            submiteditedInfo.Content = "updating";
            string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
            var dataBaseName = App.Configuration.GetSection("AppSettings:DataBaseName").Value;
            var usersCollectionName= App.Configuration.GetSection("AppSettings:UserUserCollectionName").Value;

            var client = new MongoClient(mongoDbConnectionString);
            var database = client.GetDatabase(dataBaseName);
            var collection = database.GetCollection<RegisteredUsersData>(usersCollectionName);
            try
            {
                var user = await collection.Find(u => u.Id == GlobalContext.userId).FirstOrDefaultAsync();

                if(user != null)
                {
                    string teamName = TeamNameTextBox.Text;
                    string countryName = CountryNameTextBox.Text;
                    string manufacturer = engineManufacturerTextbox.Text;
                    string racingCategory = BackgroundRadioButtons.SelectedItem?.ToString();
                    string podium = podiumTextBox.Text;
                    string numberOfGp = numberofGpTextBox.Text;
                    string highestGridPosition = highestGridPositionTextbox.Text;
                    string championships = numberofChampionshipsTextbox.Text;
                    string manufactureAwards = numberofManufactureAwardsTextbox.Text;
                    string ownerName = ownnerNameTextbox.Text;


                    if (teamName!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.TeamName, teamName);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(countryName!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.countryName, countryName);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(manufacturer!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.EngineManufacturer, manufacturer);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(racingCategory!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.RacingCategory, racingCategory);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(podium!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.podiums, podium);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if (numberOfGp != "")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.numberofGp, numberOfGp);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(highestGridPosition!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.highestPostionInGrid, highestGridPosition);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(championships!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.champoinships, championships);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(manufactureAwards!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.manufacturerAwards, manufactureAwards);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    if(ownerName!="")
                    {
                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Id, GlobalContext.userId);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.OwnerName, ownerName);
                        var result = await collection.UpdateOneAsync(filter, update);
                    }
                    submiteditedInfo.Content = "Updated!";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
        private void goBackHyperLinkBtn(object sender,RoutedEventArgs e)
        {
            string userId = GlobalContext.userId;
            var parameters = new Dictionary<string, string>();
            parameters.Add("userId", userId);

            Frame.Navigate(typeof(HomePage), parameters);
            
        }
    }
}
