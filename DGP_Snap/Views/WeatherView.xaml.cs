using DGP_Daily_V2.Models;
using DGP_Daily_V2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DGP_Snap.Views
{
    /// <summary>
    /// WeatherView.xaml 的交互逻辑
    /// </summary>
    public partial class WeatherView : UserControl
    {
        public WeatherView()
        {
            //WeatherInformation = new WeatherInformation();
            //WeatherInformation.RealTimeTemp = "20Z";
            //WeatherInformation.Forecast = new Forecast();
            //WeatherInformation.Forecast.Weathers = new List<Weather>();
            //WeatherInformation.Forecast.Weathers.Add(new Weather { Day = new WeatherDayHalf { State = "晴" } });
            InitializeComponent();
            BaseControl.WeatherInformation = WeatherInformation;
        }

        public WeatherInformation WeatherInformation
        {
            get
            {
                return GetValue(WeatherInformationProperty) as WeatherInformation;
            }
            set
            {
                SetValue(WeatherInformationProperty, value);
            }
        }
        public static readonly DependencyProperty WeatherInformationProperty = DependencyProperty.Register("WeatherInformation", typeof(WeatherInformation), typeof(WeatherView), new PropertyMetadata(null));

        private async void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            WeatherInformation = await WeatherInformationService.GetWeatherInfomationAsync();
        }
    }
}
