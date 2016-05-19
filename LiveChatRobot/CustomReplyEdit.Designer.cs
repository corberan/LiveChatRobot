namespace LiveChatRobot
{
    partial class CustomReplyEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_contains = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox_removes = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox_replys = new System.Windows.Forms.RichTextBox();
            this.button_save = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "弹幕包含这些关键词：";
            // 
            // richTextBox_contains
            // 
            this.richTextBox_contains.Location = new System.Drawing.Point(15, 29);
            this.richTextBox_contains.Name = "richTextBox_contains";
            this.richTextBox_contains.Size = new System.Drawing.Size(215, 58);
            this.richTextBox_contains.TabIndex = 1;
            this.richTextBox_contains.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "同时不含这些关键词：";
            // 
            // richTextBox_removes
            // 
            this.richTextBox_removes.Location = new System.Drawing.Point(15, 105);
            this.richTextBox_removes.Name = "richTextBox_removes";
            this.richTextBox_removes.Size = new System.Drawing.Size(215, 58);
            this.richTextBox_removes.TabIndex = 3;
            this.richTextBox_removes.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "则回复内容：";
            // 
            // richTextBox_replys
            // 
            this.richTextBox_replys.Location = new System.Drawing.Point(15, 183);
            this.richTextBox_replys.Name = "richTextBox_replys";
            this.richTextBox_replys.Size = new System.Drawing.Size(215, 58);
            this.richTextBox_replys.TabIndex = 5;
            this.richTextBox_replys.Text = "";
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(74, 268);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 6;
            this.button_save.Text = "确定";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(155, 268);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 7;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "*关键词间用中文逗号隔开";
            // 
            // CustomReplyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 297);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.richTextBox_replys);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox_removes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox_contains);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "CustomReplyEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_contains;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox_removes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox_replys;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label4;
    }
}