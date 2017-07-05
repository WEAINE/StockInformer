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
using System.Collections.ObjectModel;
using StockInformer.Fomulars;

namespace StockInformer
{
    /// <summary>
    /// FormularWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FormularWindow : Window
    {
        private MainWindow MainWindowHandler;
        private SingleStockDisplayWindow SingleStockDisplayWindowHandler;
        private ObservableCollection<StockInfo> _StockInfoCollection;
        private StockInfo _StockInfo;
        private bool FromMainWindow = true;
        private string Input = "";

        public FormularWindow(MainWindow Handler, ObservableCollection<StockInfo> Info)
        {
            InitializeComponent();

            MainWindowHandler = Handler;
            _StockInfoCollection = Info;
            FromMainWindow = true;
        }

        public FormularWindow(SingleStockDisplayWindow Handler, StockInfo Info)
        {
            InitializeComponent();

            SingleStockDisplayWindowHandler = Handler;
            _StockInfo = Info;
            FromMainWindow = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if (FromMainWindow)
                {
                    if (Input.Equals("ABI")) Text.Text = "ABI: " + new ABI(_StockInfoCollection).Output;
                    else if (Input.Equals("TBR")) Text.Text = "TBR: " + new TBR(_StockInfoCollection).Output;
                    else MessageBox.Show("公式未定义。");
                }
                else
                {
                    if (Input.Equals("AMOV")) Text.Text = "AMOV: " + new AMOV(_StockInfo).Output;
                    else if (Input.Equals("VV")) Text.Text = "VV: " + new VV(_StockInfo).Output;
                    else MessageBox.Show("公式未定义。");
                }

                Input = "";
                Text.Text = Input;

                return;
            }

            Input += e.Key.ToString();
            Text.Text = Input;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (FromMainWindow) MainWindowHandler.IsEnabled = true;
            else SingleStockDisplayWindowHandler.IsEnabled = true;
        }
    }
}
