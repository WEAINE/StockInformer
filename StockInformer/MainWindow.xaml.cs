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
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using System.Diagnostics;


namespace StockInformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, ObservableCollection<StockInfo>> Info = new Dictionary<string, ObservableCollection<StockInfo>>();
        private int CurrentDigit = 0;

        public MainWindow()
        {
            InitializeComponent();

            RefreshProgress.Maximum = 5500;

            Thread RefreshDataThread = new Thread(new ThreadStart(InitData));
            RefreshDataThread.IsBackground = true;

            Thread UpdateUIThread = new Thread(new ThreadStart(UpdateUI));
            UpdateUIThread.IsBackground = true;

            if (File.Exists("./Data/SZ.info") && File.Exists("./Data/SH.info"))
            {
                Info = StockInfo.PackageStockInfoFromFiles();
                MainList.DataContext = Info["ALL"];
            }
            else MainList.DataContext = StockInfo.GetInitialInfo();

            RefreshDataThread.Start();
            UpdateUIThread.Start();
        }

        private void InitData()
        {
            if (!Directory.Exists("./Data")) Directory.CreateDirectory("./Data");

            SinaAPI.RefreshAll(out CurrentDigit);
        }

        private void UpdateUI()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                RefreshProgress.Visibility = Visibility.Visible;
                RefreshStocks.IsEnabled = false;
                RefreshStocks.Content = "（正在更新）";
                this.Title = "StockInformer - 正在后台更新数据";
                Chart.DataContext = SinaAPI.GetDailyChartURL("sz399001");
                NewsList.DataContext = News.Get();
            }));

            while (true)
            {
                this.Dispatcher.Invoke(new Action(() => {
                    RefreshProgress.Value = CurrentDigit;
                }));

                Thread.Sleep(100);

                if (CurrentDigit == 6000)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        RefreshProgress.Visibility = Visibility.Hidden;
                        Info = StockInfo.PackageStockInfoFromFiles();
                        MainList.DataContext = Info["ALL"];
                        RefreshStocks.IsEnabled = true;
                        RefreshStocks.Content = "更新数据";
                        this.Title = "StockInformer - 已更新最新数据";
                    }));

                    break;
                }
            }
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            MainList.DataContext = Info["ALL"];
        }

        private void SZ_Switcher_Click(object sender, RoutedEventArgs e)
        {
            MainList.DataContext = Info["SZ"];
        }

        private void SH_Switcher_Click(object sender, RoutedEventArgs e)
        {
            MainList.DataContext = Info["SH"];
        }

        private void SearchContent_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        private void SearchContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Search_Click(this, e);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string Keyword = SearchContent.Text;

            if (Keyword.Equals("") || Keyword.Equals("股票代码，如sz000001")) return;

            ObservableCollection<StockInfo> ResultInfo = new ObservableCollection<StockInfo>();

            foreach (StockInfo InfoBeingTested in Info["ALL"]) if (InfoBeingTested.Code.IndexOf(Keyword) > -1 || InfoBeingTested.Name.IndexOf(Keyword) > -1) ResultInfo.Add(InfoBeingTested);
            if (ResultInfo.Count == 0) ResultInfo.Add(new StockInfo { BidSell = "无匹配。" });

            MainList.DataContext = ResultInfo;
        }

        private void RefreshStocks_Click(object sender, RoutedEventArgs e)
        {
            CurrentDigit = 0;

            Thread RefreshDataThread = new Thread(new ThreadStart(InitData));
            RefreshDataThread.IsBackground = true;
            RefreshDataThread.Start();

            Thread UpdateUIThread = new Thread(new ThreadStart(UpdateUI));
            UpdateUIThread.IsBackground = true;           
            UpdateUIThread.Start();
        }

        private void SZImage_Click(object sender, RoutedEventArgs e)
        {
            SZImage.IsEnabled = false;
            SHImage.IsEnabled = true;

            if (DailyChart.IsChecked == true) Chart.DataContext = SinaAPI.GetDailyChartURL("sz399001");
            else Chart.DataContext = SinaAPI.GetKLineURL("sz399001");
        }

        private void SHImage_Click(object sender, RoutedEventArgs e)
        {
            SHImage.IsEnabled = false;
            SZImage.IsEnabled = true;

            if (DailyChart.IsChecked == true) Chart.DataContext = SinaAPI.GetDailyChartURL("sh000001");
            else Chart.DataContext = SinaAPI.GetKLineURL("sh000001");
        }

        private void DailyChart_Click(object sender, RoutedEventArgs e)
        {
            if (SZImage.IsEnabled == false) Chart.DataContext = SinaAPI.GetDailyChartURL("sz399001");
            else Chart.DataContext = SinaAPI.GetDailyChartURL("sh000001");
        }

        private void KLine_Click(object sender, RoutedEventArgs e)
        {
            if (SZImage.IsEnabled == false) Chart.DataContext = SinaAPI.GetKLineURL("sz399001");
            else Chart.DataContext = SinaAPI.GetKLineURL("sh000001");
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            NewsWindow DisplayNewsWindow = new NewsWindow(this);
            DisplayNewsWindow.Show();
            DisplayNewsWindow.DisplayNews(e.Uri.AbsoluteUri);
        }

        private void MainList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid HitGrid = sender as DataGrid;

            if (HitGrid.SelectedItems != null & HitGrid.SelectedItems.Count == 1)
            {
                new SingleStockDisplayWindow(HitGrid.SelectedItem as StockInfo).Show();
            }
        }

        private void Formulars_Manager_Click(object sender, RoutedEventArgs e)
        {
            new FormularsManagementWindow().Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.IsEnabled = false;
                new FormularWindow(this, Info["ALL"]).Show();
            }
        }
    }
}
