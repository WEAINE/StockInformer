using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;

namespace StockCodesGenerator
{
    class StockCodesGenerator
    {
        private static string[] SZ = new string[4] { "000", "001", "002", "300" };
        private static string[] SH = new string[2] { "600", "601" };
        private static string SinaAPI = "http://hq.sinajs.cn/list=";
        private static int SZCurrentDigit = 0;
        private static int SHCurrentDigit = 0;
        private static SZThreadCallBackDelegate SZCallBack = SZThreadCallBack;
        private static SHThreadCallBackDelegate SHCallBack = SHThreadCallBack;

        static void Main(string[] args)
        {
            ClearHistories();

            int Total = 2000;
            int CurrentDigit = 0;

            Thread SZThread = new Thread(new ThreadStart(SZCodesGenerator));
            Thread SHThread = new Thread(new ThreadStart(SHCodesGenerator));

            SZThread.Start();
            SHThread.Start();

            while (true)
            {
                Console.Clear();

                CurrentDigit = SZCurrentDigit + SHCurrentDigit;

                Console.WriteLine((CurrentDigit + 2) * 100 / Total + "%");
                Thread.Sleep(1000);

                if (CurrentDigit == 1998) break;
            }
        }

        private static string ThreeDigitConstructor(int Number)
        {
            if (Number < 0 || Number > 999) return "000";
            return Number.ToString().PadLeft(3, '0');
        }

        private static bool CodeInspector(string CodeBeingTested)
        {
            if (HttpGet(SinaAPI, CodeBeingTested).Length > 25) return true;
            return false;
        }

        private static void SZCodesGenerator()
        {
            for (int Digit = 0; Digit < 1000; Digit++)
            {
                if (CodeInspector("sz" + SZ[0] + ThreeDigitConstructor(Digit))) WriteFile("SZ_000.list", SZ[0] + ThreeDigitConstructor(Digit) + Environment.NewLine);
                if (CodeInspector("sz" + SZ[1] + ThreeDigitConstructor(Digit))) WriteFile("SZ_001.list", SZ[1] + ThreeDigitConstructor(Digit) + Environment.NewLine);
                if (CodeInspector("sz" + SZ[2] + ThreeDigitConstructor(Digit))) WriteFile("SZ_002.list", SZ[2] + ThreeDigitConstructor(Digit) + Environment.NewLine);
                if (CodeInspector("sz" + SZ[3] + ThreeDigitConstructor(Digit))) WriteFile("SZ_300.list", SZ[3] + ThreeDigitConstructor(Digit) + Environment.NewLine);

                SZCallBack(Digit);
            }
        }
        
        private static void SHCodesGenerator()
        {
            for (int Digit = 0; Digit < 1000; Digit++)
            {
                if (CodeInspector("sh" + SH[0] + ThreeDigitConstructor(Digit))) WriteFile("SH_600.list", SH[0] + ThreeDigitConstructor(Digit) + Environment.NewLine);
                if (CodeInspector("sh" + SH[1] + ThreeDigitConstructor(Digit))) WriteFile("SH_601.list", SH[1] + ThreeDigitConstructor(Digit) + Environment.NewLine);

                SHCallBack(Digit);
            }
        }

        private static string HttpGet(string URL, string Data)
        {
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(URL + Data);
                Request.Method = "GET";
                Request.ContentType = "text/html;charset=UTF-8";
                Request.Timeout = 5000;

                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                Stream ResponseStream = Response.GetResponseStream();
                StreamReader ResponseStreamReader = new StreamReader(ResponseStream);
                string ReturnString = ResponseStreamReader.ReadToEnd();

                ResponseStreamReader.Close();
                ResponseStream.Close();

                return ReturnString;
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        private static void WriteFile(string FileName, string Content)
        {
            FileStream WritingFileStream = new FileStream(FileName, FileMode.Append);
            StreamWriter FileStreamWriter = new StreamWriter(WritingFileStream);

            FileStreamWriter.Write(Content);
            FileStreamWriter.Close();
            WritingFileStream.Close();
        }

        private static void ClearHistories()
        {
            if (File.Exists("SZ_000.list")) File.Delete("SZ_000.list");
            if (File.Exists("SZ_001.list")) File.Delete("SZ_001.list");
            if (File.Exists("SZ_002.list")) File.Delete("SZ_002.list");
            if (File.Exists("SZ_300.list")) File.Delete("SZ_300.list");

            if (File.Exists("SH_600.list")) File.Delete("SH_600.list");
            if (File.Exists("SH_601.list")) File.Delete("SH_601.list");
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
