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
    public partial class Tetris_Battle_tpm_room_ : Form
    {

        private static byte[] result = new byte[1024];       
        private Socket clientSocket;
        private Thread receiveMessageThread;

        public IPAddress ip ;
        public int _port;

        int px, py; //移動方塊點
        int i;
        int j;
        int k;
        int x;
        int dir;
        int cube = 1;
        int bar = 2;
        int cubevirtual = -1;
        int losebar = -2;
        int cubetype;   //圖形種類
        int type;   //圖形翻轉種類
        int movetype;
        int puttype;

        bool cubeclear;
        bool gamestart;

        string msg;
        string msgcheck;
        string msgwho;
        string msgwork;
        string playID;
        string playNum;

        Random random = new Random();

        List<List<int>> map;
        List<int> maplist;
        List<int> cubelist;

        List<Image> cubenextimages;

        WebImage cubenext;


        public Tetris_Battle_tpm_room_()
        {
            InitializeComponent();
        }

        private void Tetris_Battle_tpm_room__Load(object sender, EventArgs e)
        {

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(ip, _port)); //配置伺服器IP與埠
            ShowMsg($"[系統]連線伺服器成功...");
            receiveMessageThread = new Thread(ReceiveMessage);
            receiveMessageThread.Start();

            GameStart();
            label_ID.Text = "玩家" + playID;
            GameLose();
        }

        private void Tetris_Battle_tpm_room__FormClosing(object sender, FormClosingEventArgs e)
        {
            clientSocket.Send(Encoding.ASCII.GetBytes("l"));
            receiveMessageThread?.Abort();
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                int receiveLength = clientSocket.Receive(result);
                msg = Encoding.ASCII.GetString(result, 0, receiveLength);
                msgcheck = msg.Substring(0, 1);
                msgwho = msg.Substring(1, 1);
                msgwork = msg.Substring(2);
                switch (msgcheck)
                {
                    case "m":
                        if (msgwho == "0")
                        {
                            ShowMsg($"[系統]:{msgwork}");
                            if (msgwork == "go")
                            {
                                GameStart();
                            }
                        }
                        else if (msgwho == "i")
                        {
                            playNum = msgwork;
                            ShowMsg($"[系統]:玩家{playNum}已進入房間");
                        }
                        else if (msgwho == "a")
                        {
                            ShowMsg($"[系統]:五秒後開始遊戲");
                        }
                        else
                        {
                            ShowMsg($"[玩家{msgwho}]:{msgwork}");
                        }
                        break;
                    case "k":
                        ShowMsg($"[玩家{msgwho}]已準備");
                        break;
                    case "d":
                        ShowMsg($"[系統]:[玩家{msgwho}]失敗");
                        GameLose();
                        break;
                    case "g":
                        playID = msgwork;
                        break;
                    case "u":
                        if (msgwho != playID)
                        {
                            k = int.Parse(msgwork);

                            CubeUp();

                            if (px > 5 + k)
                            {
                                px -= k;
                            }

                            movetype = 0;
                            CubeTypeMove();

                            CubeMove();
                        }
                        break;
                }
            }
        }

        /*void Check()
        {
            switch (msgcheck)
            {
                case "m":
                    if (msgwho == "0")
                    {
                        ShowMsg($"[系統]:{msgwork}");
                    }
                    else
                    {
                        ShowMsg($"[玩家{msgwho}]:{msgwork}");
                    }                   
                    break;
                case "k":
                    ShowMsg($"[玩家{msgwho}]已準備");
                    break;
                case "g":
                    playID = msgwork;
                    label_ID.Text = "玩家" + playID;
                    break;
            }
        }*/

        private void ShowMsg(string msg, bool newLine = true)
        {
            Console.WriteLine(msg);
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

        private delegate void Screen(Control control, int i, string text);

        public static void gameScreen(Control control, int i, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Screen(gameScreen), new object[] { control, i, text});
            }
            else
            {
                RichTextBox target = (RichTextBox)control;
                target.Size = new Size(275, 415);
                target.Enabled = false;
                if (i == 0)
                {
                    target.Clear();
                }
                if (i == 1)
                {
                    target.AppendText(text);
                }
            }
        }

        private delegate void Flp(Control control, int i, Control addc);

        public static void flp(Control control, int i, Control addc)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Flp(flp), new object[] { control, i, addc });
            }
            else
            {
                FlowLayoutPanel target = (FlowLayoutPanel)control;
                if (i == 0)
                {
                    target.Controls.Clear();
                }
                if (i == 1)
                {
                    target.Controls.Add(addc);
                }
            }
        }

        private void btn_msgsend_Click(object sender, EventArgs e)
        {
            clientSocket.Send(Encoding.ASCII.GetBytes("m"+tb_msg.Text));
        }

        private void btn_ready_Click(object sender, EventArgs e)
        {
            clientSocket.Send(Encoding.ASCII.GetBytes("k"));
            btn_ready.Visible = false;
        }

        void GameStart()
        {
            map = new List<List<int>>();
            cubelist = new List<int>();

            cubenextimages = new List<Image>();

            cubenext = new WebImage();

            cubenextimages = cubenext.GetImage("5WphR4d");

            for (j = 0; j < 22; j++)
            {
                maplist = new List<int>();

                for (i = 0; i < 12; i++)
                {
                    maplist.Add(0);
                }
                map.Add(maplist);
            }

            px = 2;
            py = 2;

            for (i = 0; i < 5; i++)
            {
                cubelist.Add(random.Next(7));
            }

            cubetype = cubelist[0];

            gamestart = true;

            CubeMove();

        }

        void GameLoad()
        {
            if (gamestart)
            {
                if (cubeclear)
                {
                    CubeClear();  //消除方塊判斷
                    cubeclear = false;
                }

                PrintMap();   //列印遊戲
            }

            //Console.Clear();
            //Console.WriteLine("Game Over!\n");
        }

        void GameLose()
        {
            gamestart = false;
            gameScreen(rtb_gameScreen, 0, null);
            //rtb_gameScreen.Clear();
        }

        /*void tran(int num)  //二進制取座標
        {
            x = (byte)(num >> 4);
            y = (byte)((byte)(num << 4) >> 4);
        }*/

        void CubeTypeMove() //方塊種類移動
        {
            if (cubetype == 0)  //土字形
            {
                if (type == 0)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py + 1] = movetype; //右
                    map[px][py - 1] = movetype; //左
                }
                if (type == 1)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py + 1] = movetype; //右
                    map[px + 1][py] = movetype; //下
                }
                if (type == 2)
                {
                    map[px][py] = movetype;
                    map[px][py + 1] = movetype; //右
                    map[px + 1][py] = movetype; //下
                    map[px][py - 1] = movetype; //左
                }
                if (type == 3)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px + 1][py] = movetype; //下
                    map[px][py - 1] = movetype; //左
                }
            }
            if (cubetype == 1)  //田字形
            {
                map[px][py] = movetype;
                map[px - 1][py] = movetype; //上
                map[px][py - 1] = movetype; //左
                map[px - 1][py - 1] = movetype; //左上
            }
            if (cubetype == 2)  //Z字形
            {
                if (type == 0)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py - 1] = movetype; //左
                    map[px - 1][py + 1] = movetype; //右上
                }
                if (type == 1)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py + 1] = movetype; //右
                    map[px + 1][py + 1] = movetype; //右下
                }
                if (type == 2)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py - 1] = movetype; //左
                    map[px - 1][py + 1] = movetype; //右上
                }
                if (type == 3)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py + 1] = movetype; //右
                    map[px + 1][py + 1] = movetype; //右下
                }
            }
            if (cubetype == 3)  //反向Z字
            {
                if (type == 0)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py + 1] = movetype; //右
                    map[px - 1][py - 1] = movetype; //左上
                }
                if (type == 1)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py - 1] = movetype; //左
                    map[px + 1][py - 1] = movetype; //左下
                }
                if (type == 2)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py + 1] = movetype; //右
                    map[px - 1][py - 1] = movetype; //左上
                }
                if (type == 3)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px][py - 1] = movetype; //左
                    map[px + 1][py - 1] = movetype; //左下
                }
            }
            if (cubetype == 4)  //L形
            {
                if (type == 0)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px + 1][py] = movetype; //下
                    map[px - 1][py - 1] = movetype; //左上
                }
                if (type == 1)
                {
                    map[px][py] = movetype;
                    map[px][py + 1] = movetype; //右
                    map[px][py - 1] = movetype; //左
                    map[px - 1][py + 1] = movetype; //右上
                }
                if (type == 2)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px + 1][py] = movetype; //下
                    map[px + 1][py + 1] = movetype; //右下
                }
                if (type == 3)
                {
                    map[px][py] = movetype;
                    map[px][py + 1] = movetype; //右
                    map[px][py - 1] = movetype; //左
                    map[px + 1][py - 1] = movetype; //左下
                }
            }
            if (cubetype == 5)  //反向L形
            {
                if (type == 0)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px + 1][py] = movetype; //下
                    map[px - 1][py + 1] = movetype; //右上
                }
                if (type == 1)
                {
                    map[px][py] = movetype;
                    map[px][py + 1] = movetype; //右
                    map[px][py - 1] = movetype; //左
                    map[px + 1][py + 1] = movetype; //右下
                }
                if (type == 2)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px + 1][py] = movetype; //下
                    map[px + 1][py - 1] = movetype; //左下
                }
                if (type == 3)
                {
                    map[px][py] = movetype;
                    map[px][py + 1] = movetype; //右
                    map[px][py - 1] = movetype; //左
                    map[px - 1][py - 1] = movetype; //左上
                }
            }
            if (cubetype == 6)  //一字形
            {
                if (type == 0)
                {
                    map[px][py] = movetype;
                    map[px][py + 1] = movetype; //右
                    map[px][py - 1] = movetype; //左
                    map[px][py + 2] = movetype; //右右
                }
                if (type == 1)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px + 1][py] = movetype; //下
                    map[px + 2][py] = movetype; //下下
                }
                if (type == 2)
                {
                    map[px][py] = movetype;
                    map[px][py + 1] = movetype; //右
                    map[px][py - 1] = movetype; //左
                    map[px][py + 2] = movetype; //右右
                }
                if (type == 3)
                {
                    map[px][py] = movetype;
                    map[px - 1][py] = movetype; //上
                    map[px + 1][py] = movetype; //下
                    map[px + 2][py] = movetype; //下下
                }
            }
        }

        void CubeTypePut()  //方塊種類放置
        {
            if (cubetype == 0)  //土字形
            {
                if (type == 0)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py + 1] == 0 && map[i][py - 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i + 1][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i][py - 1] = cubevirtual; //左

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py + 1] = cube; //右
                                    map[i][py - 1] = cube; //左

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 1)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py + 1] == 0 && map[i + 1][py] == 0)
                        {
                            if (map[i + 1][py + 1] > 0 || map[i + 2][py] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i + 1][py] = cubevirtual; //下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py + 1] = cube; //右
                                    map[i + 1][py] = cube; //下

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 2)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i][py + 1] == 0 && map[i + 1][py] == 0 && map[i][py - 1] == 0)
                        {
                            if (map[i + 1][py + 1] > 0 || map[i + 2][py] > 0 || map[i + 1][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i][py - 1] = cubevirtual; //左

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i][py + 1] = cube; //右
                                    map[i + 1][py] = cube; //下
                                    map[i][py - 1] = cube; //左

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 3)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i + 1][py] == 0 && map[i][py - 1] == 0)
                        {
                            if (map[i + 2][py] > 0 || map[i + 1][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i][py - 1] = cubevirtual; //左

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i + 1][py] = cube; //下
                                    map[i][py - 1] = cube; //左

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (cubetype == 1)  //田字形
            {
                for (i = px; i < 21; i++)
                {
                    if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py - 1] == 0 && map[i - 1][py - 1] == 0)
                    {
                        if (map[i + 1][py] > 0 || map[i + 1][py - 1] > 0)
                        {
                            if (puttype == cubevirtual)
                            {
                                map[i][py] = cubevirtual;
                                map[i - 1][py] = cubevirtual; //上
                                map[i][py - 1] = cubevirtual; //左
                                map[i - 1][py - 1] = cubevirtual; //左上

                                break;
                            }

                            if (puttype == cube)
                            {
                                map[i][py] = cube;
                                map[i - 1][py] = cube; //上
                                map[i][py - 1] = cube; //左
                                map[i - 1][py - 1] = cube; //左上

                                break;
                            }
                        }
                    }
                }
            }
            if (cubetype == 2)  //Z字形
            {
                if (type == 0)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py - 1] == 0 && map[i - 1][py + 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py - 1] > 0 || map[i][py + 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i - 1][py + 1] = cubevirtual; //右上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py - 1] = cube; //左
                                    map[i - 1][py + 1] = cube; //右上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 1)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py + 1] == 0 && map[i + 1][py + 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 2][py + 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i + 1][py + 1] = cubevirtual; //右下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py + 1] = cube; //右
                                    map[i + 1][py + 1] = cube; //右下

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 2)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py - 1] == 0 && map[i - 1][py + 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py - 1] > 0 || map[i][py + 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i - 1][py + 1] = cubevirtual; //右上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py - 1] = cube; //左
                                    map[i - 1][py + 1] = cube; //右上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 3)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py + 1] == 0 && map[i + 1][py + 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 2][py + 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i + 1][py + 1] = cubevirtual; //右下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py + 1] = cube; //右
                                    map[i + 1][py + 1] = cube; //右下

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (cubetype == 3)  //反向Z字
            {
                if (type == 0)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py + 1] == 0 && map[i - 1][py - 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i - 1][py - 1] = cubevirtual; //左上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py + 1] = cube; //右
                                    map[i - 1][py - 1] = cube; //左上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 1)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py - 1] == 0 && map[i + 1][py - 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 2][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i + 1][py - 1] = cubevirtual; //左下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py - 1] = cube; //左
                                    map[i + 1][py - 1] = cube; //左下

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 2)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py + 1] == 0 && map[i - 1][py - 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i - 1][py - 1] = cubevirtual; //左上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py + 1] = cube; //右
                                    map[i - 1][py - 1] = cube; //左上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 3)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i][py - 1] == 0 && map[i + 1][py - 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 2][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i + 1][py - 1] = cubevirtual; //左下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i][py - 1] = cube; //左
                                    map[i + 1][py - 1] = cube; //左下

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (cubetype == 4)  //L形
            {
                if (type == 0)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i + 1][py] == 0 && map[i - 1][py - 1] == 0)
                        {
                            if (map[i + 2][py] > 0 || map[i][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i - 1][py - 1] = cubevirtual; //左上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i + 1][py] = cube; //下
                                    map[i - 1][py - 1] = cube; //左上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 1)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i][py + 1] == 0 && map[i][py - 1] == 0 && map[i - 1][py + 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i + 1][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i - 1][py + 1] = cubevirtual; //右上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i][py + 1] = cube; //右
                                    map[i][py - 1] = cube; //左
                                    map[i - 1][py + 1] = cube; //右上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 2)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i + 1][py] == 0 && map[i + 1][py + 1] == 0)
                        {
                            if (map[i + 2][py] > 0 || map[i + 2][py + 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i + 1][py + 1] = cubevirtual; //右下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i + 1][py] = cube; //下
                                    map[i + 1][py + 1] = cube; //右下

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 3)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i][py + 1] == 0 && map[i][py - 1] == 0 && map[i + 1][py - 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i + 2][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i + 1][py - 1] = cubevirtual; //左下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i][py + 1] = cube; //右
                                    map[i][py - 1] = cube; //左
                                    map[i + 1][py - 1] = cube; //左下

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (cubetype == 5)  //反向L形
            {
                if (type == 0)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i + 1][py] == 0 && map[i - 1][py + 1] == 0)
                        {
                            if (map[i + 2][py] > 0 || map[i][py + 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i - 1][py + 1] = cubevirtual; //右上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i + 1][py] = cube; //下
                                    map[i - 1][py + 1] = cube; //右上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 1)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i][py + 1] == 0 && map[i][py - 1] == 0 && map[i + 1][py + 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py - 1] > 0 || map[i + 2][py + 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i + 1][py + 1] = cubevirtual; //右下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i][py + 1] = cube; //右
                                    map[i][py - 1] = cube; //左
                                    map[i + 1][py + 1] = cube; //右下

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 2)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i + 1][py] == 0 && map[i + 1][py - 1] == 0)
                        {
                            if (map[i + 2][py] > 0 || map[i + 2][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i + 1][py - 1] = cubevirtual; //左下

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i + 1][py] = cube; //下
                                    map[i + 1][py - 1] = cube; //左下

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 3)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i][py + 1] == 0 && map[i][py - 1] == 0 && map[i - 1][py - 1] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i + 1][py - 1] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i - 1][py - 1] = cubevirtual; //左上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i][py + 1] = cube; //右
                                    map[i][py - 1] = cube; //左
                                    map[i - 1][py - 1] = cube; //左上

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (cubetype == 6)  //一字形
            {
                if (type == 0)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i][py + 1] == 0 && map[i][py - 1] == 0 && map[i][py + 2] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i + 1][py - 1] > 0 || map[i + 1][py + 2] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i][py + 2] = cubevirtual; //右右

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i][py + 1] = cube; //右
                                    map[i][py - 1] = cube; //左
                                    map[i][py + 2] = cube; //右右

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 1)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i + 1][py] == 0 && map[i - 2][py] == 0)
                        {
                            if (map[i + 2][py] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i - 2][py] = cubevirtual; //上上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i + 1][py] = cube; //下
                                    map[i - 2][py] = cube; //上上

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 2)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i][py + 1] == 0 && map[i][py - 1] == 0 && map[i][py + 2] == 0)
                        {
                            if (map[i + 1][py] > 0 || map[i + 1][py + 1] > 0 || map[i + 1][py - 1] > 0 || map[i + 1][py + 2] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i][py + 1] = cubevirtual; //右
                                    map[i][py - 1] = cubevirtual; //左
                                    map[i][py + 2] = cubevirtual; //右右

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i][py + 1] = cube; //右
                                    map[i][py - 1] = cube; //左
                                    map[i][py + 2] = cube; //右右

                                    break;
                                }
                            }
                        }
                    }
                }
                if (type == 3)
                {
                    for (i = px; i < 21; i++)
                    {
                        if (map[i][py] == 0 && map[i - 1][py] == 0 && map[i + 1][py] == 0 && map[i - 2][py] == 0)
                        {
                            if (map[i + 2][py] > 0)
                            {
                                if (puttype == cubevirtual)
                                {
                                    map[i][py] = cubevirtual;
                                    map[i - 1][py] = cubevirtual; //上
                                    map[i + 1][py] = cubevirtual; //下
                                    map[i - 2][py] = cubevirtual; //上上

                                    break;
                                }

                                if (puttype == cube)
                                {
                                    map[i][py] = cube;
                                    map[i - 1][py] = cube; //上
                                    map[i + 1][py] = cube; //下
                                    map[i - 2][py] = cube; //上上

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        void CubeMove()     //方塊移動
        {
            switch (dir)
            {
                case 0:
                    px++;
                    break;
                case 1:
                    if (map[1][1] == 0 && map[2][1] == 0 && map[3][1] == 0)
                    {
                        py--;
                    }
                    break;
                case 2:
                    if (map[1][10] == 0 && map[2][10] == 0 && map[3][10] == 0)
                    {
                        py++;
                    }
                    break;
            }

            if (py == 10)
            {
                if (cubetype == 6 && type == 0)
                {
                    py -= 2;
                }
                if (cubetype == 6 && type == 2)
                {
                    py -= 2;
                }
            }

            movetype = cube;
            CubeTypeMove();

            if (map[px + 1][0] == 1 || map[px][0] == 1 || map[px - 1][0] == 1)
            {
                movetype = 0;
                CubeTypeMove();
                py++;
                movetype = cube;
                CubeTypeMove();
            }

            if (map[px + 1][11] == 1 || map[px][11] == 1 || map[px - 1][11] == 1)
            {
                movetype = 0;
                CubeTypeMove();
                py--;
                movetype = cube;
                CubeTypeMove();
            }

            for (i = 0; i < 22; i++)
            {
                for (j = 0; j < 12; j++)
                {
                    if (i == 0 || i == 21 || j == 0 || j == 11)
                    {
                        map[i][j] = bar;
                    }
                    else if (map[i][j] == 0 || map[i][j] == cubevirtual)
                    {
                        map[i][j] = 0;
                    }
                }
            }

            map[5][0] = losebar;
            map[5][11] = losebar;

            puttype = cubevirtual;

            CubeTypePut();

            GameLoad();
        }

        void CubePut()      //放置方塊
        {
            for (i = 0; i < 22; i++)
            {
                for (j = 0; j < 12; j++)
                {
                    if (i == 0 || i == 21 || j == 0 || j == 11)
                    {
                        map[i][j] = bar;
                    }
                    else if (map[i][j] == 0 || map[i][j] == cubevirtual)
                    {
                        map[i][j] = 0;
                    }
                }
            }

            puttype = cube;

            CubeTypePut();

            SwitchCube();
        }

        void CubeUp()   //對手消方塊受到攻擊
        {
            for (i = 5 + k; i < 21; i++)
            {
                for (j = 1; j < 11; j++)
                {
                    map[i - k][j] = map[i][j];
                }
            }
            for (i = 21 - k; i < 21; i++)
            {
                for (j = 1; j < 11; j++)
                {
                    map[i][j] = cube;
                }

                x = random.Next(10) + 1;

                map[i][x] = 0;
            }
        }

        void CubeStop() //方塊是否碰地
        {
            if (cubetype == 0)  //土字形
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py + 1] = cube; //右
                        map[px][py - 1] = cube; //左

                        SwitchCube();
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py + 1] > 0 || map[px + 2][py] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py + 1] = cube; //右
                        map[px + 1][py] = cube; //下

                        SwitchCube();
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py + 1] > 0 || map[px + 2][py] > 0 || map[px + 1][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px][py + 1] = cube; //右
                        map[px + 1][py] = cube; //下
                        map[px][py - 1] = cube; //左

                        SwitchCube();
                    }
                }
                if (type == 3)
                {
                    if (map[px + 2][py] > 0 || map[px + 1][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px + 1][py] = cube; //下
                        map[px][py - 1] = cube; //左

                        SwitchCube();
                    }
                }
            }
            if (cubetype == 1)  //田字形
            {
                if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0)
                {
                    map[px][py] = cube;
                    map[px - 1][py] = cube; //上
                    map[px][py - 1] = cube; //左
                    map[px - 1][py - 1] = cube; //左上

                    SwitchCube();
                }
            }
            if (cubetype == 2)  //Z字形
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0 || map[px][py + 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py - 1] = cube; //左
                        map[px - 1][py + 1] = cube; //右上

                        SwitchCube();
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py + 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py + 1] = cube; //右
                        map[px + 1][py + 1] = cube; //右下

                        SwitchCube();
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0 || map[px][py + 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py - 1] = cube; //左
                        map[px - 1][py + 1] = cube; //右上

                        SwitchCube();
                    }
                }
                if (type == 3)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py + 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py + 1] = cube; //右
                        map[px + 1][py + 1] = cube; //右下

                        SwitchCube();
                    }
                }
            }
            if (cubetype == 3)  //反向Z字
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py + 1] = cube; //右
                        map[px - 1][py - 1] = cube; //左上

                        SwitchCube();
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py - 1] = cube; //左
                        map[px + 1][py - 1] = cube; //左下

                        SwitchCube();
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py + 1] = cube; //右
                        map[px - 1][py - 1] = cube; //左上

                        SwitchCube();
                    }
                }
                if (type == 3)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px][py - 1] = cube; //左
                        map[px + 1][py - 1] = cube; //左下

                        SwitchCube();
                    }
                }
            }
            if (cubetype == 4)  //L形
            {
                if (type == 0)
                {
                    if (map[px + 2][py] > 0 || map[px][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px + 1][py] = cube; //下
                        map[px - 1][py - 1] = cube; //左上

                        SwitchCube();
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px][py + 1] = cube; //右
                        map[px][py - 1] = cube; //左
                        map[px - 1][py + 1] = cube; //右上

                        SwitchCube();
                    }
                }
                if (type == 2)
                {
                    if (map[px + 2][py] > 0 || map[px + 2][py + 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px + 1][py] = cube; //下
                        map[px + 1][py + 1] = cube; //右下

                        SwitchCube();
                    }
                }
                if (type == 3)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 2][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px][py + 1] = cube; //右
                        map[px][py - 1] = cube; //左
                        map[px + 1][py - 1] = cube; //左下

                        SwitchCube();
                    }
                }
            }
            if (cubetype == 5)  //反向L形
            {
                if (type == 0)
                {
                    if (map[px + 2][py] > 0 || map[px][py + 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px + 1][py] = cube; //下
                        map[px - 1][py + 1] = cube; //右上

                        SwitchCube();
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0 || map[px + 2][py + 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px][py + 1] = cube; //右
                        map[px][py - 1] = cube; //左
                        map[px + 1][py + 1] = cube; //右下

                        SwitchCube();
                    }
                }
                if (type == 2)
                {
                    if (map[px + 2][py] > 0 || map[px + 2][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px + 1][py] = cube; //下
                        map[px + 1][py - 1] = cube; //左下

                        SwitchCube();
                    }
                }
                if (type == 3)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0)
                    {
                        map[px][py] = cube;
                        map[px][py + 1] = cube; //右
                        map[px][py - 1] = cube; //左
                        map[px - 1][py - 1] = cube; //左上

                        SwitchCube();
                    }
                }
            }
            if (cubetype == 6)  //一字形
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0 || map[px + 1][py + 2] > 0)
                    {
                        map[px][py] = cube;
                        map[px][py + 1] = cube; //右
                        map[px][py - 1] = cube; //左
                        map[px][py + 2] = cube; //右右

                        SwitchCube();
                    }
                }
                if (type == 1)
                {
                    if (map[px + 2][py] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px + 1][py] = cube; //下
                        map[px - 2][py] = cube; //上上

                        SwitchCube();
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0 || map[px + 1][py + 2] > 0)
                    {
                        map[px][py] = cube;
                        map[px][py + 1] = cube; //右
                        map[px][py - 1] = cube; //左
                        map[px][py + 2] = cube; //右右

                        SwitchCube();
                    }
                }
                if (type == 3)
                {
                    if (map[px + 2][py] > 0)
                    {
                        map[px][py] = cube;
                        map[px - 1][py] = cube; //上
                        map[px + 1][py] = cube; //下
                        map[px - 2][py] = cube; //上上

                        SwitchCube();
                    }
                }
            }
        }

        bool YCanMove() //方塊是否碰地
        {
            if (cubetype == 0)  //土字形
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py + 1] > 0 || map[px + 2][py] > 0)
                    {
                        return true;
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py + 1] > 0 || map[px + 2][py] > 0 || map[px + 1][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 3)
                {
                    if (map[px + 2][py] > 0 || map[px + 1][py - 1] > 0)
                    {
                        return true;
                    }
                }
            }
            if (cubetype == 1)  //田字形
            {
                if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0)
                {
                    return true;
                }
            }
            if (cubetype == 2)  //Z字形
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0 || map[px][py + 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py + 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0 || map[px][py + 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 3)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py + 1] > 0)
                    {
                        return true;
                    }
                }
            }
            if (cubetype == 3)  //反向Z字
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 3)
                {
                    if (map[px + 1][py] > 0 || map[px + 2][py - 1] > 0)
                    {
                        return true;
                    }
                }
            }
            if (cubetype == 4)  //L形
            {
                if (type == 0)
                {
                    if (map[px + 2][py] > 0 || map[px][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 2)
                {
                    if (map[px + 2][py] > 0 || map[px + 2][py + 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 3)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 2][py - 1] > 0)
                    {
                        return true;
                    }
                }
            }
            if (cubetype == 5)  //反向L形
            {
                if (type == 0)
                {
                    if (map[px + 2][py] > 0 || map[px][py + 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 1)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py - 1] > 0 || map[px + 2][py + 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 2)
                {
                    if (map[px + 2][py] > 0 || map[px + 2][py - 1] > 0)
                    {
                        return true;
                    }
                }
                if (type == 3)
                {
                    return true;
                }
            }
            if (cubetype == 6)  //一字形
            {
                if (type == 0)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0 || map[px + 1][py + 2] > 0)
                    {
                        return true;
                    }
                }
                if (type == 1)
                {
                    if (map[px + 2][py] > 0)
                    {
                        return true;
                    }
                }
                if (type == 2)
                {
                    if (map[px + 1][py] > 0 || map[px + 1][py + 1] > 0 || map[px + 1][py - 1] > 0 || map[px + 1][py + 2] > 0)
                    {
                        return true;
                    }
                }
                if (type == 3)
                {
                    if (map[px + 2][py] > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool XCanMove(int d)    //X軸是否可移動
        {
            if (cubetype == 0)  //土字形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 2] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 2] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 2] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 2] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 1)  //田字形
            {
                if (d == 1)
                {
                    if (map[px][py - 2] == 0 && map[px - 1][py - 2] == 0)
                    {
                        return true;
                    }
                }
                if (d == -1)
                {
                    if (map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                    {
                        return true;
                    }
                }
            }
            if (cubetype == 2)  //Z字形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 2] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 1] == 0 && map[px - 1][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 2] == 0 && map[px][py + 2] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 2] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 1] == 0 && map[px - 1][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 2] == 0 && map[px][py + 2] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 3)  //反向Z字
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px - 1][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 2] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 2] == 0 && map[px][py - 2] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px - 1][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 2] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 2] == 0 && map[px][py - 2] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 4)  //L形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 2] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 2] == 0 && map[px - 1][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 2] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 2] == 0 && map[px][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py] == 0 && map[px][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 5)  //反向L形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py] == 0 && map[px][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 2] == 0 && map[px][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 2] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 2] == 0 && map[px - 1][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 2] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 6)  //一字形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 3] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 2][py - 1] == 0 && map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 2][py + 1] == 0 && map[px + 1][py + 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 3] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 2][py - 1] == 0 && map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 2][py + 1] == 0 && map[px + 1][py + 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        bool CubeRotate(int d)  //方塊轉動判定
        {
            if (cubetype == 0)  //土字形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 1)  //田字形
            {
                return true;
            }
            if (cubetype == 2)  //Z字形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py - 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py - 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 3)  //反向Z字
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px][py + 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px][py + 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py + 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 4)  //L形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py] == 0 && map[px - 1][py] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px][py - 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py] == 0 && map[px - 1][py] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 5)  //反向L形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py - 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py] == 0 && map[px - 1][py] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px - 1][py - 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py + 1] == 0 && map[px][py - 1] == 0 && map[px][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 1][py] == 0 && map[px - 1][py] == 0 && map[px - 1][py + 1] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 1][py - 1] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            if (cubetype == 6)  //一字形
            {
                if (type == 0)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 1)
                {
                    if (d == 1)
                    {
                        if (map[px + 2][py] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 2][py] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 2)
                {
                    if (d == 1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px][py - 1] == 0 && map[px][py + 1] == 0 && map[px][py + 2] == 0)
                        {
                            return true;
                        }
                    }
                }
                if (type == 3)
                {
                    if (d == 1)
                    {
                        if (map[px + 2][py] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                    if (d == -1)
                    {
                        if (map[px + 2][py] == 0 && map[px + 1][py] == 0 && map[px - 1][py] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        void CubeClear()    //消除方塊判定
        {
            int cubedown = new int();   //下降格數
            int cubedownstart = new int();  //下降起始點

            for (i = 0; i < 22; i++)
            {
                int cubefull = new int();
                for (int j = 1; j < 11; j++)
                {
                    cubefull += map[i][j];
                }
                if (cubefull == 10)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        map[i][j] = 0;
                    }
                    cubedown += 1;
                    cubedownstart = i;
                }
            }

            if (cubedown > 0)
            {
                for (i = cubedownstart - cubedown; i > 3; i--)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        for (k = 4; k < 4 + cubedown; k++)
                        {
                            map[k][j] = 0;
                        }
                        map[i + cubedown][j] = map[i][j];
                    }
                }
                if (cubetype == 6)
                {
                    if (type == 1 || type == 3)
                    {
                        map[px + 2][py] = cube;
                    }
                }

                clientSocket.Send(Encoding.ASCII.GetBytes($"u{cubedown}"));
            }

        }

        void SwitchCube()   //更換隨機方塊
        {
            if (cubetype == 6)
            {
                if (type == 0 || type == 2)
                {
                    map[px][py + 2] = 0; //右右
                }
                if (type == 1 || type == 3)
                {
                    map[px + 2][py] = 0; //下下
                }
            }

            dir = 3;
            cubelist.Remove(cubelist[0]);
            cubelist.Add(random.Next(7));
            cubetype = cubelist[0];
            type = 0;


            px = 2;

            CubeMove();

            cubeclear = true;

            GameLoad();

            CubeCheck();
        }

        void CubeCheck()    //超過邊界判斷
        {
            for (i = 1; i < 11; i++)
            {
                if (map[5][i] != 0)
                {
                    clientSocket.Send(Encoding.ASCII.GetBytes($"d"));

                    break;
                }
            }
        }

        void PrintMap()     //繪製遊戲畫面
        {
            gameScreen(rtb_gameScreen, 0, "");
            //rtb_gameScreen.Clear();

            int i, j;
            //根據地圖上每點的情況繪製遊戲（ j 表示 x 軸，i 表示 y 軸），按行列印

            for (i = 0; i < 2; i++)
            {
                gameScreen(rtb_gameScreen, 1, "\n");
                //rtb_gameScreen.Text += ("\n");
            }

            for (i = 0; i < 22; i++)
            {
                //rtb_gameScreen.Text += ("          ");
                for (j = 0; j < 12; j++)
                {
                    //空白地方
                    if (map[i][j] == 0)
                    {
                        gameScreen(rtb_gameScreen, 1, "     ");
                        //rtb_gameScreen.Text += ("     ");
                    }
                    //方塊
                    else if (map[i][j] == cube)
                    {
                        gameScreen(rtb_gameScreen, 1, " ■");
                        //rtb_gameScreen.Text += (" ■");
                    }
                    //邊框
                    else if (map[i][j] == bar)
                    {
                        gameScreen(rtb_gameScreen, 1, " ◆");
                        //rtb_gameScreen.Text += (" ◆");
                    }
                    //方塊(要下的地方)
                    else if (map[i][j] == cubevirtual)
                    {
                        gameScreen(rtb_gameScreen, 1, " □");
                        //rtb_gameScreen.Text += (" □");
                    }
                    else if (map[i][j] == losebar)
                    {
                        gameScreen(rtb_gameScreen, 1, " ◇");
                        //rtb_gameScreen.Text += (" ◇");
                    }
                }
                gameScreen(rtb_gameScreen, 1, "\n");
                //rtb_gameScreen.Text += ('\n');

            }

            flp(flp_cubenext, 0, null);
            //flp_cubenext.Controls.Clear();

            for (i = 0; i < 5; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Image = cubenextimages[cubelist[i]];
                flp(flp_cubenext, 1, pictureBox);
                //flp_cubenext.Controls.Add(pictureBox);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gamestart)
            {
                dir = 0;

                movetype = 0;
                CubeTypeMove();

                CubeStop();

                CubeMove();
            }
            
        }

        private void Tetris_Battle_tpm_room__KeyDown(object sender, KeyEventArgs e)
        {
            if (gamestart == true)
            {
                dir = 3;

                if (e.KeyCode == Keys.A)
                {
                    if (XCanMove(1))
                    {
                        dir = 1;
                    }
                }
                if (e.KeyCode == Keys.D)
                {
                    if (XCanMove(-1))
                    {
                        dir = 2;
                    }
                }

                movetype = 0;
                CubeTypeMove();

                if (e.KeyCode == Keys.W)
                {
                    if (CubeRotate(1))
                    {
                        dir = 3;
                        type += 1;
                        if (type == 4)
                        {
                            type = 0;
                        }
                    }
                }
                if (e.KeyCode == Keys.S)
                {
                    if (CubeRotate(-1))
                    {
                        dir = 3;
                        type -= 1;
                        if (type == -1)
                        {
                            type = 3;
                        }
                    }
                }
                if (e.KeyCode == Keys.J)
                {
                    dir = 3;
                    CubePut();
                }
                if (e.KeyCode == Keys.K)
                {
                    if (cubetype == 6)
                    {
                        if (type == 0 || type == 2)
                        {
                            map[px][py + 2] = 0; //右右
                        }
                        if (type == 1 || type == 3)
                        {
                            map[px + 2][py] = 0; //下下
                        }
                    }
                    dir = 0;
                    cubetype = random.Next(7);
                    type = 0;

                    GameLoad();
                }
                if (e.KeyCode == Keys.E)
                {
                    if (YCanMove() == false)
                    {
                        dir = 0;
                    }
                }

                CubeMove();
            }
            if (e.KeyCode == Keys.L)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }


        }
    }
}
