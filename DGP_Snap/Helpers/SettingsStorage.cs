using DGP_Daily_V2.Helpers;
using DGP_Snap.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DGP_Snap.Helpers
{
    public static class SettingsStorage
    {
        public static AppSettings AppSettings { get; set; } = new AppSettings();

        private const string settingsfile = "settings.json";
        private static readonly string settingspath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settingsfile);


        public static async Task SaveSettingsAsync(AppSettings appSettings)
        {
            //File.WriteAllText(settingspath,await Json.StringifyAsync(appSettings));
            //File.WriteAllText(settingspath, "aaa");
            //using (FileStream filestream = new FileStream(settingspath, FileMode.Create))
            //{
            //using (StreamWriter stringwriter = new StreamWriter(settingspath))
            //{
            //    //stringwriter.WriteLine(await Json.StringifyAsync(appSettings));
            //    await stringwriter.WriteLineAsync("aaa");
            //}
            //}
            using (FileStream fs = new FileStream(settingspath, FileMode.Create))
            {
                //写入
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    
                    //await sw.WriteLineAsync("aaa");
                    await sw.WriteAsync(await Json.StringifyAsync(AppSettings));
                }

            }
        }

        public static async Task SaveSettingsAsync()
        {
            await SaveSettingsAsync(AppSettings);
        }

        public async static Task<AppSettings> RetriveSettingsAsync()
        {
            //using (FileStream fileStream = new FileStream(settingspath, FileMode.OpenOrCreate))
            return await Task.Run(() => { return new AppSettings(); });
            //await Json.ToObjectAsync<AppSettings>(File.ReadAllText(settingspath));

            //using (FileStream fileStream = new FileStream(settingspath, FileMode.OpenOrCreate))
            //{
            //    StreamReader streamReader = new StreamReader(fileStream);
            //    char[] buffer = new char[streamReader.BaseStream.Length];
            //    await streamReader.ReadAsync(buffer, 0, (int)streamReader.BaseStream.Length);
            //    streamReader.Close();
            //    fileStream.Close();
            //    return await Json.ToObjectAsync<AppSettings>(new string(buffer));

            //}
        }
    }
}
