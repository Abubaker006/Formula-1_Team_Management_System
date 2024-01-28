using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula_1_Management_System
{
    public class AppConfiguration
    {
        public string MongoDbConnectionString { get; set; } //done
        public string GoogleAppsPassword { get; set; } //done
        public string UserCollectionName { get; set; } //done
        public string DataBaseName { get; set; } //done
        public string DriverCollectionName { get; set; } //done
        public string GmailAccountName { get; set; } //done
    }
}
