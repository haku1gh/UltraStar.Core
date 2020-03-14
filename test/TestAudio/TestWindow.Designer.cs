namespace TestAudio
{
    partial class TestWindow
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
            this.cbRecordingDevices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbRecordingName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbRecordingType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbRecordingIsDefault = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbRecordingInputs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbRecordingChannels = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbRecordingSampleRate = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbRecordingVolume = new System.Windows.Forms.TextBox();
            this.tbRecordingInputNames = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.butRecordingRefresh = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbRecordingDevices
            // 
            this.cbRecordingDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRecordingDevices.FormattingEnabled = true;
            this.cbRecordingDevices.Location = new System.Drawing.Point(130, 22);
            this.cbRecordingDevices.Name = "cbRecordingDevices";
            this.cbRecordingDevices.Size = new System.Drawing.Size(232, 21);
            this.cbRecordingDevices.TabIndex = 0;
            this.cbRecordingDevices.SelectedIndexChanged += new System.EventHandler(this.cbRecordingDevices_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Recording Devices";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbRecordingName);
            this.groupBox1.Controls.Add(this.tbRecordingInputNames);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbRecordingType);
            this.groupBox1.Controls.Add(this.tbRecordingVolume);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbRecordingIsDefault);
            this.groupBox1.Controls.Add(this.tbRecordingSampleRate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbRecordingInputs);
            this.groupBox1.Controls.Add(this.tbRecordingChannels);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(20, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 277);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recording Device Info";
            // 
            // tbRecordingName
            // 
            this.tbRecordingName.Location = new System.Drawing.Point(110, 23);
            this.tbRecordingName.Name = "tbRecordingName";
            this.tbRecordingName.ReadOnly = true;
            this.tbRecordingName.Size = new System.Drawing.Size(232, 20);
            this.tbRecordingName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRecordingType
            // 
            this.tbRecordingType.Location = new System.Drawing.Point(110, 49);
            this.tbRecordingType.Name = "tbRecordingType";
            this.tbRecordingType.ReadOnly = true;
            this.tbRecordingType.Size = new System.Drawing.Size(232, 20);
            this.tbRecordingType.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Is Default?";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRecordingIsDefault
            // 
            this.tbRecordingIsDefault.Location = new System.Drawing.Point(110, 75);
            this.tbRecordingIsDefault.Name = "tbRecordingIsDefault";
            this.tbRecordingIsDefault.ReadOnly = true;
            this.tbRecordingIsDefault.Size = new System.Drawing.Size(232, 20);
            this.tbRecordingIsDefault.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Number of Inputs";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRecordingInputs
            // 
            this.tbRecordingInputs.Location = new System.Drawing.Point(110, 101);
            this.tbRecordingInputs.Name = "tbRecordingInputs";
            this.tbRecordingInputs.ReadOnly = true;
            this.tbRecordingInputs.Size = new System.Drawing.Size(232, 20);
            this.tbRecordingInputs.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Channels";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRecordingChannels
            // 
            this.tbRecordingChannels.Location = new System.Drawing.Point(110, 188);
            this.tbRecordingChannels.Name = "tbRecordingChannels";
            this.tbRecordingChannels.ReadOnly = true;
            this.tbRecordingChannels.Size = new System.Drawing.Size(232, 20);
            this.tbRecordingChannels.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Sample Rate";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRecordingSampleRate
            // 
            this.tbRecordingSampleRate.Location = new System.Drawing.Point(110, 214);
            this.tbRecordingSampleRate.Name = "tbRecordingSampleRate";
            this.tbRecordingSampleRate.ReadOnly = true;
            this.tbRecordingSampleRate.Size = new System.Drawing.Size(232, 20);
            this.tbRecordingSampleRate.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "Volume";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRecordingVolume
            // 
            this.tbRecordingVolume.Location = new System.Drawing.Point(110, 240);
            this.tbRecordingVolume.Name = "tbRecordingVolume";
            this.tbRecordingVolume.ReadOnly = true;
            this.tbRecordingVolume.Size = new System.Drawing.Size(232, 20);
            this.tbRecordingVolume.TabIndex = 15;
            // 
            // tbRecordingInputNames
            // 
            this.tbRecordingInputNames.Location = new System.Drawing.Point(110, 127);
            this.tbRecordingInputNames.Multiline = true;
            this.tbRecordingInputNames.Name = "tbRecordingInputNames";
            this.tbRecordingInputNames.ReadOnly = true;
            this.tbRecordingInputNames.Size = new System.Drawing.Size(232, 55);
            this.tbRecordingInputNames.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "Input Names";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // butRecordingRefresh
            // 
            this.butRecordingRefresh.Location = new System.Drawing.Point(368, 21);
            this.butRecordingRefresh.Name = "butRecordingRefresh";
            this.butRecordingRefresh.Size = new System.Drawing.Size(75, 23);
            this.butRecordingRefresh.TabIndex = 3;
            this.butRecordingRefresh.Text = "Refresh";
            this.butRecordingRefresh.UseVisualStyleBackColor = true;
            this.butRecordingRefresh.Click += new System.EventHandler(this.butRecordingRefresh_Click);
            // 
            // TestWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.butRecordingRefresh);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRecordingDevices);
            this.Name = "TestWindow";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRecordingDevices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbRecordingName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbRecordingType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRecordingIsDefault;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbRecordingInputs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbRecordingChannels;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbRecordingSampleRate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbRecordingVolume;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbRecordingInputNames;
        private System.Windows.Forms.Button butRecordingRefresh;
    }
}

