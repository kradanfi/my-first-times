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
  
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
           
        }


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e) // swich to register page
        {
           // this.Hide();
            register next = new register(this);
            serialPort1.Close();
            next.setSerialPort(serialPort1.PortName,serialPort1.BaudRate, toolStripProgressBar1.Value); // set state for register form 
            next.ShowDialog();

        }

        private void main_Load(object sender, EventArgs e)
        {
            String[] strPortNames = SerialPort.GetPortNames();
            foreach (string n in strPortNames)
            {
                toolStripComboBox1.Items.Add(n);
            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)//play Button
        {
            serialPort1.PortName = toolStripComboBox1.Text;
            serialPort1.BaudRate = Int32.Parse(toolStripComboBox2.Text);
            serialPort1.Handshake = Handshake.None;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = System.IO.Ports.StopBits.One;
            serialPort1.Parity = System.IO.Ports.Parity.None;
            Console.WriteLine(serialPort1.PortName);

            serialPort1.Open();
            if (serialPort1.IsOpen)
                Console.WriteLine("open na");
            toolStripProgressBar1.Value = 100;
            PlayButton.Enabled = false;
            string str = "Extension methods have all the capabilities of regular static methods.";

            // Write the string and include the quotation marks.
            System.Console.WriteLine("\"{0}\"", str);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            toolStripProgressBar1.Value = 0;
            PlayButton.Enabled = true;
        }

        public void setSerialPort(string pname, int brate, int pbar)
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
            }
            else
            {
                PlayButton.Enabled = true;
            }
        }

    }
  

}
