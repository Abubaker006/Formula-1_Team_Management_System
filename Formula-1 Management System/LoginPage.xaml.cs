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
using System.Threading.Tasks;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.Extensions.Configuration;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public class RegisteredUsersData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

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

    public sealed partial class LoginPage : Page
    {
        //this code from line 37- is intended for unit-test class.
        //public TextBox EmailTextbox
        //{
        //    get { return emailTextbox; }
        //    set { emailTextbox = value; }
        //}
       

        public LoginPage()
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

        public bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Decode the stored password from Base64
            byte[] storedPasswordBytes = Convert.FromBase64String(storedPassword);

            // Extract the salt (first 16 bytes) and hashed password
            byte[] salt = storedPasswordBytes.Take(16).ToArray();
            byte[] hashedPassword = storedPasswordBytes.Skip(16).ToArray();

            // Hash the input password with the extracted salt
            using (var sha256 = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] hashedInputPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword).Concat(salt).ToArray());

                // Compare the hashed input password with the stored hashed password
                return hashedInputPassword.SequenceEqual(hashedPassword);
            }
        }

        public bool isValidEmail(string email)
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
        private async void LoginBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button == submitLoginButton)
            {
                button.Content = "Logging In";

                try 
                {
                    if(emailTextbox.Text !="" &&  passwordTextbox.Password!="")
                    {
                        string email = emailTextbox.Text;
                        string password = passwordTextbox.Password;
                        if(isValidEmail(email))
                        {
                            string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
                            var dataBaseName= App.Configuration.GetSection("AppSettings:DataBaseName").Value;
                            var userCollectionName= App.Configuration.GetSection("AppSettings:UserCollectionName").Value;

                            var client = new MongoClient(mongoDbConnectionString);
                            var database = client.GetDatabase(dataBaseName);
                            var collection = database.GetCollection<RegisteredUsersData>(userCollectionName);

                            var user = await collection.Find(u => u.Email == email).FirstOrDefaultAsync();

                            if (user != null)
                            {
                                bool isPassword = VerifyPassword(password, user.Password);

                                if (isPassword)
                                {
                                    //Frame.Navigate(typeof(DashBoard));

                                    string userId = user.Id;
                                    var parameters = new Dictionary<string, string>();
                                    parameters.Add("userId", userId);

                                    Frame.Navigate(typeof(DashBoard), parameters);
                                }
                                else
                                {
                                    wrongPassword.Text = "Password Is Wrong!";
                                    button.Content = "Sign-In";
                                }
                            }
                            else
                            {
                                wrongPassword.Text = "The User Does Not Exist.";
                                button.Content = "Sign-In";
                            }
                        }
                        else
                        {
                            wrongPassword.Text = "Enter valid Email.";
                            button.Content = "Sign-In";
                        }

                   
                    }
                    else
                    {
                        wrongPassword.Text = "Please Enter the Credentials.";
                        button.Content = "Sign-In";
                    }
                   
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error IN Login Page " + ex);
                }
                
            }
        }
        

        private void HyperlinkButton_Click(object sender,RoutedEventArgs e)
        {
            HyperlinkButton clickedButton = sender as HyperlinkButton;
            if (clickedButton != null)
            {
                if (clickedButton == changePageBtn)
                {
                    Frame.Navigate(typeof(RegistrationPage));
                }
                else if (clickedButton == forgotPasswordBtn)
                {
                    Frame.Navigate(typeof(ForgetPassword)); //there would be ForgetPassword.xaml
                }
            }
        }
    }
}
