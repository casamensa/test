﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SerialPortListener.Serial;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace SerialPortListener
{
    public partial class MainForm : Form
    {
        SerialPortManager _spManager;

        Stopwatch sw = new Stopwatch();

        string returnCode;
        string previousSlaveId = "999";
        string previousFunctionCode = "999";
        string filename = "c:\\test.log";
       
        StringBuilder previousMessage = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        StringBuilder outText = new StringBuilder();

       
        StringBuilder[] messageHistory = new StringBuilder[150];
        

        int serialCount = 0;
        int rxCRC1 = 0;
        int rxCRC2 = 0;
        int messageHistoryCount;
        int replayCount;

        bool response;
        
        bool RX = false;

        public MainForm()
        {
            InitializeComponent();

            for (int i = 0; i < messageHistory.Length; i++)
                messageHistory[i] = new StringBuilder();

            UserInitialization();



            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }


        private void UserInitialization()
        {
            _spManager = new SerialPortManager();
            SerialSettings mySerialSettings = _spManager.CurrentSerialSettings;
            serialSettingsBindingSource.DataSource = mySerialSettings;
            portNameComboBox.DataSource = mySerialSettings.PortNameCollection;
            baudRateComboBox.DataSource = mySerialSettings.BaudRateCollection;
            dataBitsComboBox.DataSource = mySerialSettings.DataBitsCollection;
            parityComboBox.DataSource = Enum.GetValues(typeof(System.IO.Ports.Parity));
            stopBitsComboBox.DataSource = Enum.GetValues(typeof(System.IO.Ports.StopBits));

            _spManager.NewSerialDataRecieved += new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved);
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            baudRateComboBox.SelectedIndex = 11; // doesnt work for some reason
            
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _spManager.Dispose();
        }

        void _spManager_NewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            
            
            if (this.InvokeRequired)
            {
                // Using this.Invoke causes deadlock when closing serial port, and BeginInvoke is good practice anyway.
                this.BeginInvoke(new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved), new object[] { sender, e });
                return;
            }

            sb.Clear();
            outText.Clear();

          

            //StringBuilder sb = new StringBuilder(); // Now declared fo full scope

            for (int i = 0; i < e.Data.Length; i++)
                sb.AppendFormat("{0:X2} \n", e.Data[i]);

            badData.AppendText(sb.ToString());
           

            response = CheckResponse(e.Data);

            if (messageHistoryCount < 149)
            {
                //badData.AppendText("stored: " + sb.ToString());
                messageHistory[messageHistoryCount].Append(sb.ToString());
                messageHistoryCount++;

               
            }
            else
            {
                messageHistoryCount = 0;
            }


           

            if (response && serialCount > 4)
            {
                decodeModbus(sb);
            }
            else // CRC check has failed then do this
            {
                badData.Font = new Font("Serif", 12, FontStyle.Bold);
                badData.AppendText("Error Detected \n");
                badData.Font = new Font("Serif", 12, FontStyle.Regular);
                decodeModbus(sb);
            
            }

            serialCount++;

           
        }

        private void decodeModbus(StringBuilder sb)
        {


            int startAddressHigh = 0;
            int startAddressLow = 0;
            int numberHigh = 0;
            int numberLow = 0;
            int startAddress = 0;
            int range = 0;
            int endAddress = 0;

            string[] words = sb.ToString().Split(' ');

            if (_spManager.getRX())
            {
               
                RX = true;
            }
            else
            {
                RX = false;
                previousFunctionCode = words[1];
            }

            //tbDataRx.AppendText(rxData.ToString());

            rxCRC1 = words.Length - 3;
            rxCRC2 = words.Length - 2;

            for (int i = 0; i < words.Length; i++)
            {

                switch (i) // Decode dependant on which part of the message is being decoded
                {

                    case 0:
                        

                        if (Convert.ToInt32(words[i], 16)==136)
                        {
                            tbData.Text = ("Bad Data - Check Polarity");
                            tbDataRx.Text = ("Bad Data - Check Polarity");
                            badData.Text = ("Bad Data - Check Polarity");
                            outText.Append("Bad Data - Check Polarity \n");
                        }

                        else if (RX)
                        {
                            

                            tbDataRx.Text = ("Slave Response \n");
                            int value = Convert.ToInt32(words[i], 16);
                            tbDataRx.AppendText("Slave Address:" + value.ToString() + "\n");
                            tbDataRx.AppendText("Calculated CRC: " + response + "\n");
                            previousSlaveId = " ";

                            outText.Append("Slave Response \n");
                            outText.Append("Slave Address:" + value.ToString() + "\n");
                            outText.Append("Calculated CRC: " + response + "\n");
                            
                        }
                        else
                        {
                            
                            tbData.Text = ("Master Request \n");
                            previousSlaveId = words[i];
                            tbData.AppendText("Slave Address: ");
                            int value = Convert.ToInt32(words[i], 16);
                            tbData.AppendText(value.ToString() + "\n");
                            tbData.AppendText("Calculated CRC: " + response + "\n");

                            outText.Append("Master Request \n");
                            outText.Append("Slave Address:" + value.ToString() + "\n");
                            outText.Append("Calculated CRC: " + response + "\n");

                        }

                        break;

                    case 1:
                        

                        getFunctionCode(words[i]);
                        if (RX)
                        {
                            tbDataRx.AppendText(returnCode + "\n");
                            outText.Append(returnCode + "\n");
                        }
                        else
                        {
                            tbData.AppendText(returnCode + "\n");
                            outText.Append(returnCode + "\n");
                        }

                        break;

                    case 2:
                        try
                        {
                            startAddressHigh = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                        }
                        catch
                        {
                            numberHigh = 999;
                        }
                        if (RX)
                        {
                            //tbDataRx.AppendText("Data High: ");
                            //tbDataRx.AppendText(startAddressHigh.ToString() + "\n");
                        }
                        else
                        {
                            tbData.AppendText("Start Address High: ");
                            tbData.AppendText(startAddressHigh.ToString() + "\n");

                            outText.Append("Start Address High: " + startAddressHigh.ToString() + "\n");
                        }

                        break;

                    case 3:
                        try
                        {
                            startAddressLow = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                        }
                        catch
                        {
                            badData.Text = ("Warning \n");
                            badData.Font = new Font("Serif", 24, FontStyle.Bold);
                            badData.AppendText("Comm port speed error \n");
                            Thread.Sleep(500);
                            tbData.AppendText("Comm port speed error \n");
                            tbDataRx.AppendText("Possible comm speed error \n");
                            badData.Font = new Font("Serif", 8, FontStyle.Regular);

                        }

                        if (RX)
                        {
                            //tbDataRx.AppendText("Data Low: ");
                           // tbDataRx.AppendText(startAddressLow.ToString() + "\n");
                        }
                        else
                        {
                            tbData.AppendText("Start Addess Low: ");
                            tbData.AppendText(startAddressLow.ToString() + "\n");

                            outText.Append("Start Addess Low: " + startAddressLow.ToString() + "\n");
                        }

                        break;

                    case 4:
                        try
                        {
                            numberHigh = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                        }
                        catch
                        {
                            numberHigh = 999;
                        }
                        if (RX)
                        {
                            //tbDataRx.AppendText("Data High: ");
                           // tbDataRx.AppendText(numberHigh.ToString() + "\n");
                        }
                        else
                        {
                            tbData.AppendText("Number High: ");
                            tbData.AppendText(numberHigh.ToString() + "\n");

                            outText.Append("Number High: "+ numberHigh.ToString() + "\n");
                        }

                        break;

                    case 5:
                        try
                        {
                            numberLow = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                        }
                        catch
                        {
                            numberLow = 999;
                        }
                        if (RX)
                        {
                           // tbDataRx.AppendText("Data Low: ");
                           // tbDataRx.AppendText(numberLow.ToString() + "\n");


                            startAddress = (startAddressHigh * 256) + (startAddressLow + 1);
                            range = (numberHigh * 256) + (numberLow);
                            endAddress = startAddress + range;

                            //tbDataRx.AppendText("\n Start of Data : " + startAddress + "\n");
                            // tbDataRx.AppendText("\n End of Data : " + endAddress + "\n");
                        }
                        else
                        {
                            tbData.AppendText("Number Low: ");
                            tbData.AppendText(numberLow.ToString() + "\n");


                            startAddress = (startAddressHigh * 256) + (startAddressLow + 1);
                            range = (numberHigh * 256) + (numberLow);
                            endAddress = startAddress + range;

                            tbData.AppendText("Start of Data : " + startAddress + "\n");
                            tbData.AppendText("End of Data : " + endAddress + "\n");


                            outText.Append("Number Low: "+ numberLow.ToString() + "\n");
                            outText.Append("\n Start of Data: " + startAddress + "\n");
                            outText.Append("\n End of Data : " + endAddress + "\n");
                        }
                        break;


                }

               

                if (i == rxCRC1)
                {
                    if (RX)
                    {
                        tbDataRx.AppendText("CRC: ");
                        tbDataRx.AppendText(words[i] + "\n");
                    }
                    else
                    {
                        tbData.AppendText("CRC: ");
                        tbData.AppendText(words[i] + "\n");
                    }
                }

                if (i == rxCRC2)
                {
                    if (RX)
                    {
                        tbDataRx.AppendText("CRC: ");
                        tbDataRx.AppendText(words[i] + "\n");
                    }
                    else
                    {
                        tbData.AppendText("CRC: ");
                        tbData.AppendText(words[i] + "\n");
                    }
                }

                if (RX && i >= 3 && i < words.Length - 3)
                {
                    try
                    {
                        numberHigh = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                    }
                    catch
                    {
                        numberHigh = 999;
                    }

                    string binary = Convert.ToString(numberHigh, 2);

                    tbDataRx.AppendText("Data: ");
                    tbDataRx.AppendText(numberHigh.ToString() + "  Binary: "+binary+"\n");

                    outText.Append("Data: "+numberHigh.ToString() + "  Binary: " + binary + "\n");

                }

            }

            tbData.AppendText("\n");
            tbData.ScrollToCaret();

            tbDataRx.AppendText("\n");
            tbDataRx.ScrollToCaret();

            if (checkBoxLogging.Checked)
            {
                writeToFile(outText);
                writeToFile(sb);
            }

        }

        private void GetCRC(byte[] message, ref byte[] CRC) // Check the CRC, accessed from CheckResponse method
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }



        private bool CheckResponse(byte[] response) // Check the CRC
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            try
            {
                if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private byte[] GetBytes(string str) // Returns a byte array from a string
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void getFunctionCode(string code) // Set the returncode to decode the function
        {
            int num;

            try
            {
                // Convert hex string to int
                num = Int32.Parse(code, System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                num = 9;
            }

            switch (num)
            {
                case 1:
                    returnCode = "Read Coil Status ";
                    break;

                case 2:
                    returnCode = "Read Input Status ";
                    break;

                case 3:
                    returnCode = "Read Holding Registers ";
                    break;

                case 4:
                    returnCode = "Read Input Registers ";
                    break;

                case 5:
                    returnCode = "Force Single Coil ";
                    break;

                case 6:
                    returnCode = "Preset Single Register ";
                    break;

                case 9:
                    returnCode = "Error Function Code";
                    break;

                default:
                    returnCode = "Unknown Function";
                    break;

            }


        }

        private void writeToFile(StringBuilder sb)
        {
            try {
                System.IO.StreamWriter file = new System.IO.StreamWriter(filename, true);
                file.WriteLine(sb.ToString() + "\n\n");

                file.Close();
            }
            catch (System.UnauthorizedAccessException e)
            {
                badData.Text = "Error writing data to file: " + e.GetBaseException();
            }
        }

        // Handles the "Start Listening"-buttom click event
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            buttonBack.Enabled = false;
            buttonForward.Enabled = false;

            serialCount = 0;
            _spManager.StartListening();
        }

        // Handles the "Stop Listening"-buttom click event
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            buttonBack.Enabled = true;
            buttonForward.Enabled = true;

            replayCount = messageHistoryCount;
            sb.Clear();
            previousFunctionCode = "999";
            
            _spManager.StopListening();
        }

        private void portNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
                textBoxPath.Text = filename;
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                if (replayCount >= 0)
                {
                    tbData.Text = replayCount.ToString();
                    badData.Text = messageHistory[replayCount].ToString();
                    //decodeModbus(messageHistory[replayCount]);
                    replayCount--;
                }
                else
                {
                    replayCount = 149;
                }
            }
            catch
            {
                tbData.Text = "End of recorded data";
            }
            
            
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (replayCount <= 149)
                {
                    replayCount++;
                    tbData.Text = replayCount.ToString();
                    badData.Text = messageHistory[replayCount].ToString();
                    //decodeModbus(messageHistory[replayCount]);
                   
                }
                else
                {
                    replayCount = 0;
                }
            }
            catch
            {
                tbData.Text = "End of recorded data";
            }
        }
    }
}
