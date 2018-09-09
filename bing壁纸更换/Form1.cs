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
        public static extern int SystemParametersInfo(int uAction,int uParam,string lpvParam,int fuWinIni);

        string bingDailyPicApi = "https://cn.bing.com/HPImageArchive.aspx?idx=0&n=1";
        string bingRandPicApi = "https://bing.ioliu.cn/v1/rand?w=1920&h=1080";
        bool startFlag = true;

        public Form1()
        {
            if (!File.Exists("Settings.set"))
            {
                XmlDocument settings = new XmlDocument();
                XmlDeclaration declaration = settings.CreateXmlDeclaration("1.0", "UTF-8", null);
                settings.AppendChild(declaration);
                XmlElement node = settings.CreateElement("设置");
                settings.AppendChild(node);
                XmlElement s1 = settings.CreateElement("壁纸源"), s2 = settings.CreateElement("更换频率"),s3 = settings.CreateElement("每日壁纸日期");
                s1.SetAttribute("id", "0");
                s2.SetAttribute("id", "1");
                s3.SetAttribute("id", "2");
                s1.InnerText = "0";
                s2.InnerText = "4";
                s3.InnerText = "";
                node.AppendChild(s1);
                node.AppendChild(s2);
                node.AppendChild(s3);
                settings.Save("Settings.set");
            }
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
        }

        private void backgroundOriginChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (backgroundOriginChoose.SelectedIndex == 0)
            {
                changeTimeChoose.Enabled = false;
                changeNow.Enabled = false;
                if (!startFlag)
                {
                    downloadBingDaily();
                    changeBackground(Directory.GetCurrentDirectory() + "\\temp\\dailyPic.bmp");
                }
            }
            else
            {
                changeTimeChoose.Enabled = true;
                changeNow.Enabled = true;
            }
            if (!startFlag)
            {
                updateSetting("0", backgroundOriginChoose.SelectedIndex.ToString());
            }
        }

        private void changeTimeChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!startFlag)
            {
                updateSetting("1", changeTimeChoose.SelectedIndex.ToString());
            }
        }

        private void changeNow_Click(object sender, EventArgs e)
        {
            downloadBingRand();
            changeBackground(Directory.GetCurrentDirectory() + "\\temp\\randPic.bmp");
        }

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

        private void downloadBingDaily()
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
            if ((node.SelectSingleNode("startdate")).InnerText != (saveTime.SelectSingleNode("每日壁纸日期")).InnerText)
            {
                //解析Xml获得每日壁纸地址
                node = urlXml.SelectSingleNode("//image");
                if (node != null)
                    imageUrl = "http://www.bing.com" + (node.SelectSingleNode("urlBase")).InnerText + "_1920x1080.jpg";
                //下载图片
                downloadPic(imageUrl, @"temp/dailyPic.bmp");
                (saveTime.SelectSingleNode("每日壁纸日期")).InnerText = (node.SelectSingleNode("startdate")).InnerText;
            }
            else
                state.Text = "";
            urlXml.Save(@"temp/temp.xml");
            settings.Save("Settings.set");
            File.Delete(@"temp/temp.xml");
        }

        private void downloadBingRand()
        {
            state.Text = "正在下载壁纸...";
            try
            {
                FileStream fs = new FileStream(@"temp/temp_1.jpg", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(bingRandPicApi);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*";
                request.Headers.Add("Accept-Language", "zh-CN");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1)";
                request.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                responseStream.Close();
                JPGtoBMP(@"temp/temp_1.jpg", @"temp/randPic.bmp");
                File.Delete(@"temp/temp_1.jpg");
                state.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void downloadPic(string url, string path)
        {
            state.Text = "正在下载壁纸...";
            try
            {
                HttpWebRequest getImage = (HttpWebRequest)WebRequest.Create(url);
                getImage.ServicePoint.Expect100Continue = false;
                getImage.Method = "GET";
                getImage.KeepAlive = true;
                getImage.ContentType = "image/*";
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

        private void changeBackground(string bmpFile)
        {
            SystemParametersInfo(20, 0, bmpFile, 0x2);
        }
    }
}
