using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LightroomSync
{
    internal class Config
    {
        public string LocalFolder { get; set; }
        public string NetworkFolder { get; set; }
        public string BackupFolder { get; set; }

        public bool AutoCheckForUpdates { get; set; }

        public Config() { 
            this.LocalFolder = "C:\\Users\\" + Environment.UserName + "\\Pictures\\Lightroom";
            this.NetworkFolder = "P:\\Lightroom";
            this.BackupFolder = "C:\\Users\\" + Environment.UserName + "\\Pictures\\LightroomBackups";
            this.AutoCheckForUpdates = true;
        }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        
    }
}
