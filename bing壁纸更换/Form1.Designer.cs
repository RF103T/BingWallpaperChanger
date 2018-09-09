namespace bing壁纸更换
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
            this.backgroundOriginChoose = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.changeTimeChoose = new System.Windows.Forms.ComboBox();
            this.changeNow = new System.Windows.Forms.Button();
            this.startWithSystem = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.state = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // backgroundOriginChoose
            // 
            this.backgroundOriginChoose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backgroundOriginChoose.FormattingEnabled = true;
            this.backgroundOriginChoose.Items.AddRange(new object[] {
            "bing每日高清壁纸（仅限一天一换）",
            "bing随机高清壁纸（https://bing.ioliu.cn/）"});
            this.backgroundOriginChoose.Location = new System.Drawing.Point(61, 17);
            this.backgroundOriginChoose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.backgroundOriginChoose.Name = "backgroundOriginChoose";
            this.backgroundOriginChoose.Size = new System.Drawing.Size(258, 25);
            this.backgroundOriginChoose.TabIndex = 0;
            this.backgroundOriginChoose.SelectedIndexChanged += new System.EventHandler(this.backgroundOriginChoose_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "壁纸源";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "更换频率";
            // 
            // changeTimeChoose
            // 
            this.changeTimeChoose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.changeTimeChoose.FormattingEnabled = true;
            this.changeTimeChoose.Items.AddRange(new object[] {
            "5分钟",
            "10分钟",
            "20分钟",
            "30分钟",
            "1小时",
            "2小时",
            "3小时"});
            this.changeTimeChoose.Location = new System.Drawing.Point(74, 59);
            this.changeTimeChoose.Name = "changeTimeChoose";
            this.changeTimeChoose.Size = new System.Drawing.Size(245, 25);
            this.changeTimeChoose.TabIndex = 3;
            this.changeTimeChoose.SelectedIndexChanged += new System.EventHandler(this.changeTimeChoose_SelectedIndexChanged);
            // 
            // changeNow
            // 
            this.changeNow.Location = new System.Drawing.Point(12, 133);
            this.changeNow.Name = "changeNow";
            this.changeNow.Size = new System.Drawing.Size(156, 23);
            this.changeNow.TabIndex = 4;
            this.changeNow.Text = "立刻更换";
            this.changeNow.UseVisualStyleBackColor = true;
            this.changeNow.Click += new System.EventHandler(this.changeNow_Click);
            // 
            // startWithSystem
            // 
            this.startWithSystem.AutoSize = true;
            this.startWithSystem.Location = new System.Drawing.Point(12, 90);
            this.startWithSystem.Name = "startWithSystem";
            this.startWithSystem.Size = new System.Drawing.Size(75, 21);
            this.startWithSystem.TabIndex = 6;
            this.startWithSystem.Text = "开机启动";
            this.startWithSystem.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 68);
            this.label3.TabIndex = 7;
            this.label3.Text = "bing壁纸更换V0.1\r\nby RF103T\r\n感谢 https://bing.ioliu.cn/\r\n提供的随机壁纸API";
            // 
            // state
            // 
            this.state.AutoSize = true;
            this.state.Location = new System.Drawing.Point(12, 113);
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(0, 17);
            this.state.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 165);
            this.Controls.Add(this.state);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.startWithSystem);
            this.Controls.Add(this.changeNow);
            this.Controls.Add(this.changeTimeChoose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.backgroundOriginChoose);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "bing壁纸更换";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox backgroundOriginChoose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox changeTimeChoose;
        private System.Windows.Forms.Button changeNow;
        private System.Windows.Forms.CheckBox startWithSystem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label state;
    }
}

