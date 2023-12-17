using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private TcpClient tcpClient;
    private StreamReader reader;
    private StreamWriter writer;
    private string username;
    private TextBox serverIPTextBox;
    private TextBox serverPortTextBox;
    private TextBox usernameTextBox;
    private Button connectButton;
    private Label connectionStatusLabel;
    private RichTextBox messagesRichTextBox;
    private TextBox messageTextBox;
    private Button sendMessageButton;
    private Button disconnectButton;
    public Form1()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        this.serverIPTextBox = new System.Windows.Forms.TextBox();
        this.serverPortTextBox = new System.Windows.Forms.TextBox();
        this.usernameTextBox = new System.Windows.Forms.TextBox();
        this.connectButton = new System.Windows.Forms.Button();
        this.connectionStatusLabel = new System.Windows.Forms.Label();
        this.messagesRichTextBox = new System.Windows.Forms.RichTextBox();
        this.messageTextBox = new System.Windows.Forms.TextBox();
        this.sendMessageButton = new System.Windows.Forms.Button();
        this.disconnectButton = new System.Windows.Forms.Button();

        // Add control initialization code here...

        // TextBox - serverIPTextBox
        this.serverIPTextBox.Location = new System.Drawing.Point(12, 12);
        this.serverIPTextBox.Name = "serverIPTextBox";
        this.serverIPTextBox.Size = new System.Drawing.Size(100, 20);
        // Add other properties as needed...

        // TextBox - serverPortTextBox
        this.serverPortTextBox.Location = new System.Drawing.Point(118, 12);
        this.serverPortTextBox.Name = "serverPortTextBox";
        this.serverPortTextBox.Size = new System.Drawing.Size(50, 20);
        // Add other properties as needed...

        // TextBox - usernameTextBox
        this.usernameTextBox.Location = new System.Drawing.Point(12, 38);
        this.usernameTextBox.Name = "usernameTextBox";
        this.usernameTextBox.Size = new System.Drawing.Size(100, 20);
        // Add other properties as needed...

        // Button - connectButton
        this.connectButton.Location = new System.Drawing.Point(174, 10);
        this.connectButton.Name = "connectButton";
        this.connectButton.Size = new System.Drawing.Size(75, 23);
        // Add other properties as needed...
        this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);

        // Label - connectionStatusLabel
        this.connectionStatusLabel.AutoSize = true;
        this.connectionStatusLabel.Location = new System.Drawing.Point(255, 15);
        this.connectionStatusLabel.Name = "connectionStatusLabel";
        this.connectionStatusLabel.Size = new System.Drawing.Size(0, 13);
        // Add other properties as needed...

        // RichTextBox - messagesRichTextBox
        this.messagesRichTextBox.Location = new System.Drawing.Point(12, 64);
        this.messagesRichTextBox.Name = "messagesRichTextBox";
        this.messagesRichTextBox.Size = new System.Drawing.Size(300, 200);
        // Add other properties as needed...

        // TextBox - messageTextBox
        this.messageTextBox.Location = new System.Drawing.Point(12, 270);
        this.messageTextBox.Name = "messageTextBox";
        this.messageTextBox.Size = new System.Drawing.Size(220, 20);
        // Add other properties as needed...

        // Button - sendMessageButton
        this.sendMessageButton.Location = new System.Drawing.Point(238, 268);
        this.sendMessageButton.Name = "sendMessageButton";
        this.sendMessageButton.Size = new System.Drawing.Size(75, 23);
        // Add other properties as needed...
        this.sendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);

        // Button - disconnectButton
        this.disconnectButton.Location = new System.Drawing.Point(255, 38);
        this.disconnectButton.Name = "disconnectButton";
        this.disconnectButton.Size = new System.Drawing.Size(75, 23);
        // Add other properties as needed...
        this.disconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);

        // Add other controls as needed...

        // Form1
        this.Controls.Add(this.serverIPTextBox);
        this.Controls.Add(this.serverPortTextBox);
        this.Controls.Add(this.usernameTextBox);
        this.Controls.Add(this.connectButton);
        this.Controls.Add(this.connectionStatusLabel);
        this.Controls.Add(this.messagesRichTextBox);
        this.Controls.Add(this.messageTextBox);
        this.Controls.Add(this.sendMessageButton);
        this.Controls.Add(this.disconnectButton);
        // Add other controls as needed...
    }

    private void ConnectButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(usernameTextBox.Text))
        {
            MessageBox.Show("Please enter a username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        username = usernameTextBox.Text;

        try
        {
            tcpClient = new TcpClient(serverIPTextBox.Text, int.Parse(serverPortTextBox.Text));
            NetworkStream networkStream = tcpClient.GetStream();
            reader = new StreamReader(networkStream, Encoding.UTF8);
            writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true };

            // Send the username to the server
            writer.WriteLine(username);

            // Start a thread to receive messages from the server
            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();

            // Update UI
            connectionStatusLabel.Text = "Connected";
            connectionStatusLabel.ForeColor = System.Drawing.Color.Green;
            connectButton.Enabled = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error connecting to the server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Display the received message in the UI
                DisplayMessage(message);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error receiving messages: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // Cleanup resources and update UI
            Disconnect();
        }
    }

    private void DisplayMessage(string message)
    {
        // Update the UI to display the received message
        messagesRichTextBox.AppendText(message + Environment.NewLine);
    }

    private void SendMessageButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(messageTextBox.Text))
        {
            // Send the message to the server
            writer.WriteLine($"ChannelName: {messageTextBox.Text}");
            messageTextBox.Clear();
        }
    }

    private void DisconnectButton_Click(object sender, EventArgs e)
    {
        Disconnect();
    }

    private void Disconnect()
    {
        if (tcpClient != null)
        {
            // Close the connection and update UI
            tcpClient.Close();
            connectionStatusLabel.Text = "Disconnected";
            connectionStatusLabel.ForeColor = System.Drawing.Color.Red;
            connectButton.Enabled = true;
        }
    }

    // Other GUI event handlers and methods...
}

public partial class DiSUcordClientGUI
{
    // Additional GUI components can be added using the designer in Visual Studio.
    private TextBox serverIPTextBox;
    private TextBox serverPortTextBox;
    private TextBox usernameTextBox;
    private Button connectButton;
    private Label connectionStatusLabel;
    private RichTextBox messagesRichTextBox;
    private TextBox messageTextBox;
    private Button sendMessageButton;
    private Button disconnectButton;

    private void InitializeComponent()
    {
        // GUI initialization code...
    }
}
