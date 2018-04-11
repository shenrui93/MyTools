using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MD5ComputeTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[] md5HashResult;
        byte[] sha1HashResult;
        byte[] sha512HashResult;
        long len = 1;
        long completeLen = 0;
        string path;


        private void button1_Click(object sender, EventArgs e)
        {
            computeWorker.CancelAsync();
            if (openFile.ShowDialog() != DialogResult.OK) return;

            System.Threading.SpinWait.SpinUntil(() => !computeWorker.IsBusy);

            path = openFile.FileName;
            txt_path.Text = path;
            ProgressCall(0, "正在计算");
            txt_hash.Text = "正在计算结果，请稍后";
            computeWorker.RunWorkerAsync();
            timer1.Enabled = true;
        }
        private void computeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] buffer = new byte[1024];
            if (computeWorker.CancellationPending) return;

            var md5Compute = System.Security.Cryptography.MD5.Create();
            var sha1Compute = System.Security.Cryptography.SHA1.Create();
            var sha512Compute = System.Security.Cryptography.SHA512.Create();

            using (var stream = System.IO.File.OpenRead(path))
            {
                len = stream.Length;
                completeLen = 0;
                int r;

                while ((r = (stream.Read(buffer, 0, 512))) > 0)
                {
                    if (computeWorker.CancellationPending) break;
                    md5Compute.TransformBlock(buffer, 0, r, buffer, 0);
                    sha1Compute.TransformBlock(buffer, 0, r, buffer, 0);
                    sha512Compute.TransformBlock(buffer, 0, r, buffer, 0);
                    completeLen += r;
                }
            }

            if (computeWorker.CancellationPending) return;
            md5Compute.TransformFinalBlock(new byte[] { }, 0, 0);
            sha1Compute.TransformFinalBlock(new byte[] { }, 0, 0);
            sha512Compute.TransformFinalBlock(new byte[] { }, 0, 0);

            if (computeWorker.CancellationPending) return;
            computeWorker.ReportProgress(1, "");
            md5HashResult = md5Compute.Hash;
            sha1HashResult = sha1Compute.Hash;
            sha512HashResult = sha512Compute.Hash;

        }
        private void computeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Enabled = false;
            try
            {

                txt_hash.Text = $@"计算结果：

   MD5：{BitConverter.ToString(md5HashResult).Replace("-", "")}
  SHA1：{BitConverter.ToString(sha1HashResult).Replace("-", "")}
SHA512：{BitConverter.ToString(sha512HashResult).Replace("-", "")}
";

                ProgressCall(100, $"已完成：{ToSizeString(len)}B/{ToSizeString(len)}B-100%"); 
            }
            catch
            {
                ProgressCall(100, $"已取消");

            }
            finally
            {
                timer1.Enabled = false;
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ProgressCall((int)(completeLen * 100 / len), 
                $"已完成：{ToSizeString(completeLen)}B/{ToSizeString(len)}B-{completeLen * 100 / len}%");
        }



        private void ProgressCall(int progress,string msg)
        {
            this.progressBar1.Value = progress;
            this.lbl_jindu.Text = msg;
        }




        /// <summary>
        /// 将字节大小转换为数据可识别字符串形式
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ToSizeString(long? size)
        {
            if (size == null) return "";

            var t = (double)size.Value;

            if (t < 1024) return $"{t.ToString("0")}";
            if (t < 1024 * 1024)
            {
                t = t / 1024;
                return $"{t.ToString("0.00")}K";
            }
            if (t < 1024 * 1024 * 1024)
            {
                t = t / 1024 / 1024;
                return $"{t.ToString("0.00")}M";
            }
            t = t / 1024 / 1024 / 1024;



            return $"{t.ToString("0.00")}G";

        }

    }
}
