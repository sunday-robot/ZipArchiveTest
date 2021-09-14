using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ZipArchiveTest
{
    /// <summary>
    /// </summary>
    class AddZipEntries
    {
        /// <summary>
        /// ストリームにZIPエントリー群を追加する。
        /// </summary>
        /// <param name="outStream">ZIPアーカイブを出力するストリーム</param>
        /// <param name="pathList">ファイルあるいはディレクトリのパス名のリスト</param>
        /// <remarks>pathListの一つ目のファイルのディレクトリが基準となるディレクトリで、リスト内のすべてのファイル、ディレクトリは基準となるディレクトリの下にあることを前提としている</remarks>
        public static void Do(Stream outStream, List<string> pathList, Func<string, bool> preAction, Action<string> postAction)
        {
            var baseDirectoryPathLength = Path.GetDirectoryName(pathList[0]).Length;
            if (baseDirectoryPathLength == 3)
            {
                // "c:\a\abc.oir"のような通常のパス名の場合、GetDirectoryName()の結果は、"c:\a"のように末尾に"\"は付かないが、
                // "c:\abc.oir"のようなルートフォルダ直下の場合、"c:"ではなく、"c;\"のように"\"が末尾に付くという相違があるのでこれを調整する。
                baseDirectoryPathLength = 2;
            }
            using (var zipArchive = new ZipArchive(outStream, ZipArchiveMode.Create, false, Encoding.UTF8))
            {
                foreach (var path in pathList)
                {
                    if (!preAction(path))
                        return;
                    var entryName = path.Substring(baseDirectoryPathLength + 1);
                    var ze = ZipFileExtensions.CreateEntryFromFile(zipArchive, path, entryName);
                    Debug.WriteLine(ze.LastWriteTime);
                    postAction(path);
                }
            }
        }
    }
}
