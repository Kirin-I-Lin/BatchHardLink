using System.Diagnostics;
using System.IO.Enumeration;

namespace BatchHardLink_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int option = 1;
            while (option != 0)
            {
                try 
                {
                    Console.WriteLine("================请输入数字以选择操作内容================");
                    Console.WriteLine("0:退出");
                    Console.WriteLine("1:创建新的批量硬链接");
                    string? input_1 = Console.ReadLine();
                    int num_1 = int.Parse(input_1);
                    switch (num_1)
                    {
                        case 0:
                            option = 0;
                            break;
                        case 1:
                            option = 1;
                            fun_create_batch_hardlink();
                            break;
                        default:
                            FormatException formatException = new FormatException("无对应选项");
                            throw formatException;
                    }
                }
                catch (FormatException fmt)
                {
                    Console.WriteLine($"FormatException:{fmt.Message}");
                }
            }
        }

        public static void fun_create_batch_hardlink()
        { 
            Console.WriteLine("请输入源文件夹地址:");
            string? sourceDir = Console.ReadLine();
            Console.WriteLine("请输入目标文件夹地址:");
            string? targetDir = Console.ReadLine();

            //获取源文件夹本级目录下的所有文件
            string[] sourceMkv = Directory.GetFiles(sourceDir,"*.mkv");
            string[] sourceMp4 = Directory.GetFiles(sourceDir, "*.mp4");
            string[] sourceAss = Directory.GetFiles(sourceDir, "*.ass");

            //获取目标文件夹本级目录下的所有文件
            string[] targetMkv = Directory.GetFiles(targetDir, "*.mkv");
            string[] targetMp4 = Directory.GetFiles(targetDir, "*.mp4");
            string[] targetAss = Directory.GetFiles(targetDir, "*.ass");


            if (sourceMkv.Length > 0)
            {
                string logMkv = fun_create_hardlink("mkv", sourceMkv, targetDir);
                Console.WriteLine(logMkv);
            }
            if (sourceMp4.Length > 0)
            {
                string logMp4 = fun_create_hardlink("mp4", sourceMp4, targetDir);
                Console.WriteLine(logMp4);
            }
            if (sourceAss.Length > 0)
            {
                string logAss = fun_create_hardlink("ass", sourceAss, targetDir);
                Console.WriteLine(logAss);
            }
        }

        public static string fun_create_hardlink(string type, string[] sourceFiles, string targetDir)
        {
            Console.WriteLine($"开始批处理{type}");
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
            Console.WriteLine("CMD已启动");

            process.StandardInput.AutoFlush = true;
            // 向cmd窗口发送输入信息
            foreach (string sourceFile in sourceFiles)
            {
                Console.WriteLine($"{sourceFile}");
                process.StandardInput.WriteLine($"mklink /h \"{targetDir}\\{Path.GetFileName(sourceFile)}\" \"{sourceFile}\"");
            }
            process.StandardInput.WriteLine("exit");


            // 获取输出信息
            string strOutput = process.StandardOutput.ReadToEnd();

            // 等待退出进程
            process.WaitForExit();
            process.Close();
            Console.WriteLine("CMD已关闭");
            return strOutput;
        }
    }
}