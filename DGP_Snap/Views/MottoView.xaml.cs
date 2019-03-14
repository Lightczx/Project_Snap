using System;
using System.Collections.Generic;
using System.IO;
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
    /// MottoView.xaml 的交互逻辑
    /// </summary>
    public partial class MottoView : UserControl
    {
        public MottoView()
        {
            InitializeComponent();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "strings.txt");
            //((TextBlock)sender).Text = File.ReadAllText(path);
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamReader streamReader = new StreamReader(fileStream, Encoding.Default/*GetEncoding("GBK")*/, true);
                //char[] buffer = new char[streamReader.BaseStream.Length];
                //streamReader.Read(buffer, 0, (int)streamReader.BaseStream.Length);
                string content;
                string conj = string.Empty;
                while ((content = streamReader.ReadLine()) != null)
                {
                    conj += "\n       " + content.ToString();
                }

                ((TextBlock)sender).Text = conj;//streamReader.ReadLine().ToString();
                streamReader.Close();
            }
        }
    }
}
