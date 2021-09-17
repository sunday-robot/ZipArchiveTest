using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ZipArchiveTest
{
    class Program
    {
        static void Usage()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var commandName = Path.GetFileNameWithoutExtension(location);
            Console.WriteLine("Usage:\n");
            Console.WriteLine($"{commandName} <ZIP file path> <file path>...\n");
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Usage();
            }
            var zipFilePath = args[0];

            using (var os = File.Create(zipFilePath))
            {
                using (var za = new ZipArchive(os, ZipArchiveMode.Create, true, Encoding.UTF8))
                {
                    for (var i = 1; i < args.Length; i++)
                    {
                        var path = args[i];
                        var entryName = Path.GetFileName(path);
                        ZipFileExtensions.CreateEntryFromFile(za, path, entryName);
                    }
                }
            }
        }
#if false
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
#endif
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
