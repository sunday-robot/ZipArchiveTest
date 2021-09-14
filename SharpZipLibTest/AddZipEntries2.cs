using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SharpZipLibTest
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
            byte[] buffer = new byte[65536];

            var baseDirectoryPathLength = Path.GetDirectoryName(pathList[0]).Length;
            if (baseDirectoryPathLength == 3)
            {
                // "c:\a\abc.oir"のような通常のパス名の場合、GetDirectoryName()の結果は、"c:\a"のように末尾に"\"は付かないが、
                // "c:\abc.oir"のようなルートフォルダ直下の場合、"c:"ではなく、"c;\"のように"\"が末尾に付くという相違があるのでこれを調整する。
                baseDirectoryPathLength = 2;
            }
            using (var zos = new ZipOutputStream(outStream))
            {
                foreach (var path in pathList)
                {
                    if (!preAction(path))
                        return;
                    var entryName = path.Substring(baseDirectoryPathLength + 1);
                    var entry = new ZipEntry(entryName);
                    entry.IsUnicodeText = true; // ファイル名、コメントをUTF8で出力させる。
                    zos.PutNextEntry(entry);
                    using (FileStream ifs = File.OpenRead(path))
                    {
                        StreamUtils.Copy(ifs, zos, buffer);
                    }

                    postAction(path);
                }
            }
        }
    }
}
