using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;
//用于画出游戏主界面，通过定时器来控制，每个单元时间重新画一幅地图，用于动态显示
namespace snakes
{
    public enum Mode
    {
        Normal1, Normal2, Devil1, Devil2
    }
    public class Map
    {
        
        private bool _gameStart = false;

        public bool GameStart
        {
            get { return _gameStart; }
            set { _gameStart = value; }
        }
        private Point mapLeft;
        //定义模式
        private Mode _mode = Mode.Normal1;

        public Mode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private static int unit = 15;
        //定义地图长，为30个蛇块的长度
        private readonly int length = 41 * unit;
        //定义地图宽，为25个蛇块的宽度
        private readonly int width = 23 * unit;
        //定义分数，初始分数为0
        public int score = 0;
        //定义蛇
        private readonly Snake snake;
        public bool victory = false;
        public Snake Snake
        {
            get { return snake; }
        }

        Bean food;
        public Map(Point start)
        {
            //把地图左上角的点的值赋值给全局变量
            mapLeft = start;
            //实例化蛇
            snake = new Snake(start, 5);
            //实例化食物
            food = new Bean();
            food.Origin = new Point(start.X + 30, start.Y + 30);
        }
        //显示新食物
        public void ShowNewFood(Graphics g)
        {
            //消除原先食物
            food.UnShowBean(g);
            //产生随机位置的食物
            food = FoodRandom();
            //显示食物
            food.ShowBean(g,_mode);
        }
        //随机产生一个新食物
        private Bean FoodRandom()
        {
            Random d = new Random();
            int x = d.Next(0, length / unit);
            int y = d.Next(0, width / unit);
            Point origin = new Point(mapLeft.X + x * 15, mapLeft.Y + y * 15);
            Bean food = new Bean();
            food.Origin = origin;
            return food;
        }
        //画地图
        public void ShowMap(Graphics g)
        {
            ////创建一支红笔
            //Pen pen = new Pen(Color.Blue);
            ////画出地图的框
            //g.DrawRectangle(pen, mapLeft.X, mapLeft.Y, length, width);
            //显示食物
            food.ShowBean(g,_mode);
            if (CheckBean())
            {
                Random rand = new Random(DateTime.Now.Second);
                int i = rand.Next(0, 7);
                if (_mode == Mode.Devil2)
                    PlaySound("Sounds\\短笑"+i+".wav");
                //吃到了食物
                //显示新食物
                ShowNewFood(g);
                //蛇变长
                snake.SnakeGrowth();
                //分数增加
                score += 10;
                //显示蛇
                snake.ShowSnake(g);
            }
            else
            {
                //保持不变
                snake.Go(g);
                snake.ShowSnake(g);
            }
        }
        //显示新食物
        public void Showfood(Graphics g)
        {
            ShowNewFood(g);
        }
        //判断是否吃到了食物
        public bool CheckBean()
        {
            return food.Origin.Equals(snake.HeadPoint);
        }

        //检查蛇是否撞墙
        public bool CheckSnake()
        {
            return !(snake.getHeadPoint.X > mapLeft.X - 5 && snake.getHeadPoint.X < (mapLeft.X + length - 5) && snake.getHeadPoint.Y > mapLeft.Y - 5 && snake.getHeadPoint.Y < (mapLeft.Y + width - 5));
        }

        //重新开始
        public void Reset(Graphics g)
        {
            snake.UnShowSnake(g);
            snake.Reset(mapLeft, 5);
        }
        //自定义类方法：播放声音文件
        public void PlaySound(string wavFile)
        {
            //装载声音文件（需要添加system.media明明空间）
            SoundPlayer soundPlay = new SoundPlayer(wavFile);
            //使用新线程播放声音
            soundPlay.Play();//注意：soundPlay.PlaySync()也可以播放声音，该方法使用用户界面
        }
    }
}
