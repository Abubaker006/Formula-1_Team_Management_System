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
using System.Security.Cryptography.X509Certificates;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class SetNewPassword : Page
    {
        public static class GlobalContext
        {
            public static string Email { get; set; }
        }
        public SetNewPassword()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Check if the NavigationEventArgs contains parameters
            if (e.Parameter is Dictionary<string, string> parameters)
            {
                // Retrieve the email parameter
                if (parameters.TryGetValue("Email", out string email))
                {
                    // Use the 'email' value to change the password or perform related actions
                    GlobalContext.Email = email;
                }
            }
        }
        private string HashAndSaltPassword(string password)
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
        private async void ChangePassBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button == SubmitNewPassword)
            {
                string newPassword=passwordTextbox1.Password;
                string confirmPassword =passwordTextbox2.Password;
                if (newPassword !="" && confirmPassword!="")
                {
                    if(newPassword==confirmPassword)
                    {
                        button.Content = "Changing";
                        string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
                        var databaseName= App.Configuration.GetSection("AppSettings:DataBaseName").Value;
                        var userCollectionName= App.Configuration.GetSection("AppSettings:UserCollectionName").Value;

                        var client = new MongoClient(mongoDbConnectionString);
                        var database = client.GetDatabase(databaseName);
                        var collection = database.GetCollection<RegisteredUsersData>(userCollectionName);
                        string userEmail = GlobalContext.Email;

                        string password = HashAndSaltPassword(newPassword);

                        var filter = Builders<RegisteredUsersData>.Filter.Eq(u => u.Email, userEmail);
                        var update = Builders<RegisteredUsersData>.Update.Set(u => u.Password, password);

                        var result = await collection.UpdateOneAsync(filter, update);

                        if (result.ModifiedCount > 0)
                        {
                            Frame.Navigate(typeof(LoginPage));
                        }
                        else
                        {
                            wrongPassword.Text = "Error in changing the password.";
                            button.Content = "Change!";
                        }

                    }
                    else
                    {
                        wrongPassword.Text = "These Passwords dont match.";
                        button.Content = "Change!";
                    }
                }
                else
                {
                    wrongPassword.Text = "Please Enter password in both boxes";
                    button.Content = "Change!";
                }
            }
        }

    }
}
