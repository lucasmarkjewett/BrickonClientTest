using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using BasicTriangle;
using OpenTK.Graphics;

namespace BrickonEditor
{
    [MoonSharpUserData]
    class BrickonApi
    {
        public string s5(float x)
        { return " " + x; }
        public void addVertex(float x,float y,float z)
        {
            Program.actionMessage += "vert" + s5(x) + s5(y) + s5(z);
            //Program.Points.Add(new Vector3(x, y, z));

        }
        public void addBlock(float x, float y, float z,float r,float g,float b)
        {
            Program.actionMessage += " block" + s5(x) + s5(y) + s5(z) + s5(r) + s5(g) + s5(b);
            //Program.Points.Add(new Vector3(x, y, z));

        }
        public void addBlockv1(float x, float y, float z,float Xh,float Yy,float Zz ,float r, float g, float b)
        {
            Program.actionMessage += " blockdt" + s5(x) + s5(y) + s5(z)+ s5(Xh)+ s5(Yy) + s5(Zz) + s5(r) + s5(g) + s5(b);
            //Program.Points.Add(new Vector3(x, y, z));

        }
        public void actionSet(string dt)
        { Program.actionMessage = dt; }
        public void addVertexCol(float x, float y, float z, float w)
        { Program.Col.Add(new Vector4(x, y, z,w)); }

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

        
        public void addRect(float x, float y,int r,int g,int b)
        { Program.panel(new Vector2(x, y), new Vector2(1, 1), new Color4(r, g, b, 1)); }
    }
}
