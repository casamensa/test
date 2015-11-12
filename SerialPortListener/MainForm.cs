using System;
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
        string filename = "c:\\test.log";

        StringBuilder previousMessage = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        StringBuilder outText = new StringBuilder();
        StringBuilder holdingRegisterText = new StringBuilder();

        StringBuilder tbDataSB = new StringBuilder();
        StringBuilder tbDataRXSB = new StringBuilder();
        StringBuilder badDataSB = new StringBuilder();
        StringBuilder coilsText = new StringBuilder();
        StringBuilder holdingRegister = new StringBuilder();


        StringBuilder[] messageHistory = new StringBuilder[150];


        Thread holdingThread = new Thread(new ThreadStart(WorkThreadFunction));

        bool[] rxHistory = new bool[150];


        int serialCount = 0;
        int rxCRC1 = 0;
        int rxCRC2 = 0;
        int messageHistoryCount;
        int replayCount;
        int coilCount = 1;
        int holdingRegisterCount = 1;

        bool response;
        bool replay = false;
        bool RX = false;
        bool replayRX;

        public MainForm()
        {
            InitializeComponent();

            for (int i = 0; i < messageHistory.Length; i++)
                messageHistory[i] = new StringBuilder();

            UserInitialization();



            btnStart.Enabled = true;
            btnStop.Enabled = false;
            buttonBack.Enabled = false;
            buttonForward.Enabled = false;



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


            mySerialSettings.BaudRate = 9600;

        }

        public static void WorkThreadFunction()
        {
            try
            {
                // do any background work
            }
            catch (Exception ex)
            {
                // log errors
            }
        }

        private void updateGUI()
        {

            string[] wordtbData = tbDataSB.ToString().Split(' ');
            string[] wordtbDataRx = tbDataRXSB.ToString().Split(' ');
            string[] wordbadData = badDataSB.ToString().Split(' ');
            string[] wordCoils = coilsText.ToString().Split(' ');



            for (int i = 0; i < wordtbData.Length; i++)
            {
                if (wordtbData[i].Contains("Master"))
                {
                    tbData.Text = "";

                }
                tbData.AppendText(wordtbData[i]);
            }
            for (int i = 0; i < wordtbDataRx.Length; i++)
            {
                if (wordtbDataRx[i].Contains("Response"))
                {
                    tbDataRx.Text = "";
                }
                tbDataRx.AppendText(wordtbDataRx[i]);
            }
            for (int i = 0; i < wordbadData.Length; i++)
            {
                badData.AppendText(wordbadData[i]);
            }
            for (int i = 0; i < wordCoils.Length; i++)
            {


                richTextBoxCoils.AppendText(wordCoils[i]);
            }

            badData.AppendText(RX.ToString());
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

            RX = _spManager.getRX();

            sb.Clear();
            outText.Clear();
            holdingRegisterText.Clear();

            badDataSB.Clear();
            richTextBoxCoils.Text = "";

            for (int i = 0; i < e.Data.Length; i++)
                sb.AppendFormat("{0:X2} \n", e.Data[i]);

            //badData.AppendText(sb.ToString());
            badDataSB.Append(sb.ToString());

            response = CheckResponse(e.Data);

            if (messageHistoryCount < 149)
            {
                //badData.AppendText("stored: " + sb.ToString());
                messageHistory[messageHistoryCount].Append(sb.ToString());


                messageHistoryCount++;

            }
            else
            {

                for (int i = 0; i < messageHistory.Length; i++)
                {
                    messageHistory[i].Clear();

                }
                messageHistoryCount = 0;

            }


            if (response && serialCount > 4)
            {
                //decodeModbus(sb);

                Thread thread = new Thread(() => decodeModbus(sb));

                thread.Start();
                updateGUI();
            }
            else // CRC check has failed then do this
            {


                badDataSB.Append("Error Detected \n");

                Thread thread = new Thread(() => decodeModbus(sb));

                thread.Start();
                updateGUI();
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

            coilCount = 1;
            holdingRegisterCount = 1;

            string[] words = sb.ToString().Split(' ');

            if (!replay)
            {
                if (_spManager.getRX())
                {
                    tbDataRXSB.Clear();


                    
                }
                else
                {
                   
                    tbDataSB.Clear();

                }
            }
            else
            {
                RX = replayRX;
            }



            rxCRC1 = words.Length - 3;
            rxCRC2 = words.Length - 2;

            for (int i = 0; i < words.Length; i++)
            {

                switch (i) // Decode dependant on which part of the message is being decoded
                {

                    case 0:


                        if (Convert.ToInt32(words[i], 16) == 136)
                        {
                            // tbData.Text = ("Bad Data - Check Polarity");
                            //tbDataRx.Text = ("Bad Data - Check Polarity");
                            // badData.Text = ("Bad Data - Check Polarity");

                            tbDataSB.Append("Bad Data - Check Polarity");
                            tbDataRXSB.Append("Bad Data - Check Polarity");
                            badDataSB.Append("Bad Data - Check Polarity");

                            outText.Append("Bad Data - Check Polarity \n");
                        }

                        else if (_spManager.getRX())
                        {


                            //tbDataRx.Text = ("Slave Response \n");
                            tbDataRXSB.Append("Slave Response \n");

                            int value = Convert.ToInt32(words[i], 16);

                            //tbDataRx.AppendText("Slave Address:" + value.ToString() + "\n");
                            tbDataRXSB.Append(" Slave Address:" + value.ToString() + "\n");

                            previousSlaveId = " ";

                            outText.Append(" Slave Response \n");
                            outText.Append(" Slave Address:" + value.ToString() + "\n");

                            if (response)
                            {
                                //tbDataRx.AppendText("Calculated CRC: Checksum Valid" + "\n" + Environment.NewLine + Environment.NewLine);
                                tbDataRXSB.Append(" Calculated CRC: Checksum Valid" + "\n" + Environment.NewLine + Environment.NewLine);
                            }
                            else
                            {
                                //tbDataRx.AppendText("Calculated CRC: Error in Checksum" + "\n" + Environment.NewLine + Environment.NewLine);
                                tbDataRXSB.Append(" Calculated CRC: Error in Checksum" + "\n" + Environment.NewLine + Environment.NewLine);

                            }

                        }
                        else
                        {

                            //tbData.Text = ("Master Request \n");
                            tbDataSB.Append("Master Request \n");

                            previousSlaveId = words[i];

                            //tbData.AppendText("Slave Address: ");
                            tbDataSB.Append(" Slave Address:");

                            int value = Convert.ToInt32(words[i], 16);

                            //tbData.AppendText(value.ToString() + "\n");
                            tbDataSB.Append(value.ToString() + "\n");


                            outText.Append("Master Request \n");
                            outText.Append(" Slave Address:" + value.ToString() + "\n");

                            if (response)
                            {
                                //tbData.AppendText("Calculated CRC: Checksum Valid" + "\n" + Environment.NewLine + Environment.NewLine);
                                tbDataSB.Append(" Calculated CRC: Checksum Valid" + "\n" + Environment.NewLine + Environment.NewLine);
                            }
                            else
                            {
                                //tbData.AppendText("Calculated CRC: Error in Checksum" + "\n" + Environment.NewLine + Environment.NewLine);
                                tbDataSB.Append(" Calculated CRC: Error in Checksum" + "\n" + Environment.NewLine + Environment.NewLine);
                            }

                        }

                        break;

                    case 1:


                        getFunctionCode(words[i]);
                        if (_spManager.getRX())
                        {
                            //tbDataRx.AppendText(returnCode + "\n" + Environment.NewLine + Environment.NewLine);
                            tbDataRXSB.Append(returnCode + "\n" + Environment.NewLine + Environment.NewLine);
                            outText.Append(returnCode + "\n");
                        }
                        else
                        {
                            //tbData.AppendText(returnCode + "\n" + Environment.NewLine + Environment.NewLine);
                            tbDataSB.Append(returnCode + "\n" + Environment.NewLine + Environment.NewLine);
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
                        if (_spManager.getRX())
                        {

                        }
                        else
                        {
                            //tbData.AppendText("Start Address High: ");
                            tbDataSB.Append(" Start Address High:");

                            //tbData.AppendText(startAddressHigh.ToString() + "\n");
                            tbDataSB.Append(startAddressHigh.ToString() + "\n");

                            outText.Append(" Start Address High:" + startAddressHigh.ToString() + "\n");
                        }

                        break;

                    case 3:
                        try
                        {
                            startAddressLow = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                        }
                        catch
                        {
                            //badData.Text = ("Warning \n");
                            badDataSB.Append(" Warning \n");

                            //badData.Font = new Font("Serif", 24, FontStyle.Bold);

                            // badData.AppendText("Comm port speed error \n");
                            badDataSB.Append(" Comm port speed error \n");

                            Thread.Sleep(500);

                            //tbData.AppendText("Comm port speed error \n");
                            tbDataSB.Append(" Comm port speed error \n");

                            //tbDataRx.AppendText("Possible comm speed error \n");
                            tbDataRXSB.Append(" Possible comm speed error \n");


                            //badData.Font = new Font("Serif", 8, FontStyle.Regular);

                        }

                        if (_spManager.getRX())
                        {
                            //tbDataRx.AppendText("Data Low: ");
                            // tbDataRx.AppendText(startAddressLow.ToString() + "\n");
                        }
                        else
                        {
                            //tbData.AppendText("Start Addess Low: ");
                            tbDataSB.Append(" Start Addess Low:");

                            //tbData.AppendText(startAddressLow.ToString() + "\n");
                            tbDataSB.Append(startAddressLow.ToString() + "\n");

                            outText.Append(" Start Addess Low:" + startAddressLow.ToString() + "\n");
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
                        if (_spManager.getRX())
                        {
                            //tbDataRx.AppendText("Data High: ");
                            // tbDataRx.AppendText(numberHigh.ToString() + "\n");
                        }
                        else
                        {
                            //tbData.AppendText("Number High: ");
                            tbDataSB.Append(" Number High:");

                            //tbData.AppendText(numberHigh.ToString() + "\n");
                            tbDataSB.Append(numberHigh.ToString() + "\n");

                            outText.Append(" Number High:" + numberHigh.ToString() + "\n");
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
                        if (_spManager.getRX())
                        {

                            startAddress = (startAddressHigh * 256) + (startAddressLow + 1);
                            range = (numberHigh * 256) + (numberLow);
                            endAddress = startAddress + range;


                        }
                        else
                        {
                            //tbData.AppendText("Number Low: ");
                            tbDataSB.Append(" Number Low:");

                            //tbData.AppendText(numberLow.ToString() + "\n");
                            tbDataSB.Append(numberLow.ToString() + "\n");

                            startAddress = (startAddressHigh * 256) + (startAddressLow + 1);
                            range = (numberHigh * 256) + (numberLow);
                            endAddress = startAddress + range;

                            //tbData.AppendText("Start of Data : " + startAddress + "\n");
                            tbDataSB.Append(" Start of Data:" + startAddress + "\n");

                            //tbData.AppendText("End of Data : " + endAddress + "\n");
                            tbDataSB.Append(" End of Data:" + endAddress + "\n");

                            outText.Append(" Number Low:" + numberLow.ToString() + "\n");
                            outText.Append(" Start of Data:" + startAddress + "\n");
                            outText.Append(" End of Data :" + endAddress + "\n");
                        }
                        break;


                }



                if (i == rxCRC1)
                {
                    if (_spManager.getRX())
                    {
                        //tbDataRx.AppendText("CRC: ");
                        tbDataRXSB.Append(" CRC:");

                        //tbDataRx.AppendText(words[i] + "\n");
                        tbDataRXSB.Append(words[i] + "\n");
                    }
                    else
                    {
                        //tbData.AppendText("CRC: ");
                        tbDataSB.Append(" CRC:");

                        //tbData.AppendText(words[i] + "\n");
                        tbDataSB.Append(words[i] + "\n");
                    }
                }

                if (i == rxCRC2)
                {
                    if (_spManager.getRX())
                    {
                        //tbDataRx.AppendText("CRC: ");
                        tbDataRXSB.Append(" CRC:");

                        //tbDataRx.AppendText(words[i] + "\n");
                        tbDataRXSB.Append(words[i] + "\n");
                    }
                    else
                    {
                        //  tbData.AppendText("CRC: ");
                        tbDataSB.Append(" CRC:");

                        //tbData.AppendText(words[i] + "\n");
                        tbDataSB.Append(words[i] + "\n");
                    }
                }

                if (_spManager.getRX() && i >= 2 && i < words.Length - 3)
                {
                    try
                    {
                        numberHigh = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                    }
                    catch
                    {
                        numberHigh = 999;
                    }

                    string binary = Convert.ToString(numberHigh, 2).PadLeft(8, '0');



                    if (i == 2)
                    {
                        //tbDataRx.AppendText("No. Bytes: ");
                        tbDataRXSB.Append(" No.Bytes:");
                        //tbDataRx.AppendText(numberHigh.ToString() + "\n");
                        tbDataRXSB.Append(numberHigh.ToString() + "\n");

                        outText.Append(" No.Bytes:");
                        outText.Append(numberHigh.ToString() + "\n");
                    }
                    else
                    {
                        //tbDataRx.AppendText("Data: ");
                        tbDataRXSB.Append(" Data:");
                        //tbDataRx.AppendText(numberHigh.ToString() + "\t  Binary: " + binary + "\n");
                        tbDataRXSB.Append(numberHigh.ToString() + "\tBinary:" + binary + "\n");

                    }

                    if (returnCode == "Read Coil Status " && _spManager.getRX())
                    {
                        numberHigh = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                        string coilBinary = Convert.ToString(numberHigh, 2).PadLeft(8, '0');
                        updateCoils(coilBinary);
                    }

                    if (returnCode == "Read Holding Registers " && _spManager.getRX())
                    {

                        updateHoldingRegister(sb.ToString());

                    }


                    outText.Append(" Data:" + numberHigh.ToString() + "\tBinary:" + binary + "\n");

                }

            }




            if (checkBoxLogging.Checked)
            {
                writeToFile(outText);
                writeToFile(sb);
            }

        }

        private void updateCoils(string binary)
        {
            if (coilCount <= 1)
            {
                coilsText.Clear();
            }
            int low;
            int high;

            try
            {
                low = Int32.Parse(textBoxCoilLow.Text);
            }
            catch
            {
                low = 1;
            }

            try
            {
                high = Int32.Parse(textBoxCoilHigh.Text);
            }

            catch
            {
                high = 254;
            }

            for (int i = binary.Length - 1; i >= 0; i--)
            {
                if (coilCount >= low + 8 && coilCount <= high + 8)
                {
                    int modifiedCount = coilCount - 8;
                    coilsText.Append(modifiedCount.ToString() + ": " + binary[i] + "\t");
                }
                //richTextBoxCoils.AppendText("\n");
                coilCount++;
            }


        }

        private void updateHoldingRegister(string decimalValue)
        {
            string test = decimalValue.Replace(" \n", string.Empty);
            if (test.Length > 16)
            {
                test = test.Substring(6, (test.Length - 8));
            }

            if (holdingRegisterCount <= 1)
            {
                //richTextBoxHoldingRegister.Text = "";
            }
            int low;
            int high;

            try
            {
                low = Int32.Parse(textBoxHoldLow.Text);
            }
            catch
            {
                low = 1;
            }

            try
            {
                high = Int32.Parse(textBoxHoldHigh.Text);
            }

            catch
            {
                high = 254;
            }


            //int modifiedCount = holdingRegisterCount - 1;
            //string newString = test.Substring(i, 4);
            int count = 1;
            for (int i = 0; i < test.Length - 1; i = i + 4)
            {
                try
                {
                    string test2 = Int32.Parse(test.Substring(i, 4), System.Globalization.NumberStyles.HexNumber).ToString();
                    if (i == 0)
                    {
                        richTextBoxHoldingRegister.Text = (count.ToString() + ": " + test2 + "\t");
                    }
                    else
                    {
                        richTextBoxHoldingRegister.AppendText(count.ToString() + ": " + test2 + "\t");
                    }
                }
                catch
                {

                }
                count++;
            }


            holdingRegisterCount++;



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
            try
            {
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

            replay = false;

            serialCount = 0;
            _spManager.StartListening();
        }

        // Handles the "Stop Listening"-buttom click event
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            buttonBack.Enabled = true;
            buttonForward.Enabled = false;

            replay = true;

            replayCount = messageHistoryCount;
            sb.Clear();
            

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
            buttonForward.Enabled = true;

            replayCount--;

            if (replayCount >= 0 && replayCount <= 149)
            {
                //tbData.Text = replayCount.ToString();
                badData.Text = messageHistory[replayCount].ToString();

                replayRX = _spManager.getHistory()[replayCount];

                if (messageHistory[replayCount].Length > 4)
                {
                    decodeModbus(messageHistory[replayCount]);
                    updateGUI();
                    badData.AppendText("\nRX: " + RX.ToString());
                }
                else
                {
                    buttonBack.Enabled = false;
                    tbData.Text = "End of replay data";
                    tbDataRx.Text = "";
                    replayCount++;
                }


            }
            else
            {
                replayCount = 149;
            }

            tbDataRXSB.Clear();
            tbDataSB.Clear();
            badDataSB.Clear();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            replayCount++;
            buttonBack.Enabled = true;

            if (replayCount <= 149 && replayCount >= 0)
            {

                //tbData.Text = replayCount.ToString();
                badData.Text = messageHistory[replayCount].ToString();
                replayRX = _spManager.getHistory()[replayCount];


                if (messageHistory[replayCount].Length > 4)
                {
                    decodeModbus(messageHistory[replayCount]);
                    updateGUI();

                    badData.AppendText("\nRX: " + RX.ToString());
                }
                else
                {
                    buttonForward.Enabled = false;
                    tbData.Text = "End of replay data";
                    tbDataRx.Text = "";
                }

            }
            else
            {
                replayCount = 0;
            }
            tbDataRXSB.Clear();
            tbDataSB.Clear();
            badDataSB.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            restrictInput(sender);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

            restrictInput(sender);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            restrictInput(sender);
        }

        private void tabPageRanges_Click(object sender, EventArgs e)
        {

        }

        private void restrictInput(object sender)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var text = textBox.Text;
            var output = new StringBuilder();
            //use this boolean to determine if the dot already exists
            //in the text so far.
            var dotEncountered = false;
            //loop through all of the text
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (char.IsDigit(c))
                {
                    //append any digit.
                    output.Append(c);
                }
                else if (!dotEncountered && c == '.')
                {
                    //append the first dot encountered
                    output.Append(c);
                    dotEncountered = true;
                }
            }
            var newText = output.ToString();
            textBox.Text = newText;
            //set the caret to the end of text
            //textBox.CaretIndex = newText.Length;

        }

        private void textBoxCoilHigh_TextChanged(object sender, EventArgs e)
        {

            restrictInput(sender);
        }
    }
}
