using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StockInformer
{
    public class FileOperator
    {
        public static void WriteFile(string FileName, string Content)
        {
            FileStream WritingFileStream = new FileStream(FileName, FileMode.Append);
            StreamWriter FileStreamWriter = new StreamWriter(WritingFileStream);

            FileStreamWriter.Write(Content);
            FileStreamWriter.Close();
            WritingFileStream.Close();
        }

        public static List<string> ReadFile(string FileName)
        {
            List<string> Content = new List<string>();
            FileStream ReadingFileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            StreamReader FileStreamReader = new StreamReader(ReadingFileStream);
            string CurrentLine = FileStreamReader.ReadLine();

            while (CurrentLine != null)
            {
                Content.Add(CurrentLine);

                CurrentLine = FileStreamReader.ReadLine();
            }

            FileStreamReader.Close();
            ReadingFileStream.Close();
            
            return Content;
        }
    }
}
