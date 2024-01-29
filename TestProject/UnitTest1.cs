using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Unit_Testing_Module.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task LoginModule_ValidCredentials_ReturnsTrue()
        {
            
            // Arrange
            Unit_Testing_Module.Program programInstance = new Unit_Testing_Module.Program();
            string validPassword = "123"; // Replace with a valid password
            string validEmail = "3@gmail.com"; // Replace with a valid email

            // Act
            bool result = await programInstance.LoginModule(validPassword, validEmail);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task LoginModule_ValidCredentials_ReturnsFalse()
        {
            // Arrange
            Unit_Testing_Module.Program programInstance = new Unit_Testing_Module.Program();
           
            string validPassword = "1234"; // Replace with a valid password
            string validEmail = "3@gmail.com"; // Replace with a valid email

            // Act
            bool result = await programInstance.LoginModule(validPassword, validEmail);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task LoginModule_ValidEmail_ReturnsTrue()
        {
            // Arrange
            Unit_Testing_Module.Program programInstance = new Unit_Testing_Module.Program();
            string validPassword = "123"; // Replace with a valid password
            string validEmail = "3@gmail.com"; // Replace with a valid email

            // Act
            bool result = await programInstance.LoginModule(validPassword, validEmail);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task LoginModule_ValidEmail_ReturnsFalse()
        {
            // Arrange
            Unit_Testing_Module.Program programInstance = new Unit_Testing_Module.Program();
            string validPassword = "1234"; // Replace with a valid password
            string validEmail = "3232"; // Replace with a valid email

            // Act
            bool result = await programInstance.LoginModule(validPassword, validEmail);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
