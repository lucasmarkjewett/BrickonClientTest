using Network.Attributes;
using Network.Packets;

namespace Shared
{
    [PacketRequest(typeof(GameRequest))]
    public class GameResponse: ResponsePacket
    {
        public GameResponse(GameRequest request) : base(request)
        {

        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }
        public string actionMessage { get; set; }
        public string Username { get; set; }
        public string Players { get; set; }
    }
}