using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 加密大师
{
    class ConsolePrinter
    {
        public ConsoleColor FontColor { get { return Console.ForegroundColor; } set { Console.ForegroundColor = value; } }

        public void InfoPrint(object content)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(content);
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public void InfoPrintln(object content)
        {
            InfoPrint(content + "\n");
        }

        public void DebugPrintln(object content)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(content);
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public void ErrorPrintln(object content)
        {
            FontColor = ConsoleColor.Red;
            Console.WriteLine(content);
        }
    }
}
