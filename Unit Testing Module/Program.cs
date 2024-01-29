using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Unit_Testing_Module
{
    public class Program
    {
        static void Main()
        {

            Program programInstance = new Program();
            Task.Run(async () =>
            {
                Console.WriteLine(await programInstance.LoginModule("123", "3@gmail.com"));
            }).Wait();
        }

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
            try
            {
                if (isValidEmail(email))
                {
                    string mongoDbConnectionString = "mongodb+srv://abubakerNaeem:pakistan123@randomcluster.lrzzlt8.mongodb.net/?retryWrites=true&w=majority";
                    var dataBaseName = "myRegisterDb";
                    var usersCollectionName = "RegisteredUsersData";

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
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in the Login Module {ex.Message}");
                return false;
            }
        }
    }
}
