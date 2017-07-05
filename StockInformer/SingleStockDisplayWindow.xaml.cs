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
using System.Threading;
using System.IO;

namespace StockInformer
{
    /// <summary>
    /// SingleStockDisplayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SingleStockDisplayWindow : Window
    {
        private StockInfo SinaInfo;

        public SingleStockDisplayWindow(StockInfo Info)
        {
            InitializeComponent();

            SinaInfo = Info;
            this.Title = "StockInformer - " + SinaInfo.Name + "（正在获取数据）";
            StockCode.DataContext = SinaInfo.Code;
            StockName.DataContext = SinaInfo.Name;
            StockPrice.DataContext = SinaInfo.Price;
            StockIncrease.DataContext = SinaInfo.IncreaseRate + "/" + SinaInfo.Increase;
            StockOpenPrice.DataContext = SinaInfo.Open;
            StockClosePrice.DataContext = SinaInfo.Close;
            StockBuyOne.DataContext = SinaInfo.BuyOnePrice + "/" + SinaInfo.BuyOne;
            StockBuyTwo.DataContext = SinaInfo.BuyTwoPrice + "/" + SinaInfo.BuyTwo;
            StockBuyThree.DataContext = SinaInfo.BuyThreePrice + "/" + SinaInfo.BuyThree;
            StockBuyFour.DataContext = SinaInfo.BuyFourPrice + "/" + SinaInfo.BuyFour;
            StockBuyFive.DataContext = SinaInfo.BuyFivePrice + "/" + SinaInfo.BuyFive;
            StockSellOne.DataContext = SinaInfo.SellOnePrice + "/" + SinaInfo.SellOne;
            StockSellTwo.DataContext = SinaInfo.SellTwoPrice + "/" + SinaInfo.SellTwo;
            StockSellThree.DataContext = SinaInfo.SellThreePrice + "/" + SinaInfo.SellThree;
            StockSellFour.DataContext = SinaInfo.SellFourPrice + "/" + SinaInfo.SellFour;
            StockSellFive.DataContext = SinaInfo.SellFivePrice + "/" + SinaInfo.SellFive;

            Thread DownloadDataThread = new Thread(new ThreadStart(InitData));
            DownloadDataThread.IsBackground = true;

            DownloadDataThread.Start();
        }

        private void InitData()
        {
            if (!Directory.Exists("./Data/HistoricalData")) Directory.CreateDirectory("./Data/HistoricalData");
            if (File.Exists("./Data/HistoricalData/" + SinaInfo.Code + ".csv")) File.Delete("./Data/HistoricalData/" + SinaInfo.Code + ".csv");

            UpdateUI(YahooAPI.DownloadStockHistoricalData(SinaInfo.Code));
        }

        private void UpdateUI(bool Downloaded)
        {
            if (!Downloaded)
            {
                MessageBox.Show("未能更新数据，请检查网络连接并重试。");

                if (File.Exists("./Data/HistoricalData/" + SinaInfo.Code +".csv")) File.Delete("./Data/HistoricalData/" + SinaInfo.Code + ".csv");

                return;
            }

            this.Dispatcher.Invoke(new Action(() =>
            {
                StockCharts.Charts[0].Collapse();
                Loading.Visibility = Visibility.Hidden;
                this.Title = "StockInformer - " + SinaInfo.Name;
                StockDataSet.ItemsSource = HistoricalStockInfo.PackageStockInfoFromFile(SinaInfo.Code);
            }));
        }

        private void Line_Click(object sender, RoutedEventArgs e)
        {
            Line.IsEnabled = false;
            Candlestick.IsEnabled = true;
            StockChart.Title = "分时";
            StockGraph.GraphType = AmCharts.Windows.Stock.GraphType.Line;
        }

        private void Candlestick_Click(object sender, RoutedEventArgs e)
        {
            Candlestick.IsEnabled = false;
            Line.IsEnabled = true;
            StockChart.Title = "K线";
            StockGraph.GraphType = AmCharts.Windows.Stock.GraphType.Candlestick;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.IsEnabled = false;
                new FormularWindow(this, SinaInfo).Show();
            }
        }
    }
}
