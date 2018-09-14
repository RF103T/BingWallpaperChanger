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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.backgroundOriginChoose = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.changeTimeChoose = new System.Windows.Forms.ComboBox();
            this.changeNow = new System.Windows.Forms.Button();
            this.startWithSystem = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.state = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateNow = new System.Windows.Forms.ToolStripMenuItem();
            this.savePicItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.show = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.savePicButton = new System.Windows.Forms.Button();
            this.debugMode = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundOriginChoose
            // 
            this.backgroundOriginChoose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backgroundOriginChoose.FormattingEnabled = true;
            this.backgroundOriginChoose.Items.AddRange(new object[] {
            "bing每日高清壁纸（仅限一天一换）",
            "bing随机高清壁纸（https://bing.ioliu.cn/）"});
            this.backgroundOriginChoose.Location = new System.Drawing.Point(73, 6);
            this.backgroundOriginChoose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.backgroundOriginChoose.Name = "backgroundOriginChoose";
            this.backgroundOriginChoose.Size = new System.Drawing.Size(247, 25);
            this.backgroundOriginChoose.TabIndex = 0;
            this.backgroundOriginChoose.SelectedIndexChanged += new System.EventHandler(this.backgroundOriginChoose_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "壁纸源";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
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
            "10分钟",
            "20分钟",
            "30分钟",
            "1小时",
            "2小时",
            "3小时",
            "4小时"});
            this.changeTimeChoose.Location = new System.Drawing.Point(84, 40);
            this.changeTimeChoose.Name = "changeTimeChoose";
            this.changeTimeChoose.Size = new System.Drawing.Size(236, 25);
            this.changeTimeChoose.TabIndex = 3;
            this.changeTimeChoose.SelectedIndexChanged += new System.EventHandler(this.changeTimeChoose_SelectedIndexChanged);
            // 
            // changeNow
            // 
            this.changeNow.Location = new System.Drawing.Point(12, 158);
            this.changeNow.Name = "changeNow";
            this.changeNow.Size = new System.Drawing.Size(156, 31);
            this.changeNow.TabIndex = 4;
            this.changeNow.Text = "立刻更换";
            this.changeNow.UseVisualStyleBackColor = true;
            this.changeNow.Click += new System.EventHandler(this.changeNow_Click);
            // 
            // startWithSystem
            // 
            this.startWithSystem.AutoSize = true;
            this.startWithSystem.Location = new System.Drawing.Point(12, 97);
            this.startWithSystem.Name = "startWithSystem";
            this.startWithSystem.Size = new System.Drawing.Size(75, 21);
            this.startWithSystem.TabIndex = 6;
            this.startWithSystem.Text = "开机启动";
            this.startWithSystem.UseVisualStyleBackColor = true;
            this.startWithSystem.CheckedChanged += new System.EventHandler(this.startWithSystem_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 68);
            this.label3.TabIndex = 7;
            this.label3.Text = "bing壁纸更换V0.4\r\nby RF103T\r\n随机壁纸接口：\r\nhttps://bing.ioliu.cn/";
            // 
            // state
            // 
            this.state.AutoSize = true;
            this.state.Location = new System.Drawing.Point(147, 83);
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(0, 17);
            this.state.TabIndex = 8;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "bing壁纸更换";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateNow,
            this.savePicItem,
            this.toolStripSeparator1,
            this.show,
            this.exit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip.Size = new System.Drawing.Size(149, 98);
            // 
            // updateNow
            // 
            this.updateNow.Name = "updateNow";
            this.updateNow.Size = new System.Drawing.Size(148, 22);
            this.updateNow.Text = "立刻更新壁纸";
            this.updateNow.Click += new System.EventHandler(this.updateNow_Click);
            // 
            // savePicItem
            // 
            this.savePicItem.Name = "savePicItem";
            this.savePicItem.Size = new System.Drawing.Size(148, 22);
            this.savePicItem.Text = "保存这张图片";
            this.savePicItem.Click += new System.EventHandler(this.savePicItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // show
            // 
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(148, 22);
            this.show.Text = "显示窗口";
            this.show.Click += new System.EventHandler(this.show_Click);
            // 
            // exit
            // 
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(148, 22);
            this.exit.Text = "退出";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // savePicButton
            // 
            this.savePicButton.Location = new System.Drawing.Point(12, 121);
            this.savePicButton.Name = "savePicButton";
            this.savePicButton.Size = new System.Drawing.Size(156, 31);
            this.savePicButton.TabIndex = 9;
            this.savePicButton.Text = "保存这张壁纸";
            this.savePicButton.UseVisualStyleBackColor = true;
            this.savePicButton.Click += new System.EventHandler(this.savePicButton_Click);
            // 
            // debugMode
            // 
            this.debugMode.AutoSize = true;
            this.debugMode.Location = new System.Drawing.Point(12, 70);
            this.debugMode.Name = "debugMode";
            this.debugMode.Size = new System.Drawing.Size(75, 21);
            this.debugMode.TabIndex = 10;
            this.debugMode.Text = "调试模式";
            this.debugMode.UseVisualStyleBackColor = true;
            this.debugMode.CheckedChanged += new System.EventHandler(this.debugMode_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 201);
            this.Controls.Add(this.savePicButton);
            this.Controls.Add(this.state);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.changeNow);
            this.Controls.Add(this.changeTimeChoose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.backgroundOriginChoose);
            this.Controls.Add(this.startWithSystem);
            this.Controls.Add(this.debugMode);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "bing壁纸更换";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exit;
        private System.Windows.Forms.ToolStripMenuItem updateNow;
        private System.Windows.Forms.ToolStripMenuItem show;
        private System.Windows.Forms.Button savePicButton;
        private System.Windows.Forms.ToolStripMenuItem savePicItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.CheckBox debugMode;
    }
}

