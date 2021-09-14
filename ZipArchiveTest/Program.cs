﻿using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ZipArchiveTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePathList = ConvertToFullPath(new List<string>() { "abc.txt", "def/ghi.txt", "jkl/pqr.txt" });

            using (var fs = File.Open("test.zip", FileMode.Create, FileAccess.Write))
            {
                //AddFilesToZipArchive(fs, "testData/", new[] { "abc.txt", "def/ghi.txt", "jkl/" });
                AddZipEntries.Do(fs, filePathList, (name) => true, (_) => { });
            }
        }

        static List<string> ConvertToFullPath(List<string> pathList)
        {
            var r = new List<string>();
            foreach (var e in pathList)
            {
                r.Add(Path.GetFullPath(e));
            }
            return r;
        }
#if false
        /// <summary>
        /// ストリームにZIPエントリー群を追加する。
        /// </summary>
        /// <param name="outStream">ZIPアーカイブを出力するストリーム</param>
        /// <param name="baseDirectoryPath">ZIPエントリーとして追加するファイル群の起点のディレクトリパス</param>
        /// <param name="relativePathList">ファイル群の相対パス</param>
        static void AddFilesToZipArchive(Stream outStream, string baseDirectoryPath, string[] relativePathList)
        {
            int absoluteBaseDirectoryPathLength;
            {
                var c = baseDirectoryPath[baseDirectoryPath.Length - 1];
                if (c == '/' || c == '\\')
                    absoluteBaseDirectoryPathLength = baseDirectoryPath.Length;
                else
                    absoluteBaseDirectoryPathLength = baseDirectoryPath.Length + 1;
            }

            using (var a = new ZipArchive(outStream, ZipArchiveMode.Create))
            {
                foreach (var relativePath in relativePathList)
                {
                    var path = Path.Combine(baseDirectoryPath, relativePath);
                    if (Directory.Exists(path))
                    {
                        // ディレクトリの場合は、サブディレクトリ内のものも含めすべてのファイルを追加する。
                        foreach (var childPath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                        {
                            var entryName = childPath.Substring(absoluteBaseDirectoryPathLength);
                            ZipFileExtensions.CreateEntryFromFile(a, childPath, entryName);
                        }
                    }
                    else
                    {
                        ZipFileExtensions.CreateEntryFromFile(a, path, relativePath);
                    }
                }
            }
        }
#endif
    }
}