using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogDelete
{
    class Program
    {
       //static System.IO.Stream fs;

        static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            string command;
            string arg;
            Queue<string> arg_queue = new Queue<string>(args);


            DateTime dt = DateTime.Now.Date.AddDays(-5);
            string path = Environment.CurrentDirectory;

            try
            { 
                #region 参数解析

                while (true)
                {
                    if (arg_queue.Count == 0)
                        break;
                    command = arg_queue.Dequeue();
                    switch (command)
                    {
                        case "-d":
                        case "--day":
                            {
                                arg = arg_queue.Dequeue();
                                int t;
                                int.TryParse(arg, out t);
                                dt = DateTime.Now.Date.AddDays(t);
                                break;
                            }
                        case "-p":
                        case "--path":
                            {
                                path = arg_queue.Dequeue();
                                break;
                            }



                        default:
                            {
                                process_help();
                                return;
                            }


                    }
                }

                #endregion 
            }
            catch (Exception)
            {
                throw;
            }


            process(dt, path);

            Log("操作完成"); 

        }

        /// <summary>
        /// 删除指定日期的日志
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        private static void process(DateTime dt, string path)
        {
            deleteFiles(dt, path);

        }
        private static void deleteFiles(DateTime dt, string path)
        {
            var fullPath = System.IO.Path.GetFullPath(path);
            if (!System.IO.Directory.Exists(fullPath))
                return;
            //列目录
            var dirs = System.IO.Directory.GetDirectories(fullPath, "*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var dirFullPath in dirs)
            {
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(dirFullPath);
                if (!dirInfo.Exists) continue;
                deleteFiles(dt, dirFullPath);
                if (dirInfo.LastWriteTime > dt) continue;
                DeleteDir(dirFullPath);
            }

            //列文件
            var files = System.IO.Directory.GetFiles(fullPath, "*.log", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var fileFullPath in files)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(fileFullPath);
                if (!fi.Exists) continue;
                if (fi.LastWriteTime > dt) continue;

                DeleteFile(fileFullPath);
            }

        }
        private static void DeleteDir(string path)
        {
            try
            {
                System.IO.Directory.Delete(path);
                Log($@"删除目录：{path}");
            }
            catch (Exception)
            {
                return;
            }
        }
        private static void DeleteFile(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                Log($@"删除文件：{path}");
            }
            catch //(Exception)
            {
                return;
            }
        }
        private static void process_help()
        {



        }
        private static void Log(string msg)
        {
            Console.WriteLine(msg);
            //byte[] data = Encoding.UTF8.GetBytes(msg+"\r\n");

            //fs.Write(data, 0, data.Length);
            //fs.Flush();
        }


    }
}
