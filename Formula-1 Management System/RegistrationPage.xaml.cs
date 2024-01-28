using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.UI;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Configuration;
using Windows.ApplicationModel.Activation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public class RegistrationDataModel
    {
        public string Email { get; set; }
        public string OwnerName { get; set; }
        public string Password { get; set; }
        public string TeamName { get; set; }
        public string EngineManufacturer { get; set; }
        public string RacingCategory { get; set; }
        public string FactoryTeam { get; set; }
        public string countryName { get; set; }
        public string podiums { get; set; }
        public string numberofGp { get; set; }
        public string highestPostionInGrid { get; set; }
        public string champoinships { get; set; }
        public string manufacturerAwards { get; set; }
    
    }

    public sealed partial class RegistrationPage : Page
    {

        public RegistrationPage()
        {
            this.InitializeComponent();
        }
        //HoverPanel are for closebtn bg
        private void HoverPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            HoverPanel.Background = new SolidColorBrush(Colors.DarkRed);
        }
        private void HoverPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            HoverPanel.Background = new SolidColorBrush(Colors.Transparent);
        }
        //represents close btn when it activates turn offs the app
        private void Close_btn(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        //verifying email input
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        //hashing and salting
        public string HashAndSaltPassword(string password)
        {
            // Generate a unique salt for each password
            byte[] salt = new byte[16];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt
            using (var sha256 = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password).Concat(salt).ToArray());
                byte[] saltedPassword = salt.Concat(hashedPassword).ToArray();
                return Convert.ToBase64String(saltedPassword);
            }
        }


        public async void RegisterBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if(sender is Button button && button== SubmitRegisterData)
            {
                SubmitRegisterData.Content = "Registering";

                string email = emailTextbox.Text;
                string ownerName = ownnerNameTextbox.Text;
                string teamName = teamNameTextbox.Text;
                string engineManufacturer = engineTexbox.Text;

                
                if (IsValidEmail(email))
                {


                    // Retrieving value from PasswordBox
                    if(passwordTextbox.Password!="")
                    {
                        string password = HashAndSaltPassword(passwordTextbox.Password);
                        // Retrieving values from RadioButtons
                        string racingCategory = BackgroundRadioButtons.SelectedItem?.ToString();
                        string factoryTeam = BorderRadioButtons.SelectedItem?.ToString();

                        var registrationData = new RegistrationDataModel
                        {
                            Email = email,
                            OwnerName = ownerName,
                            Password = password,
                            TeamName = teamName,
                            EngineManufacturer = engineManufacturer,
                            RacingCategory = racingCategory,
                            FactoryTeam = factoryTeam,
                            countryName ="",
                            podiums ="",
                            numberofGp="",
                            highestPostionInGrid="", 
                            champoinships ="",
                            manufacturerAwards="",
                            
                        };

                        //mongodb connection being done

                        string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
                        var dataBaseName= App.Configuration.GetSection("AppSettings:DataBaseName").Value;
                        var userCollectionName= App.Configuration.GetSection("AppSettings:UserCollectionName").Value;

                        var settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
                        // Set the ServerApi field of the settings object to Stable API version 1
                        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                        // Create a new client and connect to the server
                        var client = new MongoClient(settings);
                        try
                        {
                            var result = client.GetDatabase(dataBaseName).RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");

                            var database = client.GetDatabase(dataBaseName);
                            var collection = database.GetCollection<RegistrationDataModel>(userCollectionName);

                            var user = await collection.Find(u => u.Email == email).FirstOrDefaultAsync();

                            if (user != null)
                            {
                                wrongPassword.Text = "Registration failed. Please try again.";
                            }
                            else
                            {
                                collection.InsertOne(registrationData);
                                wrongPassword.Text = "Registration successful.";
                                await Task.Delay(3000);

                                Frame.Navigate(typeof(LoginPage));
                                
                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            wrongPassword.Text = "Registration failed due to an error. Please try again.";
                        }



                        // Clear input fields
                        emailTextbox.Text = "";
                        ownnerNameTextbox.Text = "";
                        teamNameTextbox.Text = "";
                        engineTexbox.Text = "";
                        passwordTextbox.Password = "";
                        BackgroundRadioButtons.SelectedItem = null;
                        BorderRadioButtons.SelectedItem = null;

                        // Change button content back to "Register"
                        SubmitRegisterData.Content = "Register";

                    }
                    else
                    {
                        //last else
                        wrongPassword.Text = "Please enter a valid password";
                    }
                }
                else
                {
                    wrongPassword.Text = "Invalid email address";
                }
            }
        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
