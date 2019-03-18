using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 加密大师
{

    class Program : ConsolePrinter
    {
        //string originalFullfilename;
        string targetFullfilename = "";
        string[] originalFullfilenames = null;

        string[] InputOriginalFullfilenames()
        {
            InfoPrint("拖入要操作的文件：");
            FontColor = ConsoleColor.Yellow;
            originalFullfilenames = Console.ReadLine().Split(new[] { "\""}, StringSplitOptions.RemoveEmptyEntries);
            //originalFullfilename = Console.ReadLine().Replace("\"","");
            FontColor = ConsoleColor.Red;
            return originalFullfilenames;
        }

        bool TrasferFile(string filename)
        {
            
            if (File.Exists(filename) == false)
                return false;
            byte[] binary = null;
            try { binary = File.ReadAllBytes(filename); }
            catch(OutOfMemoryException)
            {
                FileStream file = new FileStream(filename, FileMode.Open);
                List<byte> targetFileBytes = new List<byte>();
                byte[] buffer = new byte[1024];
                int offset = 0;
                while (true)
                {
                    int readNum = file.Read(buffer, offset, buffer.Length);
                    offset += readNum;
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] ^= 0xff;
                    }
                    targetFileBytes.AddRange(buffer);
                    if (readNum < buffer.Length)
                        break;
                }
                file.Close();
                binary = targetFileBytes.ToArray();
                targetFileBytes.Clear();
            }

            for (int i = 0; i < binary.Length; i++)
                binary[i] ^= 0xff;
            File.WriteAllBytes(filename, binary);
            return true;
        }

        void Run()
        {
            InputOriginalFullfilenames();
            foreach (String originalFullfilename in originalFullfilenames)
            {
                if (originalFullfilename.IndexOf(".rar") == -1) //加密
                {
                    //GenerateCompressionTargetFullFilename();
                    string newname = originalFullfilename.Remove(originalFullfilename.LastIndexOf("\\"));
                    newname += "\\" + new Random().Next(1000000000) + ".rar";
                    targetFullfilename = newname;
                    try
                    {
                        WinRARUtil.CompressFile(new string[] { originalFullfilename }, newname);
                    }
                    catch
                    {
                        ErrorPrintln("此软件需要WinRAR的支持，请您手动安装此软件");
                    }
                    DebugPrintln("压缩完成，正在加密中，请耐心等待！");
                    if (TrasferFile(newname) == false)
                        targetFullfilename = "";
                    if (targetFullfilename == "")
                    {
                        ErrorPrintln("失败，因异常行为引起中断操作！");
                    }
                    else
                    {
                        DebugPrintln("写入成功！");
                    }
                }
                else //解密
                {
                    DebugPrintln("正在解密文件，请耐心等待！");
                    TrasferFile(originalFullfilename);
                    //GenerateDecompressionTargetFullFilename();
                    try
                    {
                        WinRARUtil.DecompressFile(originalFullfilename);
                    }
                    catch (Exception e)
                    {
                        ErrorPrintln(e);
                        ErrorPrintln("此软件需要WinRAR的支持，请您手动安装此软件");
                        return;
                    }
                    DebugPrintln("解压成功！");
                }

                if (targetFullfilename != "")
                {
                    DebugPrintln("源文件：" + originalFullfilename);
                    DebugPrintln("目标文件：" + targetFullfilename);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            while(true)
                new Program().Run();
        }
    }
}
