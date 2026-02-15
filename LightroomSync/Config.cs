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

        /// <summary>If true, skip the Start Sync confirmation dialog.</summary>
        public bool SkipStartSyncConfirmation { get; set; }

        /// <summary>When a catalog was last successfully uploaded to the network.</summary>
        public DateTime? LastSyncTime { get; set; }

        /// <summary>Folder for log files. Empty = sync folder\Logs.</summary>
        public string LogFolder { get; set; } = "";

        /// <summary>If true, show the activity log panel. Default false.</summary>
        public bool ShowActivityLog { get; set; } = false;

        public Config()
        {
            if (Utils.IsDevMode)
            {
                this.LocalFolder = "";
                this.NetworkFolder = "";
                this.BackupFolder = "";
            }
            else
            {
                this.LocalFolder = "C:\\Users\\" + Environment.UserName + "\\Pictures\\Lightroom";
                this.NetworkFolder = "P:\\Lightroom";
                this.BackupFolder = "C:\\Users\\" + Environment.UserName + "\\Pictures\\LightroomBackups";
            }
        }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        
    }
}
