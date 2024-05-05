using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using ListViewElement = System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using System.Diagnostics;



namespace lab3
{
    public partial class Form1 : Form
    {
        
        private Thread serverThread=null;
        public Form1()
        {
            InitializeComponent();
             
        }

    private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private bool isThreadRunning = true;
        private bool isListening = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isThreadRunning = false;
        }
        private void StartListen(object  sender, EventArgs e)
        {
            
        }
        private StringBuilder receivedData = new StringBuilder();
        void StartUnsafeThread()
        {
            int bytesReceived = 0;
            byte[] buffer = new byte[1];
            Socket clientSocket;
            Socket listenerSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            listenerSocket.Bind(ipepServer);
            listenerSocket.Listen(-1);
            clientSocket = listenerSocket.Accept();
            textBox1.Text = "New client connected\r";


            while (clientSocket.Connected && isThreadRunning) 
            {
                string text = "";

                
                do
                {
                    bytesReceived = clientSocket.Receive(buffer);
                    text += Encoding.ASCII.GetString(buffer);
                    if (isThreadRunning==false)
                    {
                        System.Diagnostics.Debugger.Break();
                        break; 
                    }
                    receivedData.Append(text);
                } while(text[text.Length - 1]!='\n');

                if (text.EndsWith("\n"))
                {
                    string message = text.ToString();
                    UpdateTextBox("\n"+message);
                    
                }

              
                
            }
            listenerSocket.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isListening && (serverThread == null || !serverThread.IsAlive))
            {
                isListening = true;

                textBox1.Text = "Listening\r";
                CheckForIllegalCrossThreadCalls = false;
                Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
                serverThread.Start();
            }

        }

       

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void DisplayMessage(string message)
        {

            textBox1.Text += message + " ";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void UpdateTextBox(string message)
        {
            try
            {
                if (textBox1.InvokeRequired && isThreadRunning)
                {
                    textBox1.Invoke(new Action<string>(UpdateTextBox), message);
                }
                else
                {
                    textBox1.AppendText(message + Environment.NewLine);
                }
            }
            catch { }
        }
    }
}


