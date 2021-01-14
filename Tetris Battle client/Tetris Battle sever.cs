using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_Battle_client
{
    public partial class Tetris_Battle_sever : Form
    {
        private static byte[] result = new byte[1024];
        public int _port = 8885;
        static Socket serverSocket;

        private IList<Socket> clientPool;

        int i;
        int playready;

        string msg;
        string msgcheck;
        string msgwork;
        string checkSocket;
        string playnum;
        string playID;
        

        public Tetris_Battle_sever()
        {
            InitializeComponent();
        }

        private void Tetris_Battle_sever_Load(object sender, EventArgs e)
        {
            clientPool = new List<Socket>();
        }

        private void Tetris_Battle_sever_Shown(object sender, EventArgs e)
        {
            tbPort.Text = _port.ToString();
        }

        private void Tetris_Battle_sever_FormClosing(object sender, FormClosingEventArgs e)
        {
            //釋放資源
            serverSocket.Close();
            clientPool.Clear();
        }

        private void btn_connet_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(cbbIP.Text);
            _port = int.Parse(tbPort.Text);

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, _port));
            serverSocket.Listen(10);    //設定最多10個排隊連線請求
            ShowMsg($"[系統]正在監聽{serverSocket.LocalEndPoint.ToString()}...");
            //通過Clientsoket傳送資料
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();

            btn_connet.Visible = false;

            Tetris_Battle_tpm_room_ tetris_Battle_Tpm_Room = new Tetris_Battle_tpm_room_();
            tetris_Battle_Tpm_Room.ip = ip;
            tetris_Battle_Tpm_Room._port = _port;
            tetris_Battle_Tpm_Room.Show();
            
        }

        private void ListenClientConnect()
        {
            //利用無窮回圈 接待 client
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();  //client 進入!        
                clientPool.Add(clientSocket);
                i += 1;
                playID = "" + i;
                clientSocket.Send(Encoding.ASCII.GetBytes($"g0{playID}"));
                ShowMsg($"[系統]偵測到客戶端{ clientSocket.RemoteEndPoint.ToString()}連入系統...");
                Thread receiveThread = new Thread(ReceiveMessage);   //綁定接收處理
                receiveThread.Start(clientSocket);
                Thread.Sleep(100);
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"mi{playID}"));
                }
            }
        }


        private void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;  //因為執行序只能傳遞 object,所以才需要在將資料強制轉回 (又稱解箱)
            
            while (true)
            {
                try
                {
                    //通過clientSocket接收資料
                    int receiveNumber = myClientSocket.Receive(result);  //受到的資料長度
                    msg = Encoding.ASCII.GetString(result, 0, receiveNumber);
                    msgcheck = msg.Substring(0, 1);
                    msgwork = msg.Substring(1);

                    checkSocket = myClientSocket.RemoteEndPoint.ToString();
                    for (i = 0; i < clientPool.Count; i++)
                    {
                        if (checkSocket == clientPool[i].RemoteEndPoint.ToString())
                        {
                            playnum = (i + 1).ToString();
                        }
                    }

                    switch (msgcheck)
                    {
                        case "m":
                            ShowMsg($"接收客戶端{myClientSocket.RemoteEndPoint.ToString()}訊息{msgwork}");
                            foreach (var socket in clientPool)
                            {
                                socket.Send(Encoding.ASCII.GetBytes($"m{playnum}" + msgwork));
                            }
                            break;
                        case "k":
                            ShowMsg($"玩家{playnum}已準備");
                            foreach (var socket in clientPool)
                            {
                                socket.Send(Encoding.ASCII.GetBytes($"k{playnum}"));
                            }
                            playready++;
                            GameStart();
                            break;
                        case "d":
                            ShowMsg($"玩家{playnum}失敗");
                            foreach (var socket in clientPool)
                            {
                                socket.Send(Encoding.ASCII.GetBytes($"d{playnum}"));
                            }
                            break;
                        case "u":
                            ShowMsg($"玩家{playnum}消{msgwork}格");
                            foreach (var socket in clientPool)
                            {
                                socket.Send(Encoding.ASCII.GetBytes($"u{playnum}{msgwork}"));
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Dispose();
                    //myClientSocket.Close();

                    break;
                }
            }

        }

        void GameStart()
        {
            if (playready == 2)
            {
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"ma"));
                }
                Thread.Sleep(1000);
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"m05"));
                }
                Thread.Sleep(1000);
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"m04"));
                }
                Thread.Sleep(1000);
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"m03"));
                }
                Thread.Sleep(1000);
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"m02"));
                }
                Thread.Sleep(1000);
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"m01"));
                }
                Thread.Sleep(1000);
                foreach (var socket in clientPool)
                {
                    socket.Send(Encoding.ASCII.GetBytes($"m0go"));
                }
            }
            
            
        }

        private void ShowMsg(string msg, bool newLine = true)
        {
            Console.WriteLine(msg);

            //AppendTextByThreadSafe(rtbMsg, );
            AppendTextByThreadSafe(rtb_msg, (newLine ? "\n " : "") + msg);
        }

        private delegate void AppendTextByThreadSafeDelegate(Control control, string text);

        public static void AppendTextByThreadSafe(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new AppendTextByThreadSafeDelegate(AppendTextByThreadSafe), new object[] { control, text });
            }
            else
            {
                RichTextBox target = (RichTextBox)control;
                target.AppendText(text);
                target.SelectionStart = target.Text.Length;
                target.ScrollToCaret();
            }
        }

        private void Tetris_Battle_sever_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
