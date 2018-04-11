using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompressTools
{
    static class Program
    {
        const int progressBar = 100;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary> 
        static void Main(string[] args)
        {
            while (true)
            {
                var dir = @"E:\----------------SVN--------------------\Cocos_Test\assets\resources";
                string savepath = null;
                if (args.Length >= 1)
                {
                    dir = args[0];
                    if (args.Length >= 2)
                    {
                        savepath = args[1];
                    }
                }

                if (string.IsNullOrEmpty(savepath))
                {
                    var dirInfo = new System.IO.DirectoryInfo(dir);
                    savepath = dirInfo.FullName;
                    if (!savepath.EndsWith("\\"))
                    {
                        savepath += "\\";
                    }
                    savepath += dirInfo.Name + ".zip";



                }
                var zipCompress = new ZipCompress(dir, savepath);
                var task = zipCompress.StartCompressAsync(progressCallback: ProgressCallback);

                task.Wait();

                Console.ReadKey();

            }


        }

        private static void ProgressCallback(CompressEventArgs obj)
        {
            var p = Console.WindowWidth - 30;
            int jindu = (int)(obj.Progress * p);
            Console.WriteLine($"压缩：{obj.Msg}");
            //Console.Write($"\r压缩进度：[{"".PadLeft(jindu, '=')}{"".PadLeft(p - jindu, '_')}]({(obj.Progress * 100).ToString("0.0")}%)");


        }
    }
}
