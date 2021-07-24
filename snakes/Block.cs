using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

//就是组成蛇的单元，玩家吃到食物，蛇的蛇块增加一节。
namespace snakes
{
    //蛇身体的每一单元，简称块
    public class Block
    {
        

        //是否为蛇头
        private bool _isHead;
        //重构封装字段，允许外部访问
        public bool IsHead
        {
            get { return _isHead; }
            set { _isHead = value; }
        }
        //蛇块的编号
        private int _blockNumber;

        public int BlockNumber
        {
            get { return _blockNumber; }
            set { _blockNumber = value; }
        }
        //蛇块的左上角位置
        private Point _origin;

        public Point Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        //根据指定位置画蛇块
        public void ShowBlock(Graphics g,int direction)
        {
            //图片
            Bitmap bitMap;
            if (IsHead)
            {
                //蛇头
                bitMap = new Bitmap("images//Head" + direction + ".png");
            }
            else
            {
                //蛇的身体部分
                bitMap = new Bitmap(Image.FromFile("images//Body.png"));
            }
            bitMap.MakeTransparent(Color.White);
            g.DrawImage(bitMap, Origin.X, Origin.Y, 15, 15);
        }

        //消除蛇块
        public void UnShowBlock(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color.Transparent);
            g.FillRectangle(brush, Origin.X, Origin.Y, 15, 15);
        }
    }
}
