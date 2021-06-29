using System;
using System.IO;
using System.Diagnostics;

namespace Crypto
{
    public static class DemoProgram
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify arguments");
                ShowHelpMessage();
                Environment.Exit(-1);
            }
            
            string mode = args[0].ToLower();
            if (mode == "h" || mode == "help")
            {
                ShowHelpMessage();
                Environment.Exit(0);
            }
            
            if (args.Length < 3)
            {
                Console.WriteLine("Not enough arguments");
                ShowHelpMessage();
                Environment.Exit(-1);
            }

            string data = args[1];
            string key = args[2];

            if (mode == "file")
            {
                if (!File.Exists(data))
                {
                    Console.WriteLine("The file doesn't exist");
                    Environment.Exit(-1);
                }

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Encryptor encryptor = new Encryptor(key);
                encryptor.EncodeFileParallel(data);
                // encryptor.EncodeFile(data);

                stopwatch.Stop();
                Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
            }
            else if (mode == "string")
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Encryptor encryptor = new Encryptor(key);
                string result = encryptor.EncodeStringParallel(data);
                Console.WriteLine(result);

                stopwatch.Stop();
                Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms");
            }
            else
            {
                Console.WriteLine("Invalid mode");
                ShowHelpMessage();
            }
        }

        private static void ShowHelpMessage()
        {
            Console.WriteLine("Available modes: file, string\r\n" +
                              "File mode is recommended because the console can't show some characters\r\n\r\n" +
                              "General usage:\r\n" +
                              "DemoProgram.exe [mode] [filepath/stringToEncode] [key]\r\n\r\n" +
                              "Examples:\r\n" +
                              "DemoProgram.exe file SecretFile.txt 123456\r\n" +
                              "DemoProgram.exe string \"Super Secret Text\" 123456");
        }
    }
}