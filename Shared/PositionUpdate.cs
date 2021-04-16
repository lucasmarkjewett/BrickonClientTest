using Network;
using Network.Packets;

namespace Shared
{
    public class GameUpdate : Packet
    {
        public string username;

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }
        public string actionMessage { get; set; }
        public string Username { get; set; }

        public string Players { get; set; }

    }
}