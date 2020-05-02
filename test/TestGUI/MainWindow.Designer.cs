namespace TestGUI
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.frameBox = new System.Windows.Forms.PictureBox();
            this.videoTimer = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.labAudioBuffer = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labAudioDelay = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labProcTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labDelay = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labBuffer = new System.Windows.Forms.Label();
            this.labJitter = new System.Windows.Forms.Label();
            this.imageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.frameBox)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // frameBox
            // 
            this.frameBox.BackColor = System.Drawing.Color.Black;
            this.frameBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frameBox.Location = new System.Drawing.Point(0, 0);
            this.frameBox.Name = "frameBox";
            this.frameBox.Size = new System.Drawing.Size(800, 450);
            this.frameBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.frameBox.TabIndex = 0;
            this.frameBox.TabStop = false;
            // 
            // videoTimer
            // 
            this.videoTimer.Enabled = true;
            this.videoTimer.Interval = 10;
            this.videoTimer.Tick += new System.EventHandler(this.videoTimer_Tick);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labAudioBuffer);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.labAudioDelay);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.labProcTime);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.labDelay);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.labTime);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.labBuffer);
            this.panel2.Controls.Add(this.labJitter);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 135);
            this.panel2.TabIndex = 4;
            // 
            // labAudioBuffer
            // 
            this.labAudioBuffer.AutoSize = true;
            this.labAudioBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAudioBuffer.ForeColor = System.Drawing.Color.White;
            this.labAudioBuffer.Location = new System.Drawing.Point(99, 107);
            this.labAudioBuffer.Name = "labAudioBuffer";
            this.labAudioBuffer.Size = new System.Drawing.Size(125, 17);
            this.labAudioBuffer.TabIndex = 14;
            this.labAudioBuffer.Text = "0000 ms | 000 kSa";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(3, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "Audio Buffer:";
            // 
            // labAudioDelay
            // 
            this.labAudioDelay.AutoSize = true;
            this.labAudioDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAudioDelay.ForeColor = System.Drawing.Color.White;
            this.labAudioDelay.Location = new System.Drawing.Point(99, 73);
            this.labAudioDelay.Name = "labAudioDelay";
            this.labAudioDelay.Size = new System.Drawing.Size(50, 17);
            this.labAudioDelay.TabIndex = 12;
            this.labAudioDelay.Text = "0.0 ms";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Audio Delay:";
            // 
            // labProcTime
            // 
            this.labProcTime.AutoSize = true;
            this.labProcTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProcTime.ForeColor = System.Drawing.Color.White;
            this.labProcTime.Location = new System.Drawing.Point(99, 56);
            this.labProcTime.Name = "labProcTime";
            this.labProcTime.Size = new System.Drawing.Size(50, 17);
            this.labProcTime.TabIndex = 10;
            this.labProcTime.Text = "0.0 ms";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Proc. Time:";
            // 
            // labDelay
            // 
            this.labDelay.AutoSize = true;
            this.labDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDelay.ForeColor = System.Drawing.Color.White;
            this.labDelay.Location = new System.Drawing.Point(99, 39);
            this.labDelay.Name = "labDelay";
            this.labDelay.Size = new System.Drawing.Size(50, 17);
            this.labDelay.TabIndex = 8;
            this.labDelay.Text = "0.0 ms";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Video Corr.:";
            // 
            // labTime
            // 
            this.labTime.AutoSize = true;
            this.labTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTime.ForeColor = System.Drawing.Color.White;
            this.labTime.Location = new System.Drawing.Point(99, 5);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(27, 17);
            this.labTime.TabIndex = 6;
            this.labTime.Text = "0 s";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(50, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Time:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(50, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Jitter:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Video Buffer:";
            // 
            // labBuffer
            // 
            this.labBuffer.AutoSize = true;
            this.labBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labBuffer.ForeColor = System.Drawing.Color.White;
            this.labBuffer.Location = new System.Drawing.Point(99, 90);
            this.labBuffer.Name = "labBuffer";
            this.labBuffer.Size = new System.Drawing.Size(90, 17);
            this.labBuffer.TabIndex = 3;
            this.labBuffer.Text = "000 ms | 0 Fr";
            // 
            // labJitter
            // 
            this.labJitter.AutoSize = true;
            this.labJitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labJitter.ForeColor = System.Drawing.Color.White;
            this.labJitter.Location = new System.Drawing.Point(99, 22);
            this.labJitter.Name = "labJitter";
            this.labJitter.Size = new System.Drawing.Size(50, 17);
            this.labJitter.TabIndex = 2;
            this.labJitter.Text = "0.0 ms";
            // 
            // imageBox
            // 
            this.imageBox.BackColor = System.Drawing.Color.Black;
            this.imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox.Location = new System.Drawing.Point(246, 12);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(135, 135);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox.TabIndex = 5;
            this.imageBox.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.imageBox);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.frameBox);
            this.Name = "MainWindow";
            this.Text = "UltraStar Core - Test GUI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.frameBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox frameBox;
        private System.Windows.Forms.Timer videoTimer;
        private System.Windows.Forms.Label labBuffer;
        private System.Windows.Forms.Label labJitter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labDelay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labProcTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labAudioDelay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labAudioBuffer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox imageBox;
    }
}

