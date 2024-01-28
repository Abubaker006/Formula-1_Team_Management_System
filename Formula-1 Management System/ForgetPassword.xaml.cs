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
using System.Runtime.CompilerServices;
using MongoDB.Driver;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgetPassword : Page
    {
       
        public ForgetPassword()
        {
            this.InitializeComponent();
        }
        private bool isValidEmail(string email)
        { //check for the email if valid or not
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
        public async Task<bool> VerifyCodeAsync(string codeString)
        {
            var tcs = new TaskCompletionSource<bool>();

            // Assume some event handler for when the verification button is clicked
            // In this case, let's assume a button click event handler
            EmailBox.Visibility = Visibility.Collapsed;
            VerificationBox.Visibility = Visibility.Visible;
            submitForgotButton.Content = "Verfiy!";
            submitForgotButton.Click +=  (sender, args) =>
            {
                string enteredCode = VerificationNumberBox.Text;

                if (enteredCode == codeString)
                {
                    // Resolve the Task with true if code matches
                    tcs.SetResult(true);
                }
                else
                {
                    // Resolve the Task with false if code doesn't match
                    tcs.SetResult(false);
                }
            };
            // Await the completion of the Task
            return await tcs.Task;
        }

        private async void ForgotBtn_Clicked(object sender,RoutedEventArgs e)
        {
            if (sender is Button button && button == submitForgotButton)
            {
                if (emailTextbox.Text != "")
                {
                    button.Content = "Checking";
                    string email = emailTextbox.Text;
                    if (isValidEmail(email))
                    {
                        //these 87-91 loc are the data fetched from configuration file
                        string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
                        var googleAppsPassword = App.Configuration.GetSection("AppSettings:GoogleAppsPassword").Value;
                        var gmailAccountName = App.Configuration.GetSection("AppSettings:GmailAccountName").Value;
                        var dataBaseName = App.Configuration.GetSection("AppSettings:DatabaseName").Value;
                        var userCollectionName=App.Configuration.GetSection("AppSettings:UserCollectionName").Value;

                        
                        var client = new MongoClient(mongoDbConnectionString);  
                        var database = client.GetDatabase(dataBaseName);
                        var collection = database.GetCollection<RegisteredUsersData>(userCollectionName);
                        var user = await collection.Find(u => u.Email == email).FirstOrDefaultAsync();

                        if (user != null)
                        {
                            //here this will send the mail
                            Random randomGenerator = new Random();
                            int code = randomGenerator.Next(100000, 999999);

                            string codeString = code.ToString();

                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                            {
                                smtpClient.UseDefaultCredentials = false;
                                //smtpClient.Credentials = new NetworkCredential("formula1teammanagement@gmail.com", "xvcp lghd gvvu vxgm");
                                smtpClient.Credentials = new NetworkCredential(gmailAccountName, googleAppsPassword);
                                smtpClient.EnableSsl = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                smtpClient.Port = 587;
                                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                                MailMessage mailMessage = new MailMessage();
                                mailMessage.From = new MailAddress("formula1teammanagement@gmail.com");
                                mailMessage.To.Add(user.Email);
                                mailMessage.Subject = "Verification Code";
                                mailMessage.Body = $"Your verification code is: {codeString}";

                                try
                                {
                                    smtpClient.Send(mailMessage);
                                    wrongPassword.Text = "Email Sent Successfully";

                                   
                                  
                                    bool isCodeVerified = await VerifyCodeAsync(codeString);

                               

                                    if(isCodeVerified)
                                    {
                                        string userEmail = user.Email;
                                        var parameters=new Dictionary<string, string>();
                                        parameters.Add("Email", userEmail);

                                        Frame.Navigate(typeof(SetNewPassword),parameters);
                                    }
                                    else
                                    {
                                        Frame.Navigate(typeof(ForgetPassword));
                                        wrongPassword.Text = "Apply for code again!";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    wrongPassword.Text = ex.Message;
                                }
                            }
                        }
                        else
                        {
                            wrongPassword.Text = "You don't exist as a user!";
                            button.Content = "Check!";
                        }
                    }
                    else
                    {
                        wrongPassword.Text = "Please Enter the Correct Email.";
                        button.Content = "Check!";
                    }
                }
                else
                {
                    wrongPassword.Text = "Please Enter the Credentials.";
                    button.Content = "Check!";
                }
            }
               

        }
        private void ForgotHyper_Click(object sender,RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }


    }
}
