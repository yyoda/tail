using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tail
{
    public class GetOpt
    {
        private string[] Opts { get; set; }
        private string[] Args { get; set; }

        public GetOpt(string[] args = null, string[] opts = null)
        {
            Args = args ?? new string[0];
            Opts = opts ?? new string[0];
        }

        public Dictionary<string, List<string>> Parse()
        {
            if (Args.Length > 0)
                Validation();

            var result = new Dictionary<string, List<string>>();
            var currentOpt = string.Empty;
            var currentParams = new List<string>();
            foreach (var arg in Args)
            {
                if (Opts.Contains(arg))
                {
                    if (currentOpt != arg)
                    {
                        currentOpt = arg;
                        currentParams = new List<string>();
                    }
                    continue;
                }
                else
                {
                    if (currentOpt.Length > 0)
                    {
                        currentParams.Add(arg);
                        result[currentOpt] = currentParams;
                    }
                }
            }

            return result;
        }

        private void Validation()
        {
            Args.Where(x => Opts.Contains(x)).GroupBy(x => x).Select(x => new { opt = x, count = x.Count() }).Any(x =>
                {
                    if (x.count >= 2)
                        throw new ArgumentException(string.Format("{0}オプションは複数指定されています.", x.opt));
                    return true;
                });
        }
    }
}
