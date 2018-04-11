namespace MD5ComputeTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_path = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.computeWorker = new System.ComponentModel.BackgroundWorker();
            this.lbl_jindu = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txt_hash = new System.Windows.Forms.TextBox();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件：";
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(60, 10);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(533, 21);
            this.txt_path.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(599, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "浏览...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // computeWorker
            // 
            this.computeWorker.WorkerReportsProgress = true;
            this.computeWorker.WorkerSupportsCancellation = true;
            this.computeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.computeWorker_DoWork);
            this.computeWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.computeWorker_RunWorkerCompleted);
            // 
            // lbl_jindu
            // 
            this.lbl_jindu.AutoSize = true;
            this.lbl_jindu.Location = new System.Drawing.Point(13, 37);
            this.lbl_jindu.Name = "lbl_jindu";
            this.lbl_jindu.Size = new System.Drawing.Size(41, 12);
            this.lbl_jindu.TabIndex = 3;
            this.lbl_jindu.Text = "label2";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txt_hash
            // 
            this.txt_hash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_hash.Location = new System.Drawing.Point(15, 74);
            this.txt_hash.Multiline = true;
            this.txt_hash.Name = "txt_hash";
            this.txt_hash.ReadOnly = true;
            this.txt_hash.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_hash.Size = new System.Drawing.Size(659, 341);
            this.txt_hash.TabIndex = 4;
            this.txt_hash.WordWrap = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 53);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(659, 15);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 427);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txt_hash);
            this.Controls.Add(this.lbl_jindu);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_path);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MD5校验工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker computeWorker;
        private System.Windows.Forms.Label lbl_jindu;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txt_hash;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

