﻿using DGP_Snap.Helpers;
using DGP_Snap.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DGP_Snap.Service
{
    public static class WallPaperService
    {
        private const string WallPaper360BasedUrL = @"http://wallpaper.apc.360.cn/index.php?c=WallPaper&a=getAppsByOrder&order=create_time&start=0&count=180&from=360chrome";
        private const string WallPaperBingBasedUrL = @"https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=8";
        private const string WallPaperBaiduBasedUrL = @"http://image.baidu.com/data/imgs?pn=0&rn=36&col=%E5%A3%81%E7%BA%B8&tag=%E9%A3%8E%E6%99%AF&tag3=%E8%87%AA%E7%84%B6%E9%A3%8E%E5%85%89&width=1920&height=1080&ic=0&ie=utf8&oe=utf-8&image_id=&fr=channel&p=channel&from=1&app=img.browse.channel.wallpaper&t=0.1993955611514664";

        public static List<string> ImageDescriptionCollection = new List<string>();

        public static async Task<List<Uri>> Get360ImageUriCollectionAsync()
        {
            List<Uri> imageUriCollection = new List<Uri>();

            Wallpaper360JsonObject wallPaper360JsonInfo =await WebRequestHelper.GetRequestImageInfoObjectAsync<Wallpaper360JsonObject>(WallPaper360BasedUrL);//await GetRequest360WallPaperImageJsonInfoAsync();

            foreach (DataItemFor360 dataItem in wallPaper360JsonInfo.Data)
            {
                //restriction
                //if (!(
                //    dataItem.Tag.Contains("性感") ||
                //    dataItem.Tag.Contains("卡通") ||
                //    dataItem.Tag.Contains("动漫") ||
                //    dataItem.Tag.Contains("游戏") ||
                //    dataItem.Tag.Contains("美女") ||
                //    dataItem.Tag.Contains("月历") ||
                //    dataItem.Tag.Contains("影视") ||
                //    dataItem.Tag.Contains("女孩") ||
                //    dataItem.Tag.Contains("明星") ||
                //    dataItem.Tag.Contains("车")
                //    ))
                imageUriCollection.Add(new Uri(dataItem.Url));
                ImageDescriptionCollection.Add(dataItem.Tag);
            }

            return imageUriCollection;
        }
        public static async Task<List<Uri>> GetBingImageUriCollectionAsync()
        {
            string basedBingUrl = "http://cn.bing.com";
            List<Uri> imageSourceCollection = new List<Uri>();

            BingImageJsonObject bingImageJsonInfo = await WebRequestHelper.GetRequestImageInfoObjectAsync<BingImageJsonObject>(WallPaperBingBasedUrL);

            foreach (ImageItemForBing imageItemForBing in bingImageJsonInfo.Images)
            {
                imageSourceCollection.Add(new Uri(basedBingUrl + imageItemForBing.Url));
                ImageDescriptionCollection.Add(imageItemForBing.Copyright);
            }
            return imageSourceCollection;
        }
        public static async Task<List<Uri>> GetBaiduImageUriCollectionAsync()
        {
            List<Uri> imageSourceCollection = new List<Uri>();

            BaiduImageJsonObject baiduImageJsonInfo = await WebRequestHelper.GetRequestImageInfoObjectAsync<BaiduImageJsonObject>(WallPaperBaiduBasedUrL);

            foreach (ImgsItemForBaidu imageItemForBaidu in baiduImageJsonInfo.Imgs)
            {
                if(imageItemForBaidu.ImageUrl != null)
                {
                    imageSourceCollection.Add(new Uri(imageItemForBaidu.ImageUrl));
                    ImageDescriptionCollection.Add(imageItemForBaidu.Desc);
                }
                else
                {
                    Debug.WriteLine("null");
                }  
            }
            return imageSourceCollection;
        }
    }

}
