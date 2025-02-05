namespace ConnectForPhone2._0
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StopConnect = new System.Windows.Forms.Button();
            this.PLC_Connect = new System.Windows.Forms.Button();
            this.Stop_Connect = new System.Windows.Forms.Button();
            this.ListBox = new System.Windows.Forms.ListBox();
            this.總計時 = new System.Windows.Forms.Timer(this.components);
            this.CleanListBox = new System.Windows.Forms.Button();
            this.Onlock = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StopConnect
            // 
            this.StopConnect.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.StopConnect.Location = new System.Drawing.Point(27, 12);
            this.StopConnect.Name = "StopConnect";
            this.StopConnect.Size = new System.Drawing.Size(193, 53);
            this.StopConnect.TabIndex = 0;
            this.StopConnect.Text = "程式停止";
            this.StopConnect.UseVisualStyleBackColor = true;
            this.StopConnect.Click += new System.EventHandler(this.StopConnect_Click);
            // 
            // PLC_Connect
            // 
            this.PLC_Connect.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.PLC_Connect.Location = new System.Drawing.Point(233, 12);
            this.PLC_Connect.Name = "PLC_Connect";
            this.PLC_Connect.Size = new System.Drawing.Size(197, 53);
            this.PLC_Connect.TabIndex = 1;
            this.PLC_Connect.Text = "PLC連線";
            this.PLC_Connect.UseVisualStyleBackColor = true;
            this.PLC_Connect.Click += new System.EventHandler(this.PLC_Connect_Click);
            // 
            // Stop_Connect
            // 
            this.Stop_Connect.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Stop_Connect.Location = new System.Drawing.Point(442, 12);
            this.Stop_Connect.Name = "Stop_Connect";
            this.Stop_Connect.Size = new System.Drawing.Size(241, 53);
            this.Stop_Connect.TabIndex = 2;
            this.Stop_Connect.Text = "PLC中斷連線";
            this.Stop_Connect.UseVisualStyleBackColor = true;
            this.Stop_Connect.Click += new System.EventHandler(this.Stop_Connect_Click);
            // 
            // ListBox
            // 
            this.ListBox.FormattingEnabled = true;
            this.ListBox.ItemHeight = 18;
            this.ListBox.Location = new System.Drawing.Point(27, 76);
            this.ListBox.Name = "ListBox";
            this.ListBox.Size = new System.Drawing.Size(685, 364);
            this.ListBox.TabIndex = 3;
            // 
            // 總計時
            // 
            this.總計時.Enabled = true;
            this.總計時.Interval = 1000;
            this.總計時.Tick += new System.EventHandler(this.總計時_Tick);
            // 
            // CleanListBox
            // 
            this.CleanListBox.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.CleanListBox.Location = new System.Drawing.Point(737, 76);
            this.CleanListBox.Name = "CleanListBox";
            this.CleanListBox.Size = new System.Drawing.Size(124, 53);
            this.CleanListBox.TabIndex = 4;
            this.CleanListBox.Text = "清空";
            this.CleanListBox.UseVisualStyleBackColor = true;
            this.CleanListBox.Click += new System.EventHandler(this.CleanListBox_Click);
            // 
            // Onlock
            // 
            this.Onlock.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Onlock.ForeColor = System.Drawing.Color.Red;
            this.Onlock.Location = new System.Drawing.Point(737, 12);
            this.Onlock.Name = "Onlock";
            this.Onlock.Size = new System.Drawing.Size(124, 53);
            this.Onlock.TabIndex = 5;
            this.Onlock.Text = "鎖定";
            this.Onlock.UseVisualStyleBackColor = true;
            this.Onlock.Visible = false;
            this.Onlock.Click += new System.EventHandler(this.Onlock_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 450);
            this.Controls.Add(this.Onlock);
            this.Controls.Add(this.CleanListBox);
            this.Controls.Add(this.ListBox);
            this.Controls.Add(this.Stop_Connect);
            this.Controls.Add(this.PLC_Connect);
            this.Controls.Add(this.StopConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StopConnect;
        private System.Windows.Forms.Button PLC_Connect;
        private System.Windows.Forms.Button Stop_Connect;
        private System.Windows.Forms.ListBox ListBox;
        private System.Windows.Forms.Timer 總計時;
        private System.Windows.Forms.Button CleanListBox;
        private System.Windows.Forms.Button Onlock;
    }
}

