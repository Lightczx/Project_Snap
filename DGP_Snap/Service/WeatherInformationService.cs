using DGP_Daily_V2.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DGP_Daily_V2.Services
{
    public class WeatherInformationService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }
            storage = value;
            OnPropertyChanged(propertyName);
        }
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public static async Task<WeatherInformation> GetWeatherInfomationAsync()
        {
            Stream xmlstream;
            WeatherInformation weatherInfomation = new WeatherInformation();
            void getstream()
            {
                xmlstream = GetWeatherInformationFromStream(CityInformation.余姚);
                if (xmlstream != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeatherInformation));
                    weatherInfomation = (WeatherInformation)xmlSerializer.Deserialize(xmlstream);
                }
            }
            await Task.Run(() => getstream());
            return weatherInfomation;
        }

        private static Stream GetWeatherInformationFromStream(CityInformation CityKey)
        {
            string infouri = @"http://wthrcdn.etouch.cn/WeatherApi?citykey=" + CityKey.ToString("D");
            //请求头
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(infouri);
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = "GET";

            Stream stream;
            string xmlstring = string.Empty;
            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    //Stream stream = new System.IO.Compression.GZipStream(webResponse.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                    stream = webResponse.GetResponseStream();
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        xmlstring = reader.ReadToEnd();
                        //stream.Dispose();
                    }
                }
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstring);

                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(sw))
                    {
                        writer.Indentation = 2;  // the Indentation
                        writer.Formatting = Formatting.Indented;
                        doc.WriteContentTo(writer);
                    }
                    xmlstring = sw.ToString();
                }
                byte[] array = Encoding.UTF8.GetBytes(xmlstring);
                MemoryStream xmlstream = new MemoryStream(array);
                return xmlstream;
            }
            catch (WebException)
            {
                return null;
            }
        }

        public static CityInformation GetCityInformationFromQueryString(string QueryString)
        {
            foreach (CityInformation item in Enum.GetValues(typeof(CityInformation)))
            {
                if (QueryString == item.ToString("F"))
                    return item;
            }
            return CityInformation.北京;
        }

        private WeatherInformation _weatherInformation=null;
        public WeatherInformation WeatherInformation
        {
            get
            {
                return _weatherInformation;
            }
            set
            {
                Set(ref _weatherInformation, value);
                //_weatherInformation = value;
            }
        }
        //public async static Task<DailyBoardViewItemModel> GetWeatherInformationForDailyBoardItemAsync()
        //{
        //    if (_weatherInformation == null)
        //        _weatherInformation = await GetWeatherInfomationAsync();
        //    return new DailyBoardViewItemModel
        //    {
        //        Title=_weatherInformation.RealTimeTemp + "°",
        //        //SubTitle=_weatherInformation.
        //        PrimaryText = _weatherInformation.City,
        //        SecondaryText = _weatherInformation.UpdateTime,
        //        NavigatedPageType = typeof(WeatherPage),
        //        NavigatedParameter = _weatherInformation,
        //    };
        //}
    }
}
