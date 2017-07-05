using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInformer.Fomulars
{
    class VV : IFormular
    {
        public string Name
        {
            get
            {
                return "VV - 变异平均";
            }
        }

        public string Content
        {
            get
            {
                return "VV:=(HIGH+OPEN+LOW+CLOSE)/4;";
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

        public VV(StockInfo Info)
        {
            _StockInfo = Info;
        }

        private string Calculate()
        {
            return ((int.Parse(_StockInfo.High) + int.Parse(_StockInfo.Open) + int.Parse(_StockInfo.Low) + int.Parse(_StockInfo.Close)) / 4).ToString();
        }
    }
}
