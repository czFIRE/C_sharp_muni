using HW02.Export;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HW02.Modelling
{
    internal class ModelGenerator
    {
        public static void RunProcessMining(string fileName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                RunProcessMiningIOS(fileName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RunProcessMiningWindows(fileName);
            }
            else
            {
                RunProcessMiningLinux(fileName);
            }
        }

        private static void RunProcessMiningLinux(string fileName)
        {
            string myScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Modelling", "process_miner.py"); //update the path if necessary
            string myLogPath = Path.Combine(Directory.GetCurrentDirectory(), fileName); //update the path if necessary
            string pathToPMResult = myLogPath.Replace(".csv", "");

            string strCmdText = $"{myScriptPath} {myLogPath} {pathToPMResult} id activity";

            Process.Start("/bin/python", strCmdText).WaitForExit();
        }

        private static void RunProcessMiningIOS(string fileName)
        {
            string myScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Modelling", "process_miner.py"); //update the path if necessary
            string myLogPath = Path.Combine(Directory.GetCurrentDirectory(), fileName); //update the path if necessary
            string pathToPMResult = myLogPath.Replace(".csv", "");

            string strCmdText = $"{myScriptPath} {myLogPath} {pathToPMResult} id activity";

            Process.Start("/usr/bin/python3", strCmdText).WaitForExit();
        }

        private static void RunProcessMiningWindows(string fileName)
        {
            string myScriptPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Modelling", "process_miner.py"); //update the path if necessary
            string myLogPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, fileName); //update the path if necessary
            string pathToPMResult = myLogPath.Replace(".csv", "");

            string strCmdText = $"/C python {myScriptPath} {myLogPath} {pathToPMResult} id activity";

            Process.Start("CMD.exe", strCmdText).WaitForExit();
        }
    }
}
