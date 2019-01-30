using DGP_Snap.Models;
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
using System.Windows.Threading;

namespace DGP_Snap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : /*Metro*/Window, INotifyPropertyChanged
    {
        


        //public WeatherInformation WeatherInformation { get; set; }

        public Frame CurrentFrame => currentFrame;

        public HamburgerMenuItem Selected { get; private set; }

        //public string Load { get { return SettingsStorage.AppSettings.Generic.WallpaperSettings.ImagegModeSettngs.LocalImageSettings.LoadFromFolderPath; } set { SettingsStorage.AppSettings.Generic.WallpaperSettings.ImagegModeSettngs.LocalImageSettings.LoadFromFolderPath = value; } }

        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
            Service.NavigationService.Navigated += Frame_Navigated;
     
        }

        

        //INotifyPropertyChanged实现
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

        //退出程序
        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            await SettingsStorage.SaveSettingsAsync();
            Application.Current.Shutdown();
        }

        private async void WindowLayer_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsStorage.AppSettings = await SettingsStorage.RetriveSettingsAsync();
        }
        //private bool islocked = false;


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

        //private async void SwitchWallPaperButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!islocked)
        //    {
        //        SwitchRandomWallPaper();
        //    }
        //    else
        //    {
        //        await this.ShowMessageAsync("Warning", "请输入正确的工程码");
        //    }

        //}



        //private /*async*/ void DownloadWallPaperButton_Click(object sender, RoutedEventArgs e)
        //{
        //    /*await*/ NewMethod();

        //}




        //private async void LockButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (islocked)
        //    {
        //        //unlock it
        //        string password = await this.ShowInputAsync("解锁", "请输入工程码");
        //        if (password == "admin")
        //        {
        //            //真正解锁
        //            ((Button)sender).Content = "";
        //            //SystemTimeHost.SwitchWallPaperTimerState();
        //            islocked = false;
        //            try
        //            {
        //                CurrentImageUri = ImageSourceUriCollection.GetRandom();
        //                CurrentImageSource = new BitmapImage(CurrentImageUri);

        //            }
        //            catch
        //            {
        //                Debug.WriteLine("切换壁纸时出错");
        //            }
        //        }
        //        else
        //        {
        //            await this.ShowMessageAsync("工程码错误", "请输入正确的工程码");
        //        }
        //    }
        //    else//notlocked
        //    {
        //        //lock it
        //        ((Button)sender).Content = "";
        //        CurrentImageSource = null;
        //        //SystemTimeHost.SwitchWallPaperTimerState();
        //        islocked = true;
        //    }         
        //}

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

        private async void WindowLayer_Unloaded(object sender, RoutedEventArgs e)
        {
            await SettingsStorage.SaveSettingsAsync();
        }
    }
}
