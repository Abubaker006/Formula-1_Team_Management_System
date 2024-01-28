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
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Formula_1_Management_System
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoard : Page
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

        public DashBoard()
        {
            this.InitializeComponent();
            Loaded += DashBoard_Loaded;
        }
        private void DashBoard_Loaded(object sender, RoutedEventArgs e)
        {
            // by default our homepage will be displayed but when 
            // Set the selected menu item (Homepage) when the page loads
            nvSample.SelectedItem = nvSample.MenuItems[0]; // Index 0 corresponds to the first menu item
            //arguments of user id are being passed to homepage
            string argumentUserId = GlobalContext.userId;
            var parameters = new Dictionary<string, string>();
            parameters.Add("userId", argumentUserId);
            contentFrame.Navigate(typeof(HomePage), parameters);
        }

        private void NavigationPanel_Invoked(NavigationView sender, NavigationViewItemInvokedEventArgs e)
        {
            //this section navigates to the settings page. 
            if(e.IsSettingsInvoked)
            {
                string argumentUserId = GlobalContext.userId;
                var parameters = new Dictionary<string, string>();
                parameters.Add("userId", argumentUserId);
                contentFrame.Navigate(typeof(Settings), parameters);
            }

           //navigates to the other pages of navigation bar
            if(e.InvokedItemContainer is NavigationViewItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString();
                //creating paramter of userId to pass to the next child file to avoid ambiguity
                string argumentUserId = GlobalContext.userId;
                var parameters = new Dictionary<string, string>();
                parameters.Add("userId", argumentUserId);

                switch (tag)
                    {
                        case "HomePage":
                          
                            contentFrame.Navigate(typeof(HomePage), parameters);
                            break;

                        case "DriversInformation":
                           
                            contentFrame.Navigate(typeof(DriversInformation),parameters);
                            break;
                      
                    }
                
            }
            
        }
        
    }
}
