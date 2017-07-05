using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInformer
{
    public interface IFormular
    {
        string Name
        {
            get;
        }

        string Content
        {
            get;
        }

        string Output
        {
            get;
        }
    }
}
