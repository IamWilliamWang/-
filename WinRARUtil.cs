using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 加密大师
{
    class WinRARUtil
    {
        private static FileInfo getWinRarExe()
        {
            string winrarExeFullfilename = string.Empty;

            string key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
            if (registryKey != null)
            {
                winrarExeFullfilename = registryKey.GetValue("").ToString();
            }
            registryKey.Close();

            return new FileInfo(winrarExeFullfilename);
        }

        public static bool CompressFile(string[] sourceFullFilenames, string targetFullFilename = null)
        {
            try
            {
                string sourceFullFilename = "";
                foreach(string fileItem in sourceFullFilenames)
                {
                    sourceFullFilename += "\"" + fileItem + "\" ";
                }
                targetFullFilename = "\"" + targetFullFilename + "\"";
                ProcessStartInfo startInfo = new ProcessStartInfo();
                FileInfo winrarExe = getWinRarExe();
                startInfo.FileName = winrarExe.Name;
                startInfo.WorkingDirectory = winrarExe.DirectoryName;
                startInfo.Arguments = "a -ep -m5 ";
                if (targetFullFilename != null)
                    startInfo.Arguments += targetFullFilename + " " + sourceFullFilename;
                else if (targetFullFilename == null)
                {
                    if (sourceFullFilename.LastIndexOf(".") != -1)
                        startInfo.Arguments += sourceFullFilename.Remove(sourceFullFilename.LastIndexOf(".")) + ".rar " + sourceFullFilename;
                    else
                        startInfo.Arguments += sourceFullFilename + ".rar " + sourceFullFilename;
                }

                Process compressProcess = new Process();
                compressProcess.StartInfo = startInfo;
                compressProcess.Start();
                return compressProcess.WaitForExit(int.MaxValue);
            }
            catch { throw; }
        }

        public static bool DecompressFile(string sourceFullFilename)
        {
            sourceFullFilename = "\"" + sourceFullFilename + "\"";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            FileInfo winrarExe = getWinRarExe();
            startInfo.FileName = winrarExe.Name;
            startInfo.WorkingDirectory = winrarExe.DirectoryName;
            
            startInfo.Arguments = "e " + sourceFullFilename + " " + sourceFullFilename.Remove(sourceFullFilename.LastIndexOf('\\'));
            Process decompressProcess = new Process();
            decompressProcess.StartInfo = startInfo;
            decompressProcess.Start();
            return decompressProcess.WaitForExit(int.MaxValue);
        }
    }
}
