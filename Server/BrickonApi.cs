using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [MoonSharpUserData]
    public class BrickonApi
    {
        public void actionSet(string dt)
        { Server.actionMessage = dt; }
        public string s5(float x)
        { return " " + x; }

        public void addBlock(float x, float y, float z, float r, float g, float b)
        {
            Server.actionMessage += " block" + s5(x) + s5(y) + s5(z) + s5(r) + s5(g) + s5(b);
            //Program.Points.Add(new Vector3(x, y, z));

        }
        public void addBlockv1(float x, float y, float z, float Xh, float Yy, float Zz, float r, float g, float b)
        {
            Server.actionMessage += " blockdt" + s5(x) + s5(y) + s5(z) + s5(Xh) + s5(Yy) + s5(Zz) + s5(r) + s5(g) + s5(b);
            //Program.Points.Add(new Vector3(x, y, z));

        }

        public async void wait(int time, DynValue callback)
        {
            await Task.Delay(time);

            if (callback.Type == DataType.Function)
            {
                callback.Function.Call();
            }
        }
        public async void while_(int time, DynValue callback)
        {
            //if(time>15)
            //{
            while (true)
            {




                if (callback.Type == DataType.Function)
                {

                    callback.Function.Call();
                }

                await Task.Delay(time);


            }
            //}
        }


    }
}
