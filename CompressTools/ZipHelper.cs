using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace CompressTools
{
    public class ZipCompress
    {
        public static readonly Action<CompressEventArgs> defaultCompressCall = r => { };

        private string dirPath;
        private string savePath;
        private Encoding encoding;
        //private MemoryStream outStream = new MemoryStream();
        private CompressEventArgs arg = new CompressEventArgs();

        public ZipCompress(string dirpath, string savepath, Encoding encoding = null)
        {
            dirPath = Path.GetFullPath(dirpath + "/");
            savePath = Path.GetFullPath(savepath);
            this.encoding = encoding ?? Encoding.UTF8;

        }
        public Task StartCompressAsync(string password = null, Action<CompressEventArgs> progressCallback = null)
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (File.Exists(savePath))
                    {
                        File.Delete(savePath);
                        System.Threading.Thread.Sleep(1);
                        continue;
                    }
                    break;
                }
                if (progressCallback == null)
                {
                    progressCallback = defaultCompressCall;
                }
                int tota;
                int pros = 0;
                var s = ZipEnity.GetZipFileEnity(dirPath, out tota);

                using (ZipArchive zip = ZipFile.Open(savePath, ZipArchiveMode.Create, encoding))
                { 
                    AddZipEnity(s, zip, tota, ref pros, progressCallback);

                }

                arg.Progress = 1;
                arg.Msg = "压缩完成";
                progressCallback(arg);
            });
        }

        private void AddZipEnity(ZipEnity s, ZipArchive zip, int tota, ref int pros, Action<CompressEventArgs> progressCallback)
        {
            if (s.Type == EnityType.Dir)
            {
                var nodes = s.Nodes;
                if (nodes == null || nodes.Length == 0) return;
                foreach (var enity in nodes)
                {
                    AddZipEnity(enity, zip, tota, ref pros, progressCallback);
                }
                return;
            }
            if (s.Type == EnityType.File)
            {
                using (FileStream fs = new FileStream(dirPath + s.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var enity = zip.CreateEntry(s.FullPath);
                    using (var zips = enity.Open())
                    {
                        fs.CopyTo(zips);
                        zips.Flush();
                        arg.Progress = ((double)++pros) / tota;
                        arg.Msg = s.FullPath;
                        arg.Name = s.Name;
                        progressCallback(arg);
                    }
                }
                return;
            }


        }

        #region ZipEnity

        /// <summary>
        /// 
        /// </summary>
        class ZipEnity
        {
            public override string ToString()
            {
                return this.FullPath;
            }
            public static ZipEnity GetZipFileEnity(string root, out int totalfilecount, string path = "")
            {
                totalfilecount = 0;
                var fullpath = System.IO.Path.GetFullPath(root);

                return new ZipEnity()
                {
                    Name = path,
                    FullPath = path,
                    Type = EnityType.Dir,
                    Nodes = GetZipFileEnityArray(fullpath, out totalfilecount, path)
                };
            }
            public static ZipEnity[] GetZipFileEnityArray(string root, out int totalfilecount, string path = "")
            {
                totalfilecount = 0;
                var fullpath = Path.GetFullPath(root + path);
                List<ZipEnity> list = new List<ZipEnity>();



                #region 列目录

                //列目录
                var dirs = Directory.GetDirectories(fullpath);
                foreach (var s in dirs)
                {
                    var dirInfo = new DirectoryInfo(s);
                    if (!dirInfo.Exists || (dirInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        //如果文件不存在或隐藏则跳过
                        continue;
                    }
                    int to;
                    //文件存在添加文件信息
                    list.Add(new ZipEnity()
                    {
                        Name = dirInfo.Name,
                        FullPath = path + dirInfo.Name + "/",
                        Type = EnityType.Dir,
                        Nodes = GetZipFileEnityArray(root, out to, path + dirInfo.Name + "/")
                    });
                    totalfilecount += to;
                }

                #endregion

                #region 列文件

                //列文件
                var files = Directory.GetFiles(fullpath);
                foreach (var s in files)
                {
                    var fileInfo = new FileInfo(s);
                    if (!fileInfo.Exists || (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        //如果文件不存在或隐藏则跳过
                        continue;
                    }

                    //文件存在添加文件信息
                    list.Add(new ZipEnity()
                    {
                        Name = fileInfo.Name,
                        FullPath = path + fileInfo.Name,
                        Type = EnityType.File,
                    });
                    totalfilecount++;
                }

                #endregion


                return list.ToArray();
            }

            public string Name;
            public string FullPath;
            public ZipEnity[] Nodes;
            public EnityType Type;
        }


        #endregion



        enum EnityType
        {
            File,
            Dir
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class CompressEventArgs
    {
        public double Progress;
        public string Msg;
        public string Name;
    }


}
