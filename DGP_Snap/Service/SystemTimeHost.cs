using DGP_Snap.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DGP_Snap.Service
{


    public class SystemTimeHost : INotifyPropertyChanged
    {
        /// <summary>
        /// Snap: 核心Timer
        /// </summary>
        private DispatcherTimer innerTimer;
        /// <summary>
        /// Snap: 壁纸Timer
        /// </summary>
        private DispatcherTimer wallPaperTimer;

        //INotifyPropertyChanged接口实现部分
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

        /// <summary>
        /// Snap: 当前日期与时间，无需实现通知
        /// </summary>
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
                PresentTimeSpanString = GetTimeSpan();
                PresentTimeSpanString2 = GetTimeSpan2();
                Set(ref _presentDateString, value);
            }
        }
        private string _presentDateString;

        /// <summary>
        /// 
        /// </summary>
        public string PresentTimeSpanString
        {
            get
            {
                return GetTimeSpan();
            }
            set
            {
                Set(ref _presentDateTimeString, value);
            }
        }

        private string GetTimeSpan()
        {
            DateTime t0 = new DateTime(2019, 6, 7);
            return "距离高考还有" + (t0 - CurrentDateTime).ToString() + "天";
        }

        private string _presentDateTimeString;//=GetTimeSpan();


        public string PresentTimeSpanString2
        {
            get
            {
                return GetTimeSpan2();
            }
            set
            {
                Set(ref _presentDateTimeString2, value);
            }
        }

        private string GetTimeSpan2()
        {
            DateTime t0 = new DateTime(2019, 4, 7);
            return "距离选考还有" + (t0 - CurrentDateTime).ToString() + "天";
        }

        private string _presentDateTimeString2;

        /// <summary>
        /// Snap: WalllPaperImageSource 总集
        /// </summary>
        public List<Uri> ImageUriCollection { get; set; }

        public SystemTimeHost()
        {
            //日期时间Timer初始化
            innerTimer = new DispatcherTimer();
            innerTimer.Tick += OnInnerTimerTicked;
            innerTimer.Interval = TimeSpan.FromSeconds(1);
            innerTimer.Start();
            //壁纸Timer初始化
            wallPaperTimer = new DispatcherTimer();
            wallPaperTimer.Tick += OnWallPaperTimerTicked;
            wallPaperTimer.Interval = TimeSpan.FromMinutes(5);
            wallPaperTimer.Start();
            //初次设置壁纸
        }

        private void OnWallPaperTimerTicked(object sender, EventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.SwitchRandomWallPaper();
        }

        public bool SwitchWallPaperTimerState()
        {
            if (wallPaperTimer.IsEnabled)
            {
                wallPaperTimer.Stop();
                Debug.WriteLine("disabled");
                return false;
            }
            else
            {
                wallPaperTimer.Start();
                Debug.WriteLine("enabled");
                return true;
            }
        }

        private void OnInnerTimerTicked(object sender, EventArgs e)
        {
            //时间
            CurrentDateTime = DateTime.Now;

            PresentTimeString = CurrentDateTime.ToString("HH:mm");

            string Month = MonthToEnglish(CurrentDateTime.Month);
            string DayOfMonth = CurrentDateTime.Day.ToString();
            string DayOfWeek = WeekdayToEnglish(CurrentDateTime.DayOfWeek);
            PresentDateString = $"{DayOfWeek} - {Month} {DayOfMonth}";
        }

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
    }
}
