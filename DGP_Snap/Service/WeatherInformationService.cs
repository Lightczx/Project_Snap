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


        public static async Task<WeatherQueryData> GetWeatherInfomationAsync()
        {
            Stream xmlstream;
            WeatherQueryData weatherInfomation = new WeatherQueryData();
            void getstream()
            {
                xmlstream = GetWeatherInformationFromStream(CityInformation.余姚);
                if (xmlstream != null)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeatherQueryData));
                    weatherInfomation = (WeatherQueryData)xmlSerializer.Deserialize(xmlstream);
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
            string xmlResponseString = string.Empty;
            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    //Stream stream = new System.IO.Compression.GZipStream(webResponse.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                    stream = webResponse.GetResponseStream();
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        xmlResponseString = streamReader.ReadToEnd();
                        //stream.Dispose();
                    }
                }
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlResponseString);

                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                    {
                        xmlTextWriter.Indentation = 2;  // the Indentation
                        xmlTextWriter.Formatting = Formatting.Indented;
                        xmlDocument.WriteContentTo(xmlTextWriter);
                    }
                    xmlResponseString = stringWriter.ToString();
                }
                byte[] array = Encoding.UTF8.GetBytes(xmlResponseString);
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

        private WeatherQueryData _weatherInformation=null;
        public WeatherQueryData WeatherInformation
        {
            get
            {
                return _weatherInformation;
            }
            set
            {
                Set(ref _weatherInformation, value);
            }
        }
    }
}
