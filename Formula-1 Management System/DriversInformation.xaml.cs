using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
  
    //this is our schema class
    public class DriverInformationDataModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public string userID { get; set; } //our foreign key
        //driver-1 data
        public string DriverOneName { get; set; }
        public string DriverOneCountry { get; set; }
        public string DriverOneAge { get; set; }
        public string DriverOneRaces { get; set; }
        public string DriverOnePodiums { get; set; }
        public string DriverOneSprints { get; set; }
        public string DriverOneChampionships { get; set; }
        public string DriverOneDescription { get; set; }
        //driver-2 Data
        public string DriverTwoName { get; set; }
        public string DriverTwoCountry { get; set; }
        public string DriverTwoAge { get; set; }
        public string DriverTwoRaces { get; set; }
        public string DriverTwoPodiums { get; set; }
        public string DriverTwoSprints { get; set; }
        public string DriverTwoChampionshps { get; set; }
        public string DriverTwoDescription { get; set; }
    }

    public sealed partial class DriversInformation : Page
    {
       
        //globalizes the userId of user so that it can be accessed from all over the code
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

        public DriversInformation()
        {
            this.InitializeComponent();
            Loaded += DriverInformation_Loaded; //as driverInformation Page is loaded it calls the req function
        }
        public  void EditDriverInformation_Click(object sender, RoutedEventArgs e)
        {
            //Navigates the user to the Drivers Editing Panel Page
            var parameters = new Dictionary<string, string>();
            parameters.Add("userId", GlobalContext.userId);
            Frame.Navigate(typeof(DriversEditingPanel), parameters);

        }
        static async Task<bool> CollectionExistsAsync(IMongoDatabase database, string collectionName,string userId)
        {
            //this checks that is the collection existing ?
            var collection = database.GetCollection<DriverInformationDataModel>(collectionName);
            //this checks that does the collection have driver informatin with foreign key
            var filter = Builders<DriverInformationDataModel>.Filter.Eq(u => u.userID, userId);
            //checks count if result is > 0 means this schema exists 
            var count = await collection.CountDocumentsAsync(filter);
            return count > 0;
        }
        private async void DriverInformation_Loaded(Object sender,RoutedEventArgs e)
        {
            try 
            {
                string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
                var dataBaseName = App.Configuration.GetSection("AppSettings:DataBaseName").Value;
                string driversCollectionName = App.Configuration.GetSection("AppSettings:DriverCollectionName").Value;
                //mongodb atlas connection configuration
                var client = new MongoClient(mongoDbConnectionString);
                var database = client.GetDatabase(dataBaseName);
    
                string collectionName = driversCollectionName; //tells our database name

                bool collectionExists = await CollectionExistsAsync(database, collectionName,GlobalContext.userId);

                //here we check if this part of user exist or not 
                if (collectionExists)
                {
                    //here if it exist then we fetch data from it
                    var collection = database.GetCollection<DriverInformationDataModel>(collectionName);

                    //now fetching data according to the user-Id
                    var filter = Builders<DriverInformationDataModel>.Filter.Eq(u => u.userID, GlobalContext.userId);
                    var driverInfo = await collection.Find(filter).FirstOrDefaultAsync();

                    //checks if actually the driver exists?
                    if(driverInfo!=null)
                    {
                        //updating UI Elements according to the data in db
                        //checking for driver-1 and updating it
                        if (driverInfo.DriverOneName != "") 
                        { driverOneInformation.Text = driverInfo.DriverOneName; }

                        if(driverInfo.DriverOneAge!="")
                        { TextBlockAgeDriverOne.Text = $"Age: {driverInfo.DriverOneAge}"; }

                        if(driverInfo.DriverOneCountry!="")
                        { TextBlockCountryDriverOne.Text = $"Country: {driverInfo.DriverOneCountry}"; }

                        if(driverInfo.DriverOnePodiums!="")
                        { TextBlockPodiumDriverOne.Text = $"Podiums: {driverInfo.DriverOnePodiums}"; }

                        if(driverInfo.DriverOneRaces!="")
                        { TextBlockRacesDriverOne.Text = $"Races: {driverInfo.DriverOneRaces}"; }

                        if(driverInfo.DriverOneSprints!="")
                        { TextBlockSprintsDriverOne.Text = $"Sprints: {driverInfo.DriverOneSprints}"; }

                        if(driverInfo.DriverOneChampionships!="")
                        { TextBlockWonChampionShipsDriverOne.Text = $"Championships: {driverInfo.DriverOneChampionships}"; }

                        if (driverInfo.DriverOneDescription != "")
                        { TextBlockDescriptionDriverOne.Text = driverInfo.DriverOneDescription; }

                        //now fetching and updating for driver-2

                        if (driverInfo.DriverTwoName != "")
                        { driverTwoInformation.Text = driverInfo.DriverTwoName; }

                        if (driverInfo.DriverTwoAge != "")
                        { TextBlockAgeDriverTwo.Text = $"Age: {driverInfo.DriverTwoAge}"; }

                        if (driverInfo.DriverTwoCountry != "")
                        { TextBlockCountryDriverTwo.Text = $"Country: {driverInfo.DriverTwoCountry}"; }

                        if (driverInfo.DriverTwoPodiums != "")
                        { TextBlockPodiumDriverTwo.Text = $"Podiums: {driverInfo.DriverTwoPodiums}"; }

                        if (driverInfo.DriverTwoRaces != "")
                        { TextBlockRacesDriverTwo.Text = $"Races: {driverInfo.DriverTwoRaces}"; }

                        if (driverInfo.DriverTwoSprints != "")
                        { TextBlockSprintsDriverTwo.Text = $"Sprints: {driverInfo.DriverTwoSprints}"; }

                        if (driverInfo.DriverTwoChampionshps != "")
                        { TextBlockWonChampionShipsDriverTwo.Text = $"Championships: {driverInfo.DriverTwoChampionshps}"; }

                        if (driverInfo.DriverTwoDescription != "")
                        { TextBlockDescriptionDriverTwo.Text = driverInfo.DriverTwoDescription; }


                        //ownerImage
                        string imagePathDriverOne = $@"c:\f1_images\driver_images\{GlobalContext.userId}driver_1.png";
                        string imagePathDriverTwo= $@"c:\f1_images\driver_images\{GlobalContext.userId}driver_2.png";
                        // Check if the file exists before setting the ImageSource
                        if (File.Exists(imagePathDriverOne))
                        {
                            var bitmapImage = new BitmapImage(new Uri(imagePathDriverOne));
                            DriverOneImage.Source = bitmapImage;
                        }
                        if(File.Exists(imagePathDriverTwo))
                        {
                            var bitmapImage = new BitmapImage(new Uri(imagePathDriverTwo));
                            DriverTwoImage.Source = bitmapImage;
                        }
                    }
                    else
                    {
                        ErrorTextBlock.Text = "There is no data of this user for drivers";
                    }
                    ErrorTextBlock.Text = $"Collection '{collectionName}' exists.";
                }
                else
                {
                    //here in this part we create the collection if it does not exist
                    var driversInformation = new DriverInformationDataModel
                    {
                        userID = GlobalContext.userId,// foreign key

                        //giving driver-1 information database
                        DriverOneName = "",
                        DriverOneAge = "",
                        DriverOneCountry = "",
                        DriverOnePodiums = "",
                        DriverOneRaces = "",
                        DriverOneSprints = "",
                        DriverOneChampionships = "",
                        DriverOneDescription = "",

                        //giving driver-2 information to database
                        DriverTwoName = "",
                        DriverTwoAge = "",
                        DriverTwoCountry = "",
                        DriverTwoPodiums = "",
                        DriverTwoRaces = "",
                        DriverTwoSprints = "",
                        DriverTwoChampionshps = "",
                        DriverTwoDescription = ""
                    };
                    var collection = database.GetCollection<DriverInformationDataModel>(driversCollectionName);
                    collection.InsertOne(driversInformation);

                    ErrorTextBlock.Text = $"The Database is Created";
                }

            }
            catch
            {
                ErrorTextBlock.Text = $"Exception in the code.";
            }
        }

        
    }
}

