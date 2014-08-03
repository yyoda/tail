using System;
using System.IO;
using System.Text;

namespace FileWritingWorker
{
    /// <summary>
    /// 負荷テスト用にファイルへの書き込みを一分間行い続ける為のツール
    /// </summary>
    class Program
    {
        private const string TestLogFile = "rash.log";
        private const int Duration = 60000;

        private static void Main(string[] args)
        {
            int writedNum = 0;
            DateTime initDate, currentDate;
            initDate = currentDate = DateTime.Now;
            var duration = TimeSpan.FromMilliseconds(Duration);
            while ((currentDate - initDate).TotalMilliseconds < duration.TotalMilliseconds)
            {
                currentDate = DateTime.Now; //現在時刻更新
                var sw = new StreamWriter(TestLogFile, true, Encoding.GetEncoding("shift_jis"));
                sw.WriteLine("データ書き込みテスト中 datetime=[{0:yyyy/MM/dd HH:mm:ss.fff}]", currentDate);
                sw.Close();
                writedNum++;
            }

            Console.WriteLine("処理終了 {0}件", writedNum);
            Console.ReadLine();
            File.Delete(TestLogFile);
        }
    }
}
