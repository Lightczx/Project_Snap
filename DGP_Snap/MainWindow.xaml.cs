using DGP_Daily_V2.Models;
using DGP_Snap.Helpers;
using DGP_Snap.Pages;
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
using System.Windows.Navigation;

namespace DGP_Snap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {

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

        public Frame CurrentFrame => currentFrame;

        public HamburgerMenu Selected { get; private set; }

        public MainWindow()
        {
            ////NativeMethods.HideSystemTaskBar();
            ////调试模式配置
            //#region
            ////WindowState = WindowState.Maximized;
            ////videoScreenMediaElement.Play();
            ////this.WindowState = System.Windows.WindowState.Normal;
            ////this.WindowStyle = System.Windows.WindowStyle.None;
            ////this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.Topmost = true;

            //this.Left = -1;
            //this.Top = -1;
            ////WindowStyle = WindowStyle.None;
            ////ResizeMode = ResizeMode.NoResize;
            //Width = SystemParameters.PrimaryScreenWidth;
            //Height = SystemParameters.PrimaryScreenHeight;
            //if (Debugger.IsAttached)
            //{
            //    WindowState = WindowState.Normal;
            //    WindowStyle = WindowStyle.SingleBorderWindow;
            //    ResizeMode = ResizeMode.CanResize;
            //}
            ////WindowState = WindowState.Maximized;
            //#endregion

            InitializeComponent();
            DataContext = this;
            Service.NavigationService.Navigated += Frame_Navigated;
            ////SystemTimeHost = new SystemTimeHost();
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

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private async void WindowLayer_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化设置
            SettingsStorage.AppSettings = await SettingsStorage.RetriveSettingsAsync();
            //初始化图片
            await InitializeWallpaper();
            //NewMethod();
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

        private async void SwitchWallPaperButton_Click(object sender, RoutedEventArgs e)
        {
            if (!islocked)
            {
                SwitchRandomWallPaper();
            }
            else
            {
                await this.ShowMessageAsync("Warning", "请输入正确的工程码");
            }
            
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

        private /*async*/ void DownloadWallPaperButton_Click(object sender, RoutedEventArgs e)
        {
            /*await*/ NewMethod();

        }

        private /*async*/ /*Task*/void  NewMethod()
        {
            using (WebClient webClient = new WebClient())
            {
                //string path = FileAccessHelper.GetFolderPickerPath("png 文件 (*.png)|*.png|All files (*.*)|*.*", "picture.png");

                if (true/*path != null && CurrentImageUri != null*/)
                {
                    try
                    {
                        int i = 1;
                        foreach(Uri item in ImageSourceUriCollection)
                        {
                            webClient.DownloadFile(item, @"D:\PICTURE\" + i.ToString()+".png");
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
                    //SystemTimeHost.SwitchWallPaperTimerState();
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
                //SystemTimeHost.SwitchWallPaperTimerState();
                islocked = true;
            }         
        }

        private void NavigationView_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            if (args.IsItemOptions)
            {
                Service.NavigationService.Navigate<SettingPage>();
                return;
            }

            var item = NavigationView.Items
                            .OfType<HamburgerMenuItem>()
                            .First(menuItem => menuItem.Label == ((HamburgerMenuItem)args.InvokedItem).Label);

            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            Service.NavigationService.Navigate(pageType);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            //if (e.Content.GetType() == typeof(SettingPage))
            //{
            //    Selected = NavigationView.OptionsItems. as NavigationViewItem;
            //    return;
            //}

            //Selected = navigationView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }
    }
}
