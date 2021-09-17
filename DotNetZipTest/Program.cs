using Ionic.Zip;
using System;
using System.IO;
using System.Text;

namespace DotNetZipTest
{
    class Program
    {
#if false
        static void Main(string[] args)
        {
            using (var os = File.Create("test.zip"))
            {
                using (var zos = new ZipOutputStream(os))
                {
                    zos.EnableZip64 = Zip64Option.Always;   // ZIP64形式でないと、Hammokで日本語ファイル名のものが文字化けしてしまう。
                    zos.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zos.AlternateEncoding = Encoding.UTF8;

                    var ze = zos.PutNextEntry("test.txt");
                    zos.Write(new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' }, 0, 4);
                    ze = zos.PutNextEntry("あ.txt");
                    zos.Write(new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' }, 0, 4);
                }
            }
        }
#else
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
            using (var outputStream = File.Create(zipFilePath))
            {
                using (var zos = new ZipOutputStream(outputStream))
                {
                    zos.EnableZip64 = Zip64Option.Always;   // ZIP64形式でないと、local file header、central directory headerの、extra fieldが空になるためか、Hammokで日本語ファイル名のものが文字化けしてしまう。
                    zos.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zos.AlternateEncoding = Encoding.UTF8;

                    for (var i = 1; i < args.Length; i++)
                    {
                        var filePath = args[i];
                        var entryName = Path.GetFileName(filePath);
                        Console.WriteLine($"START : {DateTime.Now}");
                        var ze = zos.PutNextEntry(entryName);
                        using (FileStream ifs = File.OpenRead(filePath))
                        {
                            while (true)
                            {
                                var length = ifs.Read(buffer, 0, buffer.Length);
                                if (length == 0)
                                {
                                    break;
                                }
                                zos.Write(buffer, 0, length);
                            }
                        }
                        Console.WriteLine($"END   : {DateTime.Now}");
                    }
                }
            }
        }
#endif
    }
}
