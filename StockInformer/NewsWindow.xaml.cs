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
using System.IO;

namespace StockInformer
{
    /// <summary>
    /// NewsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewsWindow : Window
    {
        private MainWindow MainWindowHandler;

        public NewsWindow(MainWindow Handler)
        {
            InitializeComponent();

            MainWindowHandler = Handler;
        }

        public void DisplayNews(string URL)
        {
            string RawHTML = HttpOperator.HttpGet(URL, "", "UTF-8", "utf-8");
            RawHTML = new Regex(@"<script[\s\S]*?[\s\S]+?/script>").Replace(RawHTML, "");
            RawHTML = new Regex(@"<a href=""\S+?"">|</a>").Replace(RawHTML, "");
            RawHTML = new Regex(@"src=""\S+?""").Replace(RawHTML, "");
            RawHTML = new Regex(@"original").Replace(RawHTML, "src");

            string NewsHTML = Regex.Matches(RawHTML, @"<article>[\s\S]+?</article>")[0].Groups[0].Value;

            FileOperator.WriteFile("Temporary.html", "<meta charset='UTF-8'><body style='background-color:#2D2D30;color:white;font-family:Microsoft Yahei;'>" + NewsHTML + "</body>");
            NewsBrowser.Navigate((Environment.CurrentDirectory + "/Temporary.html").Replace("%20", " "));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowHandler.NewsList.IsEnabled = false;

            if (File.Exists("Temporary.html")) File.Delete("Temporary.html");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindowHandler.NewsList.IsEnabled = true;

            if (File.Exists("Temporary.html")) File.Delete("Temporary.html");
        }
    }
}
