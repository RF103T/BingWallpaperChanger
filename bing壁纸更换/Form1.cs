using System;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using IWshRuntimeLibrary;

namespace bing壁纸更换
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        string bingDailyPicApi = "https://cn.bing.com/HPImageArchive.aspx?idx=0&n=1";
        string bingRandPicApi;
        bool startFlag = true;
        int time = 0;//设定的延迟时间
        int screenHeight, screenWidth, screenNum;//屏幕的参数

        //全局计时器
        System.Timers.Timer dailyTimer = new System.Timers.Timer(3600000);
        System.Timers.Timer randTimer = new System.Timers.Timer();

        //跨线程委托
        delegate void setText();
        delegate void setEnabled();

        //主线程ID
        static int mainThreadID;

        //加载前的准备工作
        public Form1()
        {
            //检测屏幕个数和屏幕分辨率
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenNum = Screen.AllScreens.Length;
            //暂不支持多屏幕
            //屏幕分辨率只有当壁纸源提供更高分辨率的壁纸时才会往上支持
            if (screenNum > 1 || (screenHeight > 1080 && screenWidth > 1920))
            {
                MessageBox.Show("此软件暂不支持多屏幕配置，而且不支持1080p以上分辨率的屏幕");
                Environment.Exit(0);
            }
            if (!System.IO.File.Exists("Settings.set"))
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
            //获取主线程ID
            mainThreadID = Thread.CurrentThread.ManagedThreadId;
            bingRandPicApi = "https://bing.ioliu.cn/v1/rand?w=" + screenWidth.ToString() + "&h=" + screenHeight.ToString();
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
            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\BingWallpaperChanger.lnk"))
                startWithSystem.Checked = true;
            else
                startWithSystem.Checked = false;
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
                downloadBingDaily("temp\\dailyPic.bmp");
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

        //保存图片按钮事件
        private void savePicButton_Click(object sender, EventArgs e)
        {
            savePic();
        }

        //开机启动复选框
        private void startWithSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (!startFlag)
            {
                if (startWithSystem.Checked)
                    CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "BingWallpaperChanger", Application.ExecutablePath);
                else
                {
                    if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\BingWallpaperChanger.lnk"))
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\BingWallpaperChanger.lnk");
                }
            }
        }

        //通知栏菜单退出按钮事件
        private void exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //通知栏菜单更新壁纸按钮事件
        private void updateNow_Click(object sender, EventArgs e)
        {
            downloadBingRand("temp\\randPic.bmp");
            randTimer.Stop();
            randTimer.Start();
        }

        //通知栏菜单显示按钮事件
        private void show_Click(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
            Show();
        }

        //通知栏菜单保存图片按钮事件
        private void savePicItem_Click(object sender, EventArgs e)
        {
            savePic();
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
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = true;
                WindowState = FormWindowState.Normal;
                Show();
            }
            else
            {
                Activate();
            }
        }

        //日图检测计时器
        private void randTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            downloadBingRand("temp\\randPic.bmp");
        }

        //随机图片更换计时器
        private void dailyTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            downloadBingDaily("temp\\Pic.bmp");
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

        //保存图片
        private void savePic()
        {
            SaveFileDialog imageDialog = new SaveFileDialog();
            imageDialog.FileName = backgroundOriginChoose.SelectedIndex == 0 ? "dailyPic.jpg" : "randPic.jpg";
            imageDialog.Filter = "JPG文件|*.jpg|BMP文件|*.bmp";
            imageDialog.ShowDialog();
            string imageAdress = imageDialog.FileName;
            if (imageDialog.FilterIndex == 0)
            {
                ImageCodecInfo[] vImageCodecInfos = ImageCodecInfo.GetImageEncoders();
                Bitmap vBitmap = new Bitmap(backgroundOriginChoose.SelectedIndex == 0 ? "temp\\dailyPic.bmp" : "temp\\randPic.bmp");
                foreach (ImageCodecInfo vImageCodecInfo in vImageCodecInfos)
                {
                    if (vImageCodecInfo.FormatDescription.ToLower() == "jpeg")
                    {
                        EncoderParameters vEncoderParameters = new EncoderParameters(1);
                        vEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 20L);
                        vBitmap.Save(imageAdress, vImageCodecInfo, vEncoderParameters);
                        break;
                    }
                }
            }
            else
            {
                if (System.IO.File.Exists(imageAdress))
                    System.IO.File.Delete(imageAdress);
                System.IO.File.Copy(backgroundOriginChoose.SelectedIndex == 0 ? "temp\\dailyPic.bmp" : "temp\\randPic.bmp", imageAdress);
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

        //方便进行多线程调用
        private void downloadBingDaily(string path)
        {
            changeEnabled(false);
            //用Lambda表达式传值，实际上是编译器帮助生成了一个新类，所以不用从object转换了
            Thread downloader = new Thread(() => _downloadBingDaily(path));
            downloader.Start();
        }

        //方便进行多线程调用
        private void downloadBingRand(string path)
        {
            changeEnabled(false);
            //用Lambda表达式传值，实际上是编译器帮助生成了一个新类，所以不用从object转换了
            Thread downloader = new Thread(() => _downloadBingRand(path));
            downloader.Start();
        }

        //调用每日bing壁纸下载，下载前先从Xml拿数据
        private void _downloadBingDaily(string path)
        {
            string imageUrl = "";
            setText setText = () => { state.Text = "正在查询更新..."; };
            state.Invoke(setText);
            try
            {
                //准备临时Xml文件
                Directory.CreateDirectory("temp");
                if (System.IO.File.Exists(@"temp\temp.xml"))
                    System.IO.File.Delete(@"temp\temp.xml");
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
                MessageBox.Show("检查壁纸更新时出错：" + ex.Message + "\n详细信息：" + ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorTimer();
            }
            //判断是不是最新的每日壁纸
            XmlDocument urlXml = new XmlDocument();
            XmlDocument settings = new XmlDocument();
            urlXml.Load(@"temp/temp.xml");
            settings.Load("Settings.set");
            XmlNode node = urlXml.SelectSingleNode("//image");
            XmlNode saveTime = settings.SelectSingleNode("//设置");
            if (!System.IO.File.Exists(@"temp/dailyPic.bmp") || (node.SelectSingleNode("startdate")).InnerText != (saveTime.SelectSingleNode("每日壁纸日期")).InnerText)
            {
                //解析Xml获得每日壁纸地址
                node = urlXml.SelectSingleNode("//image");
                if (node != null)
                    imageUrl = "http://www.bing.com" + (node.SelectSingleNode("urlBase")).InnerText + "_" + screenWidth.ToString() + "x" + screenHeight.ToString() + ".jpg";
                //下载图片
                downloadPic(imageUrl, path);
                (saveTime.SelectSingleNode("每日壁纸日期")).InnerText = (node.SelectSingleNode("startdate")).InnerText;
            }
            else
            {
                setText = () => { state.Text = ""; };
                state.Invoke(setText);
            }
            urlXml.Save(@"temp/temp.xml");
            settings.Save("Settings.set");
            changeBackground(Directory.GetCurrentDirectory() + "\\" + path);
            System.IO.File.Delete(@"temp/temp.xml");
        }

        //调用随机bing壁纸下载
        private void _downloadBingRand(string path)
        {
            downloadPic(bingRandPicApi, path);
            changeBackground(Directory.GetCurrentDirectory() + "\\" + path);
        }

        //下载和保存壁纸
        private void downloadPic(string url, string path)
        {
            setText setText = () => { state.Text = "正在下载壁纸..."; };
            state.Invoke(setText);
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
                if (stream != null)
                {
                    Image.FromStream(stream).Save(@"temp/temp.jpg");
                    JPGtoBMP(@"temp/temp.jpg", path);
                    stream.Dispose();
                    stream.Close();
                    image.Close();
                    System.IO.File.Delete(@"temp/temp.jpg");
                }
                else
                {
                    stream.Dispose();
                    stream.Close();
                    image.Close();
                    errorTimer();
                }
                setText = () => { state.Text = ""; };
                state.Invoke(setText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("下载壁纸时出错：" + ex.Message + "\n详细信息：" + ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorTimer();
            }
        }

        //遇到错误重新执行的倒计时（只允许在非主线程上运行）
        private void errorTimer()
        {
            if (mainThreadID != Thread.CurrentThread.ManagedThreadId)
            {
                for(int i = 3;i > 0;i--)
                {
                    setText setText = () =>
                    {
                        state.Text = "遇到错误，将在" + i.ToString() + "秒后重试...";
                    };
                    state.Invoke(setText);
                    Thread.Sleep(1000);
                }
                if(backgroundOriginChoose.SelectedIndex == 0)
                    _downloadBingDaily("temp\\Pic.bmp");
                else
                    _downloadBingRand("temp\\randPic.bmp");
            }
        }

        //修改影响下载的相关控件的可用状态
        private void changeEnabled(bool enabledState)
        {
            if (mainThreadID != Thread.CurrentThread.ManagedThreadId)
            {
                setEnabled setEnabled = () =>
                {
                    if(backgroundOriginChoose.SelectedIndex != 0)
                    {
                        changeNow.Enabled = enabledState;
                        changeTimeChoose.Enabled = enabledState;
                        (contextMenuStrip.Items.Find("updateNow", true))[0].Enabled = enabledState;
                    }
                    backgroundOriginChoose.Enabled = enabledState;
                    savePicButton.Enabled = enabledState;
                    (contextMenuStrip.Items.Find("savePicItem", true))[0].Enabled = enabledState;
                };
                state.Invoke(setEnabled);
            }
            else
            {
                if (backgroundOriginChoose.SelectedIndex != 0)
                {
                    changeNow.Enabled = enabledState;
                    changeTimeChoose.Enabled = enabledState;
                    (contextMenuStrip.Items.Find("updateNow", true))[0].Enabled = enabledState;
                }
                backgroundOriginChoose.Enabled = enabledState;
                savePicButton.Enabled = enabledState;
                (contextMenuStrip.Items.Find("savePicItem", true))[0].Enabled = enabledState;
            }
        }

        //.jpg转.bmp
        private void JPGtoBMP(string jpgFile, string bmpFile)
        {
            try
            {
                //转换jpg到bmp
                using (Bitmap source = new Bitmap(jpgFile))
                {
                    using (Bitmap bmp = new Bitmap(source.Width, source.Height, PixelFormat.Format16bppRgb565))
                    {
                        Graphics.FromImage(bmp).DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height));
                        bmp.Save(bmpFile, ImageFormat.Bmp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("转换壁纸时出错：" + ex.Message + "\n详细信息：" + ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //更改壁纸
        private void changeBackground(string bmpFile)
        {
            SystemParametersInfo(20, 0, bmpFile, 0x2);
            changeEnabled(true);
        }

        //创建快捷方式
        public static void CreateShortcut(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string shortcutPath = Path.Combine(directory, string.Format("{0}.lnk", shortcutName));
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);//创建快捷方式对象
            shortcut.TargetPath = targetPath;//指定目标路径
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);//设置起始位置
            shortcut.WindowStyle = 1;//设置运行方式，默认为常规窗口
            shortcut.Description = description;//设置备注
            shortcut.IconLocation = targetPath;//设置图标路径
            shortcut.Save();//保存快捷方式
        }
    }
}