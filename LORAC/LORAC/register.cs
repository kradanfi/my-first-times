using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace LORAC
{
    public partial class register : Form
    {
        main form1;
        public register(main tempf)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            form1 = tempf;         
            form1.Hide();
        }

        public void setSerialPort(string pname,int brate,int pbar)
        {
            serialPort1.PortName = pname;
            serialPort1.BaudRate = brate;
            serialPort1.Handshake = Handshake.None;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = System.IO.Ports.StopBits.One;
            serialPort1.Parity = System.IO.Ports.Parity.None;
            toolStripProgressBar1.Value = pbar;
            toolStripComboBox2.Text = brate.ToString();
            toolStripComboBox1.Text = pname;
            if (pbar == 100)
            {
                serialPort1.Open();
                PlayButton.Enabled = false;
            }
            

        }

        ListViewItem[] regis = new ListViewItem[120];
        bool pair = false;
        private void button1_Click(object sender, EventArgs e)// form turn back to main
        {
            serialPort1.Close();
            PlayButton.Enabled = true;
            pair = false;
            this.Hide();
            form1.setSerialPort(serialPort1.PortName, serialPort1.BaudRate, toolStripProgressBar1.Value);
            form1.Show();
        }

        private void register_Load(object sender, EventArgs e)
        {
            String[] strPortNames = SerialPort.GetPortNames();
            foreach (string n in strPortNames) {
                toolStripComboBox1.Items.Add(n);
            }
            

            for (int i = 1; i < 115; i++)
            {
                regis[i] = new ListViewItem("0x00");
                regis[i].SubItems.Add("###");
                regis[i].SubItems.Add("0x00");
                listView1.Items.Add(regis[i]);
            }
            
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = toolStripComboBox1.Text;
            serialPort1.BaudRate = Int32.Parse(toolStripComboBox2.Text);
            serialPort1.Handshake = Handshake.None;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = System.IO.Ports.StopBits.One;
            serialPort1.Parity = System.IO.Ports.Parity.None;
            Console.WriteLine(serialPort1.PortName);

            serialPort1.Open();
            if(serialPort1.IsOpen)
                Console.WriteLine("open na");
            toolStripProgressBar1.Value = 100;
            PlayButton.Enabled = false;
            string str = "Extension methods have all the capabilities of regular static methods.";

            // Write the string and include the quotation marks.
            System.Console.WriteLine("\"{0}\"", str);
            

        }

       
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    char[] delimiterChars = { '[','=' };
                    
                    // textBox1.AppendText(serialPort1.ReadLine());
                    string text = serialPort1.ReadLine();
                    textBox1.AppendText(text+"\n");
                    System.Console.WriteLine("Original text: '{0}'", text);

                    string[] words = text.Split(delimiterChars);
                    //         words[0]= words[0].Substring(words[0].IndexOf('['), words[0].IndexOf('0') - words[0].IndexOf('[') - 1);
                    System.Console.WriteLine("{0} words in text:", words.Length);

                    foreach (string s in words)
                    {
                        System.Console.WriteLine(s);
                    }

                    if (pair == false && words[0].EndsWith("Done"))
                        pair = true;

                    if (pair == true && (words.Length>2)&&(!words[0].EndsWith("Done")) && words[1].StartsWith("reg", System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        int numRegis = Convert.ToInt32(words[1].Substring(words[1].IndexOf(':') + 3, 2), 16);
                        regis[numRegis + 1].SubItems[0].Text = words[1].Substring(words[1].IndexOf(':') + 1, 4);
                        regis[numRegis + 1].SubItems[1].Text = words[1].Substring(0, words[1].IndexOf(':'));
                        regis[numRegis + 1].SubItems[2].Text = words[2].Substring(0,4);
                    }
                }
                catch (System.IO.IOException error)
                {
                    pair = false;
                    textBox1.AppendText("stop!");
                    return;                    
                }
                catch (System.InvalidOperationException error)
                {
                    serialPort1.Close();
                    toolStripProgressBar1.Value = 0;
                    PlayButton.Enabled = true;
                    pair = false;
                    return;
                }
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            pair = false;
            serialPort1.Close();
            toolStripProgressBar1.Value = 0;
            PlayButton.Enabled = true;
        }

        private void register_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
            PlayButton.Enabled = true;
            pair = false;
            this.Hide();
            form1.setSerialPort(serialPort1.PortName, serialPort1.BaudRate, toolStripProgressBar1.Value);
            form1.Show();
        }
    }
}
