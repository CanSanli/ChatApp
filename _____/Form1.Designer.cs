
namespace ChatApp
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbPersons = new System.Windows.Forms.ListBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.tbNick = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // lbPersons
            // 
            this.lbPersons.FormattingEnabled = true;
            this.lbPersons.Location = new System.Drawing.Point(12, 37);
            this.lbPersons.Name = "lbPersons";
            this.lbPersons.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbPersons.Size = new System.Drawing.Size(197, 342);
            this.lbPersons.TabIndex = 13;
            this.lbPersons.SelectedIndexChanged += new System.EventHandler(this.lbPersons_SelectedIndexChanged);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSend.Location = new System.Drawing.Point(538, 310);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(93, 70);
            this.btnSend.TabIndex = 12;
            this.btnSend.Text = "Gönder";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(215, 310);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(317, 70);
            this.tbMessage.TabIndex = 11;
            // 
            // rtbMessages
            // 
            this.rtbMessages.AcceptsTab = true;
            this.rtbMessages.Location = new System.Drawing.Point(215, 13);
            this.rtbMessages.MaximumSize = new System.Drawing.Size(416, 291);
            this.rtbMessages.MinimumSize = new System.Drawing.Size(416, 291);
            this.rtbMessages.Name = "rtbMessages";
            this.rtbMessages.ReadOnly = true;
            this.rtbMessages.RightMargin = 1;
            this.rtbMessages.Size = new System.Drawing.Size(416, 291);
            this.rtbMessages.TabIndex = 10;
            this.rtbMessages.Text = "";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(145, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 20);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Tamam";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tbNick
            // 
            this.tbNick.Location = new System.Drawing.Point(12, 12);
            this.tbNick.Name = "tbNick";
            this.tbNick.Size = new System.Drawing.Size(127, 20);
            this.tbNick.TabIndex = 8;
            this.tbNick.Text = "Kullanıcı adı...";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "greenDot.png");
            this.imageList1.Images.SetKeyName(1, "redDot.png");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 388);
            this.Controls.Add(this.lbPersons);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.rtbMessages);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbNick);
            this.Name = "Form1";
            this.Text = "UdpChat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbPersons;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox tbNick;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

