using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using System.Runtime.InteropServices;  //DllImport命名空间的引用   

namespace snakes
{
  
    public partial class FormMain : Form
    {
        [DllImport("winmm.dll")]
        public static extern bool PlaySound(String Filename, int Mod, int Flags);
        public FormMain()
        {
            InitializeComponent();
            //定义地图
            map = new Map(new Point(63,72));
            //定义背景颜色
            BackColor = Color.Silver;

        }

        private bool _setting=false;
        private readonly Map map;
        private int gradeNum = 100;
        private int _count = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (map.Mode == Mode.Devil2)
            {
                Random rand = new Random(DateTime.Now.Second);
                int interval = rand.Next(50, 90);
                timer1.Interval = interval;
            }

            lblScore.Text = map.score.ToString();
            //分数大于500，游戏结束
            if (map.score >= 150)
            {
                PlaySound("Sounds\\Win"+((int)(map.Mode)+1).ToString()+".wav");
                timer1.Enabled = false;
                MessageBox.Show("恭喜，成功！！！");
                timer1.Enabled = false;
                map.GameStart = false;
                //更换背景
                pictureBox1.BackgroundImage = Properties.Resources.StartBack;
                //影藏标签
                label5.Visible = false;
                lblGrade.Visible = false;
                lblScore.Visible = false;
                label2.Visible = false;
            }
            Bitmap bitmap = null;
            if (map.Mode == Mode.Normal1 || map.Mode == Mode.Normal2)
                bitmap = Properties.Resources.PlayBack2_1;
            else
                bitmap = Properties.Resources.PlayBackDark;
            Bitmap bmp = new Bitmap(bitmap, Width, Height);
            pictureBox1.BackgroundImage = bmp;
            Graphics g = Graphics.FromImage(bmp);
            map.ShowMap(g);
            if (map.Mode == Mode.Devil1)
            {
                _count++;
                if (_count == 37)
                {
                    _count = 0;
                    map.Showfood(g);
                }
            }
            //如果运动过程中撞到墙或者撞到自身，游戏结束，关闭timer1
            if (map.CheckSnake()||map.Snake.IsTouchMyself())
            {  //播放相应的声音文件
                if (map.Mode == Mode.Normal1 || map.Mode == Mode.Normal2)
                    PlaySound("Sounds\\GameOverBaby.wav");
                else if(map.Mode == Mode.Devil1)
                    PlaySound("Sounds\\GameOverDevil.wav");
                else
                    PlaySound("Sounds\\狂笑.wav");
                timer1.Enabled = false;
                map.GameStart = false;
                MessageBox.Show("很遗憾，失败了！！！");
                //更换背景
                
                pictureBox1.BackgroundImage = Properties.Resources.StartBack;
                //影藏标签
                label5.Visible = false;
                lblGrade.Visible = false;
                lblScore.Visible = false;
                label2.Visible = false;
            }
            
        }


        //自定义类方法：播放声音文件
        public void PlaySound(string wavFile)
        {
            System.Media.SoundPlayer sp = new SoundPlayer();
            //装载声音文件（需要添加system.media明明空间）
            sp = new SoundPlayer(wavFile);
            //使用新线程播放声音
            sp.Play();//注意：soundPlay.PlaySync()也可以播放声音，该方法使用用户界面
        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!map.GameStart && !_setting)
            {
                //碰到了开始按键
                if (Math.Sqrt(Math.Pow(e.X - 357, 2) + Math.Pow(e.Y - 207, 2)) <= 65)
                {
                    if (map.Mode == Mode.Normal1 || map.Mode == Mode.Normal2)
                        pictureBox1.BackgroundImage = Properties.Resources.StartBackShining;
                    else 
                        pictureBox1.BackgroundImage = Properties.Resources.StartBackShiningDark;
                }
                else if (Math.Sqrt(Math.Pow(e.X - 53, 2) + Math.Pow(e.Y - 373, 2)) <= 42)
                {
                    if (map.Mode == Mode.Normal1 || map.Mode == Mode.Normal2)
                        pictureBox1.BackgroundImage = Properties.Resources.StartBackSetting;
                    else
                        pictureBox1.BackgroundImage = Properties.Resources.StartBackSettingDark;
                }
                else
                {
                    if (map.Mode == Mode.Normal1 || map.Mode == Mode.Normal2)
                        pictureBox1.BackgroundImage = Properties.Resources.StartBack;
                    else 
                        pictureBox1.BackgroundImage = Properties.Resources.StartBackDark;

                }

            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!map.GameStart)
            {
                if (Math.Sqrt(Math.Pow(e.X - 357, 2) + Math.Pow(e.Y - 207, 2)) <= 50&&!_setting)
                {
                    System.Media.SoundPlayer sp = new SoundPlayer();
                    if (map.Mode == Mode.Normal1 || map.Mode == Mode.Normal2)
                        sp.SoundLocation = "BGMNormal.wav";
                    else if(map.Mode==Mode.Devil1)
                        sp.SoundLocation = "BGMDevil1.wav";
                    else 
                        sp.SoundLocation = "BGMDevil2.wav";
                    sp.PlayLooping();
                    //焦点
                    this.Focus();
                    map.GameStart = true;
                    timer1.Enabled = true;
                    map.Reset(CreateGraphics());
                    map.score = 0;
                    //显示标签
                    label5.Visible = true;
                    lblGrade.Visible = true;
                    lblScore.Visible = true;
                    label2.Visible = true;

                }
                //设置
                else if (Math.Sqrt(Math.Pow(e.X - 53, 2) + Math.Pow(e.Y - 373, 2)) <= 42)
                {
                    _setting = true;
                    if (map.Mode == Mode.Normal1 || map.Mode == Mode.Normal2)
                        pictureBox1.BackgroundImage = Properties.Resources.Setting3_1;
                    else 
                        pictureBox1.BackgroundImage = Properties.Resources.Setting3;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    button4.Visible = true;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            map.Mode = Mode.Normal1;
            gradeNum = 130;
            lblGrade.Text = "婴儿";
            //设定计时器的时间间隔为gradeNum
            //也就是间隔越长，游戏越简单
            //选择级别之后，会在级别的地方有所显示
            timer1.Interval = gradeNum;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            _setting = false;
            pictureBox1.BackgroundImage = Properties.Resources.StartBack;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            map.Mode = Mode.Normal2;
            gradeNum = 95;
            lblGrade.Text = "孩子";
            timer1.Interval = gradeNum;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            _setting = false;
            pictureBox1.BackgroundImage = Properties.Resources.StartBack;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            map.Mode = Mode.Devil1;
            gradeNum = 100;
            lblGrade.Text = "捣蛋鬼";
            timer1.Interval = gradeNum;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            _setting = false;
            pictureBox1.BackgroundImage = Properties.Resources.StartBack;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            map.Mode = Mode.Devil2;
            gradeNum = 50;
            lblGrade.Text = "小恶魔";
            timer1.Interval = gradeNum;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            _setting = false;
            pictureBox1.BackgroundImage = Properties.Resources.StartBack;
        }


        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            //用键盘控制蛇的运动
            int k, d = 0;
            k = e.KeyValue;
            if (k == 37) //左
                d = 3;
            else if (k == 40) //下
                d = 2;
            else if (k == 38) //上
                d = 0;
            else if (k == 39) //右
                d = 1;
            map.Snake.TurnDirection(d);
        }


    }
}
