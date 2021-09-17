using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace SharpZipLibTest
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

            byte[] buffer = new byte[65536];
            Console.WriteLine($"create FileStream.");
            //using (var os = File.Create(zipFilePath))
            using (var os = UnseekableStream.Create(zipFilePath))
            {
                Console.WriteLine($"create ZipOutputStream.");
                using (var zos = new ZipOutputStream(os))
                {
                    zos.UseZip64 = UseZip64.On; // 常にZIP64形式のZIPファイルを作成する
                    for (var i = 1; i < args.Length; i++)
                    {
                        var filePath = args[i];
                        var entryName = Path.GetFileName(filePath);
                        var entry = new ZipEntry(entryName)
                        {
                            IsUnicodeText = true // ファイル名、コメントをUTF8で出力させる。
                        };
                        Console.WriteLine($"PutNextEntry({entryName})");
                        zos.PutNextEntry(entry);
                        using var ifs = File.OpenRead(filePath);
                        while (true)
                        {
                            var len = ifs.Read(buffer);
                            if (len == 0)
                            {
                                break;
                            }
                            Console.WriteLine($"Write(,, {len})");
                            zos.Write(buffer, 0, len);
                        }
                    }
                }
                Console.WriteLine($"disposed ZipOutputStream.");
            }
            Console.WriteLine($"disposed FileStream.");
            Console.ReadLine();
        }
    }
}
