using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ZipArchiveTestCore
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

            using var os = File.Create(zipFilePath);
            using var za = new ZipArchive(os, ZipArchiveMode.Create, true, Encoding.UTF8);
            for (var i = 1; i < args.Length; i++)
            {
                var path = args[i];
                var entryName = Path.GetFileName(path);
                ZipFileExtensions.CreateEntryFromFile(za, path, entryName);
            }
        }
    }
}
