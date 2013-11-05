using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace The_Game
{
    class Speed
    {
        public double x, y;
        public double abs()
        {
            return Math.Sqrt(x*x+y*y);
        }
        public Speed(double slozkax,double slozkay)
        {
            x = slozkax;
            y = slozkay;
        }
        public Speed add(Speed b)
        {
            Speed temp = new Speed(x + b.x, y + b.y);
            return temp;
        }
    }
}
