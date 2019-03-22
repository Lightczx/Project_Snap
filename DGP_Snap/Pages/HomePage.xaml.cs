using DGP_Snap.Models;
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
using System.Windows.Threading;
using System.IO;
using System.Text;
using MahApps.Metro.Controls;
using DGP_Snap.Views;

namespace DGP_Snap.Pages
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page,INotifyPropertyChanged
    {
        #region INotifyPropertyChanged实现
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

        private DispatcherTimer innerTimer = new DispatcherTimer(DispatcherPriority.Send);

        public DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// Snap: 显示的时间
        /// </summary>
        public string PresentTimeString
        {
            get
            {
                return _presentTimeString;
            }
            set
            {
                Set(ref _presentTimeString, value);
            }
        }
        private string _presentTimeString;

        /// <summary>
        /// Snap: 显示的日期
        /// </summary>
        public string PresentDateString
        {
            get
            {
                return _presentDateString;
            }
            set
            {
                
                Set(ref _presentDateString, value);
            }
        }
        private string _presentDateString;

        /// <summary>
        /// Snap: 显示的倒计时
        /// </summary>
        public string PresentTimeSpanString
        {
            get
            {
                return _presentTimeSpanString = GetTimeSpan();
            }
            set
            {
                Set(ref _presentTimeSpanString, value);
            }
        }
        private string _presentTimeSpanString;

        private string GetTimeSpan()
        {
            DateTime t0 = new DateTime(2019, 6, 7);
            DateTime t1 = new DateTime(2019, 4, 6);
            string GaokaoTimeSpan = (t0 - CurrentDateTime).Days.ToString();
            string XuanKaoTimeSpan = (t1 - CurrentDateTime).Days.ToString();
            if (DateTime.Compare(DateTime.Now, new DateTime(2019, 4, 6)) > 0)
            {
                //return $"高考 / 选考    {GaokaoTimeSpan} / {XuanKaoTimeSpan}  天";
                return $"距高考还有    {GaokaoTimeSpan}    天";
            }
            else
            {
                //return $"距高考还有    {GaokaoTimeSpan}    天";
                return $"高考 / 选考    {GaokaoTimeSpan} / {XuanKaoTimeSpan}  天";
            }
            
        }

        public HomePage()
        {
            InitializeComponent();

            DataContext = this;

            innerTimer.Tick += OnInnerTimerTicked;
            innerTimer.Interval = TimeSpan.FromSeconds(1);
            innerTimer.Start();
        }

        private void OnInnerTimerTicked(object sender, EventArgs e) => UpdatePresentTime();

        private void UpdatePresentTime()
        {
            CurrentDateTime = DateTime.Now;
            PresentTimeSpanString = GetTimeSpan();

            PresentTimeString = CurrentDateTime.ToString("HH:mm");

            string Month = MonthToEnglish(CurrentDateTime.Month);
            string DayOfMonth = CurrentDateTime.Day.ToString();
            string DayOfWeek = WeekdayToEnglish(CurrentDateTime.DayOfWeek);

            PresentDateString = $"{DayOfWeek} - {Month} {DayOfMonth}";
        }

        #region 输出格式化方法

        private string MonthToEnglish(int Month)
        {
            switch (Month)
            {
                case 1:
                    return "January";
                case 2:
                    return "Feberuary";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
            }
            return string.Empty;
        }

        private string WeekdayToEnglish(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Monday";
                case DayOfWeek.Tuesday:
                    return "Tuesday";
                case DayOfWeek.Wednesday:
                    return "Wednesday";
                case DayOfWeek.Thursday:
                    return "Thursday";
                case DayOfWeek.Friday:
                    return "Friday";
                case DayOfWeek.Saturday:
                    return "Saturday";
                case DayOfWeek.Sunday:
                    return "Sunday";
            }
            return string.Empty;
        }


        #endregion

        private void TransitioningContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((TransitioningContentControl)sender).Content = new MottoView();
        }
    }
}
