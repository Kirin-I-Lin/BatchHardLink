using System.Diagnostics;

namespace HardLink_Trigger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 额外手动维护的txt文件，记录rss订阅下载的源目标地址和媒体库使用的目标地址
            string triggerPath = "C:\\Users\\Administrator\\Desktop\\Trigger.txt";
            StreamReader triggerRead = File.OpenText(triggerPath);
            // 存储从文件中获取到的地址对
            List<(string, string)> sourceAndTargetDirList = new List<(string, string)>();
            int line = 1;
            (string, string) sourceAndTargetDir = ("", "");

            while (!triggerRead.EndOfStream)
            {
                switch (line % 3)
                {
                    case 1:
                        sourceAndTargetDir.Item1 = triggerRead.ReadLine();
                        line++;
                        break;
                    case 2:
                        sourceAndTargetDir.Item2 = triggerRead.ReadLine();
                        sourceAndTargetDirList.Add(sourceAndTargetDir);
                        line++;
                        break;
                    default:
                        triggerRead.ReadLine();
                        sourceAndTargetDir = ("", "");
                        line++;
                        break;
                }
            }
            foreach ((string, string) item in sourceAndTargetDirList)
            {
                string? sourceDir = item.Item1;
                string? targetDir = item.Item2;

                //获取源文件夹本级目录下的所有文件
                string[] sourceMkv = Directory.GetFiles(sourceDir, "*.mkv");
                string[] sourceMp4 = Directory.GetFiles(sourceDir, "*.mp4");
                string[] sourceAss = Directory.GetFiles(sourceDir, "*.ass");

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
        }

        // 再创建同名硬链接好像不会进行后缀+数字的情况，直接偷懒一把梭
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