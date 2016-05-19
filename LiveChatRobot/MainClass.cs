using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Net;

//using System.Reflection; // Assembly(output dll)

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Threading;

using System.Speech.Synthesis;
using System.Globalization;

using System.Collections;// ArrayList

namespace LiveChatRobot
{
    public class MainClass : BilibiliDM_PluginFramework.DMPlugin
    {
        public class DanmuData
        {
            public string userId;
            public string userName;
            public string danmuText;
            public string location;
        }

        // 登录信息
        private string bili_cookie = "";
        private string tuling_uname = "", tuling_psw = "", tuling_key = "";
        private string tuling_cookie = "";

        // 自动答复
        private bool isFilterDanmu = true, isReplyOnly = true;
        private string[] filterStrs = { "" }, containStrs = { "" };
        private string customReplyData = "";

        // 公告
        private bool isAutoSendMsg = true;
        private string[] autoSendStrs = { "" };
        private int autoSendInterval_int = 30;

        // 礼物答谢 single/multi
        private bool isReGift = true, isReGift_SS = false, isReGift_SM = false, isReGift_MM = false, isAutoMerge = true, isDelayReGift = true;
        private string[] reGiftStrs_SS = { "" }, reGiftStrs_SM = { "" }, reGiftStrs_MM = { "" };
        private int reGiftDelay_int = 5, mergeCount_int = 3;

        // 其他
        private bool isAllowPubTeach = true, isLimitPara = true, isUseVoice = false, isUseRobot = true;
        private int limitParaCount_int = 5, danmukuMaxLen_int = 20;
        private string danmukuColor = "0xffffff";

        //private bool isFilterDanmu = true, isAutoSendMsg = true, isReGift = true, isAllowPubTeach = true, isLimitPara = true, isUseVoice = false, isUseRobot = true, isReplyOnly = true;
        //private string danmukuColor = "", customReplyData = "";
        //private string[] autoSendStrs = { "" }, containStrs = { "" }, filterStrs = { "" }, reGiftStrs_single = { "" }, reGiftStrs_all = { "" };
        //private int autoSendInterval_int = 30, reGiftDelay_int = 5, limitParaCount_int = 5, danmukuMaxLen_int = 20;

        private string xmlFilePath;

        // 直播1秒内不能发送过多弹幕，缓冲用
        //private static Dictionary<System.Timers.Timer, string> danmuPool;
        private DanmukuSendList danmukuSendList;

        // 读弹幕
        private SpeechSynthesizer speechSynthesizer;

        // 定时发送
        private System.Timers.Timer autoSender_timer;

        // 礼物感谢合并发送用
        //private ArrayList thankGiftTimerList;
        private ArrayList user_gift_List;
        //private double giftStartTime = 0;
        private Mutex reGiftMutex;
        private System.Timers.Timer thankGiftDelay_timer;

        private SettingForm settingForm;

        public MainClass()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.Connected += MainClass_Connected;
            this.Disconnected += MainClass_Disconnected;
            this.ReceivedDanmaku += MainClass_ReceivedDanmaku;
            this.ReceivedRoomCount += MainClass_ReceivedRoomCount;
            this.PluginAuth = "liuzx";
            this.PluginName = "房管助手";
            this.PluginDesc = "房管必备";
            this.PluginCont = "liuz430524@hotmail.com";
            this.PluginVer = "0.3.1";
            
            // 弹幕缓存
            danmukuSendList = new DanmukuSendList();          
            // 定时发送
            autoSender_timer = new System.Timers.Timer();
            autoSender_timer.Elapsed += new System.Timers.ElapsedEventHandler(AutoSendMsg_Elapsed);
            //thankGiftTimerList = new ArrayList();
            // 礼物答谢
            user_gift_List = new ArrayList();
            reGiftMutex = new Mutex();
            thankGiftDelay_timer = new System.Timers.Timer();
            thankGiftDelay_timer.AutoReset = false;
            thankGiftDelay_timer.Elapsed += new System.Timers.ElapsedEventHandler(ThankGiftDelay_Elapsed);

        }

        public override void Start()
        {
            base.Start();            
            // 检查用户信息是否存在
            xmlFilePath = @System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\弹幕姬\聊天姬\settings.xml";
            FileInfo fileInfo = new FileInfo(xmlFilePath);
            if (!fileInfo.Exists || fileInfo.Length <= 0)
            {
                MessageBox.Show("请右击插件，选择管理，填写好用户名和密码！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Stop();
                return;
            }
            else
            {
                new Thread(new ThreadStart(ReadSettings)).Start();
            }            
        }

        private void ReadSettings()
        {
            settingForm = new SettingForm();

            foreach (Control control in settingForm.Controls)
            {
                if (control.HasChildren)
                {
                    System.Text.RegularExpressions.Regex numberRegex = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
                    foreach (Control lowerControl in control.Controls)
                    {
                        if (lowerControl is TextBox)
                        {
                            TextBox txtBox = (TextBox)lowerControl;
                            switch (txtBox.Name)
                            {
                                case "textBox_tulingKey": tuling_key = txtBox.Text; break;
                                case "textBox_tulingPSW": tuling_psw = txtBox.Text; break;
                                case "textBox_tulingUNAME": tuling_uname = txtBox.Text; break;
                                case "textBox_autoSendInterval":
                                    {
                                        if (numberRegex.IsMatch(txtBox.Text)) autoSendInterval_int = int.Parse(txtBox.Text);
                                        break;
                                    }
                                case "textBox_reGiftDelay":
                                    {
                                        if (numberRegex.IsMatch(txtBox.Text)) reGiftDelay_int = int.Parse(txtBox.Text);
                                        break;
                                    }
                                case "textBox_mergeCount":
                                    {
                                        if (numberRegex.IsMatch(txtBox.Text)) mergeCount_int = int.Parse(txtBox.Text);
                                        break;
                                    }
                                case "textBox_limitParaCount":
                                    {
                                        if (numberRegex.IsMatch(txtBox.Text)) limitParaCount_int = int.Parse(txtBox.Text);
                                        break;
                                    }
                                case "textBox_danmukuMaxLen":
                                    {
                                        if (numberRegex.IsMatch(txtBox.Text)) danmukuMaxLen_int = int.Parse(txtBox.Text);
                                        break;
                                    }
                                case "textBox_danmukuColor": danmukuColor = txtBox.Text; break;
                                default: break;
                            }
                        }

                        if (lowerControl is RichTextBox)
                        {
                            RichTextBox richTxtBox = (RichTextBox)lowerControl;
                            switch (richTxtBox.Name)
                            {
                                case "richTextBox_cookie": bili_cookie = richTxtBox.Text; break;
                                case "richTextBox_reGiftStrList_SS":
                                    {
                                        reGiftStrs_SS = richTxtBox.Text.Split('\n');
                                        break;
                                    }
                                case "richTextBox_reGiftStrList_SM":
                                    {
                                        reGiftStrs_SM = richTxtBox.Text.Split('\n');
                                        break;
                                    }
                                case "richTextBox_reGiftStrList_MM":
                                    {
                                        reGiftStrs_MM = richTxtBox.Text.Split('\n');
                                        break;
                                    }
                                case "richTextBox_autoSendStrList":
                                    {
                                        autoSendStrs = richTxtBox.Text.Split('\n');
                                        break;
                                    }
                                case "richTextBox_filterStrList":
                                    {
                                        filterStrs = richTxtBox.Text.Split('\n');
                                        break;
                                    }
                                case "richTextBox_containStrList":
                                    {
                                        containStrs = richTxtBox.Text.Split('\n');
                                        break;
                                    }
                                case "richTextBox_customReplyData":
                                    {
                                        customReplyData = richTxtBox.Text;
                                        break;
                                    }
                                default: break;
                            }
                        }

                        if (lowerControl is CheckBox)
                        {
                            CheckBox chkBox = (CheckBox)lowerControl;
                            switch (chkBox.Name)
                            {
                                case "checkBox_isUseRobot": isUseRobot = chkBox.Checked; break;
                                case "checkBox_isUseVoice": isUseVoice = chkBox.Checked; break;
                                case "checkBox_isLimitPara": isLimitPara = chkBox.Checked; break;
                                case "checkBox_isAllowPubTeach": isAllowPubTeach = chkBox.Checked; break;
                                case "checkBox_isReGift": isReGift = chkBox.Checked; break;
                                case "checkBox_isDelayReGift": isDelayReGift = chkBox.Checked; break;
                                case "checkBox_isAutoMerge": isAutoMerge = chkBox.Checked; break;
                                case "checkBox_isAutoSendMsg": isAutoSendMsg = chkBox.Checked; break;
                                case "checkBox_isFilterDanmu": isFilterDanmu = chkBox.Checked; break;
                                case "checkBox_replyOnly": isReplyOnly = chkBox.Checked; break;
                                default: break;
                            }
                        }

                        if (lowerControl is RadioButton)
                        {
                            RadioButton radioButton = (RadioButton)lowerControl;
                            switch (radioButton.Name)
                            {
                                case "radioButton_reGift_SS": isReGift_SS = radioButton.Checked; break;
                                case "radioButton_reGift_SM": isReGift_SM = radioButton.Checked; break;
                                case "radioButton_reGift_MM": isReGift_MM = radioButton.Checked; break;
                                default: break;
                            }
                        }

                    }
                }
            }

            // 读取完毕，启动聊天机器人
            StartRobot();
        }

        private void StartRobot()
        {
            if (bili_cookie == "" || tuling_uname == "" || tuling_psw == "" || tuling_key == "")
            {
                this.AddDM("请填写必要的信息~");
                Stop();
                return;
            }

            // 公告-自动发送
            //System.Text.RegularExpressions.Regex numberRegex = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            //if (!numberRegex.IsMatch(autoSendInterval))
            //{
            //    autoSendInterval = "30";
            //}
            //else
            //{
            //    if (Int64.Parse(autoSendInterval) < 0) autoSendInterval = "30";
            //}
            if (isAutoSendMsg)
            {
                autoSender_timer.Stop();
                autoSender_timer.Interval = autoSendInterval_int * 1000;
                autoSender_timer.Start();
            }

            // 初始化SpeechSynthesizer
            if (isUseVoice)
            {
                speechSynthesizer = new SpeechSynthesizer();
                speechSynthesizer.SetOutputToDefaultAudioDevice();
                speechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new CultureInfo("zh-CN"));
            }

            // 延迟答谢
            //if (!numberRegex.IsMatch(reGiftDelay))
            //{
            //    reGiftDelay = "5";
            //}
            //else
            //{
            //    if (Int64.Parse(reGiftDelay) < 0) reGiftDelay = "5";
            //}
            // 分段限制
            //if (!numberRegex.IsMatch(danmukuMaxCount))
            //{
            //    danmukuMaxCount = "10";
            //}
            //else
            //{
            //    if (Int32.Parse(danmukuMaxCount) < 0) danmukuMaxCount = "10";
            //}
            //this.AddDM("已启动~");
        }

        private void AutoSendMsg_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 定时发送信息
            if (isAutoSendMsg && autoSendStrs.Length > 0)
            {
                Random num = new Random();
                int point = num.Next(0, autoSendStrs.Length);
                SendDanmuSplited(autoSendStrs[point]);
            }
        }

        //static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    var assName = new AssemblyName(args.Name).FullName;
        //    if (assName.Contains("Newtonsoft.Json"))
        //    {
        //        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Newtonsoft.Json.dll"))
        //        {
        //            var bytes = new byte[stream.Length];
        //            stream.Read(bytes, 0, (int)stream.Length);
        //            return Assembly.Load(bytes);
        //        }
        //    }
        //    //MessageBox.Show(assName);
        //    throw new DllNotFoundException(assName);
        //}

        private void MainClass_ReceivedRoomCount(object sender, BilibiliDM_PluginFramework.ReceivedRoomCountArgs e)
        {

        }

        private void MainClass_ReceivedDanmaku(object sender, BilibiliDM_PluginFramework.ReceivedDanmakuArgs e)
        {
            if (e.Danmaku.MsgType == BilibiliDM_PluginFramework.MsgTypeEnum.Comment)
            {
                //弹幕过滤
                if (isFilterDanmu)
                {
                    foreach (string filter in filterStrs)
                    {
                        if (e.Danmaku.CommentText.Contains(filter)) return;
                    }
                }

                if (isReplyOnly)
                {
                    bool isContain = false;
                    foreach (string str in containStrs)
                    {
                        if (e.Danmaku.CommentText.Contains(str))
                        {
                            isContain = true;
                            break;
                        }
                    }
                    if (!isContain) return;
                }

                // 需要从info中解析uid，用于判断发送此弹幕的用户
                if (!e.Danmaku.RawData.Contains("info"))
                {
                    Stop();
                    throw new Exception("抱歉，B站弹幕信息发生了变化(没有info)，无法解析相应属性,请反馈给我liuz430524@hotmail.com");
                }
                else
                {
                    JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(e.Danmaku.RawData);
                    string uid = jsonResponse["info"][2][0].ToString();
                    DanmuData danmuData = new DanmuData();
                    danmuData.danmuText = e.Danmaku.CommentText;
                    danmuData.userId = uid;
                    new Thread(new ParameterizedThreadStart(ReplyDanmu)).Start(danmuData);
                }

            }
            else if (e.Danmaku.MsgType == BilibiliDM_PluginFramework.MsgTypeEnum.GiftSend && isReGift)
            {
                if (reGiftMutex.WaitOne(1000))
                {
                    //double nowTime = DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds;

                    // 打断，重新开始
                    thankGiftDelay_timer.Enabled = false;
                    if (isDelayReGift)
                        thankGiftDelay_timer.Interval = reGiftDelay_int * 1000;
                    else
                        thankGiftDelay_timer.Interval = 100;
                    thankGiftDelay_timer.Enabled = true;

                    string userGiftstr = e.Danmaku.GiftUser + "\t" + e.Danmaku.GiftName;
                    bool isContain = false;
                    for (int i = 0; i < user_gift_List.Count; ++i)
                    {
                        if (((string)user_gift_List[i]) == userGiftstr)
                        {
                            isContain = true;
                            break;
                        }
                    }
                    if (!isContain)
                    {
                        user_gift_List.Add(userGiftstr);
                    }
                    
                    //new Thread(new ParameterizedThreadStart(thankGiftDelay)).Start(thankStr);

                    
                    //if (nowTime - lastReGiftTime > Int64.Parse(reGiftDelay) * 1000)
                    //{
                    //    string thankStr = "";

                    //    for (int i = 0; i < userGiftList.Count; ++i)
                    //    {
                    //        userGiftstr = (string)userGiftList[i];
                    //        string[] unameAndgname = userGiftstr.Split('\t');
                    //        if (unameAndgname.Length > 1)
                    //        {
                    //            Random num = new Random();
                    //            int point = num.Next(0, reGiftStrs.Length);
                    //            thankStr = reGiftStrs[point];
                    //            string uname = unameAndgname[0];
                    //            string gname = unameAndgname[1];
                    //            thankStr = thankStr.Replace("%name%", uname).Replace("%gift%", gname);
                    //            thankGift(thankStr);
                    //        }
                    //    }
                    //    userGiftList.Clear();
                    //    lastReGiftTime = nowTime;
                    //}
                    
                    reGiftMutex.ReleaseMutex();
                }            
            }
        }

        private void thankGift(object str)
        {
            string thankStr = (string)str;
            // 停止并清除未执行的timer
            //for(int i=0;i<thankGiftTimerList.Count;++i)
            //{
            //    ((System.Timers.Timer)thankGiftTimerList[i]).Enabled = false;
            //    thankGiftTimerList.RemoveAt(i);
            //}

            System.Timers.Timer thankGift_timer = new System.Timers.Timer();
            //thankGift_timer.Interval = Int64.Parse(reGiftDelay) * 1000;
            thankGift_timer.AutoReset = false;            
            thankGift_timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => ThankGift_Elapsed(sender, e, thankStr));
            //thankGift_timer.Start();
            danmukuSendList.Add(thankGift_timer);

            //thankGiftTimerList.Add(thankGift_timer);
        }

        private void ThankGift_Elapsed(object sender, System.Timers.ElapsedEventArgs e, string thankStr)
        {
            SendDanmuSplited(thankStr);
        }

        private void ThankGiftDelay_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (reGiftMutex.WaitOne(1000))
            {
                ArrayList thankStrList = new ArrayList();
                //string thankStr = "";
                if (isReGift_SS && reGiftStrs_SS.Length > 0)
                {
                    for (int i = 0; i < user_gift_List.Count; ++i)
                    {
                        string userGiftstr = (string)user_gift_List[i];
                        string[] uname_gname = userGiftstr.Split('\t');
                        if (uname_gname.Length > 1)
                        {
                            string userName = uname_gname[0];
                            string giftName = uname_gname[1];
                            
                            int point = new Random().Next(0, reGiftStrs_SS.Length);
                            string thankStr = reGiftStrs_SS[point];
                            thankStr = thankStr.Replace("%name%", userName).Replace("%gift%", giftName);
                            thankStrList.Add(thankStr);
                            //thankGift(thankStr);
                        }
                    }
                }

                if (isReGift_SM && reGiftStrs_SM.Length > 0)
                {
                    for (int i = 0; i < user_gift_List.Count; ++i)
                    {
                        string userGiftstr = (string)user_gift_List[i];
                        string[] uname_gname = userGiftstr.Split('\t');
                        if (uname_gname.Length > 1)
                        {
                            string userName = uname_gname[0];
                            string giftName = uname_gname[1];
                            for (int j = i + 1; j < user_gift_List.Count; ++j)
                            {
                                userGiftstr = (string)user_gift_List[j];
                                uname_gname = userGiftstr.Split('\t');
                                if (uname_gname.Length > 1)
                                {
                                    if (userName == uname_gname[0])
                                    {
                                        if (!giftName.Contains(uname_gname[1])) giftName += "、" + uname_gname[1];
                                        user_gift_List[i] = userName + '\t' + giftName;
                                        user_gift_List.RemoveAt(j);
                                    }
                                }
                            }
                            int point = new Random().Next(0, reGiftStrs_SM.Length);
                            string thankStr = reGiftStrs_SM[point];
                            thankStr = thankStr.Replace("%name%", userName).Replace("%gifts%", giftName);
                            thankStrList.Add(thankStr);
                            //thankGift(thankStr);
                        }
                    }
                }

                if ((isReGift_MM && reGiftStrs_MM.Length > 0) || (isAutoMerge && thankStrList.Count > mergeCount_int))
                {
                    string userNames = "", giftNames = "";
                    for (int i = 0; i < user_gift_List.Count; ++i)
                    {
                        string userGiftstr = (string)user_gift_List[i];
                        string[] uname_gname = userGiftstr.Split('\t');
                        if (uname_gname.Length > 1)
                        {
                            string userName = uname_gname[0];
                            string giftName = uname_gname[1];
                            if (!userNames.Contains(userName))
                            {
                                userNames += userName;
                                userNames += "、";
                            }
                            if (!giftNames.Contains(giftName))
                            {
                                giftNames += giftName;
                                giftNames += "、";
                            }
                        }
                    }
                    if (userNames.EndsWith("、")) userNames = userNames.Substring(0, userNames.LastIndexOf("、"));
                    if (giftNames.EndsWith("、")) giftNames = giftNames.Substring(0, giftNames.LastIndexOf("、"));
                    int point = new Random().Next(0, reGiftStrs_MM.Length);
                    string thankStr = reGiftStrs_MM[point];
                    thankStr = thankStr.Replace("%names%", userNames).Replace("%gifts%", giftNames);
                    thankGift(thankStr);
                    thankStrList.Clear();
                }

                for (int i = 0; i < thankStrList.Count; ++i)
                {
                    string thankStr = (string)thankStrList[i];
                    thankGift(thankStr);
                }

                user_gift_List.Clear();
                reGiftMutex.ReleaseMutex();
            }
        }

        private void ReplyDanmu(object data)
        {
            DanmuData danmuData = (DanmuData)data;

            // 调教弹幕
            if (isAllowPubTeach || bili_cookie.Contains(danmuData.userId))
            {
                if (danmuData.danmuText.IndexOf("?") == 0 || danmuData.danmuText.IndexOf("？") == 0)
                {
                    if ((danmuData.danmuText.Contains(":") || danmuData.danmuText.Contains("：")))
                    {
                        string danmu = danmuData.danmuText.Substring(1).Replace("：", ":");
                        if (danmu.IndexOf(":") > 0 && danmu.IndexOf(":") < danmu.Length - 1)
                        {
                            string[] qa = danmu.Split(new char[] { ':' });
                            string question = qa[0], answer = qa[1];
                            string result = AddNLP(question, answer);
                            if (result != null) SendDanmuSplited(result);
                            return;
                        }
                    }
                }
            }

            // 普通弹幕
            if (!bili_cookie.Contains(danmuData.userId))
            {
                // 自定义回复
                string[] rows = customReplyData.Split('\n');
                foreach (string row in rows)
                {
                    string[] columns = row.Split('\t');
                    if (columns.Length == 4)
                    {
                        if (columns[0] == "true")
                        {
                            string[] contains = columns[1].Split('，');
                            string[] filters = columns[2].Split('，');
                            string replyStr = columns[3];
                            bool isContains = false;
                            foreach (string contain in contains)
                            {
                                if (contain.Length <= 0) continue;
                                if (danmuData.danmuText.Contains(contain))
                                {
                                    isContains = true;
                                    break;
                                }
                            }
                            if (isContains)
                            {
                                bool hasFilter = false;
                                foreach (string filter in filters)
                                {
                                    if (filter.Length <= 0) continue;
                                    if (danmuData.danmuText.Contains(filter))
                                    {
                                        hasFilter = true;
                                        break;
                                    }
                                }
                                if (!hasFilter)
                                {
                                    SendDanmuSplited(replyStr);
                                    return;
                                }
                            }
                        }
                    }
                }

                if (isUseRobot)
                {
                    // 获取用户名和地理位置
                    WebClient client = new WebClient();
                    byte[] response = client.DownloadData("http://api.bilibili.cn/userinfo?mid=" + danmuData.userId);
                    string responseJson = Encoding.UTF8.GetString(response);
                    JObject json = (JObject)JsonConvert.DeserializeObject(responseJson);
                    danmuData.userName = json["name"].ToString();
                    danmuData.location = json["place"].ToString().Replace(" ", "");
                    string text = SendToTulin(danmuData.danmuText, danmuData.userId, danmuData.location);
                    SendDanmuSplited(text);
                }
                
            }
        }

        private void SendDanmuSplited(string text)
        {
            if (text != null)
            {
                if (isUseVoice)
                {
                    ReadDanmu(text);
                }
                else
                {
                    // 字数大于20，用Timer处理
                    int cutLength = danmukuMaxLen_int;
                    int timerCount = text.Length / cutLength + 1;
                    if (isLimitPara) timerCount = timerCount > limitParaCount_int ? limitParaCount_int : timerCount;
                    //Log(timerCount + "");
                    for (int i = 0; i < timerCount; i++)
                    {
                        System.Timers.Timer SendDanmu_timer = new System.Timers.Timer();
                        //SendDanmu_timer.Interval = 1100 * (i + 1);
                        SendDanmu_timer.AutoReset = false; // 只执行一次

                        int start = i * cutLength;
                        if (start + cutLength > text.Length)
                        {
                            SendDanmu_timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => SendDanmu_Elapsed(sender, e, text.Substring(start)));
                            //danmuPool.Add(SendDanmu_timer, text.Substring(start));                        
                        }
                        else
                        {
                            SendDanmu_timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => SendDanmu_Elapsed(sender, e, text.Substring(start, cutLength)));
                            //danmuPool.Add(SendDanmu_timer, text.Substring(start, cutLength));                        
                        }
                        danmukuSendList.Add(SendDanmu_timer);
                        //SendDanmu_timer.Start();
                    }
                }

            }
        }

        private void SendDanmu_Elapsed(object sender, System.Timers.ElapsedEventArgs e, string str)
        {
            //System.Timers.Timer thisTimer = (System.Timers.Timer)sender;
            SendDanmuLess20Chars(str);
            //if (danmuPool.ContainsKey(thisTimer))
            //{                
            //    SendDanmuLess20Chars(danmuPool[thisTimer]);
            //    danmuPool.Remove(thisTimer);
            //}
        }

        private string SendToTulin(string text, string userId, string loc)
        {
            string formData = "key=" + tuling_key
                + "&info=" + text
                + "&userid=" + userId
                + "&loc=" + loc;
            string url = "http://www.tuling123.com/openapi/api";
            try
            {
                JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(HttpPost(formData, null, url));
                // {"code":X00000,"text":""}     
                string code = jsonResponse["code"].ToString();
                switch (code)
                {
                    case "100000": return jsonResponse["text"].ToString();
                    case "200000": return jsonResponse["text"].ToString() + ":" + jsonResponse["url"].ToString();
                    case "302000": return jsonResponse["text"].ToString() + ":" + jsonResponse["list"].ToString();
                    case "308000": return jsonResponse["text"].ToString() + ":" + jsonResponse["list"].ToString();
                    default: return jsonResponse["text"].ToString();
                }
            }
            catch
            {
                return null;
            }
        }

        private string AddNLP(string question, string answer)
        {
            if (tuling_cookie == null)
            {
                Encoding encoding = Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.tuling123.com/web/member!toLogin.action?errCount=0");
                request.Method = "post";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] buffer = encoding.GetBytes("member.username=" + tuling_uname + "&member.password=" + tuling_psw);
                request.ContentLength = buffer.Length;
                request.AllowAutoRedirect = false; // 坑
                request.GetRequestStream().Write(buffer, 0, buffer.Length); // 登录
                WebResponse response = request.GetResponse();
                if (response.Headers["Set-Cookie"] != null)
                {
                    tuling_cookie = response.Headers["Set-Cookie"].ToString();
                }
                else
                {
                    MessageBox.Show("不正确的用户名或密码导致登录不成功，调教功能已关闭", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isAllowPubTeach = false;
                    return null;
                }
                
            }

            //Console.WriteLine(response.Headers["Set-Cookie"]);            
            string strPostdata = "oneknwledge.question=" + question + "&oneknwledge.answer=" + answer;
            string responseText = HttpPost(strPostdata, tuling_cookie, "http://www.tuling123.com/web/knowledge!ajaxaddKnowledge.action");
            if (responseText.Contains("html"))
            {
                tuling_cookie = null;
                return AddNLP(question, answer);
            }
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(responseText);
            //if (jsonResponse["status"].ToString() != "success")   
            return jsonResponse["message"].ToString();
        }

        private bool SendDanmuLess20Chars(string text)
        {            
            string formData = "color=" + Convert.ToInt32(danmukuColor, 16)
                + "&fontsize=25"
                + "&mode=1"
                + "&msg=" + text
                + "&rnd=" + Convert.ToInt64((DateTime.Now - DateTime.Parse("1970-01-01 00:00:00")).TotalMilliseconds / 1000)
                + "&roomid=" + base.RoomId;
                //+ "&roomid=58472";
            string url = "http://live.bilibili.com/msg/send";
            //Log(formData);            
            try
            {
                JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(HttpPost(formData, bili_cookie, url));
                // {"code":0,"msg":"","data":[]}      
                string msg = jsonResponse["msg"].ToString();
                if (msg.Length > 0)
                {
                    Log("发送弹幕不成功：" + msg);
                    //Log("如果你一直遇到这种情况，可能需要在管理界面重新登录一下~");
                }
                if (jsonResponse["code"].ToString() != "0") return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string HttpPost(string formData, string cookie, string url)
        {
            //Console.WriteLine("post: \n" + formData + "\n" + cookie + "\n" + url + "\npost over");
            byte[] responseData;
            byte[] postData = Encoding.UTF8.GetBytes(formData);            
            WebClient client = new WebClient();
            try
            {
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36");
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                if (cookie != null) client.Headers.Add("Cookie", cookie);
                responseData = client.UploadData(url, "POST", postData);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                throw ex;
            }
            //Log(Encoding.UTF8.GetString(responseData));
            return Encoding.UTF8.GetString(responseData);
        }

        private void ReadDanmu(string text)
        {
            AddDM(text);
            Log(text);
            if (speechSynthesizer != null)
            {
                speechSynthesizer.Speak(text);
            }
        }

        private void MainClass_Disconnected(object sender, BilibiliDM_PluginFramework.DisconnectEvtArgs e)
        {
            //throw new NotImplementedException();
        }

        private void MainClass_Connected(object sender, BilibiliDM_PluginFramework.ConnectedEvtArgs e)
        {
            //throw new NotImplementedException();
        }

        public override void Admin()
        {
            base.Admin();
            settingForm = new SettingForm();
            //SettingForm settingForm = new SettingForm();
            settingForm.ShowDialog();
            //AddDM("对设置做了改动的话，需要重新启动聊天姬哦");
            this.Stop();
            this.Start();
            this.AddDM("已生效");
        }

        public override void Stop()
        {
            base.Stop();
            if (speechSynthesizer != null)
            {
                ((IDisposable)speechSynthesizer).Dispose();
            }
            autoSender_timer.Stop();
            //this.AddDM("已停止");
        }

    }
}
