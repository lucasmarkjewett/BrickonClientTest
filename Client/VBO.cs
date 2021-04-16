using BasicTriangle;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickonEditor
{
    public class Model
    {




        List<Vector3> Points = new List<Vector3>();
        List<Vector4> Col = new List<Vector4>();

        int VertexBufferObject;
        int VertexArrayObject;
        int VertexColBuffer;
        public Model(string file)
        { Brickon.Model.Model.Load(file, out Points, out Col); }

        public void setup1()
        {
            VertexArrayObject = GL.GenVertexArray();

            VertexColBuffer = GL.GenBuffer();
            VertexBufferObject = GL.GenBuffer();

            
            GL.BindVertexArray(VertexArrayObject);

            //No clue what I did exactly but array locations might be hardcoded in the shader
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            //Set some info for the shader to know how big each buffer, how many bytes each element take, their offset, etc their kind
            //Bind the buffer we want to be used in the "slot" we specify in the next call
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //We have a vector 3 for the positions so 3 floats, not normalized, the stride is 3 because we don't store anything else in this buffer and offset 0 because we start from start
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            //Same but for the colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexColBuffer);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            //Some bad equivalent to GL.End
            //Bind the buffer which to be used
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //Send data to bound buffer in the specified "slot"
            //The kind of buffer, The size of the array i(the array length * size of an element), The data(because we had a list we convert it to an array), Buffer Usage, doesn't matter that much nowadays
            GL.BufferData(BufferTarget.ArrayBuffer, Points.Count* Vector3.SizeInBytes, Points.ToArray(), BufferUsageHint.StaticDraw);
            //Same thing but for colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexColBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Col.Count* Vector4.SizeInBytes, Col.ToArray(), BufferUsageHint.StaticDraw);
           
        }
        public void setup()
        {
            
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //Send data to bound buffer in the specified "slot"
            //The kind of buffer, The size of the array i(the array length * size of an element), The data(because we had a list we convert it to an array), Buffer Usage, doesn't matter that much nowadays
            GL.BufferData(BufferTarget.ArrayBuffer, Points.Count* Vector3.SizeInBytes, Points.ToArray(), BufferUsageHint.StaticDraw);
            //Same thing but for colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexColBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Col.Count* Vector4.SizeInBytes, Col.ToArray(), BufferUsageHint.StaticDraw);
        }
        public void renderModel()
        {
            GL.BindVertexArray(VertexArrayObject);
        }
        public void render()
        {
            
            GL.DrawArrays(PrimitiveType.Quads, 0, Points.Count);
        }
    }
    public static class ModelService
    {
        public static void InstertModel(Model model,Vector3 scale,Vector3 pos,Vector3 col)
        {
            model.renderModel();
            Vector3 feetCol = col;//new Vector3(1, 0, 1)
            GL.Uniform3(GL.GetUniformLocation(Program.ShaderProgram, "col"), ref feetCol);
            Program.model = Matrix4.Identity * Matrix4.CreateScale(scale)* Matrix4.CreateTranslation(pos) * Matrix4.CreateRotationY(Program.Camera.Facing * 5) * Matrix4.CreateRotationX(Program.Camera.Pitch * 5);
            GL.UniformMatrix4(GL.GetUniformLocation(Program.ShaderProgram, "model"), true, ref Program.model);
            model.render();
        }

    }
}
