using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace StockInformer
{
    class News
    {
        public string Title { get; set; }
        public string URL { get; set; }

        public static ObservableCollection<News> Get()
        {
            string RawHTML = HttpOperator.HttpGet("http://m.hexun.com", "", "UTF-8", "utf-8");
            string NewsPattern = @"<div class='news-cont-a'>[\s\S]+?</div>";
            MatchCollection NewsMatches = Regex.Matches(RawHTML, NewsPattern);
            string MatchString = "";
            ObservableCollection<News> NewsData = new ObservableCollection<News>();

            foreach (Match NewsMatch in NewsMatches)
            {
                MatchString = NewsMatch.Groups[0].Value;

                NewsData.Add(new News { Title = new Regex(@"\s+").Replace(new Regex(@"<[\s\S]+?>").Replace(MatchString, ""), ""), URL = Regex.Match(MatchString, @"http[\S]+?html").Groups[0].Value });
            }
            
            return NewsData;
        }
    }
}
