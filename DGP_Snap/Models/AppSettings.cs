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
        [JsonProperty("Generic")]
        public Generic Generic { get; set; } = new Generic();
    }


    public class Generic
    {
        [JsonProperty("UISettings")]
        public UISettings UISettings { get; set; } = new UISettings();
        [JsonProperty("WallpaperSettings")]
        public WallpaperSettings WallpaperSettings { get; set; } = new WallpaperSettings();
    }
    
    public class UISettings
    {
        [JsonProperty("ThemeSettings")]
        public ThemeSettings ThemeSettings { get; set; } = new ThemeSettings();
        [JsonProperty("NavigationViewSettiing")]
        public NavigationViewSettiing NavigationViewSettiing { get; set; } = new NavigationViewSettiing();
    }
   
    public class ThemeSettings
    {
        [JsonProperty("ThemeMode")]
        public ThemeMode ThemeMode { get; set; } = ThemeMode.Dark;
    }

    public class NavigationViewSettiing
    {
        [JsonProperty("NavigationPaneOpacity")]
        public double NavigationPaneOpacity { get; set; } = 0.6;
    }
    
    public class WallpaperSettings
    {
        [JsonProperty("ImageModeSettngs")]
        public ImageModeSettngs ImagegModeSettngs { get; set; } = new ImageModeSettngs();
        [JsonProperty("ImageLightness")]
        public double ImageLightness { get; set; } = 0.6;
    }
    
    public class ImageModeSettngs
    {
        [JsonProperty("WebImageSettings")]
        public WebImageSettings WebImageSettings { get; set; } = new WebImageSettings();
        [JsonProperty("LocalImageSettings")]
        public LocalImageSettings LocalImageSettings { get; set; } = new LocalImageSettings();
    }

    public class WebImageSettings
    {
        [JsonProperty("RequestQueueSettings")]
        RequestQueueSettings RequestQueueSettings { get; set; } = new RequestQueueSettings();
        [JsonProperty("RestoredFolderPath")]
        public string RestoredFolderPath { get; set; } = "";
        [JsonProperty("SwitchIntervalByMinutes")]
        public double SwitchIntervalByMinutes { get; set; } = 5;
    }

    public class RequestQueueSettings
    {
        [JsonProperty("IsBingOnRequestQueue")]
        public bool IsBingOnRequestQueue { get; set; } = true;
        [JsonProperty("Is360OnRequestQueue")]
        public bool Is360OnRequestQueue { get; set; } = true;
        [JsonProperty("IsBaiduOnRequestQueue")]
        public bool IsBaiduOnRequestQueue { get; set; } = true;
        [JsonProperty("IsSougouOnRequestQueue")]
        public bool IsSougouOnRequestQueue { get; set; } = true;
    }

    public class LocalImageSettings
    {
        [JsonProperty("LoadFromFolderPath")]
        public string LoadFromFolderPath { get; set; } = "";
    }
}
