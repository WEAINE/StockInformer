using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace StockInformer.Fomulars
{
    class ABI : IFormular
    {
        public string Name
        {
            get
            {
                return "ABI - 绝对广量指标";
            }
        }

        public string Content
        {
            get
            {
                return "ABI:100*ABS(ADVANCE-DECLINE)/(ADVANCE+DECLINE);";
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

        public ABI(ObservableCollection<StockInfo> Info)
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

            return (100 * Math.Abs(AdvanceCount - DeclineCount) / (AdvanceCount + DeclineCount)).ToString();
        }
    }
}
