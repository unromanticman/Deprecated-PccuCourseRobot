using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Specialized;
namespace PccuPost
{
    public partial class Form1 : Form
    {
        public struct Data
        {
            public string value;
            public string department;
            public string openClass;
            public string firstCode;
            public string endCode;
        }

        List<Data> hackData = new List<Data>();

        private CookieContainer cc;
        private SpWebClient spwc;
        private string webtoken;
        string str = "";
        string ret = "";
        private System.Threading.Thread hackThread;
        private System.Diagnostics.Stopwatch sw;

        //Start working or not
        private volatile bool isStartHackWorking = false;

        EASYINI courseData; 

        public Form1()
        {
            InitializeComponent();
            //INIT 
            CheckForIllegalCrossThreadCalls = false;
            //選課
            this.cc = new CookieContainer();
            this.spwc = new SpWebClient(cc);
            this.spwc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.72 Safari/537.36");
            //創立couse.um 儲存課程資料
            courseData = new EASYINI(".//course.um");
            //讀取檔案假如有的話
            getCourseData();
        }


        public void getCourseData(){

            try
            {
                a1.Text = courseData.getINI("course01", "01");
                a2.Text = courseData.getINI("course01", "02");
                a3.Text = courseData.getINI("course01", "03");
                a4.Text = courseData.getINI("course01", "04");
                a5.Text = courseData.getINI("course01", "05");

                b1.Text = courseData.getINI("course02", "01");
                b2.Text = courseData.getINI("course02", "02");
                b3.Text = courseData.getINI("course02", "03");
                b4.Text = courseData.getINI("course02", "04");
                b5.Text = courseData.getINI("course02", "05");

                c1.Text = courseData.getINI("course03", "01");
                c2.Text = courseData.getINI("course03", "02");
                c3.Text = courseData.getINI("course03", "03");
                c4.Text = courseData.getINI("course03", "04");
                c5.Text = courseData.getINI("course03", "05");

                d1.Text = courseData.getINI("course04", "01");
                d2.Text = courseData.getINI("course04", "02");
                d3.Text = courseData.getINI("course04", "03");
                d4.Text = courseData.getINI("course04", "04");
                d5.Text = courseData.getINI("course04", "05");

                e1.Text = courseData.getINI("course05", "01");
                e2.Text = courseData.getINI("course05", "02");
                e3.Text = courseData.getINI("course05", "03");
                e4.Text = courseData.getINI("course05", "04");
                e5.Text = courseData.getINI("course05", "05");

                f1.Text = courseData.getINI("course06", "01");
                f2.Text = courseData.getINI("course06", "02");
                f3.Text = courseData.getINI("course06", "03");
                f4.Text = courseData.getINI("course06", "04");
                f5.Text = courseData.getINI("course06", "05");


                g1.Text = courseData.getINI("course07", "01");
                g2.Text = courseData.getINI("course07", "02");
                g3.Text = courseData.getINI("course07", "03");
                g4.Text = courseData.getINI("course07", "04");
                g5.Text = courseData.getINI("course07", "05");
            }
            catch (Exception ex)
            {

            }
          

        }
        public void setCourseData(){

            courseData.setINI("course01","01",a1.Text);
            courseData.setINI("course01","02",a2.Text);
            courseData.setINI("course01","03",a3.Text);
            courseData.setINI("course01","04",a4.Text);
            courseData.setINI("course01","05",a5.Text);

            courseData.setINI("course02", "01", b1.Text);
            courseData.setINI("course02", "02", b2.Text);
            courseData.setINI("course02", "03", b3.Text);
            courseData.setINI("course02", "04", b4.Text);
            courseData.setINI("course02", "05", b5.Text);

            courseData.setINI("course03", "01", c1.Text);
            courseData.setINI("course03", "02", c2.Text);
            courseData.setINI("course03", "03", c3.Text);
            courseData.setINI("course03", "04", c4.Text);
            courseData.setINI("course03", "05", c5.Text);

            courseData.setINI("course04", "01", d1.Text);
            courseData.setINI("course04", "02", d2.Text);
            courseData.setINI("course04", "03", d3.Text);
            courseData.setINI("course04", "04", d4.Text);
            courseData.setINI("course04", "05", d5.Text);

            courseData.setINI("course05", "01", e1.Text);
            courseData.setINI("course05", "02", e2.Text);
            courseData.setINI("course05", "03", e3.Text);
            courseData.setINI("course05", "04", e4.Text);
            courseData.setINI("course05", "05", e5.Text);

            courseData.setINI("course06", "01", f1.Text);
            courseData.setINI("course06", "02", f2.Text);
            courseData.setINI("course06", "03", f3.Text);
            courseData.setINI("course06", "04", f4.Text);
            courseData.setINI("course06", "05", f5.Text);

            courseData.setINI("course07", "01", g1.Text);
            courseData.setINI("course07", "02", g2.Text);
            courseData.setINI("course07", "03", g3.Text);
            courseData.setINI("course07", "04", g4.Text);
            courseData.setINI("course07", "05", g5.Text);

        }

        private void startHack()
        {
            while (isStartHackWorking == true)
            {
                listBox1.Items.Add("共填寫選課" + (hackData.Count) + "筆，執行中...");

                NameValueCollection data = new NameValueCollection();

                data.Add("Account", ID.Text);
                data.Add("PassWord", PWD.Text);
                data.Add("PrjLangType", "");

                ret = Encoding.UTF8.GetString(spwc.UploadValues("https://mycourse.pccu.edu.tw/SCASele/login/lsChkLogin.asp", data));

                str = ret.Substring(ret.IndexOf("ApGUID={") + 7);
                str = str.Substring(0, str.IndexOf("}") + 1);

                if (str.Length > 0)
                {
                    label20.Text = "登入成功";
                    label20.ForeColor = Color.Green;
                }
                else
                {
                    label20.Text = "登入失敗";
                    label20.ForeColor = Color.Red;
                    MessageBox.Show("登入失敗，請再次確認帳號密碼是否正確!");
                    button1.Text = "機器人開始搶課";
                    listBox1.Items.Add("<已停止搶課>");
                    isStartHackWorking = false;
                    hackThread.Abort();
                }


                ret = Encoding.Default.GetString(spwc.UploadValues("https://mycourse.pccu.edu.tw/ScaSele/frame/apMain.asp?ApGUID=" + str + "&SeleLoginServer=mycourse", data));
                ret = spwc.DownloadString("https://mycourse.pccu.edu.tw/ScaSele/student/index.asp?ApGUID=" + str + "&SeleLoginServer=mycourse");
                CookieCollection cookies = cc.GetCookies(new Uri("https://mycourse.pccu.edu.tw/ScaSele/login/lsSetSession.asp?ApGUID=" + str + "&SeleLoginServer=mycourse"));
                foreach (Cookie cookie in cookies)
                {
                    //Console.WriteLine(cookie.Value);
                    if (cookie.Name == "ASPSESSIONIDSGRDBTCC")
                        this.webtoken = cookie.Value;
                }
                data.Clear();

                try
                {
                    foreach(Data hackDataCursor in hackData)
                    {
                        //取得課程名稱
                        data.Add("chkCourseCode", hackDataCursor.value + "," + hackDataCursor.department + "," + hackDataCursor.openClass + " ," + hackDataCursor.firstCode + "," + hackDataCursor.endCode);
                        ret = Encoding.UTF8.GetString(spwc.UploadValues("https://mycourse.pccu.edu.tw/SCASele/Student/SeleAddConfirm.asp?QuerySource=SeleByStudent&ApGUID=" + str + "&SeleLoginServer=mycourse&MaintainType=Add", data));
                        data.Clear();
                        ret = spwc.DownloadString("https://mycourse.pccu.edu.tw/ScaSele/student/SeleList.asp?prjno=lvMainMenuIndex=0&QuerySource=SeleByStudent&ApGuid=" + str + "&SeleLoginServer=mycourse");
                        ret = spwc.DownloadString("https://mycourse.pccu.edu.tw/SCASele/Student/SeleAdd.asp?QuerySource=SeleByStudent&ApGUID=" + str + "&SeleLoginServer=mycourse&CourseCode=" + hackDataCursor.value + "&MaintainType=Add");
                        if (ret.Contains("加選課程成功"))
                        {
                            listBox2.Items.Add("value -" + hackDataCursor.value + "代碼 :" + hackDataCursor.firstCode + "[" + hackDataCursor.endCode + "]" + " 加選成功!");
                        }
                    }

                    //for (int hackDataIndex = 0; hackDataIndex < 6; hackDataIndex++)
                    //{
                    
                    //        //取得課程名稱
                    //        data.Add("chkCourseCode", hackData[hackDataIndex].value + "," + hackData[hackDataIndex].department + "," + hackData[hackDataIndex].openClass + " ," + hackData[hackDataIndex].firstCode + "," + hackData[hackDataIndex].endCode);
                    //        ret = Encoding.UTF8.GetString(spwc.UploadValues("https://mycourse.pccu.edu.tw/SCASele/Student/SeleAddConfirm.asp?QuerySource=SeleByStudent&ApGUID=" + str + "&SeleLoginServer=mycourse&MaintainType=Add", data));
                    //        //沒有課程名稱
                    //        data.Clear();
                    //        ret = spwc.DownloadString("https://mycourse.pccu.edu.tw/ScaSele/student/SeleList.asp?prjno=lvMainMenuIndex=0&QuerySource=SeleByStudent&ApGuid=" + str + "&SeleLoginServer=mycourse");
                    //        ret = spwc.DownloadString("https://mycourse.pccu.edu.tw/SCASele/Student/SeleAdd.asp?QuerySource=SeleByStudent&ApGUID=" + str + "&SeleLoginServer=mycourse&CourseCode=" + hackData[hackDataIndex].value + "&MaintainType=Add");
                    //        if (ret.Contains("加選課程成功"))
                    //        {
                    //            listBox2.Items.Add("value -" + hackData[hackDataIndex].value + "代碼 :" + hackData[hackDataIndex].firstCode + "[" + hackData[hackDataIndex].endCode + "]" + " 加選成功!");
                    //        }                    
                    //}
                }
                catch (Exception e)
                {
                }

                try
                {
                    sw.Stop();
                    listBox1.Items.Add("耗時(s):" + sw.Elapsed.TotalMilliseconds / 1000);
                }
                catch (Exception ex)
                {
                }
            }
            
            ////取得課程名稱
            //data.Add("chkCourseCode", "270697,UENCI,2A ,7304,00");
            //ret = Encoding.UTF8.GetString(spwc.UploadValues("https://mycourse.pccu.edu.tw/SCASele/Student/SeleAddConfirm.asp?QuerySource=SeleByStudent&ApGUID=" + str + "&SeleLoginServer=mycourse&MaintainType=Add", data));

            ////沒有課程名稱
            //data.Clear();
            //ret = spwc.DownloadString("https://mycourse.pccu.edu.tw/ScaSele/student/SeleList.asp?prjno=lvMainMenuIndex=0&QuerySource=SeleByStudent&ApGuid=" + str + "&SeleLoginServer=mycourse");
            ////data.Add("SeleCode", "SeleByStudent");
            ////data.Add("AddSele", "�T�w�[��");
            //ret = spwc.DownloadString("https://mycourse.pccu.edu.tw/SCASele/Student/SeleAdd.asp?QuerySource=SeleByStudent&ApGUID=" + str + "&SeleLoginServer=mycourse&CourseCode=270697&MaintainType=Add");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(isStartHackWorking)
            {
                isStartHackWorking = false;
                sw.Stop();
                sw.Reset();

                System.Threading.Thread.Sleep(1);

                hackThread.Join();

                listBox1.Items.Add("<已停止搶課>");

                button1.Text = "機器人開始搶課";
            }
            else
            {
                isStartHackWorking = true;
                //計時     
                sw = new System.Diagnostics.Stopwatch();
                sw.Reset();//碼表歸零
                sw.Start();//碼表開始計時

                hackThread = new System.Threading.Thread(this.startHack);
                hackData.Clear();

                if (a1.Text != "")
                {
                    hackData.Add(new Data() { value = a1.Text, department = a2.Text, openClass = a3.Text, firstCode = a4.Text, endCode = a5.Text });
                }
                if (b1.Text != "")
                {
                    hackData.Add(new Data() { value = b1.Text, department = b2.Text, openClass = b3.Text, firstCode = b4.Text, endCode = b5.Text });
                }
                if (c1.Text != "")
                {
                    hackData.Add(new Data() { value = c1.Text, department = c2.Text, openClass = c3.Text, firstCode = c4.Text, endCode = c5.Text });
                }
                if (d1.Text != "")
                {
                    hackData.Add(new Data() { value = d1.Text, department = d2.Text, openClass = d3.Text, firstCode = d4.Text, endCode = d5.Text });

                }
                if (e1.Text != "")
                {
                    hackData.Add(new Data() { value = e1.Text, department = e2.Text, openClass = e3.Text, firstCode = e4.Text, endCode = e5.Text });
                }
                if (f1.Text != "")
                {
                    hackData.Add(new Data() { value = f1.Text, department = f2.Text, openClass = f3.Text, firstCode = f4.Text, endCode = f5.Text });
                }
                if (g1.Text != "")
                {
                    hackData.Add(new Data() { value = g1.Text, department = g2.Text, openClass = g3.Text, firstCode = g4.Text, endCode = g5.Text });
                } 

                //執行搶課線程
                hackThread.Start();                

                button1.Text = "再按一次停止";
            }
        }        

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.Add("[-]文大搶課王 已經成功開啟");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("http://imgur.com/H4KdVPO");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Close Thread
            try
            {
                hackThread.Abort();
            }
            catch (Exception ex) { }
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://never-nop.com");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setCourseData();
        }
    }
}
