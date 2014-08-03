using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Tail
{
    public class Command
    {
        private const int Interval = 100;
        private static readonly string[] TailOpts = new[] { "-f", "-n" };

        public string[] Args { get; private set; }
        public int LineNumberOfFirst { get; private set; }
        public bool HasFileOnlyOne { get; private set; }
        public int HasFileNum { get; private set; }

        public Command(string[] args)
        {
            Args = args;
            LineNumberOfFirst = 10;
            HasFileOnlyOne = true;
            HasFileNum = 0;
        }

        public void DoTail()
        {
            const int endless = 0;
            DoTail(endless);
        }

        public void DoTail(int durationAsMilliSecond)
        {
            if (Args == null || Args.Length == 0)
                throw new ArgumentException("引数が存在しません.");

            var filePaths = new List<string>();
            if (Args.Any(x => TailOpts.Contains(x)))
            {
                var opt = new GetOpt(Args, TailOpts).Parse();

                //-f option
                List<string> fOpt;
                if (opt.TryGetValue("-f", out fOpt))
                    filePaths = fOpt;

                //-n option
                List<string> nOpt;
                if (opt.TryGetValue("-n", out nOpt))
                {
                    int nOptValue;
                    if (Int32.TryParse(nOpt.First(), out nOptValue))
                        LineNumberOfFirst = nOptValue;
                    else
                        throw new ArgumentException("-n オプションの指定値が不正です.整数値を指定してください.");
                }
            }
            else
                filePaths = Args.ToList();

            var searchedfilePaths = SearchFileHelper<List<string>>.GetFileListByFilename(filePaths);
            HasFileNum = searchedfilePaths.Count;
            HasFileOnlyOne = (searchedfilePaths.Count == 1);
            Parallel.ForEach(searchedfilePaths, filePath =>
            {
                var seekedPosition = TailFast(filePath);
                TailFollowing(filePath, seekedPosition, durationAsMilliSecond);
            });
        }

        private long TailFast(string filePath)
        {
            return FileStreamReaderMethod(filePath, (fs, sr) =>
                {
                    var rb = new RingBuffer<string>(LineNumberOfFirst / HasFileNum);
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        rb.Add(line);

                    foreach (var s in rb)
                        Output(filePath, s);

                    return fs.Position;
                });
        }

        private void TailFollowing(string filePath, long seekPosition, int durationAsMilliSecond)
        {
            DateTime initDate, currentDate;
            initDate = currentDate = DateTime.Now;
            var duration = TimeSpan.FromMilliseconds(durationAsMilliSecond);
            while ((durationAsMilliSecond == 0) || (currentDate - initDate).TotalMilliseconds < duration.TotalMilliseconds)
            {
                currentDate = DateTime.Now; //現在時刻更新
                FileStreamReaderMethod(filePath, (fs, sr) =>
                    {
                        fs.Seek(seekPosition, SeekOrigin.Begin);

                        var line = "";
                        while ((line = sr.ReadLine()) != null)
                            Output(filePath, line);

                        seekPosition = fs.Position;
                        return true;
                    });
                
                Thread.Sleep(Interval);
            }
        }

        private static T FileStreamReaderMethod<T>(string filePath, Func<FileStream, StreamReader, T> fileStreamReaderMethod)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                return fileStreamReaderMethod(fs, sr);
            }
        }

        private void Output(string filePath, string line)
        {
            var buf = HasFileOnlyOne
                ? line
                : "[" + filePath + "] " + line;

            Console.WriteLine(buf);
        }
    }
}
