using DGP_Snap.Models;
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
    /// ForecastControl.xaml 的交互逻辑
    /// </summary>
    public partial class ForecastControl : UserControl
    {
        public ForecastControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty ForecastInformationProperty = DependencyProperty.Register("ForecastInformation", typeof(Weather), typeof(ForecastControl), new PropertyMetadata(null));

        public WeatherQueryModel ForecastInformation
        {
            get
            {
                return GetValue(ForecastInformationProperty) as WeatherQueryModel;
            }
            set
            {
                SetValue(ForecastInformationProperty, value);
            }
        }
    }
}
