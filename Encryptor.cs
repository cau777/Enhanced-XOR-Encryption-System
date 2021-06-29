using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Crypto
{
    public class Encryptor
    {
        private readonly int _bytesPerThread;
        private readonly byte[] _key;

        public Encryptor(string strKey, int bytesPerThread = 1024)
        {
            _bytesPerThread = bytesPerThread;

            byte[] keyBytes = StringToBytes(strKey);
            SHA512 sha512 = new SHA512Managed();
            _key = sha512.ComputeHash(keyBytes);
        }

        private static byte[] StringToBytes(string str)
        {
            return str.Select(o => (byte) o).ToArray();
        }

        private static string BytesToString(IEnumerable<byte> bytes)
        {
            return new string(bytes.Select(o => (char) o).ToArray());
        }

        private static T[] ResizeArray<T>(T[] original, int length)
        {
            T[] newArray = new T[length];

            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = original[i % original.Length];
            }

            return newArray;
        }

        public string EncodeString(string str)
        {
            byte[] strBytes = StringToBytes(str);
            EncodeBytes(strBytes);
            
            return BytesToString(strBytes);
        }

        public string EncodeStringParallel(string str)
        {
            byte[] strBytes = StringToBytes(str);
            EncodeBytesParallel(strBytes).Wait();
            
            return BytesToString(strBytes);
        }

        public void EncodeFile(string filepath)
        {
            byte[] fileBytes = File.ReadAllBytes(filepath);
            EncodeBytes(fileBytes);
            File.WriteAllBytes(filepath, fileBytes);
        }
        
        public void EncodeFileParallel(string filepath)
        {
            byte[] fileBytes = File.ReadAllBytes(filepath);
            EncodeBytesParallel(fileBytes).Wait();
            File.WriteAllBytes(filepath, fileBytes);
        }

        private static byte EncodeByte(byte original, byte keyDigit)
        {
            return (byte) (original ^ keyDigit);
        }
        
        private static void EncodeByteSpan(Memory<byte> memory, byte[] keyHash)
        {
            Span<byte> bytes = memory.Span;
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = EncodeByte(bytes[i], keyHash[i]);
            }
        }

        private void EncodeBytes(byte[] bytes)
        {
            byte[] sizedKey = ResizeArray(_key, bytes.Length);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = EncodeByte(bytes[i], sizedKey[i]);
            }
        }

        private async Task EncodeBytesParallel(byte[] bytes)
        {
            byte[] sizedKey = ResizeArray(_key, _bytesPerThread);
            int totalThreads = bytes.Length / _bytesPerThread;
            List<Task> tasks = new List<Task>(totalThreads);
            
            for (int i = 0; i < totalThreads; i++)
            {
                int start = _bytesPerThread * i;

                Memory<byte> memory = new Memory<byte>(bytes, start, _bytesPerThread);
                tasks.Add(Task.Run(() => EncodeByteSpan(memory, sizedKey)));
            }
            
            await Task.WhenAll(tasks);
            
            // Encode remaining bytes
            int remainingStart = _bytesPerThread * totalThreads;
            EncodeByteSpan(new Memory<byte>(bytes, remainingStart, bytes.Length - remainingStart), sizedKey);
        }
    }
}