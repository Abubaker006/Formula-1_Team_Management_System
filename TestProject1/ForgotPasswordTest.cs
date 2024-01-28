using Formula_1_Management_System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class ForgetPasswordTests
    {
        [Fact]
        public async void VerifyCodeAsync_CodeMatches_ReturnsTrue()
        {
            // Arrange
            var forgetPasswordPage = new ForgetPassword();
            var codeField = typeof(ForgetPassword).GetField("codeString", BindingFlags.NonPublic | BindingFlags.Instance);
            codeField.SetValue(forgetPasswordPage, "123456");  // Set a known code for testing

            // Act
            bool result = await forgetPasswordPage.VerifyCodeAsync("123456");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void VerifyCodeAsync_CodeDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            var forgetPasswordPage = new ForgetPassword();
            var codeField = typeof(ForgetPassword).GetField("codeString", BindingFlags.NonPublic | BindingFlags.Instance);
            codeField.SetValue(forgetPasswordPage, "123456");  // Set a known code for testing

            // Act
            bool result = await forgetPasswordPage.VerifyCodeAsync("654321");

            // Assert
            Assert.False(result);
        }

        // Add more test methods as needed for other functionalities of ForgetPassword class
    }
}
