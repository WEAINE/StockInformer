using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace StockInformer
{
    class SinaAPI
    {
        private static string[] SZ = new string[4] { "000", "001", "002", "300" };
        private static string[] SH = new string[2] { "600", "601" };
        private static string SinaDataAPI = "http://hq.sinajs.cn/list=";
        private static string SinaDailyChartAPI = "http://image.sinajs.cn/newchart/min/n/";
        private static string SinaKLineAPI = "http://image.sinajs.cn/newchart/daily/n/";
        private static int SZCurrentDigit = 0;
        private static int SHCurrentDigit = 0;
        private static SZThreadCallBackDelegate SZCallBack = SZThreadCallBack;
        private static SHThreadCallBackDelegate SHCallBack = SHThreadCallBack;

        public static void RefreshAll(out int CurrentDigit)
        {
            SZCurrentDigit = 0;
            SHCurrentDigit = 0;

            ClearHistories();

            Thread SZThread = new Thread(new ThreadStart(SZInfoGenerator));
            SZThread.IsBackground = true;
            SZThread.Start();

            Thread SHThread = new Thread(new ThreadStart(SHInfoGenerator));
            SHThread.IsBackground = true;
            SHThread.Start();
            
            while (true)
            {
                CurrentDigit = SZCurrentDigit + SHCurrentDigit;

                Thread.Sleep(100);

                if (CurrentDigit == 6000) break;
            }
        }

        public static string GetDailyChartURL(string StockCode)
        {
            return SinaDailyChartAPI + StockCode + ".gif";
        }

        public static string GetKLineURL(string StockCode)
        {
            return SinaKLineAPI + StockCode + ".gif";
        }

        private static string ThreeDigitConstructor(int Number)
        {
            if (Number < 0 || Number > 999) return "000";
            return Number.ToString().PadLeft(3, '0');
        }

        private static string CodeInspector(string CodeBeingTested)
        {
            string RetValue = HttpOperator.HttpGet(SinaDataAPI, CodeBeingTested, "GB2312", "gb2312");

            if (RetValue.Length > 25) return RetValue;
            return null;
        }

        private static void SZInfoGenerator()
        {
            string StockCode = "";
            string RetValue = null;

            for (int Index = 0; Index < SZ.Length; Index++)
            {
                for (int Digit = 0; Digit < 1000; Digit++)
                {
                    StockCode = "sz" + SZ[Index] + ThreeDigitConstructor(Digit);
                    RetValue = CodeInspector(StockCode);

                    if (RetValue != null) FileOperator.WriteFile("./Data/SZ.info", StockCode + "," + RetValue.Split(new char[1] { '"' })[1] + Environment.NewLine);

                    SZCallBack((Index + 1) * (Digit + 1));
                }
            }
        }

        private static void SHInfoGenerator()
        {
            string StockCode = "";
            string RetValue = null;

            for (int Index = 0; Index < SH.Length; Index++)
            {
                for (int Digit = 0; Digit < 1000; Digit++)
                {
                    StockCode = "sh" + SH[Index] + ThreeDigitConstructor(Digit);
                    RetValue = CodeInspector(StockCode);

                    if (RetValue != null) FileOperator.WriteFile("./Data/SH.info", StockCode + "," + RetValue.Split(new char[1] { '"' })[1] + Environment.NewLine);

                    SHCallBack((Index + 1) * (Digit + 1));
                }
            }
        }

        private static void ClearHistories()
        {
            if (File.Exists("./Data/SZ.info")) File.Delete("./Data/SZ.info");
            if (File.Exists("./Data/SH.info")) File.Delete("./Data/SH.info");
        }

        private static void SZThreadCallBack(int CurrentDigit)
        {
            SZCurrentDigit = CurrentDigit;
        }

        private delegate void SZThreadCallBackDelegate(int CurrentDigit);

        private static void SHThreadCallBack(int CurrentDigit)
        {
            SHCurrentDigit = CurrentDigit;
        }

        private delegate void SHThreadCallBackDelegate(int CurrentDigit);
    }
}
