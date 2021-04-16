using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Player
    {





        public float x = 1;
        public float y = 1;
        public float z = 1;
        public int id = 4;
        public int hearths = 100;
        public string actionCode;

        public string username = "BOBUX";
        public void setX(float x_)
        {
            x = x_;
        }

        public void setUsername(string string_)
        {
            username = string_;
        }
        public Player(string nick)
        {
            username = nick;
        }
    }
}
