using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace FileHasher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() != 3)
            {
                Console.WriteLine("The correct use is FileHasher [Algorithm] [Input file path] [Output file hash].");
            }
            else
            {
                var hashName = args[0];
                var inputFilePath = args[1];
                var outputFilePath = args[2];

                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine("The input file path must be valid.");
                }
                else
                {
                    using (var inputFileStream = new FileStream(inputFilePath, FileMode.Open))
                    {
                        using (var hashAlgorithm = HashAlgorithm.Create(hashName))
                        {
                            if (hashAlgorithm == null)
                            {
                                Console.WriteLine("The Hash Algorithm must be valid (SHA1, SHA256, SHA384, SHA512, MD5).");
                            }
                            else
                            {
                                var fileHash = hashAlgorithm.ComputeHash(inputFileStream);
                                File.WriteAllText(outputFilePath, BitConverter.ToString(fileHash));
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Done.");
        }
    }
}