using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchHardLink_Console
{
    internal class Demo
    {
        public void fun_demo()
        {
            Console.WriteLine("cmd test");


            Process process = new Process();
            // 设置要启动的应用程序
            process.StartInfo.FileName = "cmd.exe";
            // 是否使用操作系统Shell启动
            process.StartInfo.UseShellExecute = false;
            // 重定向标准输入流以接受来自调用程序的输入信息
            process.StartInfo.RedirectStandardInput = true;
            // 重定向标准输出流
            process.StartInfo.RedirectStandardOutput = true;
            // 
            process.StartInfo.RedirectStandardError = true;
            // 是否显示程序窗口
            process.StartInfo.CreateNoWindow = false;


            process.Start();


            // 向cmd窗口发送输入信息
            process.StandardInput.WriteLine("ipconfig&exit");

            process.StandardInput.AutoFlush = true;

            //打成一行一行是可行的
            //process.StandardInput.WriteLine("ipconfig");
            //process.StandardInput.WriteLine("ipconfig");
            //process.StandardInput.WriteLine("exit");


            // 获取输出信息
            string strOutput = process.StandardOutput.ReadToEnd();

            // 等待退出进程
            process.WaitForExit();
            process.Close();


            //
            Console.WriteLine(strOutput);
            Console.ReadKey();
        }
    }
}
