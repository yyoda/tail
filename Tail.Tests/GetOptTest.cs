using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tail.Tests
{
    [TestClass]
    public class GetOptTest
    {
        private static readonly string[] OptsPattern = new[] { "-a", "-b", "-c", "-d", "-e", "-f" };

        #region HelperMEthods
        private static Dictionary<string, List<string>> ParseMethodHelper(string[] param = null)
        {
            return (param == null) ? new GetOpt().Parse() : new GetOpt(param, OptsPattern).Parse();
        }

        private static void AssertDictionaryHelper(Dictionary<string, List<string>> expected, Dictionary<string, List<string>> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (var key in expected.Keys)
                CollectionAssert.AreEquivalent(expected[key], actual[key]);
        }
        #endregion

        [TestMethod]
        public void 引数なしで実行したら結果もゼロ()
        {
            Assert.AreEqual(0, ParseMethodHelper().Count);
        }

        [TestMethod]
        public void 値引数を１個用意したオプション引数の結果を検証()
        {
            AssertDictionaryHelper(
                expected: new Dictionary<string, List<string>> { { "-a", new List<string> { "10" } }, },
                actual: ParseMethodHelper(new[] { "-a", "10" }));
        }

        [TestMethod]
        public void 値引数２個用意したオプション引数の結果を検証()
        {
            AssertDictionaryHelper(
                expected: new Dictionary<string, List<string>> { { "-a", new List<string> { "10", "20" } }, },
                actual: ParseMethodHelper(new[] { "-a", "10", "20" }));
        }

        [TestMethod]
        public void 値引数１個用意したオプション引数を２種類用意して検証()
        {
            AssertDictionaryHelper(
                expected: new Dictionary<string, List<string>>
                {
                    { "-a", new List<string>() { "10" } },
                    { "-b", new List<string>() { "20" } }, 
                },
                actual: ParseMethodHelper(new[] { "-a", "10", "-b", "20" }));
        }

        [TestMethod]
        public void 値引数２個用意したオプション引数を２種類用意して検証()
        {
            AssertDictionaryHelper(
                expected: new Dictionary<string, List<string>>
                {
                    { "-a", new List<string>() { "10", "20" } },
                    { "-b", new List<string>() { "30", "40" } }, 
                },
                actual: ParseMethodHelper(new[] { "-a", "10", "20", "-b", "30", "40" }));
        }

        [TestMethod]
        public void オプション引数0個に対してパラメータ引数1個()
        {
            Assert.AreEqual(
                expected: 0,
                actual: ParseMethodHelper(new[] { "10" }).Count);
        }

        [TestMethod]
        public void オプション引数だけで呼び出すとエラーとなること()
        {
            try { ParseMethodHelper(new[] { "-a" }); }
            catch (Exception ex) { Assert.IsTrue(ex is ArgumentException); }

            try { ParseMethodHelper(new[] { "-a", "-b" }); }
            catch (Exception ex) { Assert.IsTrue(ex is ArgumentException); }
        }
    }
}
