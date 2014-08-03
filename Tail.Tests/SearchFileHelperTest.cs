using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tail.Tests
{
    [TestClass]
    public class SearchFileHelperTest
    {
        private static readonly string LogfilePath = test.Default.LogfilePath;

        #region HelperMethods
        private static void AssertCollectionHelper(ICollection expected, ICollection actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            CollectionAssert.AreEquivalent(expected, actual);
        }
        #endregion

        [TestMethod]
        public void フルパス１つによる検索で１件取得できたら成功()
        {
            var expected = new List<string> { LogfilePath + "kvsstats.2013-08-20.log" };
            var actual = SearchFileHelper<List<string>>.GetFileListByFilename(expected[0]);
            AssertCollectionHelper(expected, actual);
        }

        [TestMethod]
        public void フルパス２つによる検索で２件取得できたら成功()
        {
            var expected = new List<string>
                {
                    LogfilePath + "kvsstats.2013-08-19.log",
                    LogfilePath + "kvsstats.2013-08-20.log",
                };
            AssertCollectionHelper(
                expected: expected,
                actual: SearchFileHelper<List<string>>.GetFileListByFilename(expected)
                );
        }

        [TestMethod]
        public void ファイル名のあいまい検索で２件取得できたら成功()
        {
            AssertCollectionHelper(
                expected: new List<string>
                {
                    LogfilePath + "kvsstats.2013-08-19.log",
                    LogfilePath + "kvsstats.2013-08-20.log",
                },
                actual: SearchFileHelper<List<string>>.GetFileListByFilename(
                    LogfilePath + "kvsstats.*.log")
                );
        }
    }
}
