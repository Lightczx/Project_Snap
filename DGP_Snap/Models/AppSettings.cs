using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGP_Snap.Models
{
    public enum ThemeMode
    {
        Dark,
        Light
    }

    [JsonObject("AppSettings")]
    public class AppSettings
    {
        public AppSettings() { }
        [JsonObject("Generic")]
        public class Generic
        {
            [JsonObject("UISettings")]
            public class UISettings
            {
                [JsonObject("ThemeSettings")]
                public class ThemeSettings
                {
                    [JsonProperty("ThemeMode")]
                    public ThemeMode ThemeMode { get; set; }
                }
                [JsonObject("NavigationViewSettiing")]
                public class NavigationViewSettiing
                {
                    [JsonProperty("NavigationPaneOpacity")]
                    public double NavigationPaneOpacity  { get;set; }
                }
            }
            [JsonObject("WwallpaperSettings")]
            public class WwallpaperSettings
            {
                public class ImagegModeSettngs
                {
                    public class WebImageSettings
                    {
                        public class RequestQueueSettings
                        {
                            public bool IsBingOnRequestQueue;

                            public bool Is360OnRequestQueue;

                            public bool IsBaiduOnRequestQueue;

                            public bool IsSougouOnRequestQueue;
                        }

                        public string RestoredFolderPath;

                        public double SwitchIntervalByMinutes;
                    }

                    public class LocalImageSettings
                    {
                        public string LoadFromFolderPath;
                    }
                }
            }
        }
    }
}
