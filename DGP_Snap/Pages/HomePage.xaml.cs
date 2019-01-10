using DGP_Daily_V2.Models;
using DGP_Snap.Helpers;
using DGP_Snap.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DGP_Snap.Pages
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page,INotifyPropertyChanged
    {
        //INotifyPropertyChanged实现
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

        public SystemTimeHost SystemTimeHost;

        public WeatherInformation WeatherInformation { get; set; }

        public List<Uri> ImageSourceUriCollection { get; set; }
        private ImageSource _currentImageSource;
        public ImageSource CurrentImageSource
        {
            get
            {
                return _currentImageSource;
            }
            set
            {
                Set(ref _currentImageSource, value);
            }
        }
        public Uri CurrentImageUri { get; set; }

        


        public HomePage()
        {
            InitializeComponent();
            SystemTimeHost = new SystemTimeHost();
            DataContext = this;
            TimePresenter.DataContext = SystemTimeHost;
            DatePresenter.DataContext = SystemTimeHost;
            TimeSpanPresenter.DataContext = SystemTimeHost;
            TimeSpanPresenter2.DataContext = SystemTimeHost;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown(/*int exitcode*/);

        private void SwitchWallPaperButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchRandomWallPaper();
        }

        public void SwitchRandomWallPaper()
        {
            try
            {
                CurrentImageUri = ImageSourceUriCollection.GetRandom();
                CurrentImageSource = new BitmapImage(CurrentImageUri);
            }
            catch
            {
                Debug.WriteLine("切换壁纸时出错");
            }
        }

        private void DownloadWallPaperButton_Click(object sender, RoutedEventArgs e)
        {

            using (WebClient webClient = new WebClient())
            {
                string path = FileAccessHelper.GetFilePickerPath("png 文件 (*.png)|*.png|All files (*.*)|*.*", "picture.png");

                if (path != null && CurrentImageUri != null)
                {
                    try
                    {
                        webClient.DownloadFile(CurrentImageUri.OriginalString, path);
                    }
                    catch (WebException)
                    {
                        Debug.WriteLine("网络出错");
                    }
                }

            }

        }

        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            bool state = SystemTimeHost.SwitchWallPaperTimerState();
            ((Button)sender).Content = state ? "" : "";
            if (state)//解锁
            {
                try
                {
                    CurrentImageUri = ImageSourceUriCollection.GetRandom();
                    CurrentImageSource = new BitmapImage(CurrentImageUri);
                }
                catch
                {
                    Debug.WriteLine("切换壁纸时出错");
                }
            }
            else
            {
                CurrentImageSource = null;
            }

        }
    }
}
