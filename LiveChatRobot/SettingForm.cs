using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
//using System.Reflection; // Assembly(output dll)

namespace LiveChatRobot
{
    public partial class SettingForm : Form
    {

        private string xmlFilePath;

        public SettingForm()
        {
            InitializeComponent();

            string dataDir = @System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\弹幕姬\聊天姬";
            xmlFilePath = dataDir + @"\settings.xml";
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            if (!File.Exists(xmlFilePath))
            {
                initSetting();
            }
            else
            {
                // 读取上次的设置
                readAllControlsValue();

                if (richTextBox_cookie.Text != "")
                {
                    button_loginBilibili.Text = "重新登录";
                }
            }
        }

        private void button_accept_Click(object sender, EventArgs e)
        {
            if (richTextBox_cookie.Text == "" || textBox_tulingUNAME.Text == "" || textBox_tulingPSW.Text == "")
            {
                MessageBox.Show("没有登录bilibili或者没有输入图灵机器人的账号密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                saveAllControlsValue(this.Controls);
                MessageBox.Show("保存成功", "成功");
            }
        }

        private void saveAllControlsValue(Control.ControlCollection controlCollection)
        {
            DataSet dataSet = new DataSet("Data");
            DataTable textDataTable = new DataTable("Controls");
            textDataTable.Columns.Add("name", typeof(String));
            textDataTable.Columns.Add("value", typeof(String));
            dataSet.Tables.Add(textDataTable);

            foreach (Control control in controlCollection)
            {
                if (control.HasChildren)
                {
                    foreach (Control lowerControl in control.Controls)
                    {
                        if (lowerControl is TextBox)
                        {
                            TextBox txtBox = (TextBox)lowerControl;
                            DataRow row = textDataTable.NewRow();
                            row["name"] = txtBox.Name;
                            row["value"] = txtBox.Text;
                            textDataTable.Rows.Add(row);
                        }

                        if (lowerControl is RichTextBox)
                        {
                            RichTextBox richTxtBox = (RichTextBox)lowerControl;
                            DataRow row = textDataTable.NewRow();
                            row["name"] = richTxtBox.Name;
                            row["value"] = richTxtBox.Text;
                            textDataTable.Rows.Add(row);
                        }

                        if (lowerControl is CheckBox)
                        {
                            CheckBox chkBox = (CheckBox)lowerControl;
                            DataRow row = textDataTable.NewRow();
                            row["name"] = chkBox.Name;
                            row["value"] = chkBox.Checked.ToString();
                            textDataTable.Rows.Add(row);
                        }

                        if (lowerControl is RadioButton)
                        {
                            RadioButton radioButton = (RadioButton)lowerControl;
                            DataRow row = textDataTable.NewRow();
                            row["name"] = radioButton.Name;
                            row["value"] = radioButton.Checked.ToString();
                            textDataTable.Rows.Add(row);
                        }

                        //if (lowerControl is ListBox)
                        //{
                        //    ListBox lstBox = (ListBox)lowerControl;
                        //    DataRow row = textDataTable.NewRow();
                        //    row["name"] = lstBox.Name;
                        //    row["value"] = lstBox.SelectedIndex.ToString();
                        //    textDataTable.Rows.Add(row);
                        //}

                    }
                }
            }

            dataSet.WriteXml(xmlFilePath);
        }

        private void readAllControlsValue()
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlFilePath);
            if (dataSet.Tables.Count > 0)
            {
                DataTable textDataTable = dataSet.Tables["Controls"];
                if (textDataTable != null)
                {
                    DataRowCollection rowCollection = textDataTable.Rows;
                    string controlName, controlValue;
                    foreach (DataRow row in rowCollection)
                    {
                        if ((controlName = row["name"].ToString()) != null && (controlValue = row["value"].ToString()) != null)
                        {
                            foreach (Control control in this.Controls)
                            {
                                if (control.HasChildren)
                                {
                                    foreach (Control lowerControl in control.Controls)
                                    {
                                        if (lowerControl is TextBox)
                                        {
                                            TextBox txtBox = (TextBox)lowerControl;
                                            if (txtBox.Name == controlName)
                                            {
                                                txtBox.Text = controlValue;
                                            }
                                        }

                                        if (lowerControl is RichTextBox)
                                        {
                                            RichTextBox richTxtBox = (RichTextBox)lowerControl;
                                            if (richTxtBox.Name == controlName)
                                            {
                                                richTxtBox.Text = controlValue;
                                            }
                                        }

                                        if (lowerControl is CheckBox)
                                        {
                                            CheckBox chkBox = (CheckBox)lowerControl;
                                            if (chkBox.Name == controlName)
                                            {
                                                if (controlValue.ToLower() == "true")
                                                {
                                                    chkBox.Checked = true;
                                                }
                                                else if (controlValue.ToLower() == "false")
                                                {
                                                    chkBox.Checked = false;
                                                }
                                            }
                                        }


                                        if (lowerControl is RadioButton)
                                        {
                                            RadioButton radioButton = (RadioButton)lowerControl;
                                            if (radioButton.Name == controlName)
                                            {
                                                if (controlValue.ToLower() == "true")
                                                {
                                                    radioButton.Checked = true;
                                                }
                                                else if (controlValue.ToLower() == "false")
                                                {
                                                    radioButton.Checked = false;
                                                }
                                            }
                                        }

                                        //if (lowerControl is ListBox)
                                        //{
                                        //    ListBox lstBox = (ListBox)lowerControl;
                                        //    if (lstBox.Name == controlName)
                                        //    {
                                        //        lstBox.SelectedIndex = int.Parse(controlValue);
                                        //    }
                                        //}

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void initSetting()
        {
            // 自动回复
            checkBox_isFilterDanmu.Checked = true;
            richTextBox_filterStrList.Text = "点歌";
            // 礼物答谢
            checkBox_isReGift.Checked = true;
            radioButton_reGift_SS.Checked = false;
            richTextBox_reGiftStrList_SS.Text = "谢谢%name%赠送的%gift%~";
            radioButton_reGift_SM.Checked = true;
            richTextBox_reGiftStrList_SM.Text = "谢谢%name%赠送的%gifts%";
            radioButton_reGift_MM.Checked = false;
            richTextBox_reGiftStrList_MM.Text = "谢谢%names%赠送的%gifts%";
            checkBox_isAutoMerge.Checked = true;
            textBox_mergeCount.Text = "3";
            checkBox_isDelayReGift.Checked = true;
            textBox_reGiftDelay.Text = "5";
            // 公告
            checkBox_isAutoSendMsg.Checked = true;
            richTextBox_autoSendStrList.Text = "发送 ？问题：回复 教机器人说话";
            textBox_autoSendInterval.Text = "30";
            // 其他
            checkBox_isAllowPubTeach.Checked = true;
            checkBox_isLimitPara.Checked = true;
            checkBox_isUseRobot.Checked = true;
            textBox_limitParaCount.Text = "5";
            textBox_danmukuColor.Text = "0xffffff";
            textBox_danmukuMaxLen.Text = "20";
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            this.Close();
        }

        private void button_loginBilibili_Click(object sender, EventArgs e)
        {
            Brower brower = new Brower();
            brower.ShowDialog();
            richTextBox_cookie.Text = brower.Cookie;
            if (richTextBox_cookie.Text.Length <= 0)
            {
                MessageBox.Show("没有登录成功，请重新登录", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("登录成功", "成功");
                button_loginBilibili.Text = "重新登录";
            }
        }

        private void button_customReply_Click(object sender, EventArgs e)
        {
            CustomReply reply = new CustomReply();
            reply.RestoreTable(richTextBox_customReplyData.Text);
            reply.ShowDialog();
            richTextBox_customReplyData.Text = reply.tableData;
        }

    }
}
