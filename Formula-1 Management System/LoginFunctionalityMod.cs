using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Formula_1_Management_System
{
    public class LoginFunctionalityMod
    {
        
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
        public async Task<bool> LoginModule(string password, string email)
        {
            string mongoDbConnectionString = App.Configuration.GetSection("AppSettings:MongoDbConnectionString").Value;
            var dataBaseName = App.Configuration.GetSection("AppSettings:DataBaseName").Value;
            var usersCollectionName = App.Configuration.GetSection("AppSettings:UserUserCollectionName").Value;

            var client = new MongoClient(mongoDbConnectionString);
            var database = client.GetDatabase(dataBaseName);
            var collection = database.GetCollection<RegisteredUsersData>(usersCollectionName);



            var user = await collection.Find(u => u.Email == email).FirstOrDefaultAsync();

            if (user != null)
            {
                bool isPassword = VerifyPassword(password, user.Password);
                if (isPassword)
                {
                    return true;
                }
            }

            return false;

        }
    }
}
