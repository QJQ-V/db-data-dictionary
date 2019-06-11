using System;
using System.IO;
using System.Text;

namespace DBDataDictionary.Utils
{
    public static class HtmlUtils
    {
        public static string ReadHtml()
        {
            StringBuilder htmltext = new StringBuilder();

            string htmlPath = Path.GetFullPath("../../HtmlTemplates/DataDictionary.html");

            try
            {
                Console.WriteLine("读取文件开始");

                using (StreamReader sr = new StreamReader(htmlPath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {

                        htmltext.Append(line);
                    }
                    sr.Close();
                }

                Console.WriteLine("读取文件结束");
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取文件错误：" + ex.ToString());
            }

            return htmltext.ToString();
        }

        public static void WriteHtml(string htmlText)
        {
            try
            {
                string filePath = Path.GetFullPath("../../HtmlPath");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = filePath + "/CNABS数据字典.html";

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }

                Console.WriteLine("写入文件开始");

                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    sw.WriteLine(htmlText);
                    sw.Flush();
                    sw.Close();
                }

                Console.WriteLine("写入文件结束");
            }
            catch (Exception ex)
            {
                Console.WriteLine("写入文件错误：" + ex.ToString());
            }
        }
    }
}
