using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompressTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var zipCompress = new ZipCompress(@"E:\----------------SVN--------------------\Cocos_Test\assets\resources"
            , @"E:\----------------SVN--------------------\Cocos_Test\assets\resources.zip");

            zipCompress.StartCompressAsync(progressCallback:ProgressCallback);
        }

        private void ProgressCallback(CompressEventArgs obj)
        {
            this.Invoke(delegate (){
                this.progressBar1.Value = (int)(obj.Progress * 100);
                this.label1.Text = $"正在压缩：{obj.Msg}";
            });
        }

        public void Invoke(Action call)
        {
            base.Invoke(call);
        }
    }
}
