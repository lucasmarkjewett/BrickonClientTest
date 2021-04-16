using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickonEditor
{
    public static class TextRenderer
    {
        public static void char_(Vector3 pos, Vector3 scale, Color4 col,Color4 coldt, Vector4 rotation, int char__)
        {
            GL.PushMatrix();
            GL.Scale(scale);//0.05f0.05f
            GL.Rotate(rotation.W, rotation.Xyz);
            GL.Translate(new Vector3(-pos.X - scale.X, pos.Y, pos.Z));


            float uv_x = (char__ % 16) / 16.0f;
            float uv_y = (char__ / 16) / 16.0f;

            // if (char__ == 'W')
            

            GL.Begin(BeginMode.Quads);
            //GL.End();
            GL.Color4(col);//Color.White




            //GL.Begin(BeginMode.Quads);
         
                //GL.End();

            GL.End();
            GL.Disable(EnableCap.Blend);
           // GL.Disable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Quads);
            GL.Color4(coldt);
            GL.TexCoord2(uv_x, 1.0f - uv_y); GL.Vertex3(-1f, -1f, 4);
            GL.TexCoord2(uv_x + 1.0f / 16.0f, 1.0f - uv_y); GL.Vertex3(1f, -1f, 4);
            GL.TexCoord2(uv_x + 1.0f / 16.0f, 1.0f - (uv_y + 1.0f / 16.0f)); GL.Vertex3(1f, 1f, 4);
            GL.TexCoord2(uv_x, 1.0f - (uv_y + 1.0f / 16.0f)); GL.Vertex3(-1f, 1f, 4);
            GL.End();
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.PopMatrix();

        }

    }
}
