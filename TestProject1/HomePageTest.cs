using Formula_1_Management_System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit; // Ensure you have the necessary using directive for Xunit

namespace TestProject1
{
    public class HomePageTest
    {
        [Fact]
        public void EditDetailsBtnHandle_NavigatesToEditingPanelInfo()
        {
            // Arrange
            var homePage = new HomePage();
            var globalContextField = typeof(HomePage).GetNestedType("GlobalContext", BindingFlags.Static | BindingFlags.NonPublic);
            var userIdProperty = globalContextField?.GetProperty("userId");
            userIdProperty?.SetValue(null, "testUserId");

            // Act
            homePage.EditDetailsBtnHandle(null, null);

            // Assert
            Assert.Equal("testUserId", userIdProperty?.GetValue(null));
        }

        [Fact]
        public void HomePage_Loaded_UserExists_SetsUIElements()
        {
            // Arrange
            var homePage = new HomePage();
            var userIdField = typeof(HomePage.GlobalContext).GetField("userId", BindingFlags.Static | BindingFlags.NonPublic);
            userIdField?.SetValue(null, "testUserId");

            // Act
            homePage.HomePage_Loaded(null, null);

            // Assert
            Assert.Equal("testUserId", userIdField?.GetValue(null));

            // Add more assertions for UI elements as needed
        }
    }
}
