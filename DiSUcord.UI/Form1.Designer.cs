using System.Windows.Forms;

namespace DiSUcord.UI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.IncomingMsgBox = new System.Windows.Forms.RichTextBox();
            this.SendMsgBox = new System.Windows.Forms.TextBox();
            this.SubBtn = new System.Windows.Forms.Button();
            this.UnsubBtn = new System.Windows.Forms.Button();
            this.SendMsgBtn = new System.Windows.Forms.Button();
            this.DisconnectBtn = new System.Windows.Forms.Button();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // IncomingMsgBox
            // 
            this.IncomingMsgBox.Location = new System.Drawing.Point(243, 54);
            this.IncomingMsgBox.Name = "IncomingMsgBox";
            this.IncomingMsgBox.Size = new System.Drawing.Size(378, 240);
            this.IncomingMsgBox.TabIndex = 0;
            this.IncomingMsgBox.Text = "";
            // 
            // SendMsgBox
            // 
            this.SendMsgBox.Location = new System.Drawing.Point(243, 325);
            this.SendMsgBox.Name = "SendMsgBox";
            this.SendMsgBox.Size = new System.Drawing.Size(287, 22);
            this.SendMsgBox.TabIndex = 1;
            this.SendMsgBox.TextChanged += new System.EventHandler(this.SendTxtBox_TextChanged);
            // 
            // SubBtn
            // 
            this.SubBtn.Location = new System.Drawing.Point(81, 118);
            this.SubBtn.Name = "SubBtn";
            this.SubBtn.Size = new System.Drawing.Size(107, 38);
            this.SubBtn.TabIndex = 2;
            this.SubBtn.Text = "Subscribe";
            this.SubBtn.UseVisualStyleBackColor = true;
            this.SubBtn.Click += new System.EventHandler(this.Subscribe_Click);
            // 
            // UnsubBtn
            // 
            this.UnsubBtn.Location = new System.Drawing.Point(81, 189);
            this.UnsubBtn.Name = "UnsubBtn";
            this.UnsubBtn.Size = new System.Drawing.Size(107, 38);
            this.UnsubBtn.TabIndex = 3;
            this.UnsubBtn.Text = "Unsubscribe";
            this.UnsubBtn.UseVisualStyleBackColor = true;
            this.UnsubBtn.Click += new System.EventHandler(this.UnsubBtn_Click);
            // 
            // SendMsgBtn
            // 
            this.SendMsgBtn.Location = new System.Drawing.Point(546, 325);
            this.SendMsgBtn.Name = "SendMsgBtn";
            this.SendMsgBtn.Size = new System.Drawing.Size(75, 23);
            this.SendMsgBtn.TabIndex = 4;
            this.SendMsgBtn.Text = "Send";
            this.SendMsgBtn.UseVisualStyleBackColor = true;
            this.SendMsgBtn.Click += new System.EventHandler(this.SendMsgBtn_Click);
            // 
            // DisconnectBtn
            // 
            this.DisconnectBtn.Location = new System.Drawing.Point(687, 189);
            this.DisconnectBtn.Name = "DisconnectBtn";
            this.DisconnectBtn.Size = new System.Drawing.Size(107, 38);
            this.DisconnectBtn.TabIndex = 5;
            this.DisconnectBtn.Text = "Disconnect";
            this.DisconnectBtn.UseVisualStyleBackColor = true;
            this.DisconnectBtn.Click += new System.EventHandler(this.DisconnectBtn_Click);
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Location = new System.Drawing.Point(687, 118);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(107, 38);
            this.ConnectBtn.TabIndex = 6;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(881, 442);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.DisconnectBtn);
            this.Controls.Add(this.SendMsgBtn);
            this.Controls.Add(this.UnsubBtn);
            this.Controls.Add(this.SubBtn);
            this.Controls.Add(this.SendMsgBox);
            this.Controls.Add(this.IncomingMsgBox);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     
        private RichTextBox IncomingMsgBox;
        private TextBox SendMsgBox;
        private Button SubBtn;
        private Button UnsubBtn;
        private Button SendMsgBtn;
        private Button DisconnectBtn;
        private Button ConnectBtn;
    }
}
