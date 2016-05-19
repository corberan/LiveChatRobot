using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiveChatRobot
{
    public partial class CustomReplyEdit : Form
    {
        public string[] strs = {};

        public CustomReplyEdit()
        {
            InitializeComponent();
        }

        public void SetStrs(string contains, string removes, string replys)
        {
            richTextBox_contains.Text = contains;
            richTextBox_removes.Text = removes;
            richTextBox_replys.Text = replys;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            strs = new string[] { richTextBox_contains.Text, richTextBox_removes.Text, richTextBox_replys.Text };
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
