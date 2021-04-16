using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickonEditor
{
    public enum Material
    {
        Water,
    
        Cold,
        Garden,
        Tiles,
        Block
        


    }
    public class Block
    {
        public static int Mt(Material material)
        { return (int)material; }
        public Material material;
        public Vector3 position;
        public Vector3 size;
        public Vector4 col;
        public int BlockId;
    }
}
