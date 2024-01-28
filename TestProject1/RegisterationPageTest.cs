using Formula_1_Management_System;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class RegistrationPageTests
    {
        [Fact]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            var registrationPage = new RegistrationPage();
            bool result = registrationPage.IsValidEmail("test@example.com");
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            var registrationPage = new RegistrationPage();
            bool result = registrationPage.IsValidEmail("invalidemail");
            Assert.False(result);
        }

        [Fact]
        public void HashAndSaltPassword_ValidPassword_ReturnsNonEmptyString()
        {
            var registrationPage = new RegistrationPage();
            string result = registrationPage.HashAndSaltPassword("password123");
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void RegisterBtn_Clicked_ValidData_ReturnsSuccessfulRegistration()
        {
            var registrationPage = new RegistrationPage();
            // Set up any necessary dependencies or properties
            // ...

            // Use reflection to access private field
            FieldInfo wrongPasswordField = typeof(RegistrationPage).GetField("wrongPassword", BindingFlags.NonPublic | BindingFlags.Instance);
            TextBlock wrongPassword = (TextBlock)wrongPasswordField.GetValue(registrationPage);

            // Modify TextBlock properties or assert its state as needed
            Assert.NotNull(wrongPassword);

            registrationPage.RegisterBtn_Clicked(null, null);

            Assert.Equal("Registration successful.", wrongPassword.Text);
        }
    }
}
