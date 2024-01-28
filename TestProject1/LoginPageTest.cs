using Formula_1_Management_System;

namespace TestProject1
{
    public class LoginPageTest
    {
        [Fact]
        public void VerifyPassword_ValidPassword_ReturnsTrue()
        {
            // Arrange
            Formula_1_Management_System.LoginPage loginPage = new Formula_1_Management_System.LoginPage();
            string inputPassword = "abubaker";
            string storedPassword = "abubaker"; // Replace with actual stored password

            // Act
            bool result = loginPage.VerifyPassword(inputPassword, storedPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            Formula_1_Management_System.LoginPage loginPage = new Formula_1_Management_System.LoginPage();
            string inputPassword = "invalidpassword";
            string storedPassword = "storedpassword"; // Replace with actual stored password

            // Act
            bool result = loginPage.VerifyPassword(inputPassword, storedPassword);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("validemail@example.com", true)]
        [InlineData("invalidemail", false)]
        [InlineData("", false)]
        public void IsValidEmail_ValidAndInvalidEmail_ReturnsExpectedResult(string email, bool expectedResult)
        {
            // Arrange
            Formula_1_Management_System.LoginPage loginPage = new Formula_1_Management_System.LoginPage();

            // Act
            bool result = loginPage.isValidEmail(email);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        // Add more test methods for other functionalities in LoginPage class
    }
}
   