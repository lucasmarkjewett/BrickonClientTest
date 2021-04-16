using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using BrickonEditor;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Brickon.Inferface;
using Brickon.Model;
using System.Net;
using MoonSharp.Interpreter;
using Movement;
using Network;
using Shared;
using System.Threading;
using System.Linq;

namespace BasicTriangle
{
    sealed class Program : GameWindow
    {

        int mouse_x, mouse_y;

        bool mouse =true;
        public static bool mtdts =true;

        public static Camera Camera;

        public Program()
    : base(800, 600, GraphicsMode.Default, "Brickon Client", GameWindowFlags.Default, DisplayDevice.Default, 1, 1, GraphicsContextFlags.Default)
        {
            VSync = VSyncMode.On;

            MouseMove += (sender, e) =>
            {
                mouse_x = e.X;
                mouse_y = e.Y;
            };

        }
        public string brickonServer = "";
        // A simple vertex shader possible. Just passes through the position vector.
        const string VertexShaderSource = @"
            #version 330

            layout(location = 0) in vec3 position;
layout(location = 1) in vec4 col;

out vec4 coldt;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
            void main(void)
            {
coldt = col;
                gl_Position = vec4(vec3(position),1.0) * model  * view *projection;//


            }
        ";

        // A simple fragment shader. Just a constant red color.
        const string FragmentShaderSource = @"
            #version 330

            out vec4 outputColor;
in vec4 coldt;
uniform vec3 col;
            void main(void)
            {
                outputColor = col*vec4(coldt);
            }
        ";

        // Points of a triangle in normalized device coordinates.
        public static List<Vector3> Points = new List<Vector3>();
        public static List<Vector4> Col = new List<Vector4>();
        int VertexShader;
        int FragmentShader;
        public static int ShaderProgram;
        int VertexBufferObject;
        int VertexArrayObject;
        int VertexColBuffer;

        BrickonEditor.Model leg = new BrickonEditor.Model("leg.3d");
        BrickonEditor.Model cylinder = new BrickonEditor.Model("cylinder.3d");
        BrickonEditor.Model block = new BrickonEditor.Model("b55.3d");
        BrickonEditor.Model torso = new BrickonEditor.Model("torso.3d");

        public void rect(Vector3 pos, Vector4 scale, Color4 coldt)
        {
            Col.Add(new Vector4(coldt.R, coldt.G, coldt.B, coldt.A));
            Points.Add(new Vector3(-scale.X + pos.X , -scale.Z + pos.Y, pos.Z));
            Col.Add(new Vector4(coldt.R, coldt.G, coldt.B, coldt.A));
            Points.Add(new Vector3(scale.Y + pos.X, -scale.W + pos.Y, pos.Z));
            Col.Add(new Vector4(coldt.R, coldt.G, coldt.B, coldt.A));
            Points.Add(new Vector3(scale.Y + pos.X, scale.W + pos.Y, pos.Z));
            Col.Add(new Vector4(coldt.R, coldt.G, coldt.B, coldt.A));
            Points.Add(new Vector3(-scale.X+ pos.X, scale.Z + pos.Y, pos.Z));
            //Col.Add(new Vector3(coldt.R, coldt.G, coldt.B));
        }
        public static void panel(Vector2 pos,Vector2 scale,Color4 col)
        {
            //GL.PushMatrix();
            //GL.Enable(EnableCap.Blend);
            
            GL.Scale(0.2f+scale.X, 0.2f + scale.Y, 0.5f);
            GL.Translate(pos.X, pos.Y, 0);
            GL.Begin(BeginMode.Quads);

            GL.Color4(col);
            GL.Vertex3(-1f, -1f, 4); GL.Vertex3(1f, -1f, 4); GL.Vertex3(1f, 1f, 4); GL.Vertex3(-1f, 1f, 4);

            GL.End();
            //GL.Disable(EnableCap.Blend);
           // GL.PopMatrix();



        }
      public static string nick;
        public List<Player> players;
        public string toconnect = "bruhtale.xyz";
        public int portnumber = 7557;
        public int resetchr;
        public int skyplane;
        public int health;
        public int chat;
        public int studs;
        protected override async void OnLoad(EventArgs e)
        {
            Console.WriteLine("Brickoneer, enter the Server to connect ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("(Only Domains like myserver.ddns.net/1.thhh5hh.ngrok.io) for connecting your server use localhost:");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Sidenote: If it crashes try reconnecting. It should connect next time.");
            toconnect = Dns.GetHostEntry(Console.ReadLine()).AddressList.First(addr => addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            //Console.WriteLine(toconnect);
            Console.WriteLine("Enter port number to connect:");
            portnumber = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter your nick: (This going to be changed in future to accounts, impersonating can be detected results in hwid ban)");
            nick = Console.ReadLine();

            Console.WriteLine(nick);
            //1. Establish a connection to the server.

            //GL.Enable(EnableCap.DepthTest);
            // Load the source of the vertex shader and compile it.
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            GL.CompileShader(VertexShader);
            Bitmap textures = new Bitmap(@"textures.png");
            Bitmap font = new Bitmap(@"fonts.bmp");
           /* Script script = new Script();
            UserData.RegisterAssembly();
            script.Globals["Brickon"] = new BrickonApi();
            
            script.DoFile("brickon.lua");*/

             Camera = new Camera(this, new Vector3(0, 0, 0));

            //font.MakeTransparent(Color.Black);
            // Brickon.Model.Model.Load("feet.3d", out Points, out Col);
            Shapes.block(new Vector4(1, 1, 1, 1));
            

            tex = newTexture(font);
            resetchr = newTexture(new Bitmap("reset.png"));
            skyplane = newTexture(new Bitmap("sky.png"));
            health = newTexture(new Bitmap("health.png"));
            studs = newTexture(new Bitmap("studs.png"));
            view = Matrix4.CreateTranslation(Camera.Position);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55f), this.Size.Width / this.Size.Height, 0.5f, 5500.0f);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "projection"), true, ref projection);
            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "view"), true, ref view);

            GL.Enable(EnableCap.Blend);
            // rect(new Vector2(1.5f, 0), new Vector4(1, 1, 1, 1), Color.Red);

            // Load the source of the fragment shader and compile it.
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            GL.CompileShader(VertexShader);

            // Load the source of the fragment shader and compile it.
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(FragmentShader);

            // Create the shader program, attach the vertex and fragment shaders and link the program.
            ShaderProgram = GL.CreateProgram();
            GL.AttachShader(ShaderProgram, VertexShader);
            GL.AttachShader(ShaderProgram, FragmentShader);
            GL.LinkProgram(ShaderProgram);

            // Create the vertex buffer object (VBO) for the vertex data.
            VertexColBuffer = GL.GenBuffer();
            VertexBufferObject = GL.GenBuffer();

            // Bind the VBO and copy the vertex data into it.


          /*  GL.BindVertexArray(VertexArrayObject);

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
            GL.BufferData(BufferTarget.ArrayBuffer, Points.Count * Vector3.SizeInBytes, Points.ToArray(), BufferUsageHint.StaticDraw);
            //Same thing but for colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexColBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Col.Count * Vector4.SizeInBytes, Col.ToArray(), BufferUsageHint.StaticDraw);*/

            leg.setup1();
            cylinder.setup1();
            block.setup1();
            torso.setup1();
            Mouse.SetPosition(this.Width , this.Height );

            ConnectionResult connectionResult = ConnectionResult.TCPConnectionNotAlive;


            // Set the clear color to blue
            GL.ClearColor(Color4.CornflowerBlue);

            TcpConnection tcpConnection;
            //"127.0.0.1"
            tcpConnection = ConnectionFactory.CreateTcpConnection(toconnect, portnumber, out connectionResult);
            

            //2. Register what happens if we get a connection
            if (connectionResult == ConnectionResult.Connected)
                Console.WriteLine($"{tcpConnection.ToString()} Connection established");

            Random random = new Random();
            bool receivedaction = false;
            while (true)
            {
                // send an update to the server
                if(connectionResult == ConnectionResult.Connected)
                {
                    tcpConnection.Send(new GameUpdate() { X = -posdt.X, Y = -posdt.Y, Z = -posdt.Z , Username = nick});
                    tcpConnection.Fragment = true;
                // ask the server for our position
                GameResponse GameServer = await tcpConnection.SendAsync<GameResponse>(new GameRequest());
               // Console.WriteLine($"X:{myPositionFromServer.X} Y:{myPositionFromServer.Y} Z:{myPositionFromServer.Z}");


                    players = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Player>>(GameServer.Players);
                    if(receivedaction == false)
                    {
                        actionMessage = GameServer.actionMessage;
                        receivedaction = true;
                    }
                    // dont stress the cpu

                    // Thread.Sleep(1);
                }
            }
            base.OnLoad(e);
        }

        public bool isChatting = false;

        protected override void OnUnload(EventArgs e)
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
            GL.DeleteProgram(ShaderProgram);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            base.OnUnload(e);
        }
        public List<Block> blocks = new List<Block>();
        protected override void OnResize(EventArgs e)
        {
            // Resize the viewport to match the window size.
            GL.Viewport(0, 0, Width, Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            base.OnResize(e);
        }
        Matrix4 view;
        Matrix4 projection;
        public static Matrix4 model;

        void labelRendering(string label, Vector3 position, int rotation, float posvalue, Color4 col)
        {
            char[] label_ = label.ToCharArray();
            float pos = 0.0f;
            
            for (int h = 0; h < label_.Length; h++)
            {
                GL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);
                pos += 1.5f + posvalue;
                // if (label_[h] == ' ')
                // pos += 1.1f;
                //Color4.DarkCyan
                TextRenderer.char_(new Vector3(pos + position.X, position.Y, position.Z),new Vector3(0.11f, 0.11f, 0.51f), Color4.Black,col , new Vector4(0, 0, 1, rotation), 126 + 65+(65- label_[h]));
            }
            GL.ClearColor(Color4.CornflowerBlue);
        }


        int newTexture(Bitmap textureBitmap)//, TextureMinFilter texture_filter
        {
            int texture;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            BitmapData data = textureBitmap.LockBits(new System.Drawing.Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            textureBitmap.UnlockBits(data);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            return texture;
        }

        int tex;


        uint dtsys;
        Color4 coltest = new Color4();
        public static string chatMsg;
        public static string actionMessage ="";
        public static float time;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Clear the color buffer.

            time+=(float)e.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            
            GL.UseProgram(0);
            GL.Enable(EnableCap.ColorArray);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);

            //GL.ClearColor(1f, 1f, 1f, 1f);

            GL.LoadMatrix(ref modelview);

           // Points = new List<Vector3>();
            //Col = new List<Vector4>();
            
             




            // rect(new Vector3(-pos.X , pos.Y , -pos.Z), new Vector4(0.1f, 0.1f, 0.1f, 0.1f), Color4.Aqua);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.OneMinusSrcAlpha, BlendingFactor.OneMinusSrcColor);
            GL.Scale(0.15f, 0.15f, 0.5f);
            //GL.Translate(-Mouse.GetState().X, -Mouse.GetState().Y, posdt.Z);


            //GL.Disable(EnableCap.DepthTest);

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);

            Byte4 col = Brickon.Inferface.InferfaceSystem.getidColor(55);
            GL.Disable(EnableCap.Blend);
            GL.BindTexture(TextureTarget.Texture2D, skyplane);
            GL.PushMatrix();
            GL.Scale(new Vector3(15.00f, 10, 1));
            GL.Disable(EnableCap.DepthTest);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(time, 1.0f - 0); GL.Vertex3(-1f, -1f, 4);
            GL.TexCoord2(time + 1.0f, 1.0f - 0); GL.Vertex3(1f, -1f, 4);
            GL.TexCoord2(time + 1.0f, 1.0f - (0 + 1.0f)); GL.Vertex3(1f, 1f, 4);
            GL.TexCoord2(time, 1.0f - (0 + 1.0f)); GL.Vertex3(-1f, 1f, 4);
            GL.End();

            GL.PopMatrix();
            GL.BindTexture(TextureTarget.Texture2D, skyplane);

            GL.BindTexture(TextureTarget.Texture2D, resetchr);
            //  GL.Enable(EnableCap.Blend);
            GL.PushMatrix();
            GL.Scale(new Vector3(1.00f, 1, 1));
            GL.Enable(EnableCap.DepthTest);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 1.0f - 0); GL.Vertex3(-1f - 5, -1f, 4);
            GL.TexCoord2(1.0f, 1.0f - 0); GL.Vertex3(1f - 5, -1f, 4);
            GL.TexCoord2(1.0f, 1.0f - (0 + 1.0f)); GL.Vertex3(1f - 5, 1f, 4);
            GL.TexCoord2(0, 1.0f - (0 + 1.0f)); GL.Vertex3(-1f - 5, 1f, 4);
            GL.End();
            GL.PopMatrix();
            GL.BindTexture(TextureTarget.Texture2D, resetchr);


            GL.BindTexture(TextureTarget.Texture2D,health);
            //  GL.Enable(EnableCap.Blend);
            int hearths = 5;
            try
            {
                hearths = players.Find(x => (x.username == nick)).hearths;
            }
            catch (NullReferenceException)
            {

                //throw;
            }
            
            GL.PushMatrix();
            GL.Scale(new Vector3(1.00f, 1, 1));
            GL.Enable(EnableCap.DepthTest);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 1.0f - 0); GL.Vertex3(-1f - 1, -0.1f, 4);
            GL.TexCoord2(1.0f, 1.0f - 0); GL.Vertex3(1f - 1-(hearths/ hearths) + 0.1f, -0.1f + 0.1f, 4);
            GL.TexCoord2(1.0f, 1.0f - (0 + 1.0f)); GL.Vertex3(1f - 1 - (hearths / hearths)+0.1f, 0.1f + 0.1f, 4);
            GL.TexCoord2(0, 1.0f - (0 + 1.0f)); GL.Vertex3(-1f - 1, 0.1f, 4);
            GL.End();
            GL.PopMatrix();
            GL.BindTexture(TextureTarget.Texture2D, health);
            // GL.Enable(EnableCap.DepthTest);

            GL.Begin(BeginMode.Quads);


            GL.Color4(Color4.DarkGray);
            GL.Vertex3(-0.0f - 7, 0.5f, 4.0f);
            GL.Vertex3(1.5f - 7, 0.5f, 4.0f);
            // GL.Color3(0.2f, 0.9f, 1.0f);
            GL.Vertex3(1.5f - 7, 0.0f, 4.0f);
            GL.Vertex3(0.0f - 7, 0.0f, 4.0f);


            // GL.Color3(Color.White);
            /*   GL.Color3(col.R, col.G , col.B ); GL.Vertex3(-0.0f+5, 0.5f, 4.0f);
               // GL.Color3(1.0f, 0.0f, 0.0f);
               //GL.Vertex3(-1.0f, -1.0f, 4.0f);

               GL.Vertex3(0.5f+5, 0.5f, 4.0f);
               // GL.Color3(0.2f, 0.9f, 1.0f);
               GL.Vertex3(0.5f+5, 0.0f, 4.0f);
               GL.Vertex3(0.0f+5, 0.0f, 4.0f);
               GL.Color4(Color4.Blue);
               GL.Vertex3(-0.0f , 0.5f, 4.0f);
               GL.Vertex3(0.5f , 0.5f, 4.0f);
               // GL.Color3(0.2f, 0.9f, 1.0f);
               GL.Vertex3(0.5f , 0.0f, 4.0f);
               GL.Vertex3(0.0f , 0.0f, 4.0f);
               GL.Color4(Color4.Green);
               GL.Vertex3(-0.0f + 7, 0.5f, 4.0f);
               GL.Vertex3(0.5f + 7, 0.5f, 4.0f);
               // GL.Color3(0.2f, 0.9f, 1.0f);
               GL.Vertex3(0.5f + 7, 0.0f, 4.0f);
               GL.Vertex3(0.0f + 7, 0.0f, 4.0f);

               GL.Color4(coltest);
               GL.Vertex3(-0.0f + 7, 0.1f+5, 4.0f);
               GL.Vertex3(1.1f + 7, 0.1f+5, 4.0f);
               // GL.Color3(0.2f, 0.9f, 1.0f);
               GL.Vertex3(1.1f + 7, -0.5f + 5, 4.0f);
               GL.Vertex3(0.0f + 7, -0.5f + 5, 4.0f);*/
            GL.End();
            Byte4 Pixel = new Byte4();


            GL.ReadPixels(mouse_x, this.Height - mouse_y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, ref Pixel);
            dtsys = Pixel.ToUint32();

            Console.WriteLine(dtsys + " sys" + col.ToUint32());//Pixel.ToString()

            GL.BindTexture(TextureTarget.Texture2D, tex);
            if (Mouse.GetState().IsButtonDown(MouseButton.Left))
            {
                mouse = true;
            }else
            {
                mouse = false;
            }
            if (Mouse.GetState().IsButtonDown(MouseButton.Right))
            {
                mtdts = true;
            }
            else
            {
                mtdts = false;
            }
            try
            {

                labelRendering("Player List", new Vector3(0, 17+15+5 , 5), 0, 0.5f, Color4.DarkOrange);
                for (int i = 0; i < players.Count; i++)
            {

                    
                    labelRendering(players[i].username, new Vector3(0, 17+15+5-(i*(1+1+0.5f)), 5), 0, 0.5f, Color4.Gold);
                //"Chat fixed lolz bruh"

            }
            }
            catch (NullReferenceException)
            {

               // throw;
            }


            //GL.Color3(Color.White);

            // if (Mouse.GetState().X>15)
            //panel(new Vector2(1.5f, (1f + .08F)/.08F), new Vector2(0.051f, 0.11f * .08F), Color.DarkCyan);//fixed function too
            panel(new Vector2(1.5f, 0.1f* .08F) , new Vector2(0.051f, 0.051f * .08F),Color.White);

            GL.Begin(BeginMode.Quads);

            
            GL.Vertex3(-1f, -1f, 4); GL.Vertex3(1f, -1f, 4); GL.Vertex3(1f, 1f, 4); GL.Vertex3(-1f, 1f, 4);

            GL.End();

            GL.Flush();
            GL.Finish();
            //GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);



            if (dtsys == 939458560)
            {
                if (mouse)
                    coltest.R += 0.01f;
                if(mtdts)
                    coltest.R -= 0.01f;

                //GL.ClearColor(.2f, .1f, .3f, 1f); // purple
            }
            if(dtsys == 2852104617)
            {
                if(mouse)
                isChatting = true;

            }else
            {
                if (mouse)
                    isChatting = false;
            }
            if (dtsys == 16711808)
                {
                if (mouse)
                    coltest.G += 0.01f;
                if(mtdts)
                    coltest.G -= 0.01f;
                //GL.ClearColor(.2f, .1f, .3f, 1f); // purple
            }
            if (dtsys == 16776960)
            {
                if (mouse)
                    coltest.B += 0.01f;
                if(mtdts)
                    coltest.B -= 0.01f;
                //GL.ClearColor(.2f, .1f, .3f, 1f); // purple
            }



            if (dtsys == uint.MaxValue)
                {
                if (mouse)
                    coltest.R += 0.01f;
                    //GL.ClearColor(.5f, .1f, .5f, 1f); // purple
                }
            
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
           
            GL.Disable(EnableCap.Blend);
          //  GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(ShaderProgram);



           
            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(1)) * Matrix4.CreateTranslation(new Vector3(posdt.X,posdt.Y,posdt.Z));
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55f), this.Size.Width / (float)this.Size.Height, 0.1f, 100.0f) ;
            
            GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "projection"), true, ref projection);
            view = Camera.CameraMatrix  ; //Matrix4.LookAt(posdt, posdt, Camera.Up) *
             GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "view"), true, ref view);
            leg.setup();
            // Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // Bind the VAO
            GL.BindVertexArray(VertexArrayObject);
            // Use/Bind the program
            GL.UseProgram(ShaderProgram);
            // This draws the triangle.

            /* GL.BindVertexArray(VertexArrayObject);

             GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
             //Send data to bound buffer in the specified "slot"
             //The kind of buffer, The size of the array i(the array length * size of an element), The data(because we had a list we convert it to an array), Buffer Usage, doesn't matter that much nowadays
             GL.BufferData(BufferTarget.ArrayBuffer, Points.Count * Vector3.SizeInBytes, Points.ToArray(), BufferUsageHint.StaticDraw);
             //Same thing but for colors
             GL.BindBuffer(BufferTarget.ArrayBuffer, VertexColBuffer);
             GL.BufferData(BufferTarget.ArrayBuffer, Col.Count * Vector4.SizeInBytes, Col.ToArray(), BufferUsageHint.StaticDraw);*/
            cylinder.setup();
            block.setup();
            leg.setup();
            torso.setup();

            block.renderModel();

            var actionMGR = actionMessage.Split(' ');
            for (int i = 0; i < actionMGR.Length; i++)
            {
                if (actionMGR[i] == "block")
                {
                    Block block = new Block();
                    block.position = new Vector3(Single.Parse(actionMGR[i + 1]) , Single.Parse(actionMGR[i + 2]) , Single.Parse(actionMGR[i + 3]) );
                    block.col = new Vector4(Single.Parse(actionMGR[i + 4]), Single.Parse(actionMGR[i + 5]), Single.Parse(actionMGR[i + 6]),1);
                    block.material = Material.Block;
                    block.size = new Vector3(1, 1, 1);
                    block.BlockId = blocks.Count + 1;
                    blocks.Add(block);

                }
                if (actionMGR[i] == "updateblock")
                {
                    if (actionMGR[i+1] == "col")
                    {
                        blocks[blocks.FindIndex(x => x.BlockId == Int16.Parse(actionMGR[i+ 1+1]))].col = new Vector4(Single.Parse(actionMGR[i+1+1+1]), Single.Parse(actionMGR[i + 1 + 1+1+1]), Single.Parse(actionMGR[i + 1 + 1+1+1+1]), Single.Parse(actionMGR[i + 1 + 1+1+1+1+1]));
                    }
                }
                if (actionMGR[i] == "blockdt")
                {
                    Block block = new Block();
                    block.position = new Vector3(Single.Parse(actionMGR[i + 1]), Single.Parse(actionMGR[i + 2]), Single.Parse(actionMGR[i + 3]));
                    block.col = new Vector4(Single.Parse(actionMGR[i + 7]), Single.Parse(actionMGR[i + 8]), Single.Parse(actionMGR[i + 9]), 1);
                    block.material = Material.Block;
                    block.size = new Vector3(Single.Parse(actionMGR[i + 4]), Single.Parse(actionMGR[i + 5]), Single.Parse(actionMGR[i + 6]));
                    blocks.Add(block);

                }
            }
            block.renderModel();
            for (int i = 0; i < blocks.Count; i++)
            {
               //Matrix4.CreateTranslation(new Vector3(-posdt.X* -posdt.X, -1, -posdt.Z *-posdt.Z ))
                model = Matrix4.Identity* Matrix4.CreateTranslation(new Vector3(blocks[i].position.X, blocks[i].position.Y-1.5f, blocks[i].position.Z) + -posdt) *Matrix4.CreateScale(blocks[i].size) * Matrix4.CreateRotationY(Program.Camera.Facing * 5) * Matrix4.CreateRotationX(Program.Camera.Pitch * 5) ;
                GL.UniformMatrix4(GL.GetUniformLocation(ShaderProgram, "model"), true, ref model);
                Vector3 coldt = new Vector3(blocks[i].col.X, blocks[i].col.Y, blocks[i].col.Z);
                GL.Uniform3(GL.GetUniformLocation(ShaderProgram, "col"), ref coldt);
                block.render();
                //GL.DrawArrays(PrimitiveType.Quads, 0, Points.Count);
            }

            //preventing people to do memory leaking to crash clients
            if (blocks.Count > 5000)
                blocks = new List<Block>();

            //character model loader i guess
            try
            {


            for (int i = 0; i < players.Count; i++)
            {
                Vector3 charService = new Vector3(players[i].x, players[i].y, players[i].z);
                if (players[i].username != nick)
                {
                        ModelService.InstertModel(leg, new Vector3(0.5f, 1f, 0.5f), new Vector3(1.7F, 1.7F, -5) + charService + -posdt, new Vector3(1, 1, 1));
                        ModelService.InstertModel(leg, new Vector3(0.5f, 1f, 0.5f), new Vector3(-1.1F, 1.7F, -5) + charService + -posdt, new Vector3(1, 1, 1));
                        ModelService.InstertModel(torso, new Vector3(3 - 1, 1, 1.5f + 0.1f), new Vector3(-0.5f, 1.7F + 0.1f, -4.1f + 0.1f) + charService + -posdt, new Vector3(1, 1, 1));
                        ModelService.InstertModel(leg, new Vector3(0.5f, 1, 0.5f), new Vector3(0, 0, -5) + charService + -posdt, new Vector3(1, 1, 1));
                        ModelService.InstertModel(cylinder, new Vector3(0.55f, 0.7f, 0.55f), new Vector3(0, -0.5f, -4.5f) + charService + -posdt, new Vector3(1, 1, 1));
                        ModelService.InstertModel(cylinder, new Vector3(0.55f, 0.7f, 0.55f), new Vector3(1, -0.5f, -4.5f) + charService + -posdt, new Vector3(1, 1, 1));
                        ModelService.InstertModel(leg, new Vector3(0.5f, 1, 0.5f), new Vector3(1, 0, -5) + charService + -posdt, new Vector3(1, 1, 1));


                    }

                }
            }
            catch (NullReferenceException)
            {

                //throw;
            }

            ModelService.InstertModel(leg, new Vector3(0.5f, 1f, 0.5f), new Vector3(1.7F, 1.7F, -5) , new Vector3(1, 1, 1));
            ModelService.InstertModel(leg, new Vector3(0.5f, 1f, 0.5f), new Vector3(-1.1F, 1.7F, -5) , new Vector3(1, 1, 1));
            ModelService.InstertModel(torso, new Vector3(3 - 1, 1, 1.5f + 0.1f), new Vector3(-0.5f, 1.7F + 0.1f, -4.1f + 0.1f) , new Vector3(1, 1, 1));
            ModelService.InstertModel(leg, new Vector3(0.5f, 1, 0.5f), new Vector3(0, 0, -5) , new Vector3(1, 1, 1));
            ModelService.InstertModel(cylinder, new Vector3(0.55f, 0.7f, 0.55f), new Vector3(0, -0.5f, -4.5f) , new Vector3(1, 1, 1));
            ModelService.InstertModel(cylinder, new Vector3(0.55f, 0.7f, 0.55f), new Vector3(1, -0.5f, -4.5f) , new Vector3(1, 1, 1));
            ModelService.InstertModel(leg, new Vector3(0.5f, 1, 0.5f), new Vector3(1, 0, -5) , new Vector3(1, 1, 1));
            if (this.Focused == true)
            {
                MovementEngine.yaw = Mouse.GetState().X ;
            }
            //Camera.Position = new Vector3(-0.5f, 5 + 0.1f, -15.1f);
            Program.actionMessage = "";
            Console.WriteLine(blocks.Count);
            //GL.Flush();


            // Swap the front/back buffers so what we just rendered to the back buffer is displayed in the window.
            
            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }
        Vector3 pos = new Vector3();

        Random rnd = new Random();

        public static Vector3 posdt;
        static float MoveVelocity = 0.1f;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {

                KeyboardState keyboard = Keyboard.GetState();
            if(!isChatting)
            {
                if (keyboard.IsKeyDown(Key.Up) || keyboard.IsKeyDown(Key.Number5))
            {
                if (this.Focused == true)
                {
                    /*Camera.X += (float)Math.Cos(Camera.Facing) * MoveVelocity;
                    Camera.Z += (float)Math.Sin(Camera.Facing) * MoveVelocity;
                    Camera.Y += (float)Math.Sin(Camera.Pitch) * MoveVelocity;*/
                    //posdt.Z += 0.1f;
                    MovementEngine.position = posdt;
                MovementEngine.walkForward(5 * (float)e.Time);
                posdt = MovementEngine.position;
              //  Console.WriteLine("bobux");

            }
                }else
                {
                    
                }
            }
        }

        //[STAThread]
        public static Program program = new Program();
        static void Main(String[] args)
        {

            
            program.Run();


        }
    }
}