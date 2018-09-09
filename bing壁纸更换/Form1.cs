using System;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Runtime.InteropServices;

namespace bing壁纸更换
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        string bingDailyPicApi = "https://cn.bing.com/HPImageArchive.aspx?idx=0&n=1";
        string bingRandPicApi = "https://bing.ioliu.cn/v1/rand?w=1920&h=1080";
        bool startFlag = true;
        int time = 0;//设定的延迟时间

        //全局计时器
        System.Timers.Timer dailyTimer = new System.Timers.Timer(3600000);
        System.Timers.Timer randTimer = new System.Timers.Timer();

        //跨线程委托
        delegate void downloadImage();

        //加载前的准备工作
        public Form1()
        {
            if (!File.Exists("Settings.set"))
            {
                XmlDocument settings = new XmlDocument();
                XmlDeclaration declaration = settings.CreateXmlDeclaration("1.0", "UTF-8", null);
                settings.AppendChild(declaration);
                XmlElement node = settings.CreateElement("设置");
                settings.AppendChild(node);
                XmlElement s1 = settings.CreateElement("壁纸源"), s2 = settings.CreateElement("更换频率"), s3 = settings.CreateElement("每日壁纸日期");
                s1.SetAttribute("id", "0");
                s2.SetAttribute("id", "1");
                s3.SetAttribute("id", "2");
                s1.InnerText = "0";
                s2.InnerText = "3";
                s3.InnerText = "";
                node.AppendChild(s1);
                node.AppendChild(s2);
                node.AppendChild(s3);
                settings.Save("Settings.set");
            }
            InitializeComponent();
        }

        //窗体加载
        private void Form1_Load(object sender, EventArgs e)
        {
            //隐藏窗体
            WindowState = FormWindowState.Minimized;
            XmlDocument settings = new XmlDocument();
            settings.Load("Settings.set");
            XmlElement node = settings.DocumentElement;
            XmlNodeList settingsList = node.ChildNodes;
            foreach (XmlNode s in settingsList)
            {
                if (s.Attributes["id"].InnerText == "0")
                    backgroundOriginChoose.SelectedIndex = int.Parse(s.InnerText);
                if (s.Attributes["id"].InnerText == "1")
                    changeTimeChoose.SelectedIndex = int.Parse(s.InnerText);
            }
            startFlag = false;
            timers();//启动计时器
        }

        //更改壁纸源的事件
        private void backgroundOriginChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (backgroundOriginChoose.SelectedIndex == 0)
            {
                changeTimeChoose.Enabled = false;
                changeNow.Enabled = false;
                (contextMenuStrip.Items.Find("updateNow", true))[0].Enabled = false;
                if (!startFlag)
                {
                    downloadBingDaily("temp\\dailyPic.bmp");
                }
            }
            else
            {
                changeTimeChoose.Enabled = true;
                changeNow.Enabled = true;
                (contextMenuStrip.Items.Find("updateNow", true))[0].Enabled = true;
            }
            if (!startFlag)
            {
                updateSetting("0", backgroundOriginChoose.SelectedIndex.ToString());
                timers();
            }
        }

        //更改时间的事件
        private void changeTimeChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            //设定延迟时间
            switch (changeTimeChoose.SelectedIndex)
            {
                case 0:
                    {
                        time = 10 * 60 * 1000;
                        break;
                    }
                case 1:
                    {
                        time = 20 * 60 * 1000;
                        break;
                    }
                case 2:
                    {
                        time = 30 * 60 * 1000;
                        break;
                    }
                case 3:
                    {
                        time = 60 * 60 * 1000;
                        break;
                    }
                case 4:
                    {
                        time = 2 * 60 * 60 * 1000;
                        break;
                    }
                case 5:
                    {
                        time = 3 * 60 * 60 * 1000;
                        break;
                    }
                case 6:
                    {
                        time = 4 * 60 * 60 * 1000;
                        break;
                    }
            }
            if (!startFlag)
            {
                updateSetting("1", changeTimeChoose.SelectedIndex.ToString());
                timers();
            }
        }

        //“立刻更换”按钮点击事件
        private void changeNow_Click(object sender, EventArgs e)
        {
            downloadBingRand("temp\\randPic.bmp");
            randTimer.Stop();
            randTimer.Start();
        }

        //任务栏菜单退出按钮事件
        private void exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //任务栏菜单更新壁纸按钮事件
        private void updateNow_Click(object sender, EventArgs e)
        {
            downloadBingRand("temp\\randPic.bmp");
            randTimer.Stop();
            randTimer.Start();
        }

        //任务栏菜单显示按钮事件
        private void show_Click(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
            Show();
        }

        //调整窗体大小
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                Visible = false;
                notifyIcon.Visible = true;
            }
        }

        //防止多线程阻止窗体hide
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        //鼠标双击通知栏图标事件
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
            Show();
        }

        //日图检测计时器
        private void randTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            IAsyncResult asyncResult = null;
            downloadImage downloader = delegate ()
            {
                downloadBingRand("temp\\randPic.bmp");
                state.EndInvoke(asyncResult);
            };
            asyncResult = state.BeginInvoke(downloader);
        }

        //随机图片更换计时器
        private void dailyTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            IAsyncResult asyncResult = null;
            downloadImage downloader = delegate ()
            {
                downloadBingDaily("temp\\Pic.bmp");
                state.EndInvoke(asyncResult);
            };
            asyncResult = state.BeginInvoke(downloader);
        }

        //全局计时器（包括随机壁纸计时和日图检测计时）
        private void timers()
        {
            randTimer.Interval = time;
            dailyTimer.AutoReset = true;
            dailyTimer.Elapsed += new System.Timers.ElapsedEventHandler(dailyTimerEvent);
            randTimer.AutoReset = true;
            randTimer.Elapsed += new System.Timers.ElapsedEventHandler(randTimerEvent);
            if (backgroundOriginChoose.SelectedIndex == 0)
            {
                if (randTimer.Enabled == true)
                    randTimer.Stop();
                if (dailyTimer.Enabled == true)
                    dailyTimer.Stop();
                dailyTimer.Start();
            }
            else if (backgroundOriginChoose.SelectedIndex == 1)
            {
                if (dailyTimer.Enabled == true)
                    dailyTimer.Stop();
                if (randTimer.Enabled == true)
                    randTimer.Stop();
                randTimer.Start();
            }
        }

        //更新设置Xml
        private void updateSetting(string id, string value)
        {
            XmlDocument settings = new XmlDocument();
            settings.Load("Settings.set");
            XmlElement node = settings.DocumentElement;
            XmlNodeList settingsList = node.ChildNodes;
            foreach (XmlNode s in settingsList)
            {
                if (s.Attributes["id"].InnerText == id)
                    s.InnerText = value;
            }
            settings.Save("Settings.set");
        }

        //调用每日bing壁纸下载，下载前先从Xml拿数据
        private void downloadBingDaily(string path)
        {
            string imageUrl = "";
            state.Text = "正在查询更新...";
            try
            {
                //准备临时Xml文件
                Directory.CreateDirectory("temp");
                if (File.Exists(@"temp\temp.xml"))
                    File.Delete(@"temp\temp.xml");
                FileStream fileStream = new FileStream(@"temp\temp.xml", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                //获取bing的Xml文件
                HttpWebRequest getImageXml = (HttpWebRequest)WebRequest.Create(bingDailyPicApi);
                getImageXml.ServicePoint.Expect100Continue = false;
                getImageXml.Method = "GET";
                getImageXml.KeepAlive = true;
                getImageXml.ContentType = "text/xml";
                HttpWebResponse imageXml = (HttpWebResponse)getImageXml.GetResponse();
                using (Stream imageXmlStream = imageXml.GetResponseStream())
                {
                    byte[] bArr = new byte[1024];
                    int size = imageXmlStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        fileStream.Write(bArr, 0, size);
                        size = imageXmlStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    fileStream.Close();
                    imageXml.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //判断是不是最新的每日壁纸
            XmlDocument urlXml = new XmlDocument();
            XmlDocument settings = new XmlDocument();
            urlXml.Load(@"temp/temp.xml");
            settings.Load("Settings.set");
            XmlNode node = urlXml.SelectSingleNode("//image");
            XmlNode saveTime = settings.SelectSingleNode("//设置");
            if (!File.Exists(@"temp/dailyPic.bmp") || (node.SelectSingleNode("startdate")).InnerText != (saveTime.SelectSingleNode("每日壁纸日期")).InnerText)
            {
                //解析Xml获得每日壁纸地址
                node = urlXml.SelectSingleNode("//image");
                if (node != null)
                    imageUrl = "http://www.bing.com" + (node.SelectSingleNode("urlBase")).InnerText + "_1920x1080.jpg";
                //下载图片
                downloadPic(imageUrl, path);
                (saveTime.SelectSingleNode("每日壁纸日期")).InnerText = (node.SelectSingleNode("startdate")).InnerText;
            }
            else
                state.Text = "";
            urlXml.Save(@"temp/temp.xml");
            settings.Save("Settings.set");
            changeBackground(Directory.GetCurrentDirectory() + "\\" + path);
            File.Delete(@"temp/temp.xml");
        }

        //调用随机bing壁纸下载
        private void downloadBingRand(string path)
        {
            downloadPic(bingRandPicApi, path);
            changeBackground(Directory.GetCurrentDirectory() + "\\" + path);
        }

        //下载和保存壁纸
        private void downloadPic(string url, string path)
        {
            state.Text = "正在下载壁纸...";
            try
            {
                HttpWebRequest getImage = (HttpWebRequest)WebRequest.Create(url);
                getImage.ServicePoint.Expect100Continue = false;
                getImage.Method = "GET";
                getImage.KeepAlive = true;
                getImage.Credentials = CredentialCache.DefaultCredentials;
                getImage.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*";
                getImage.Headers.Add("Accept-Language", "zh-CN");
                getImage.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1)";
                getImage.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse image = (HttpWebResponse)getImage.GetResponse();
                Stream stream = null;
                stream = image.GetResponseStream();
                Image.FromStream(stream).Save(@"temp/temp.jpg");
                JPGtoBMP(@"temp/temp.jpg", path);
                stream.Dispose();
                stream.Close();
                image.Close();
                File.Delete(@"temp/temp.jpg");
                state.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //.jpg转.bmp
        private void JPGtoBMP(string jpgFile, string bmpFile)
        {
            //转换jpg到bmp
            using (Bitmap source = new Bitmap(jpgFile))
            {
                using (Bitmap bmp = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                {
                    Graphics.FromImage(bmp).DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(bmpFile, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }

        //更改壁纸
        private void changeBackground(string bmpFile)
        {
            SystemParametersInfo(20, 0, bmpFile, 0x2);
        }
    }
}
