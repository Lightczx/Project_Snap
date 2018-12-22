using DGP_Daily_V2.Helpers;
using DGP_Snap.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DGP_Snap.Helpers
{
    public static class SettingsStorage
    {
        public static AppSettings AppSettings { get; set; }

        private const string settingsfile = "setting.json";
        private static readonly string settingspath = AppDomain.CurrentDomain.BaseDirectory + settingsfile;

        public static async Task SaveSettingsAsync(AppSettings appSettings)
        {
            using (FileStream filestream = new FileStream(settingspath, FileMode.Create))
            {
                StreamWriter stringwriter = new StreamWriter(filestream);
                await stringwriter.WriteAsync(await Json.StringifyAsync(appSettings));
                stringwriter.Flush();
                stringwriter.Close();
            }
        }

        public static async Task<AppSettings> RetriveSettingsAsync()
        {
            using(FileStream fileStream =new FileStream(settingspath, FileMode.OpenOrCreate))
            {
                StreamReader streamReader = new StreamReader(fileStream);
                char[] buffer = new char[streamReader.BaseStream.Length];
                await streamReader.ReadAsync(buffer, 0, (int)streamReader.BaseStream.Length);
                streamReader.Close();
                return await Json.ToObjectAsync<AppSettings>(new string(buffer));
            }
        }
    }
}
