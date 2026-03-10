using NetPacket;
using UnityEngine;

namespace NetCore
{
    public static class NetLogic
    {
        public static TCP tcp;

        public static void NetMoveCharacter(int instanceId, Vector2 speed)
        {
            transformation transform = new();
            {
                transform.X = speed.x;
                transform.Y = speed.y;
            }

            tcp.AsyncSend(PacketType.Transformation, transform);
        }
    }
}