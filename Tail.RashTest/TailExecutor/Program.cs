using Tail;

namespace TailExecutor
{
    /// <summary>
    /// 負荷テスト
    /// </summary>
    class Program
    {
        private static void Main(string[] args)
        {
            //RushTest();
            new Command(new[] { "rash.log" }).DoTail();
        }

        #region 旧ソース
        //#region 負荷テスト
        //private const string TestLogFile = "rash.log";
        //private const int DurationMilliSecond = 10000;

        //private static void RushTest()
        //{
        //    var writedNum = 0;
        //    var ts = TimeSpan.FromMilliseconds(DurationMilliSecond);
        //    Parallel.Invoke(() => FileWriting(ts, out writedNum), () => DoTail(DurationMilliSecond));
        //    Console.WriteLine("処理終了 {0}件", writedNum);
        //    Console.ReadLine();
        //    File.Delete(TestLogFile);
        //}

        //private static void DoTail(int duration)
        //{
        //    new Command(new[] { TestLogFile }).DoTail(duration);
        //}

        //private static void FileWriting(TimeSpan duration, out int writedNum)
        //{
        //    int _writedNum = 0;
        //    DateTime initDate, currentDate;
        //    initDate = currentDate = DateTime.Now;
        //    while ((currentDate - initDate).TotalMilliseconds < duration.TotalMilliseconds)
        //    {
        //        currentDate = DateTime.Now; //現在時刻更新
        //        var sw = new StreamWriter(TestLogFile, true, Encoding.GetEncoding("shift_jis"));
        //        sw.WriteLine("データ書き込みテスト中 datetime=[{0:yyyy/MM/dd HH:mm:ss.fff}]", currentDate);
        //        sw.Close();
        //        _writedNum++;
        //    }
        //    writedNum = _writedNum;
        //}
        //#endregion
        #endregion
    }
}
