using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

//就是玩家要吃的那个食物，里面实现了两个方法：
//食物的显示与食物的消失(因为食物被吃了以后应该消失)
namespace snakes
{
    //食物类
    public class Bean
    {
        //用于画食物的顶端坐标
        private Point _origin;
        //设置为public，允许外部访问
        public Point Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        //显示食物
        public void ShowBean(Graphics g,Mode mode)
        {
            Bitmap bitmap;
            if (mode == Mode.Normal1 || mode == Mode.Normal2)
            {
                bitmap = new Bitmap("images//Apple.png");
                //食物
                g.DrawImage(bitmap, Origin.X, Origin.Y, 15, 15);
            }
            else
            {
                bitmap = new Bitmap("images//CorruptApple.png");
                g.DrawImage(bitmap, Origin.X, Origin.Y, 20, 20);
            }
        }

        public void UnShowBean(Graphics g)
        {
            //定义系统背景颜色的画笔
            SolidBrush brush = new SolidBrush(Color.Transparent);
            //画实心矩形颜色为系统背景颜色，相当于食物被吃掉了
            g.FillRectangle(brush, Origin.X, Origin.Y, 15, 15);
        }
    }
}
