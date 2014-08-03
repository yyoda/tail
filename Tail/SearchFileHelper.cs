using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tail
{
    public static class SearchFileHelper<TList>
        where TList : IList<string>, new()
    {
        public static TList GetFileListByFilename(string filePath)
        {
            return GetFileListByFilename(new TList { filePath });
        }

        public static TList GetFileListByFilename(TList filePaths)
        {
            var result = new TList();

            //パスとファイルを分割
            var separatedfilePaths = filePaths.Select(x =>
                {
                    var bufs = x.Split('\\');

                    //末尾を除去してパスのみ抽出.パスが存在しない場合はカレントディレクトリを適用
                    var paths = new List<string>();
                    for (var i = 0; i < bufs.Length - 1; i++)
                        paths.Add(bufs[i]);
                    if (paths.Count == 0)
                        paths = new List<string> { Environment.CurrentDirectory };

                    //ファイル名を取得
                    var filename = bufs.LastOrDefault() ?? string.Empty;

                    //KeyValuePairで返却
                    return new KeyValuePair<string, string>(string.Join(@"\", paths), filename);
                });

            foreach (var separatedfilePath in separatedfilePaths)
            {
                foreach (var file in Directory.GetFiles(separatedfilePath.Key, separatedfilePath.Value))
                    result.Add(file);
            }

            return result;
        }
    }
}
