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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace BoqiangH5
{
    /// <summary>
    /// AdjustSOCWnd.xaml 的交互逻辑
    /// </summary>
    public partial class AdjustSOCWnd : Window
    {
        public AdjustSOCWnd(int soc)
        {
            InitializeComponent();
            tbCurrentSOC.Text = soc.ToString();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public byte alterSOC = 0x00;
        private void btnAdjust_Clicked(object sender, RoutedEventArgs e)
        {
            string str = @"^[0-9]{1,3}$";
            if (!Regex.IsMatch(tbSOC.Text, str))    // if (string.IsNullOrEmpty(txtBoxBarcode.Text))
            {
                MessageBox.Show("请输入正确的 SOC 值！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            byte socVal = byte.Parse(tbSOC.Text);

            if (socVal < 0 || socVal > 100)
            {
                MessageBox.Show("请输入正确的 SOC 值！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            BoqiangH5.BQProtocol.BqProtocol.BqInstance.BQ_AlterSOC(socVal);
            this.Close();
        }
    }
}
