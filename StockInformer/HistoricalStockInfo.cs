using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInformer
{
    class HistoricalStockInfo
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }

        public static List<HistoricalStockInfo> PackageStockInfoFromFile(string SinaCode)
        {
            List<HistoricalStockInfo> Info = new List<HistoricalStockInfo>();
            List<string> RawStrings = FileOperator.ReadFile("./Data/HistoricalData/" + SinaCode + ".csv");

            RawStrings.RemoveAt(0);

            string[] DataMetas = new string[7];

            foreach(string SingleLineData in RawStrings)
            {
                DataMetas = SingleLineData.Split(new char[1] { ',' });
                System.Globalization.DateTimeFormatInfo DateInfo = new System.Globalization.DateTimeFormatInfo();
                DateInfo.ShortDatePattern = "yyyy-MM-dd";

                Info.Add(new HistoricalStockInfo
                {
                    Date = Convert.ToDateTime(DataMetas[0], DateInfo),
                    Open = double.Parse(DataMetas[1]),
                    Close = double.Parse(DataMetas[4]),
                    High = double.Parse(DataMetas[2]),
                    Low = double.Parse(DataMetas[3]),
                    Volume = double.Parse(DataMetas[5])
                });
            }

            return Info;
        }
    }
}
