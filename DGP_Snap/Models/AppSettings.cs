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
        public Generic Generic { get; set; }
    }

    [JsonObject("Generic")]
    public class Generic
    {
        public UISettings UISettings { get; set; }
        [JsonProperty("WallpaperSettings")]
        public WallpaperSettings WallpaperSettings { get; set; }
    }

    [JsonObject("UISettings")]
    public class UISettings
    {
        [JsonProperty("ThemeSettings")]
        public ThemeSettings ThemeSettings { get; set; }
        [JsonProperty("NavigationViewSettiing")]
        public NavigationViewSettiing NavigationViewSettiing { get; set; }
    }
   
    public class ThemeSettings
    {
        [JsonProperty("ThemeMode")]
        public ThemeMode ThemeMode { get; set; }
    }

    public class NavigationViewSettiing
    {
        [JsonProperty("NavigationPaneOpacity")]
        public double NavigationPaneOpacity { get; set; }
    }
    
    public class WallpaperSettings
    {
        [JsonProperty("ImageModeSettngs")]
        public ImageModeSettngs ImagegModeSettngs { get; set; }
    }
    
    public class ImageModeSettngs
    {
        [JsonProperty("WebImageSettings")]
        public WebImageSettings WebImageSettings { get; set; }
        [JsonProperty("LocalImageSettings")]
        public LocalImageSettings LocalImageSettings { get; set; }
    }

    public class WebImageSettings
    {
        [JsonProperty("RequestQueueSettings")]
        RequestQueueSettings RequestQueueSettings { get; set; }
        [JsonProperty("RestoredFolderPath")]
        public string RestoredFolderPath;
        [JsonProperty("SwitchIntervalByMinutes")]
        public double SwitchIntervalByMinutes;
    }

    public class RequestQueueSettings
    {
        [JsonProperty("IsBingOnRequestQueue")]
        public bool IsBingOnRequestQueue;
        [JsonProperty("Is360OnRequestQueue")]
        public bool Is360OnRequestQueue;
        [JsonProperty("IsBaiduOnRequestQueue")]
        public bool IsBaiduOnRequestQueue;
        [JsonProperty("IsSougouOnRequestQueue")]
        public bool IsSougouOnRequestQueue;
    }

    public class LocalImageSettings
    {
        [JsonProperty("LoadFromFolderPath")]
        public string LoadFromFolderPath;
    }
}
