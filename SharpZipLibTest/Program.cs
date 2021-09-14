using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace SharpZipLibTest
{
    class Program
    {
        static void usage()
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
                usage();
            }
            var zipFilePath = args[0];

            byte[] buffer = new byte[65536];
            using (var os = File.Create(zipFilePath))
            {
                using (var zos = new ZipOutputStream(os))
                {
                    zos.UseZip64 = UseZip64.On; // 常にZIP64形式のZIPファイルを作成する
                    for (var i = 1; i < args.Length; i++)
                    {
                        var filePath = args[i];
                        var entryName = Path.GetFileName(filePath);
                        Console.WriteLine($"START : {DateTime.Now}");
                        var entry = new ZipEntry(entryName);
                        entry.IsUnicodeText = true; // ファイル名、コメントをUTF8で出力させる。
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
    }
}
