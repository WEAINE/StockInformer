using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInformer.Fomulars
{
    class AMOV : IFormular
    {
        public string Name
        {
            get
            {
                return "AMOV - 成本价均";
            }
        }

        public string Content
        {
            get
            {
                return "AMOV:=VOL*(OPEN+CLOSE)/2;";
            }
        }

        public string Output
        {
            get
            {
                return Calculate();
            }
        }

        private StockInfo _StockInfo;

        public AMOV(StockInfo Info)
        {
            _StockInfo = Info;
        }

        private string Calculate()
        {
            return (int.Parse(_StockInfo.Volume) * (int.Parse(_StockInfo.Open) + int.Parse(_StockInfo.Close)) / 2).ToString();
        }
    }
}
