using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenToolkit.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;


namespace Movement
{
    public class Camera
    {
        public Vector3 Position ;
        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public float Z
        {
            get { return Position.Z; }
            set { Position.Z = value; }
        }
        public Vector3 Up = Vector3.UnitY;
        public Matrix4 CameraMatrix = Matrix4.Identity;
        public Matrix4 CameraMatrixh = Matrix4.Identity;
        public float Pitch = 0;
        public float Facing = 0;
        public float HorizontalSensitivity = 3;
        public float VerticalSensitivity = 6;
        public float Fog = 10000;
        public Point ScreenCenter { get { return new Point(Window.Bounds.Left + (Window.Bounds.Width / 2), Window.Bounds.Top + (Window.Bounds.Height / 2)); } }
        public Point WindowCenter { get { return new Point(Window.Width / 2, Window.Height / 2); } }
        public Point MouseDelta { get; private set; }
        public GameWindow Window { get; private set; }
        public Vector3 lookatPoint;
        public Vector3 dt;
        public Camera() { }
        public Camera(GameWindow window, float x, float y, float z) : this(window, new Vector3(x, y, z)) { }
        public Camera(GameWindow window, Vector3 position) : this(window, position, Vector3.UnitY) { }
        public Camera(GameWindow window, Vector3 position, Vector3 up)
        {
            Window = window;
            Position = position;
            
            Up = up;

            MouseDelta = new Point();

            /*Window.Resize += (sender, e) =>
            {
                Mouse.SetPosition(ScreenCenter.X,ScreenCenter.Y);
                

                GL.Viewport(Window.ClientRectangle.X, Window.ClientRectangle.Y, Window.ClientRectangle.Width, Window.ClientRectangle.Height);

               /* Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Window.Width / (float)Window.Height, 1f, Fog);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projection);*/
          /*  };*/

            Window.RenderFrame+= (sender, e) =>
            {
 
                // Mouse.SetPosition( p.X, p.Y);
                if(BasicTriangle.Program.mtdts)
                {
                    Facing = MathHelper.DegreesToRadians(Mouse.GetState().X);
                Pitch = MathHelper.DegreesToRadians(-Mouse.GetState().Y) ;

                lookatPoint = new Vector3((float)Math.Cos(Facing ), (float)Math.Tan(Pitch) , (float)Math.Sin(Facing ));
                    Vector3 lookatPointh = new Vector3(0, 0, (float)Math.Sin(15));//(float)Math.Cos(Facing)
                    dt = Position + lookatPoint;
                    CameraMatrix = Matrix4.LookAt(new Vector3(0,-15,0),  lookatPointh, Up);
                    CameraMatrixh = Matrix4.LookAt(Position,Position+ lookatPointh, Up);
                   // Console.WriteLine("bruh");
                }
            };
        }
    }
}