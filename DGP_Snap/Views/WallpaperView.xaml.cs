using DGP_Snap.Helpers;
using DGP_Snap.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DGP_Snap.Views
{
    /// <summary>
    /// WallpaperView.xaml 的交互逻辑
    /// </summary>
    public partial class WallpaperView : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private DispatcherTimer innerTimer=new DispatcherTimer(DispatcherPriority.Send);

        public WallpaperView()
        {
            DataContext = this;
            InitializeComponent();
            //壁纸计时器初始化
            innerTimer.Tick += OnInnerTimerTicked;
            innerTimer.Interval = TimeSpan.FromMinutes(5.0);
            innerTimer.Start();
        }

        private void OnInnerTimerTicked(object sender, EventArgs e)
        {
            SwitchRandomWallPaper();
        }

        public List<Uri> ImageUriCollection { get; set; }

        private ImageSource _currentImageSource;
        public ImageSource CurrentImageSource
        {
            get => _currentImageSource;
            set => Set(ref _currentImageSource, value);
        }
        public Uri CurrentImageUri { get; set; }

        private async Task InitializeWallpaperAsync()
        {

            //try
            //{
            ImageUriCollection = await WallPaperService.GetWallPaperImageSourceCollectionAsync();
            //List<Uri> WallpaperFrom360 = 
            //    ImageUriCollection = ImageUriCollection.Union(WallpaperFrom360).ToList();

                SwitchRandomWallPaper();
                //GetAllWallpaper();
            //}
            //catch
            //{
            //    Debug.WriteLine("Snap: 异步获取图片时出错");
            //}

        }

        public void SwitchRandomWallPaper()
        {
            try
            {
                CurrentImageUri = ImageUriCollection.GetRandom();
                CurrentImageSource = new BitmapImage(CurrentImageUri);
            }
            catch
            {
                Debug.WriteLine("切换壁纸时出错");
            }
        }

        private /*async*/ /*Task*/void GetAllWallpaper()
        {
            using (WebClient webClient = new WebClient())
            {
                //string path = FileAccessHelper.GetFolderPickerPath("png 文件 (*.png)|*.png|All files (*.*)|*.*", "picture.png");

                if (true/*path != null && CurrentImageUri != null*/)
                {
                    try
                    {
                        int i = 1;
                        foreach (Uri item in ImageUriCollection)
                        {
                            webClient.DownloadFile(item, @"D:\PICTURE\" + i.ToString() + ".png");
                            i++;
                        }


                        //await this.ShowMessageAsync("Success", "Download image successfully.");
                    }
                    catch (WebException)
                    {
                        Debug.WriteLine("网络出错");
                    }
                }

            }
        }

        //INotifyPropertyChanged
        #region 
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
        #endregion

        private async void Root_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeWallpaperAsync();
        }
    }
}
