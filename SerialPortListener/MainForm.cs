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

namespace SerialPortListener
{
    public partial class MainForm : Form
    {
        SerialPortManager _spManager;

        string returnCode;
        string previousSlaveId = "999";
        string str;

        byte[] rxByte = new byte[1000];

        StringBuilder previousMessage;
        StringBuilder sb = new StringBuilder();
        StringBuilder rxData = new StringBuilder();
        StringBuilder rxByteView = new StringBuilder();

        int repeatCount = 0;
        int rxCRC1 = 0;
        int rxCRC2 = 0;

        bool response;
        bool rxResponse;
        bool RX = false;
        
        public MainForm()
        {
            InitializeComponent();


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

            baudRateComboBox.SelectedIndex = 11;
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
           
            str = Encoding.ASCII.GetString(e.Data);

            
            //StringBuilder sb = new StringBuilder();
            
                for (int i = 0; i < e.Data.Length; i++)
                    sb.AppendFormat("{0:X2} \n", e.Data[i]);

            badData.Text = sb.ToString();
            
            response =  CheckResponse(e.Data);

            if (response)
            {
                decodeModbus(sb);
            }
            else // CRC check has failed then do this
            {
                badData.AppendText("Error Detected \n");

                rxResponse = CheckResponse(GetBytes(rxData.ToString()));

                badData.AppendText("RX CRC: " + rxResponse.ToString() + "\n");
                
                rxData.Append(sb);
                

            }




        }

        private void decodeModbus(StringBuilder sb)
        {
            if (sb != null && previousMessage != null)
            { 
                if (sb.ToString() == previousMessage.ToString())
                {
                    repeatCount++;
                }
                else
                {
                    repeatCount = 0;
                }
        }
         
            int startAddressHigh=0;
            int startAddressLow=0;
            int numberHigh=0;
            int numberLow=0;
            int startAddress=0;
            int range = 0;
            int endAddress = 0; 
          
            string[] words = sb.ToString().Split(' ');
            
                //tbDataRx.AppendText(rxData.ToString());

                rxCRC1 = words.Length - 3;
                rxCRC2 = words.Length - 2;
      
                for (int i = 0; i < words.Length; i++)
                {

                    switch (i) // Decode dependant on which part of the message is being decoded
                    {

                        case 0:
                            if (previousSlaveId == words[i] )
                            {
                                RX = true;
                                
                                tbDataRx.Text = ("Slave Response \n");
                                int value = Convert.ToInt32(previousSlaveId, 16);
                                tbDataRx.AppendText("Slave Address:" + value.ToString() + "\n");
                                tbDataRx.AppendText("Calculated CRC: " + response + "\n");
                                previousSlaveId = " ";
                            }
                            else
                            {
                                RX = false;
                                tbData.Text = ("Master Request \n");
                                previousSlaveId = words[i];
                                tbData.AppendText("Slave Address: ");
                                int value = Convert.ToInt32(words[i], 16);
                                tbData.AppendText(value.ToString() + "\n");
                                tbData.AppendText("Calculated CRC: " + response + "\n");
                            }

                            break;

                        case 1:
                            getFunctionCode(words[i]);
                            if (RX)
                            {
                                tbDataRx.AppendText(returnCode + "\n");
                            }
                            else
                            {
                                tbData.AppendText(returnCode + "\n");
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
                                tbDataRx.AppendText("Data High: ");
                                tbDataRx.AppendText(startAddressHigh.ToString() + "\n");
                            }
                            else
                            {
                                tbData.AppendText("Start Address High: ");
                                tbData.AppendText(startAddressHigh.ToString() + "\n");
                            }

                            break;

                        case 3:
                            startAddressLow = Int32.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                            if (RX)
                            {
                                tbDataRx.AppendText("Data Low: ");
                                tbDataRx.AppendText(startAddressLow.ToString() + "\n");
                            }
                            else
                            {
                                tbData.AppendText("Start Addess Low: ");
                                tbData.AppendText(startAddressLow.ToString() + "\n");
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
                                tbDataRx.AppendText("Data High: ");
                                tbDataRx.AppendText(numberHigh.ToString() + "\n");
                            }
                            else
                            {
                                tbData.AppendText("Number High: ");
                                tbData.AppendText(numberHigh.ToString() + "\n");
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
                                tbDataRx.AppendText("Data Low: ");
                                tbDataRx.AppendText(numberLow.ToString() + "\n");


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

                                tbData.AppendText("\n Start of Data : " + startAddress + "\n");
                                tbData.AppendText("\n End of Data : " + endAddress + "\n");
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



                }

                tbData.AppendText("\n");
                tbData.ScrollToCaret();

                tbDataRx.AppendText("\n");
                tbDataRx.ScrollToCaret();
                previousMessage = sb;

                rxResponse = CheckResponse(GetBytes(rxData.ToString()));

                badData.AppendText("RX CRC: " + rxResponse.ToString() + "\n");

                

            

                rxData.Clear();
                rxByteView.Clear();
                Array.Clear(rxByte, 0, rxByte.Length);

            
           
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
                    returnCode = "Broken ";
                    break;

                default:
                    returnCode = "Unknown Function";
                    break;

            }
            

        }

        // Handles the "Start Listening"-buttom click event
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            _spManager.StartListening();
        }

        // Handles the "Stop Listening"-buttom click event
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            _spManager.StopListening();
        }

        private void portNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
