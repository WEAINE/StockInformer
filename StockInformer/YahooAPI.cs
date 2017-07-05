using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StockInformer
{
    class YahooAPI
    {
        private static string YahooDataAPI = "http://table.finance.yahoo.com/table.csv?s=";

        public static bool DownloadStockHistoricalData(string SinaCode)
        {
            ClearHistory(SinaCode);

            for (int Count = 0; Count < 10; Count++)
                if (HttpOperator.HttpDownload(YahooDataAPI + SinaCodeToYahooCode(SinaCode), "./Data/HistoricalData/" + SinaCode + ".csv")) return true;

            return false;
        }

        private static string SinaCodeToYahooCode(string SinaCode)
        {
            string Postfix = null;

            switch (SinaCode.Substring(0, 2))
            {
                case "sz":
                    Postfix = ".sz";
                    break;
                case "sh":
                    Postfix = ".ss";
                    break;
            }

            if (Postfix == null) return null;

            return SinaCode.Substring(2) + Postfix;
        }

        private static void ClearHistory(string SinaCode)
        {
            if (File.Exists("./Data/HistoricalData/" + SinaCode + ".csv")) File.Delete("./Data/HistoricalData/" + SinaCode + ".csv");
        }
    }
}
