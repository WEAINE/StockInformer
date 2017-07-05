using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace StockInformer
{
    public class StockInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Open { get; set; }
        public string Close { get; set; }
        public string Price { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string BidBuy { get; set; }
        public string BidSell { get; set; }
        public string Volume { get; set; }
        public string Turnover { get; set; }
        public string BuyOne { get; set; }
        public string BuyOnePrice { get; set; }
        public string BuyTwo { get; set; }
        public string BuyTwoPrice { get; set; }
        public string BuyThree { get; set; }
        public string BuyThreePrice { get; set; }
        public string BuyFour { get; set; }
        public string BuyFourPrice { get; set; }
        public string BuyFive { get; set; }
        public string BuyFivePrice { get; set; }
        public string SellOne { get; set; }
        public string SellOnePrice { get; set; }
        public string SellTwo { get; set; }
        public string SellTwoPrice { get; set; }
        public string SellThree { get; set; }
        public string SellThreePrice { get; set; }
        public string SellFour { get; set; }
        public string SellFourPrice { get; set; }
        public string SellFive { get; set; }
        public string SellFivePrice { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Increase { get; set; }
        public string IncreaseRate { get; set; }
        public string Color { get; set; }

        public static Dictionary<string, ObservableCollection<StockInfo>> PackageStockInfoFromFiles()
        {
            ObservableCollection<StockInfo> SZInfo = new ObservableCollection<StockInfo>();
            ObservableCollection<StockInfo> SHInfo = new ObservableCollection<StockInfo>();
            ObservableCollection<StockInfo> ALLInfo = new ObservableCollection<StockInfo>();
            Dictionary<string, ObservableCollection<StockInfo>> Info = new Dictionary<string, ObservableCollection<StockInfo>>();

            FillStockInfoCollections("./Data/SZ.info", SZInfo, ALLInfo);
            FillStockInfoCollections("./Data/SH.info", SHInfo, ALLInfo);

            Info.Add("SZ", SZInfo);
            Info.Add("SH", SHInfo);
            Info.Add("ALL", ALLInfo);

            return Info;
        }

        public static ObservableCollection<StockInfo> GetInitialInfo()
        {
            ObservableCollection<StockInfo> InitialInfo = new ObservableCollection<StockInfo>();

            InitialInfo.Add(new StockInfo { BidSell = "更新中…" });

            return InitialInfo;
        }

        private static void FillStockInfoCollections(string SourceFileName, ObservableCollection<StockInfo> TargetCollection, ObservableCollection<StockInfo> FullCollection)
        {
            StockInfo TempInfo;

            foreach (string SingleLine in FileOperator.ReadFile(SourceFileName))
            {
                string[] Metas = SingleLine.Split(new char[1] { ',' });

                if (Metas.Length == 34)
                {
                    TempInfo = InitSingleStockInfo(Metas);

                    TargetCollection.Add(TempInfo);
                    FullCollection.Add(TempInfo);
                }
            }
        }

        private static StockInfo InitSingleStockInfo(string[] Metas)
        {
            float IncreaseNum = float.Parse(Metas[4]) - float.Parse(Metas[3]);

            return new StockInfo
            {
                Code = Metas[0],
                Name = Metas[1],
                Open = Metas[2],
                Close = Metas[3],
                Price = Metas[4],
                High = Metas[5],
                Low = Metas[6],
                BidBuy = Metas[7],
                BidSell = Metas[8],
                Volume = Metas[9],
                Turnover = Metas[10],
                BuyOne = Metas[11],
                BuyOnePrice = Metas[12],
                BuyTwo = Metas[13],
                BuyTwoPrice = Metas[14],
                BuyThree = Metas[15],
                BuyThreePrice = Metas[16],
                BuyFour = Metas[17],
                BuyFourPrice = Metas[18],
                BuyFive = Metas[19],
                BuyFivePrice = Metas[20],
                SellOne = Metas[21],
                SellOnePrice = Metas[22],
                SellTwo = Metas[23],
                SellTwoPrice = Metas[24],
                SellThree = Metas[25],
                SellThreePrice = Metas[26],
                SellFour = Metas[27],
                SellFourPrice = Metas[28],
                SellFive = Metas[29],
                SellFivePrice = Metas[30],
                Date = Metas[31],
                Time = Metas[32],
                Increase = IncreaseNum.ToString("F2"),
                IncreaseRate = (IncreaseNum / float.Parse(Metas[3]) * 100).ToString("F2") + "%",
                Color = IncreaseNum < 0 ? "Green" : (IncreaseNum == 0 ? "WhiteSmoke" : "Red")
            };
        }
    }
}
