using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveChatRobot
{
    public partial class Brower : Form
    {
        public string Cookie = "";
        private WebBrowser webBrowser;
        public Brower()
        {
            InitializeComponent();
            webBrowser = webBrowser1;
            webBrowser.Navigate("https://account.bilibili.com/ajax/miniLogin/minilogin?" + System.DateTime.Now.ToFileTime().ToString()); // 哔哩哔哩快速登录页                  
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Login); 
        }

        private void Login(object sender, WebBrowserDocumentCompletedEventArgs e)
        {            
            this.Text = webBrowser.DocumentTitle;
            if (webBrowser.Url.ToString().Contains("https://passport.bilibili.com/ajax/miniLogin/redirect"))
            {
                //MessageBox.Show("登录成功！", "成功");
                //Cookie = webBrowser.Document.Cookie;
                //webBrowser.Dispose();
                //this.Close();                
                webBrowser.Navigate("http://live.bilibili.com/all");
                //webBrowser.Visible = false;
            }
            if (webBrowser.Url.ToString().Contains("http://live.bilibili.com/all"))
            {
                Cookie = webBrowser.Document.Cookie;
                webBrowser.Dispose();
                this.Close();
            }
        }

    }
}
