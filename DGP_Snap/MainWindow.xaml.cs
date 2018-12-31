using DGP_Daily_V2.Models;
using DGP_Snap.Helpers;
using DGP_Snap.Service;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DGP_Snap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
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


        public MainWindow()
        {
            //调试模式配置
            #region
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            if (System.Diagnostics.Debugger.IsAttached)
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
                ResizeMode = ResizeMode.CanResize;
            }
            #endregion

            InitializeComponent();

            SystemTimeHost = new SystemTimeHost();
            TimePresenter.DataContext = SystemTimeHost;
            DatePresenter.DataContext = SystemTimeHost;
            //WallPaperImage.DataContext = this;
            DataContext = this;
        }

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


        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown(/*int exitcode*/);

        private async void WindowLayer_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化设置
            SettingsStorage.AppSettings = await SettingsStorage.RetriveSettingsAsync();
            //初始化图片
            await InitializeWallpaper();

        }

        private async Task InitializeWallpaper()
        {
            try
            {
                ImageSourceUriCollection = await WallPaperService.GetBingImageSourceCollectionAsync();
                ImageSourceUriCollection = ImageSourceUriCollection.Union(await WallPaperService.GetWallPaperImageSourceCollectionAsync()).ToList();
                CurrentImageUri = ImageSourceUriCollection.GetRandom();
                CurrentImageSource = new BitmapImage(CurrentImageUri);
            }
            catch
            {
                Debug.WriteLine("Snap: 异步获取图片时出错");
            }
        }

        //private void MenuButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (LeftPane.ActualWidth == 48)
        //    {
        //        //打开面板
        //        FunctionButtonPanel.Visibility = Visibility.Visible;

        //        (/*Application.Current.*/Resources["PaneOpenAnimation"] as Storyboard).Begin();
        //        //(/*Application.Current.*/Resources["PaneOpenColorAnimation"] as Storyboard).Begin();
        //        (/*Application.Current.*/Resources["BackgroundBlurEffectInAnimation"] as Storyboard).Begin();
        //    }
        //    else
        //    {
        //        //关闭面板
        //        FunctionButtonPanel.Visibility = Visibility.Collapsed;
        //        (/*Application.Current.*/Resources["PaneCloseAnimation"] as Storyboard).Begin();
        //        //(/*Application.Current.*/Resources["PaneCloseColorAnimation"] as Storyboard).Begin();
        //        (/*Application.Current.*/Resources["BackgroundBlurEffectOutAnimation"] as Storyboard).Begin();
        //    }

        //}

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
                string path= FileAccessHelper.GetFilePickerPath("png 文件 (*.png)|*.png|All files (*.*)|*.*","picture.png");

                if(path!=null&& CurrentImageUri!=null)
                {
                    try
                    {
                        webClient.DownloadFile(CurrentImageUri.OriginalString, path);
                    }
                    catch(WebException)
                    {
                        Debug.WriteLine("网络出错");
                    }
                }
                    
            }

        }

        private bool islocked = false;
        private async void LockButton_Click(object sender, RoutedEventArgs e)
        {
            if (islocked)
            {
                //unlock it
                string password = await this.ShowInputAsync("解锁", "请输入工程码");
                if (password == "admin")
                {
                    //真正解锁
                    ((Button)sender).Content = "";
                    SystemTimeHost.SwitchWallPaperTimerState();
                    islocked = false;
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
                    await this.ShowMessageAsync("工程码错误", "请输入正确的工程码");
                }
            }
            else//notlocked
            {
                //lock it
                ((Button)sender).Content = "";
                CurrentImageSource = null;
                SystemTimeHost.SwitchWallPaperTimerState();
                islocked = true;
            }         
        }
    }
}
