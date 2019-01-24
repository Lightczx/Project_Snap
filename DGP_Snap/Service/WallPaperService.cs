using DGP_Daily_V2.Helpers;
using DGP_Snap.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DGP_Snap.Service
{
    public static class WallPaperService
    {
        

        private static async Task<WallPaper360JsonInfo> GetRequestWallPaperImageJsonInfoAsync()
        {

            string basedRequestUrl = @"http://wallpaper.apc.360.cn/index.php?c=WallPaper&a=getAppsByOrder&order=create_time&start=0&count=180&from=360chrome";//必应

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(basedRequestUrl);
            request.Method = "GET";
            request.ContentType = "application/json;charset=UTF-8";

            //try
            //{
                string jsonMetaString;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//获取响应
                {
                    using (StreamReader responseStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        jsonMetaString = responseStreamReader.ReadToEnd();
                    }
                }
                return await Json.ToObjectAsync<WallPaper360JsonInfo>(jsonMetaString);
            //}
            //catch
            //{
            //    return null;
            //}
        }

        public static async Task<List<Uri>> GetWallPaperImageSourceCollectionAsync()
        {
            List<Uri> imageSourceCollection = new List<Uri>();

            WallPaper360JsonInfo wallPaper360JsonInfo = (await GetRequestWallPaperImageJsonInfoAsync());
            foreach (DataItemFor360 dataItem in wallPaper360JsonInfo.Data)
            {
                //restriction
                if (!(
                    dataItem.Tag.Contains("性感") ||
                    //dataItem.Tag.Contains("卡通") ||
                    //dataItem.Tag.Contains("动漫") ||
                    dataItem.Tag.Contains("游戏") ||
                    dataItem.Tag.Contains("美女") ||
                    dataItem.Tag.Contains("月历") ||
                    dataItem.Tag.Contains("影视") ||
                    dataItem.Tag.Contains("女孩") ||
                    dataItem.Tag.Contains("明星") ||
                    dataItem.Tag.Contains("车")
                    ))
                    imageSourceCollection.Add(new Uri(dataItem.Url));
            }

            return imageSourceCollection;
        }

        private static async Task<BingImageJsonInfo> GetRequestBingImageJsonInfoAsync()
        {

            string basedBingRequestUrl = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=8";//必应

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(basedBingRequestUrl);
            request.Method = "GET";
            request.ContentType = "text/xml;charset=UTF-8";

            try
            {
                string jsonMetaString;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//获取响应
                {
                    using (StreamReader responseStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        jsonMetaString = responseStreamReader.ReadToEnd();
                    }
                }
                return await Json.ToObjectAsync<BingImageJsonInfo>(jsonMetaString);

            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Uri>> GetBingImageSourceCollectionAsync()
        {
            string basedBingUrl = "http://cn.bing.com";
            List<Uri> imageSourceCollection = new List<Uri>();

            BingImageJsonInfo bingImageJsonInfo = await GetRequestBingImageJsonInfoAsync();
            foreach (ImageItemForBing imageItemForBing in bingImageJsonInfo.Images)
            {
                imageSourceCollection.Add(new Uri(basedBingUrl + imageItemForBing.Url));
            }
            return imageSourceCollection;
        }
    }

}
