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

        public bool AutoCheckForUpdates { get; set; }

        public Config() { 
            // DEV build: empty paths so prod catalog is never touched
            this.LocalFolder = "";
            this.NetworkFolder = "";
            this.AutoCheckForUpdates = true;
        }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        
    }
}
