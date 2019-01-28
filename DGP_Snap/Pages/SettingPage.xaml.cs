using DGP_Snap.Helpers;
using DGP_Snap.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DGP_Snap.Pages
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page, INotifyPropertyChanged
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


        public SettingPage()
        {
            DataContext = this;
            InitializeComponent();
        }

        public double ImageLightness
        {
            get
            {
                return SettingsStorage.AppSettings.Generic.WallpaperSettings.ImageLightness;
            }
            set
            {
                if (Equals(SettingsStorage.AppSettings.Generic.WallpaperSettings.ImageLightness, value))
                {
                    return;
                }
                SettingsStorage.AppSettings.Generic.WallpaperSettings.ImageLightness = value;
                OnPropertyChanged("ImageLightness");
                
            }
        }


    }
}
