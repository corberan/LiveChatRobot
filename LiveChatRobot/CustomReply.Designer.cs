namespace LiveChatRobot
{
    partial class CustomReply
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView_reply = new System.Windows.Forms.ListView();
            this.button_save = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_add = new System.Windows.Forms.Button();
            this.button_edit = new System.Windows.Forms.Button();
            this.button_del = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_reply
            // 
            this.listView_reply.Location = new System.Drawing.Point(12, 12);
            this.listView_reply.Name = "listView_reply";
            this.listView_reply.Size = new System.Drawing.Size(453, 210);
            this.listView_reply.TabIndex = 0;
            this.listView_reply.UseCompatibleStateImageBehavior = false;
            this.listView_reply.SelectedIndexChanged += new System.EventHandler(this.listView_reply_SelectedIndexChanged);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(309, 228);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 1;
            this.button_save.Text = "确定";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(390, 228);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "选中行进行操作";
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(472, 13);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 4;
            this.button_add.Text = "新增";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_edit
            // 
            this.button_edit.Location = new System.Drawing.Point(472, 43);
            this.button_edit.Name = "button_edit";
            this.button_edit.Size = new System.Drawing.Size(75, 23);
            this.button_edit.TabIndex = 5;
            this.button_edit.Text = "编辑";
            this.button_edit.UseVisualStyleBackColor = true;
            this.button_edit.Click += new System.EventHandler(this.button_edit_Click);
            // 
            // button_del
            // 
            this.button_del.Location = new System.Drawing.Point(472, 73);
            this.button_del.Name = "button_del";
            this.button_del.Size = new System.Drawing.Size(75, 23);
            this.button_del.TabIndex = 6;
            this.button_del.Text = "删除";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            // 
            // CustomReply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 261);
            this.Controls.Add(this.button_del);
            this.Controls.Add(this.button_edit);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.listView_reply);
            this.MaximizeBox = false;
            this.Name = "CustomReply";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自定义回复";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_reply;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_edit;
        private System.Windows.Forms.Button button_del;
    }
}