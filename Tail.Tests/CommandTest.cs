using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tail;

namespace Tail.Tests
{
    [TestClass]
    public class CommandTest
    {
        public static readonly string LogfilePath = global::Tail.Tests.test.Default.LogfilePath;

        [TestMethod]
        public void 疎通テスト()
        {
            var arg = new[]
                {
                    "-n",
                    "8",
                    "-f",
                    LogfilePath + "kvsstats.*.log",
                };
            var cmd = new Command(arg);
            cmd.DoTail(1);
        }
    }
}
