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

    }
}
