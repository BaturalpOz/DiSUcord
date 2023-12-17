using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DiSUcord.UI
{
    public partial class Form1 : Form
    {

        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread receiveThread;

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void Subscribe_Click(object sender, EventArgs e)
        {
            // Subscribe to a channel
            string channel = "IF 100"; // Replace with the actual channel
            writer.WriteLine($"Subscribe:{channel}");

            // Update UI
            IncomingMsgBox.AppendText($"Subscribed to {channel}.\n");
        }

        private void SendTxtBox_TextChanged(object sender, EventArgs e)
        {
            SendMsgBtn.Enabled = !string.IsNullOrWhiteSpace(SendMsgBox.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Disconnect when the form is closed
            if (tcpClient != null && tcpClient.Connected)
            {
                DisconnectBtn_Click(sender, e);
            }
        }
        private void UnsubBtn_Click(object sender, EventArgs e)
        {
            // Unsubscribe from a channel
            string channel = "IF 100"; // Replace with the actual channel
            writer.WriteLine($"Unsubscribe:{channel}");

            // Update UI
            IncomingMsgBox.AppendText($"Unsubscribed from {channel}.\n");
        }

        private void SendMsgBtn_Click(object sender, EventArgs e)
        {
            // Send a message to the subscribed channel
            string message = SendMsgBox.Text;
            writer.WriteLine($"IF 100:{message}"); // Replace with the actual channel

            // Update UI
            SendMsgBox.Clear();
            IncomingMsgBox.AppendText($"You: {message}\n");
        }

        private void DisconnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Disconnect from the server
                tcpClient.Close();
                receiveThread.Abort();

                // Update UI
                IncomingMsgBox.AppendText("Disconnected from the server.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Establish connection to the server
                tcpClient = new TcpClient("127.0.0.1", 12345); // Update with your server IP and port
                NetworkStream networkStream = tcpClient.GetStream();
                reader = new StreamReader(networkStream, Encoding.UTF8);
                writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

                // Send the username to the server
                writer.WriteLine("Username"); // Replace "Username" with the actual username

                // Start a thread to receive messages from the server
                receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();

                // Update UI
                IncomingMsgBox.AppendText("Connected to the server.\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadLine();
                    if (message == null)
                    {
                        break;
                    }

                    // Update UI with incoming messages
                    IncomingMsgBox.Invoke((MethodInvoker)delegate
                    {
                        IncomingMsgBox.AppendText(message + "\n");
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
