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
using System.Diagnostics.Metrics;
using System.Reflection;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    //schema of database
   
    public sealed partial class DriversEditingPanel : Page
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
        public DriversEditingPanel()
        {
            this.InitializeComponent();
        }

        private const int MaxCharacters = 150; // Character limit set for driver description

        private void TextBox_KeyDown(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            // Checking if text length exceeds the limit
            if (textBox.Text.Length > MaxCharacters)
            {
                // If it exceeds, trim the text to the maximum limit
                textBox.Text = textBox.Text.Substring(0, MaxCharacters);
                textBox.Select(MaxCharacters, 0); // Set cursor position to the end
            }
        }
        private void goBackHyperLinkBtn(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DriversInformation)); //takes user back to information page
        }
        private async void  EditBtnClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                //connnection configuration for Mongodb Atlas
                string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
                string DriverCollectionName = App.Configuration.GetSection("AppSettings:DriverCollectionName").Value;
                var DataBaseName = App.Configuration.GetSection("AppSettings:DataBaseName").Value;

      
                var client = new MongoClient(mongoDbConnectionString);
                var database = client.GetDatabase(DataBaseName);

              string collectionName = DriverCollectionName; //the collection of user
          

              if (sender is Button button && button == submittedDriverInfoBtn)
              {
                 submittedDriverInfoBtn.Content = "Updating!";

                 string[] driverOneInformation=new string[8];  //string for holding driver-1 information
                 string[] driverTwoInformation = new string[8]; //string for holding driver-2 information

                  //storing the information of driver-1 in string array
               
                  driverOneInformation[0] =TextBoxDriverOneName.Text;
                  driverOneInformation[1] = TextBoxDriverOneAge.Text;
                  driverOneInformation[2] = TextBoxDriverOneCountry.Text;
                  driverOneInformation[3] = TextBoxDriverOnePodiums.Text;
                  driverOneInformation[4] = TextBoxDriverOneRacesWon.Text;
                  driverOneInformation[5] = TextBoxDriverOneSprintsWon.Text;
                  driverOneInformation[6] = TextBoxDriverTwoChampionshipsWon.Text;
                  driverOneInformation[7] = TextBoxDriverOneDescription.Text;
                  //storing the information of driver-2 in string array
                  driverTwoInformation[0] = TextBoxDriverTwoName.Text;
                  driverTwoInformation[1] = TextBoxDriverTwoeAge.Text;
                  driverTwoInformation[2] = TextBoxDriverTwoCountry.Text;
                  driverTwoInformation[3] = TextBoxDriverTwoPodiums.Text;
                  driverTwoInformation[4] = TextBoxDriverTwoRacesWon.Text;
                  driverTwoInformation[5] = TextBoxDriverTwoSprintsWon.Text;
                  driverTwoInformation[6] = TextBoxDriverTwoChampionshipsWon.Text;
                  driverTwoInformation[7] = TextBoxDriverTwoDescription.Text;


                  //here if it exist then we fetch data from it
                  var collection = database.GetCollection<DriverInformationDataModel>(collectionName);

                  //now fetching data according to the user-Id
                  var filter = Builders<DriverInformationDataModel>.Filter.Eq(u => u.userID, GlobalContext.userId);
                  var driverInfo = await collection.Find(filter).FirstOrDefaultAsync();

                  if(driverInfo!=null)
                  {
                        
                        //var updateResult = await collection.ReplaceOneAsync(filter, driverInfo);
                        //updating the data of driver-2
                        if (driverOneInformation[0]!="")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOneName, driverOneInformation[0]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverOneInformation[1] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOneAge, driverOneInformation[1]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverOneInformation[2] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOneCountry, driverOneInformation[2]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverOneInformation[3] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOnePodiums, driverOneInformation[3]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverOneInformation[4] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOneRaces, driverOneInformation[4]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverOneInformation[5] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOneSprints, driverOneInformation[5]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverOneInformation[6] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOneChampionships, driverOneInformation[6]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverOneInformation[7] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverOneDescription, driverOneInformation[7]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }



                        //updating the data of driver-2
                        if (driverTwoInformation[0] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoName, driverTwoInformation[0]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverTwoInformation[1] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoAge, driverTwoInformation[1]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverTwoInformation[2] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoCountry, driverTwoInformation[2]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverTwoInformation[3] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoPodiums, driverTwoInformation[3]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverTwoInformation[4] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoRaces, driverTwoInformation[4]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverTwoInformation[5] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoSprints, driverTwoInformation[5]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverTwoInformation[6] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoChampionshps, driverTwoInformation[6]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        if (driverTwoInformation[7] != "")
                        {
                            var update = Builders<DriverInformationDataModel>.Update.Set(u => u.DriverTwoDescription, driverTwoInformation[7]);
                            var result = await collection.UpdateOneAsync(filter, update);
                        }
                        submittedDriverInfoBtn.Content = "Updated!";
                    }
                  else
                  {
                        ErrorTextBlock.Text = "The Driver Information Doesnt Exist";
                  }
              }
            }
            catch
            {
                ErrorTextBlock.Text = "There is a exception in editing driver information";
            }
        }


        private async void UploadImage_DriverOne_Click(object sender, RoutedEventArgs e)
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
                {//this piece of code checks if the image was selected and then uploads it
                    ImageUploaderBtnDriverOne.Content = "Uploading!";

                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    var bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                    SelectedImageDriverOne.Source = bitmapImage;

                    using (var dataReader = new DataReader(stream.GetInputStreamAt(0)))
                    {
                        if (stream != null && GlobalContext.userId != null)
                        {
                            await dataReader.LoadAsync((uint)stream.Size);
                            var imageBytes = new byte[stream.Size];
                            dataReader.ReadBytes(imageBytes);

                            // Save image to local file system
                            string baseDirectory = @"C:\f1_images\driver_images"; // Change this path to your desired base directory
                            //string userDirectory = Path.Combine(baseDirectory);

                            if (!Directory.Exists(baseDirectory))
                            {
                                Directory.CreateDirectory(baseDirectory);
                            }

                            // Generate a unique file name
                            string fileName = $"{GlobalContext.userId}driver_1.png"; // Change file extension as needed
                            string filePath = Path.Combine(baseDirectory, fileName);

                            // Write the image bytes to the file
                            File.WriteAllBytes(filePath, imageBytes);



                            if (File.Exists(filePath))
                            {

                                // Update UI indicating successful upload and file saving
                                ImageUploaderBtnDriverOne.Content = $"Saved at: {filePath}";
                            }
                            else
                            {
                                // File was not written successfully
                                ImageUploaderBtnDriverOne.Content = "Not Saved!";
                            }
                        }
                        else
                        {
                            // Handle invalid data or missing userId
                            ErrorTextBlock.Text = "Invalid data or user";
                        }
                    }

                }
            }
            catch 
            {
                ErrorTextBlock.Text = "Exception in uploading driver Image";
                //wrongPassword.Text = ex.Message;
            }
        }
        private async void UploadImage_DriverTwo_Click(object sender, RoutedEventArgs e)
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
                    ImageUploaderBtnDriverTwo.Content = "Uploading!";

                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    var bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                    SelectedImageDriverTwo.Source = bitmapImage;

                    using (var dataReader = new DataReader(stream.GetInputStreamAt(0)))
                    {
                        if (stream != null && GlobalContext.userId != null)
                        {
                            await dataReader.LoadAsync((uint)stream.Size);
                            var imageBytes = new byte[stream.Size];
                            dataReader.ReadBytes(imageBytes);

                            // Save image to local file system
                            string baseDirectory = @"C:\f1_images\driver_images"; // Change this path to your desired base directory
                            //string userDirectory = Path.Combine(baseDirectory);

                            if (!Directory.Exists(baseDirectory))
                            {
                                Directory.CreateDirectory(baseDirectory);
                            }

                            // Generate a unique file name
                            string fileName = $"{GlobalContext.userId}driver_2.png"; // Change file extension as needed
                            string filePath = Path.Combine(baseDirectory, fileName);

                            // Write the image bytes to the file
                            File.WriteAllBytes(filePath, imageBytes);



                            if (File.Exists(filePath))
                            {

                                // Update UI indicating successful upload and file saving
                                ImageUploaderBtnDriverTwo.Content = $"Saved at: {filePath}";
                            }
                            else
                            {
                                // File was not written successfully
                                ImageUploaderBtnDriverTwo.Content = "Not Saved!";
                            }
                        }
                        else
                        {
                            // Handle invalid data or missing userId
                            ErrorTextBlock.Text = "Invalid data or user";
                        }
                    }

                }
            }
            catch
            {
                ErrorTextBlock.Text = "Exception in uploading driver Image";
                //wrongPassword.Text = ex.Message;
            }
        }
        private void DeleteImageButtonDriverOne_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                string imagePathToDelete = $@"c:\f1_images\driver_images\{GlobalContext.userId}driver_1.png";

                if (File.Exists(imagePathToDelete))
                {
                    File.Delete(imagePathToDelete);
                    // Optionally, update UI or perform further actions after deletion
                    SelectedImageDriverOne.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/userImage.png"));
                }
                else
                {
                    // Handle the scenario where the file does not exist
                }
            }
            catch 
            {
                ErrorTextBlock.Text = "Exception in Deleting driver Image";
            }
        }
        private void DeleteImageButtonDriverTwo_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                string imagePathToDelete = $@"c:\f1_images\driver_images\{GlobalContext.userId}driver_2.png";

                if (File.Exists(imagePathToDelete))
                {
                    File.Delete(imagePathToDelete);
                    // Optionally, update UI or perform further actions after deletion
                    SelectedImageDriverTwo.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/userImage.png"));
                }
                else
                {
                    // Handle the scenario where the file does not exist
                }
            }
            catch
            {
                ErrorTextBlock.Text = "Exception in Deleting driver Image";
            }
        }
    }
}
