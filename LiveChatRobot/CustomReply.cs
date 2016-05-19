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
    public partial class CustomReply : Form
    {
        public string tableData = "";

        public CustomReply()
        {
            InitializeComponent();

            listView_reply.View = View.Details;
            listView_reply.CheckBoxes = true;
            listView_reply.FullRowSelect = true;
            //listView_reply.LabelEdit = true;
            listView_reply.GridLines = true;

            listView_reply.Columns.Add("生效", -2, HorizontalAlignment.Center);
            listView_reply.Columns.Add("包含", -2, HorizontalAlignment.Left);
            listView_reply.Columns.Add("不含", -2, HorizontalAlignment.Left);
            listView_reply.Columns.Add("回复", -2, HorizontalAlignment.Left);

        }

        public void RestoreTable(string data)
        {
            tableData = data;
            listView_reply.Items.Clear();
            string[] rows = tableData.Split('\n');
            foreach(string row in rows)
            {
                string[] columns = row.Split('\t');
                if (columns.Length == 4)
                {
                    ListViewItem item = new ListViewItem();
                    if (columns[0] == "true") item.Checked = true;
                    item.SubItems.Add(columns[1]);
                    item.SubItems.Add(columns[2]);
                    item.SubItems.Add(columns[3]);
                    listView_reply.Items.Add(item);
                }
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            CustomReplyEdit edit = new CustomReplyEdit();
            edit.ShowDialog();
            if (edit.strs.Length > 2)
            {
                ListViewItem item = new ListViewItem();
                item.Checked = true;
                item.SubItems.Add(edit.strs[0]);
                item.SubItems.Add(edit.strs[1]);
                item.SubItems.Add(edit.strs[2]);
                listView_reply.Items.Add(item);
            }
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            if (listView_reply.FocusedItem != null)
            {
                CustomReplyEdit edit = new CustomReplyEdit();
                ListViewItem foucsItem = listView_reply.FocusedItem;
                edit.SetStrs(foucsItem.SubItems[1].Text, foucsItem.SubItems[2].Text, foucsItem.SubItems[3].Text);
                edit.ShowDialog();
                if (edit.strs.Length > 2)
                {
                    foucsItem.SubItems[1].Text = edit.strs[0];
                    foucsItem.SubItems[2].Text = edit.strs[1];
                    foucsItem.SubItems[3].Text = edit.strs[2];
                }
            }
        }

        private void listView_reply_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button_del_Click(object sender, EventArgs e)
        {
            if (listView_reply.FocusedItem != null)
            {
                listView_reply.FocusedItem.Remove();
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            tableData = "";
            for(int i=0;i<listView_reply.Items.Count;i++)
            {
                if (listView_reply.Items[i].SubItems.Count == 4)
                {
                    tableData = tableData
                        + listView_reply.Items[i].Checked.ToString().ToLower() + '\t'
                        + listView_reply.Items[i].SubItems[1].Text + '\t'
                        + listView_reply.Items[i].SubItems[2].Text + '\t'
                        + listView_reply.Items[i].SubItems[3].Text + '\n';
                }
            }
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
