using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace StockInformer.Fomulars
{
    class TBR : IFormular
    {
        public string Name
        {
            get
            {
                return "TBR - 指数平滑广量";
            }
        }

        public string Content
        {
            get
            {
                return "TBR:100*ADVANCE/(ADVANCE+DECLINE);";
            }
        }

        public string Output
        {
            get
            {
                return Calculate();
            }
        }

        private ObservableCollection<StockInfo> _StockInfo;

        public TBR(ObservableCollection<StockInfo> Info)
        {
            _StockInfo = Info;
        }

        private string Calculate()
        {
            int AdvanceCount = 0;
            int DeclineCount = 0;

            foreach(StockInfo SingleInfo in _StockInfo)
            {
                if (SingleInfo.Color.Equals("Red")) AdvanceCount++;
                if (SingleInfo.Color.Equals("Green")) DeclineCount++;
            }

            return (100 * AdvanceCount / (AdvanceCount + DeclineCount)).ToString();
        }
    }
}
