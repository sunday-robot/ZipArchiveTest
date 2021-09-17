using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace SharpZipLibTest2
{
    class Program
    {
        static void Usage()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var commandName = Path.GetFileNameWithoutExtension(location);
            Console.WriteLine("Usage:\n");
            Console.WriteLine($"{commandName} <zip file path> <directory path>\n");
            Environment.Exit(1);
        }

        static void CreateDirectoryZipFile(string zipFilePath, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.Error.WriteLine($"Error : {directoryPath} is not directory.");
                return;
            }
            directoryPath = Path.GetFullPath(directoryPath);

            byte[] buffer = new byte[65536];
            using (var os = File.Create(zipFilePath))
            {
                using (var zos = new ZipOutputStream(os))
                {
                    zos.UseZip64 = UseZip64.On; // 常にZIP64形式のZIPファイルを作成する
                    foreach (var filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
                    {
                        //Console.WriteLine($"{filePath}");
                        var entryName = filePath.Substring(directoryPath.Length + 1);
                        Console.WriteLine($"{entryName}");
                        Console.WriteLine($"START : {DateTime.Now}");
                        var entry = new ZipEntry(entryName)
                        {
                            IsUnicodeText = true // ファイル名、コメントをUTF8で出力させる。
                        };
                        zos.PutNextEntry(entry);
                        using (FileStream ifs = File.OpenRead(filePath))
                        {
                            StreamUtils.Copy(ifs, zos, buffer);
                        }
                        Console.WriteLine($"END   : {DateTime.Now}");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Usage();
            }

            CreateDirectoryZipFile(args[0], args[1]);
            Console.ReadLine();
        }
    }
}
