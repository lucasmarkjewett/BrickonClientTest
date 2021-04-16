using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickonEditor
{

   /* enum Direction
    {
        Right,
        Left ,
        Top,
        Bottom,
        Back,
        Front

    }*/
    static class MovementEngine
    {
        /*public static static Vector3[] directiondt = new Vector3[6] {
            new Vector3(1,0,0),new Vector3(-1,0,0),new Vector3(0,1,0),new Vector3(0,-1,0),new Vector3(0,0,-1),new Vector3(0,0,1)};*/
        public static float yaw = 0.0f;
        public static Vector3 position;
        public static void walkForward(float distance)
        {
            // double yaw_ =  (Math.PI / 180) * yaw;
            position.X += distance * -(float)Math.Sin((Math.PI / 180) * (yaw*5));
            position.Z -= distance * -(float)Math.Cos((Math.PI / 180) * (yaw*5));
        }
        public static void walkBackwards(float distance)
        {
            // double yaw_ =  (Math.PI / 180) * yaw;

            position.X -= distance * (float)Math.Sin((Math.PI / 180) * yaw);
            position.Z += distance * (float)Math.Cos((Math.PI / 180) * yaw);
        }
        public static void walkRight(float distance)
        {
            // double yaw_ =  (Math.PI / 180) * yaw;
            position.X += distance * (float)Math.Sin((Math.PI / 180) * (-yaw*5) + 90);
            position.Z -= distance * (float)Math.Cos((Math.PI / 180) * (-yaw*5) - 90); // // // // // // // //
        }

        public static void walkLeft(float distance)
        {
            // double yaw_ =  (Math.PI / 180) * yaw;

            position.X -= distance * (float)Math.Sin((Math.PI / 180) * -yaw + 90);
            position.Z += distance * (float)Math.Cos((Math.PI / 180) * -yaw - 90); //
        }



    }
}
