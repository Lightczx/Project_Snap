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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DGP_Snap.Views
{
    /// <summary>
    /// WallpaperView.xaml 的交互逻辑
    /// </summary>
    public partial class WallpaperView : UserControl, INotifyPropertyChanged
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

        public List<Uri> ImageUriCollection { get; set; } = new List<Uri>();

        private string _imageDescription=string.Empty;
        public string CurrentImageDescription
        {
            get
            {
                return _imageDescription;
            }
            set
            {
                Set(ref _imageDescription, value);
            }
        }

        private ImageSource _currentImageSource;
        public ImageSource CurrentImageSource
        {
            get => _currentImageSource;
            set => Set(ref _currentImageSource, value);
        }
        public Uri CurrentImageUri { get; set; }

        private async Task InitializeWallpaperAsync()
        {
            ImageUriCollection = ImageUriCollection.Union(await WallPaperService.GetBaiduImageUriCollectionAsync()).ToList();
            ImageUriCollection = ImageUriCollection.Union(await WallPaperService.Get360ImageUriCollectionAsync()).ToList();
            ImageUriCollection = ImageUriCollection.Union(await WallPaperService.GetBingImageUriCollectionAsync()).ToList();

            SwitchRandomWallPaper();

        }

        public void SwitchRandomWallPaper()
        {
            //DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            //EasingDoubleKeyFrame sd = new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000)));
            //da.KeyFrames.Add(sd);
            //Storyboard.SetTargetName(da, WallPaperImage.Name);
            //DependencyProperty[] propertyChain = new DependencyProperty[]
            //{
            //    OpacityProperty
            //};
            //Storyboard.SetTargetProperty(da, new PropertyPath("(0)", propertyChain));

            //DoubleAnimationUsingKeyFrames da2 = new DoubleAnimationUsingKeyFrames
            //{
            //    BeginTime = new TimeSpan(0, 0, 0, 1, 5)
            //};
            //EasingDoubleKeyFrame sd2 = new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1200)));
            //da2.KeyFrames.Add(sd2);
            //Storyboard.SetTargetName(da2, WallPaperImage.Name);
            //Storyboard.SetTargetProperty(da2, new PropertyPath("(0)", propertyChain));

            //ObjectAnimationUsingKeyFrames oa = new ObjectAnimationUsingKeyFrames();
            //DiscreteObjectKeyFrame diso = new DiscreteObjectKeyFrame();
            //diso.Value= ImageUriCollection.GetRandom();
            //oa.KeyFrames.Add(diso);
            //oa.BeginTime = new TimeSpan(0, 0, 0, 1, 0);
            //Storyboard.SetTargetName(oa, WallPaperImage.Name);
            //DependencyProperty[] propertyChain2 = new DependencyProperty[]
            //{
                
            //    Image.SourceProperty
            //};
            // Storyboard.SetTargetProperty(oa, new PropertyPath("(0)", propertyChain2));

            //Storyboard bgstoryboard = new Storyboard();
            //bgstoryboard.Children.Add(da);
            //bgstoryboard.Children.Add(oa);
            //bgstoryboard.Children.Add(da2);

            try
            {
                //bgstoryboard.Begin();
                CurrentImageUri = ImageUriCollection.GetRandom();
                CurrentImageSource = new BitmapImage(CurrentImageUri);
                CurrentImageDescription = WallPaperService.ImageDescriptionCollection[ListExtension.CurrentIndex];
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

        
        #region 实现INotifyPropertyChanged
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

        private void SwitchWallPaperButton_Click(object sender, RoutedEventArgs e) => SwitchRandomWallPaper();

        private void DownloadWallPaperButton_Click(object sender, RoutedEventArgs e)
        {
            using (WebClient webClient = new WebClient())
            {
                string fileName = DateTimeOffset.Now.Date.ToShortDateString().Replace("/", "-") + DateTimeOffset.Now.TimeOfDay.ToString().Replace(":","-");
                string path = FileAccessHelper.GetFilePickerPath("png 文件 (*.png)|*.png|All files (*.*)|*.*", fileName);

                if (path != null && CurrentImageUri != null)
                {
                    try
                    {
                        webClient.DownloadFile(CurrentImageUri.OriginalString,path);
                    }
                    catch (WebException)
                    {
                        Debug.WriteLine("网络出错");
                    }
                }
            }
        }

        private async void ToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                ImageUriCollection = new List<Uri>();
                if (Switch360.IsChecked == true)
                {
                    ImageUriCollection = ImageUriCollection.Union(await WallPaperService.Get360ImageUriCollectionAsync()).ToList();   
                }
                if (SwitchBaidu.IsChecked == true)
                {
                    ImageUriCollection = ImageUriCollection.Union(await WallPaperService.GetBaiduImageUriCollectionAsync()).ToList();
                }
                if (SwitchBing.IsChecked == true)
                {
                    ImageUriCollection = ImageUriCollection.Union(await WallPaperService.GetBingImageUriCollectionAsync()).ToList();
                }
            }
            
        }
    }
}
