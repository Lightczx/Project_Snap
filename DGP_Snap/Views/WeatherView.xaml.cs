using DGP_Snap.Models;
using DGP_Snap.Services;
using System.Windows;
using System.Windows.Controls;

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

        public WeatherQueryModel WeatherInformation
        {
            get
            {
                return GetValue(WeatherInformationProperty) as WeatherQueryModel;
            }
            set
            {
                SetValue(WeatherInformationProperty, value);
            }
        }
        public static readonly DependencyProperty WeatherInformationProperty = DependencyProperty.Register("WeatherInformation", typeof(WeatherQueryModel), typeof(WeatherView), new PropertyMetadata(null));

        private async void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            WeatherInformation = await WeatherInformationService.GetWeatherInfomationAsync();
        }


    }
}
