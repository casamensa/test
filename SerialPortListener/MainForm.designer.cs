namespace SerialPortListener
{
    partial class MainForm
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
            System.Windows.Forms.Label baudRateLabel;
            System.Windows.Forms.Label stopBitsLabel;
            System.Windows.Forms.Label dataBitsLabel;
            System.Windows.Forms.Label portNameLabel;
            System.Windows.Forms.Label parityLabel;
            this.btnStart = new System.Windows.Forms.Button();
            this.tbData = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.tbDataRx = new System.Windows.Forms.TextBox();
            this.badData = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSerial = new System.Windows.Forms.TabPage();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.baudRateComboBox = new System.Windows.Forms.ComboBox();
            this.stopBitsComboBox = new System.Windows.Forms.ComboBox();
            this.portNameComboBox = new System.Windows.Forms.ComboBox();
            this.dataBitsComboBox = new System.Windows.Forms.ComboBox();
            this.parityComboBox = new System.Windows.Forms.ComboBox();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.checkBoxLogging = new System.Windows.Forms.CheckBox();
            this.buttonSet = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.tabPageRanges = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxHoldHigh = new System.Windows.Forms.TextBox();
            this.textBoxHoldLow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCoilHigh = new System.Windows.Forms.TextBox();
            this.textBoxCoilLow = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageHex = new System.Windows.Forms.TabPage();
            this.tabPageCoils = new System.Windows.Forms.TabPage();
            this.richTextBoxCoils = new System.Windows.Forms.RichTextBox();
            this.tabPageHolding = new System.Windows.Forms.TabPage();
            this.richTextBoxHoldingRegister = new System.Windows.Forms.RichTextBox();
            this.serialSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            baudRateLabel = new System.Windows.Forms.Label();
            stopBitsLabel = new System.Windows.Forms.Label();
            dataBitsLabel = new System.Windows.Forms.Label();
            portNameLabel = new System.Windows.Forms.Label();
            parityLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageSerial.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.tabPageRanges.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageHex.SuspendLayout();
            this.tabPageCoils.SuspendLayout();
            this.tabPageHolding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serialSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // baudRateLabel
            // 
            baudRateLabel.AutoSize = true;
            baudRateLabel.Location = new System.Drawing.Point(14, 46);
            baudRateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            baudRateLabel.Name = "baudRateLabel";
            baudRateLabel.Size = new System.Drawing.Size(79, 17);
            baudRateLabel.TabIndex = 11;
            baudRateLabel.Text = "Baud Rate:";
            // 
            // stopBitsLabel
            // 
            stopBitsLabel.AutoSize = true;
            stopBitsLabel.Location = new System.Drawing.Point(14, 145);
            stopBitsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            stopBitsLabel.Name = "stopBitsLabel";
            stopBitsLabel.Size = new System.Drawing.Size(68, 17);
            stopBitsLabel.TabIndex = 19;
            stopBitsLabel.Text = "Stop Bits:";
            // 
            // dataBitsLabel
            // 
            dataBitsLabel.AutoSize = true;
            dataBitsLabel.Location = new System.Drawing.Point(14, 79);
            dataBitsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataBitsLabel.Name = "dataBitsLabel";
            dataBitsLabel.Size = new System.Drawing.Size(69, 17);
            dataBitsLabel.TabIndex = 13;
            dataBitsLabel.Text = "Data Bits:";
            // 
            // portNameLabel
            // 
            portNameLabel.AutoSize = true;
            portNameLabel.Location = new System.Drawing.Point(14, 12);
            portNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            portNameLabel.Name = "portNameLabel";
            portNameLabel.Size = new System.Drawing.Size(79, 17);
            portNameLabel.TabIndex = 17;
            portNameLabel.Text = "Port Name:";
            // 
            // parityLabel
            // 
            parityLabel.AutoSize = true;
            parityLabel.Location = new System.Drawing.Point(14, 112);
            parityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            parityLabel.Name = "parityLabel";
            parityLabel.Size = new System.Drawing.Size(48, 17);
            parityLabel.TabIndex = 15;
            parityLabel.Text = "Parity:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(390, 38);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(113, 28);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Start listening";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbData
            // 
            this.tbData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbData.Location = new System.Drawing.Point(13, 226);
            this.tbData.Margin = new System.Windows.Forms.Padding(4);
            this.tbData.Multiline = true;
            this.tbData.Name = "tbData";
            this.tbData.ReadOnly = true;
            this.tbData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbData.Size = new System.Drawing.Size(486, 547);
            this.tbData.TabIndex = 13;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(390, 78);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(113, 28);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Stop listening";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tbDataRx
            // 
            this.tbDataRx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDataRx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDataRx.Location = new System.Drawing.Point(510, 227);
            this.tbDataRx.Margin = new System.Windows.Forms.Padding(4);
            this.tbDataRx.Multiline = true;
            this.tbDataRx.Name = "tbDataRx";
            this.tbDataRx.ReadOnly = true;
            this.tbDataRx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbDataRx.Size = new System.Drawing.Size(491, 557);
            this.tbDataRx.TabIndex = 14;
            // 
            // badData
            // 
            this.badData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.badData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.badData.Location = new System.Drawing.Point(0, 0);
            this.badData.Margin = new System.Windows.Forms.Padding(4);
            this.badData.Multiline = true;
            this.badData.Name = "badData";
            this.badData.ReadOnly = true;
            this.badData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.badData.Size = new System.Drawing.Size(487, 182);
            this.badData.TabIndex = 15;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSerial);
            this.tabControl1.Controls.Add(this.tabPageOptions);
            this.tabControl1.Controls.Add(this.tabPageRanges);
            this.tabControl1.Location = new System.Drawing.Point(12, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(371, 210);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPageSerial
            // 
            this.tabPageSerial.Controls.Add(this.baudRateComboBox);
            this.tabPageSerial.Controls.Add(baudRateLabel);
            this.tabPageSerial.Controls.Add(this.stopBitsComboBox);
            this.tabPageSerial.Controls.Add(stopBitsLabel);
            this.tabPageSerial.Controls.Add(dataBitsLabel);
            this.tabPageSerial.Controls.Add(this.portNameComboBox);
            this.tabPageSerial.Controls.Add(this.dataBitsComboBox);
            this.tabPageSerial.Controls.Add(portNameLabel);
            this.tabPageSerial.Controls.Add(parityLabel);
            this.tabPageSerial.Controls.Add(this.parityComboBox);
            this.tabPageSerial.Location = new System.Drawing.Point(4, 25);
            this.tabPageSerial.Name = "tabPageSerial";
            this.tabPageSerial.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSerial.Size = new System.Drawing.Size(363, 181);
            this.tabPageSerial.TabIndex = 0;
            this.tabPageSerial.Text = "Serial Settings";
            this.tabPageSerial.UseVisualStyleBackColor = true;
            // 
            // buttonForward
            // 
            this.buttonForward.Location = new System.Drawing.Point(391, 142);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(113, 28);
            this.buttonForward.TabIndex = 22;
            this.buttonForward.Text = "Step Forward";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(390, 183);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(113, 28);
            this.buttonBack.TabIndex = 21;
            this.buttonBack.Text = "Step Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.button1_Click);
            // 
            // baudRateComboBox
            // 
            this.baudRateComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "BaudRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.baudRateComboBox.FormattingEnabled = true;
            this.baudRateComboBox.Location = new System.Drawing.Point(104, 42);
            this.baudRateComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.baudRateComboBox.Name = "baudRateComboBox";
            this.baudRateComboBox.Size = new System.Drawing.Size(160, 24);
            this.baudRateComboBox.TabIndex = 12;
            // 
            // stopBitsComboBox
            // 
            this.stopBitsComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "StopBits", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.stopBitsComboBox.FormattingEnabled = true;
            this.stopBitsComboBox.Location = new System.Drawing.Point(104, 142);
            this.stopBitsComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.stopBitsComboBox.Name = "stopBitsComboBox";
            this.stopBitsComboBox.Size = new System.Drawing.Size(160, 24);
            this.stopBitsComboBox.TabIndex = 20;
            // 
            // portNameComboBox
            // 
            this.portNameComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "PortName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.portNameComboBox.FormattingEnabled = true;
            this.portNameComboBox.Location = new System.Drawing.Point(101, 7);
            this.portNameComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.portNameComboBox.Name = "portNameComboBox";
            this.portNameComboBox.Size = new System.Drawing.Size(160, 24);
            this.portNameComboBox.TabIndex = 18;
            // 
            // dataBitsComboBox
            // 
            this.dataBitsComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "DataBits", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dataBitsComboBox.FormattingEnabled = true;
            this.dataBitsComboBox.Location = new System.Drawing.Point(104, 75);
            this.dataBitsComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.dataBitsComboBox.Name = "dataBitsComboBox";
            this.dataBitsComboBox.Size = new System.Drawing.Size(160, 24);
            this.dataBitsComboBox.TabIndex = 14;
            // 
            // parityComboBox
            // 
            this.parityComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "Parity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.parityComboBox.FormattingEnabled = true;
            this.parityComboBox.Location = new System.Drawing.Point(104, 108);
            this.parityComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.parityComboBox.Name = "parityComboBox";
            this.parityComboBox.Size = new System.Drawing.Size(160, 24);
            this.parityComboBox.TabIndex = 16;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.checkBoxLogging);
            this.tabPageOptions.Controls.Add(this.buttonSet);
            this.tabPageOptions.Controls.Add(this.textBoxPath);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 25);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptions.Size = new System.Drawing.Size(363, 181);
            this.tabPageOptions.TabIndex = 1;
            this.tabPageOptions.Text = "Options";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogging
            // 
            this.checkBoxLogging.AutoSize = true;
            this.checkBoxLogging.Location = new System.Drawing.Point(6, 47);
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.Size = new System.Drawing.Size(129, 21);
            this.checkBoxLogging.TabIndex = 3;
            this.checkBoxLogging.Text = "Enable Logging\r\n";
            this.checkBoxLogging.UseVisualStyleBackColor = true;
            this.checkBoxLogging.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // buttonSet
            // 
            this.buttonSet.Location = new System.Drawing.Point(266, 43);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(75, 23);
            this.buttonSet.TabIndex = 2;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(6, 13);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(335, 22);
            this.textBoxPath.TabIndex = 1;
            // 
            // tabPageRanges
            // 
            this.tabPageRanges.Controls.Add(this.label2);
            this.tabPageRanges.Controls.Add(this.textBoxHoldHigh);
            this.tabPageRanges.Controls.Add(this.textBoxHoldLow);
            this.tabPageRanges.Controls.Add(this.label1);
            this.tabPageRanges.Controls.Add(this.textBoxCoilHigh);
            this.tabPageRanges.Controls.Add(this.textBoxCoilLow);
            this.tabPageRanges.Location = new System.Drawing.Point(4, 25);
            this.tabPageRanges.Name = "tabPageRanges";
            this.tabPageRanges.Size = new System.Drawing.Size(363, 181);
            this.tabPageRanges.TabIndex = 2;
            this.tabPageRanges.Text = "Output Ranges";
            this.tabPageRanges.UseVisualStyleBackColor = true;
            this.tabPageRanges.Click += new System.EventHandler(this.tabPageRanges_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Holding Range";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBoxHoldHigh
            // 
            this.textBoxHoldHigh.Location = new System.Drawing.Point(178, 80);
            this.textBoxHoldHigh.Name = "textBoxHoldHigh";
            this.textBoxHoldHigh.Size = new System.Drawing.Size(37, 22);
            this.textBoxHoldHigh.TabIndex = 4;
            this.textBoxHoldHigh.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBoxHoldLow
            // 
            this.textBoxHoldLow.Location = new System.Drawing.Point(125, 80);
            this.textBoxHoldLow.Name = "textBoxHoldLow";
            this.textBoxHoldLow.Size = new System.Drawing.Size(37, 22);
            this.textBoxHoldLow.TabIndex = 3;
            this.textBoxHoldLow.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Coil Range";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxCoilHigh
            // 
            this.textBoxCoilHigh.Location = new System.Drawing.Point(178, 23);
            this.textBoxCoilHigh.Name = "textBoxCoilHigh";
            this.textBoxCoilHigh.Size = new System.Drawing.Size(37, 22);
            this.textBoxCoilHigh.TabIndex = 1;
            this.textBoxCoilHigh.TextChanged += new System.EventHandler(this.textBoxCoilHigh_TextChanged);
            // 
            // textBoxCoilLow
            // 
            this.textBoxCoilLow.Location = new System.Drawing.Point(125, 24);
            this.textBoxCoilLow.Name = "textBoxCoilLow";
            this.textBoxCoilLow.Size = new System.Drawing.Size(37, 22);
            this.textBoxCoilLow.TabIndex = 0;
            this.textBoxCoilLow.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageHex);
            this.tabControl2.Controls.Add(this.tabPageCoils);
            this.tabControl2.Controls.Add(this.tabPageHolding);
            this.tabControl2.Location = new System.Drawing.Point(510, 13);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(496, 207);
            this.tabControl2.TabIndex = 17;
            // 
            // tabPageHex
            // 
            this.tabPageHex.Controls.Add(this.badData);
            this.tabPageHex.Location = new System.Drawing.Point(4, 25);
            this.tabPageHex.Name = "tabPageHex";
            this.tabPageHex.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHex.Size = new System.Drawing.Size(488, 178);
            this.tabPageHex.TabIndex = 0;
            this.tabPageHex.Text = "HEX";
            this.tabPageHex.UseVisualStyleBackColor = true;
            // 
            // tabPageCoils
            // 
            this.tabPageCoils.Controls.Add(this.richTextBoxCoils);
            this.tabPageCoils.Location = new System.Drawing.Point(4, 25);
            this.tabPageCoils.Name = "tabPageCoils";
            this.tabPageCoils.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCoils.Size = new System.Drawing.Size(488, 178);
            this.tabPageCoils.TabIndex = 1;
            this.tabPageCoils.Text = "Coils";
            this.tabPageCoils.UseVisualStyleBackColor = true;
            // 
            // richTextBoxCoils
            // 
            this.richTextBoxCoils.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxCoils.Location = new System.Drawing.Point(-4, 0);
            this.richTextBoxCoils.Name = "richTextBoxCoils";
            this.richTextBoxCoils.Size = new System.Drawing.Size(496, 182);
            this.richTextBoxCoils.TabIndex = 0;
            this.richTextBoxCoils.Text = "";
            // 
            // tabPageHolding
            // 
            this.tabPageHolding.Controls.Add(this.richTextBoxHoldingRegister);
            this.tabPageHolding.Location = new System.Drawing.Point(4, 25);
            this.tabPageHolding.Name = "tabPageHolding";
            this.tabPageHolding.Size = new System.Drawing.Size(488, 178);
            this.tabPageHolding.TabIndex = 2;
            this.tabPageHolding.Text = "Holding Register";
            this.tabPageHolding.UseVisualStyleBackColor = true;
            // 
            // richTextBoxHoldingRegister
            // 
            this.richTextBoxHoldingRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxHoldingRegister.Location = new System.Drawing.Point(-9, -1);
            this.richTextBoxHoldingRegister.Name = "richTextBoxHoldingRegister";
            this.richTextBoxHoldingRegister.Size = new System.Drawing.Size(489, 177);
            this.richTextBoxHoldingRegister.TabIndex = 0;
            this.richTextBoxHoldingRegister.Text = "";
            // 
            // serialSettingsBindingSource
            // 
            this.serialSettingsBindingSource.DataSource = typeof(SerialPortListener.Serial.SerialSettings);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 797);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tbDataRx);
            this.Controls.Add(this.tbData);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Modbus Monitor";
            this.tabControl1.ResumeLayout(false);
            this.tabPageSerial.ResumeLayout(false);
            this.tabPageSerial.PerformLayout();
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
            this.tabPageRanges.ResumeLayout(false);
            this.tabPageRanges.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPageHex.ResumeLayout(false);
            this.tabPageHex.PerformLayout();
            this.tabPageCoils.ResumeLayout(false);
            this.tabPageHolding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serialSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource serialSettingsBindingSource;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbData;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox tbDataRx;
        private System.Windows.Forms.TextBox badData;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSerial;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.ComboBox stopBitsComboBox;
        private System.Windows.Forms.ComboBox portNameComboBox;
        private System.Windows.Forms.ComboBox dataBitsComboBox;
        private System.Windows.Forms.ComboBox parityComboBox;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.CheckBox checkBoxLogging;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageHex;
        private System.Windows.Forms.TabPage tabPageCoils;
        private System.Windows.Forms.RichTextBox richTextBoxCoils;
        private System.Windows.Forms.TabPage tabPageHolding;
        private System.Windows.Forms.RichTextBox richTextBoxHoldingRegister;
        private System.Windows.Forms.TabPage tabPageRanges;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCoilHigh;
        private System.Windows.Forms.TextBox textBoxCoilLow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxHoldHigh;
        private System.Windows.Forms.TextBox textBoxHoldLow;
    }
}

