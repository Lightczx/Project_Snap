using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DGP_Snap.Helpers
{
    class WebRequestHelper
    {
        public static async Task<T> GetRequestImageInfoObjectAsync<T>(string basedRequestUrl,string contentType= "application/json;charset=UTF-8")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(basedRequestUrl);
            request.Method = "GET";
            request.ContentType = contentType;

            string jsonMetaString;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//获取响应
            {
                using (StreamReader responseStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                        jsonMetaString = responseStreamReader.ReadToEnd();
                }
            }
            return await Json.ToObjectAsync<T>(jsonMetaString); 
        }
    }
}
