using BasicTriangle;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickonEditor
{
    public static class Shapes
    {
        public static void block(Vector4 col)
        {
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(1, 1, -1));
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(-1, 1, -1));
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(-1, 1, 1));
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(1, 1, 1));

            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(1, -1, 1));
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(-1, -1, 1));
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(-1, -1, -1));
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(1, -1, -1));

            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(1.0f, 1.0f, 1.0f));
            Program.Points.Add(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f));
            Program.Points.Add(new OpenTK.Vector3(-1.0f, -1.0f, 1.0f));
            Program.Points.Add(new OpenTK.Vector3(1.0f, -1.0f, 1.0f));


            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(1.0f, -1.0f, -1.0f));
            Program.Points.Add(new OpenTK.Vector3(-1.0f, -1.0f, -1.0f));
            Program.Points.Add(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f));
            Program.Points.Add(new OpenTK.Vector3(1.0f, 1.0f, -1.0f));

            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(-1.0f, 1.0f, 1.0f));
            Program.Points.Add(new OpenTK.Vector3(-1.0f, 1.0f, -1.0f));
            Program.Points.Add(new OpenTK.Vector3(-1.0f, -1.0f, -1.0f));
            Program.Points.Add(new OpenTK.Vector3(-1.0f, -1.0f, 1.0f));
            Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col); Program.Col.Add(col);
            Program.Points.Add(new OpenTK.Vector3(1.0f, 1.0f, -1.0f));
            Program.Points.Add(new OpenTK.Vector3(1.0f, 1.0f, 1.0f));
            Program.Points.Add(new OpenTK.Vector3(1.0f, -1.0f, 1.0f));
            Program.Points.Add(new OpenTK.Vector3(1.0f, -1.0f, -1.0f));
        }
    }
}
